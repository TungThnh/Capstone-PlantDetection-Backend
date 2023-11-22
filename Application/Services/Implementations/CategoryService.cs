using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly new IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _categoryRepository = unitOfWork.Category;
        }

        public async Task<IActionResult> GetCategories(CategoryFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _categoryRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(c => c.Name.Contains(filter.Name));
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var categories = await query.AsNoTracking()
                    .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                .ToListAsync();

                return new ObjectResult(categories.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetCategory(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetMany(cl => cl.Id.Equals(id))
                    .AsNoTracking()
                    .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (category == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(category)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}