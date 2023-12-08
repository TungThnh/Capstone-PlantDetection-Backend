using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Common.Extensions;
using Data;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class QuestionService : BaseService, IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICloudStorageService _cloudStorageService;
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _questionRepository = unitOfWork.Question;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> GetQuestions(QuestionFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _questionRepository.GetAll();

                if (filter.Title != null)
                {
                    query = query.Where(qt => qt.Title.Contains(filter.Title));
                }

                var totalRow = query.Count();
                var questions = await query.AsNoTracking()
                    .ProjectTo<QuestionViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(questions.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<QuestionViewModel> GetQuestionById(Guid id)
        {
            try
            {
                return await _questionRepository.GetMany(qt => qt.Id.Equals(id))
                    .ProjectTo<QuestionViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync() ?? null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetQuestionResult(Guid id)
        {
            try
            {
                var question = await _questionRepository.GetMany(qt => qt.Id.Equals(id))
                    .ProjectTo<QuestionResultViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (question == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                return new ObjectResult(question)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetQuestion(Guid id)
        {
            try
            {
                var question = await GetQuestionById(id);
                if (question == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(question)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateQuestion(QuestionCreateModel model)
        {
            try
            {
                var id = Guid.NewGuid();
                var question = _mapper.Map<Question>(model);
                question.Id = id;
                if (model.Image != null)
                {
                    question.ImageUrl = await _cloudStorageService.Upload(Guid.NewGuid(), model.Image.ContentType, model.Image.OpenReadStream());
                }

                _questionRepository.Add(question);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    var response = await GetQuestionById(id);
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
    }
}
