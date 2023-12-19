using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class ExamService : BaseService, IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        public ExamService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _examRepository = unitOfWork.Exam;
            _questionRepository = unitOfWork.Question;
        }

        private async Task<ExamViewModel> GetExamById(Guid id)
        {
            try
            {
                var exam = await _examRepository.GetMany(ex => ex.Id.Equals(id))
                    .ProjectTo<ExamViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return exam != null ? exam : null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetExamResult(Guid id)
        {
            try
            {
                var exam = await _examRepository.GetMany(ex => ex.Id.Equals(id))
                    .ProjectTo<ExamResultViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (exam == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(exam)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetExam(Guid id)
        {
            try
            {
                var exam = await GetExamById(id);
                if (exam == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(exam)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudentExams(Guid id, ExamFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _examRepository.GetMany(ex => ex.StudentId.Equals(id));

                if (filter.Latest == null && filter.Latest == true)
                {
                    query = query.OrderByDescending(ex => ex.CreateAt);
                }

                if (filter.Latest == null && filter.Latest == false)
                {
                    query = query.OrderBy(ex => ex.CreateAt);
                }

                var totalRow = query.Count();
                var exams = await query.AsNoTracking()
                    .ProjectTo<ExamResultViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(exams.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AutoSubmitExam(Guid id)
        {
            try
            {
                var exam = await _examRepository.GetMany(ex => ex.Id.Equals(id))
                    .Include(ex => ex.QuestionExams)
                    .ThenInclude(qe => qe.Question)
                    .FirstOrDefaultAsync();

                if (exam == null)
                {
                    return;
                }

                if (exam.IsSubmitted)
                {
                    return;
                }

                exam.SubmitAt = DateTime.Now.AddHours(7);
                exam.IsSubmitted = true;

                exam.Score = 0.0;

                _examRepository.Update(exam);
                var result = await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateExam(Guid studentId)
        {
            try
            {
                var exam = new Exam
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId
                };

                var questions = await _questionRepository.GetAll().OrderBy(x => Guid.NewGuid()).Take(10).ToListAsync();

                foreach (var question in questions)
                {
                    exam.QuestionExams.Add(new QuestionExam
                    {
                        ExamId = exam.Id,
                        QuestionId = question.Id,
                    });
                }

                _examRepository.Add(exam);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    BackgroundJob.Schedule(() => AutoSubmitExam(exam.Id), TimeSpan.FromMinutes(11));
                    var response = await GetExamById(exam.Id);
                    return new ObjectResult(response)
                    {
                        StatusCode = StatusCodes.Status201Created
                    };
                }
                return new ObjectResult(CustomErrors.UnprocessableEntity)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> SubmitExam(Guid id, ExamSubmitCreateModel model)
        {
            try
            {
                var exam = await _examRepository.GetMany(ex => ex.Id.Equals(id))
                    .Include(ex => ex.QuestionExams)
                    .ThenInclude(qe => qe.Question)
                    .FirstOrDefaultAsync();

                if (exam == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                if (exam.IsSubmitted)
                {
                    return new ObjectResult(CustomErrors.ExamSubmitted)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                exam.SubmitAt = DateTime.Now.AddHours(7);
                exam.IsSubmitted = true;
                foreach (var questionExam in model.QuestionExams)
                {
                    foreach (var crQuestionExam in exam.QuestionExams)
                    {
                        if (questionExam.QuestionId.Equals(crQuestionExam.QuestionId))
                        {
                            crQuestionExam.SelectedAnswer = questionExam.SelectedAnswer;
                        }
                    }
                }
                exam.Score = CalculateExamScore(exam);

                _examRepository.Update(exam);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetExam(id);
                }
                return new ObjectResult(CustomErrors.UnprocessableEntity)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CalculateResult(Guid id)
        {
            try
            {
                var exam = await _examRepository.GetMany(ex => ex.Id.Equals(id))
                    .ProjectTo<ExamResultViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (exam == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                if (exam.IsSubmitted == false)
                {
                    return new ObjectResult(CustomErrors.UnsubmittedExam)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                var score = 0.0;

                foreach (var question in exam.QuestionExams)
                {
                    if (question.SelectedAnswer != null && question.SelectedAnswer.Equals(question.Question.CorrectAnswer))
                    {
                        score += CalculateScore(exam.QuestionExams.Count());
                    }
                }

                exam.Score = Math.Round(score, 1);

                return new ObjectResult(exam)
                {
                    StatusCode = StatusCodes.Status200OK
                };

            }
            catch (Exception)
            {
                throw;
            }
        }

        private double CalculateExamScore(Exam exam)
        {
            try
            {
                var score = 0.0;

                foreach (var question in exam.QuestionExams)
                {
                    if (question.SelectedAnswer != null && question.SelectedAnswer.Equals(question.Question.CorrectAnswer))
                    {
                        score += CalculateScore(exam.QuestionExams.Count());
                    }
                }
                return Math.Round(score, 1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private double CalculateScore(int numberOfQuestion)
        {
            var totalScore = 10;
            return totalScore / numberOfQuestion;
        }
    }
}
