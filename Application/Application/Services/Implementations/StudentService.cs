using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class StudentService : BaseService, IStudentService
    {
        private readonly new IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _studentRepository = unitOfWork.Student;
        }

        public async Task<IActionResult> GetStudent(Guid id)
        {
            try
            {
                var student = await _studentRepository.GetMany(st => st.Id.Equals(id))
                    .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (student == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(student)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudents(PaginationRequestModel pagination, StudentFilterModel filter)
        {
            try
            {
                var query = _studentRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(st => st.FirstName.Contains(filter.Name) || st.LastName.Contains(filter.Name));
                }
                var totalRow = await query.AsNoTracking().CountAsync();
                var students = await query.AsNoTracking()
                    .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(st => st.LastName)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(students.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateStudent(Guid id, StudentUpdateModel model)
        {
            try
            {
                var student = await _studentRepository.FirstOrDefaultAsync(st => st.Id.Equals(id));
                if (student == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                _mapper.Map(model, student);

                _studentRepository.Update(student);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetStudent(id);
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
