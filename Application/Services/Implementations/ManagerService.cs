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
    public class ManagerService : BaseService, IManagerService
    {
        private readonly new IMapper _mapper;
        private readonly IManagerRepository _managerRepository;
        public ManagerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _managerRepository = unitOfWork.Manager;
        }

        public async Task<IActionResult> GetManager(Guid id)
        {
            try
            {
                var manager = await _managerRepository.GetMany(st => st.Id.Equals(id))
                    .ProjectTo<ManagerViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (manager == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(manager)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetManagers(PaginationRequestModel pagination, ManagerFilterModel filter)
        {
            try
            {
                var query = _managerRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(st => st.FirstName.Contains(filter.Name) || st.LastName.Contains(filter.Name));
                }
                var totalRow = await query.AsNoTracking().CountAsync();
                var managers = await query.AsNoTracking()
                    .ProjectTo<ManagerViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(st => st.LastName)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(managers.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateManager(Guid id, ManagerUpdateModel model)
        {
            try
            {
                var manager = await _managerRepository.FirstOrDefaultAsync(st => st.Id.Equals(id));
                if (manager == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var managerUpdate = _mapper.Map(model, manager);

                _managerRepository.Update(managerUpdate);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetManager(id);
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