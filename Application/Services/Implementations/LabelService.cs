﻿using Application.Services.Interfaces;
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
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class LabelService : BaseService, ILabelService
    {
        private readonly new IMapper _mapper;
        private readonly ILabelRepository _labelRepository;
        public LabelService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _labelRepository = unitOfWork.Label;
        }

        private async Task<LabelViewModel> GetLabelById(Guid id)
        {
            try
            {
                var label = await _labelRepository.GetMany(cl => cl.Id.Equals(id))
                    .AsNoTracking()
                    .ProjectTo<LabelViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return label != null ? label : null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetLabels(LabelFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _labelRepository.GetAll();

                if (filter.Name != null)
                {
                    query = query.Where(c => c.Name.Contains(filter.Name));
                }

                if (filter.ClassId != null)
                {
                    query = query.Where(c => c.ClassLabels.Any(lb => lb.ClassId.Equals(filter.ClassId)));
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var labels = await query.AsNoTracking()
                    .ProjectTo<LabelViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                .ToListAsync();

                return new ObjectResult(labels.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetLabel(Guid id)
        {
            try
            {
                var label = await _labelRepository.GetMany(cl => cl.Id.Equals(id))
                    .AsNoTracking()
                    .ProjectTo<LabelViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (label == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(label)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateLabel(LabelCreateModel model)
        {
            try
            {
                if (_labelRepository.Any(lb => lb.Name.Equals(model.Name)))
                {
                    return new ObjectResult(CustomErrors.LabelNameAlreadyExists)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

                var id = Guid.NewGuid();
                var label = _mapper.Map<Label>(model);
                label.Id = id;

                _labelRepository.Add(label);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    var response = await GetLabelById(id);
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

        public async Task<IActionResult> UpdateLabel(Guid id, LabelUpdateModel model)
        {
            try
            {
                if (_labelRepository.Any(lb => lb.Name.Equals(model.Name)))
                {
                    return new ObjectResult(CustomErrors.LabelNameAlreadyExists)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }
                var label = await _labelRepository.GetMany(lb => lb.Id.Equals(id)).FirstOrDefaultAsync();

                if (label == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                _mapper.Map(model, label);

                _labelRepository.Update(label);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    return await GetLabel(id);
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

        public async Task<IActionResult> RemoveLabel(Guid id)
        {
            try
            {
                var label = await _labelRepository.GetMany(lb => lb.Id.Equals(id))
                    .Include(lb => lb.ClassLabels)
                    .FirstOrDefaultAsync();

                if (label == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                if (label.ClassLabels.Any())
                {
                    return new ObjectResult(CustomErrors.LabelAlreadyInClass)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                _labelRepository.Remove(label);
                var result = await _unitOfWork.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
