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
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class ReportService : BaseService, IReportService
    {
        private readonly new IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly ICloudStorageService _cloudStorageService;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _reportRepository = unitOfWork.Report;
            _cloudStorageService = cloudStorageService;
        }

        private async Task<ReportViewModel> GetReportById(Guid id)
        {
            try
            {
                return await _reportRepository.GetMany(rp => rp.Id.Equals(id))
                    .ProjectTo<ReportViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync() ?? null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetReport(Guid id)
        {
            try
            {
                var report = await GetReportById(id);
                if (report == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(report)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetManagerReports(Guid managerId, ReportFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _reportRepository.GetMany(rp => rp.Student.StudentClass!.Class.ManagerId.Equals(managerId)
                    && !rp.Student.StudentClass.Class.Status.Equals(ClassStatuses.PendingApproval)
                );

                if (filter.Name != null)
                {
                    query = query.Where(pl => pl.Label.Name.Contains(filter.Name));
                }
                if (filter.Status != null)
                {
                    query = query.Where(pl => pl.Status.Equals(filter.Status));
                }
                if (filter.ClassId != null)
                {
                    query = query.Where(pl => pl.Student.StudentClass != null ? pl.Student.StudentClass.ClassId.Equals(filter.ClassId) : false);
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var reports = await query.AsNoTracking()
                    .ProjectTo<ReportViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(report => report.Status.Equals(ReportStatuses.Pending))
                    .ThenByDescending(report => report.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(reports.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudentReports(Guid id, PaginationRequestModel pagination, ReportFilterModel filter)
        {
            try
            {
                var query = _reportRepository.GetMany(rp => rp.Student.Id.Equals(id));
                if (filter.Name != null)
                {
                    query = query.Where(pl => pl.Label.Name.Contains(filter.Name));
                }
                if (filter.Status != null)
                {
                    query = query.Where(pl => pl.Status.Equals(filter.Status));
                }
                if (filter.LabelId != null)
                {
                    query = query.Where(pl => pl.LabelId.Equals(filter.LabelId));
                }
                if (filter.ClassId != null)
                {
                    query = query.Where(pl => pl.Student.StudentClass != null ? pl.Student.StudentClass.ClassId.Equals(filter.ClassId) : false);
                }
                if (filter.Latest != null && filter.Latest == true)
                {
                    query = query.OrderByDescending(pl => pl.CreateAt).ThenBy(pl => pl.Status);
                }
                if (filter.Latest != null && filter.Latest == false)
                {
                    query = query.OrderBy(pl => pl.CreateAt).ThenBy(pl => pl.Status);
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var reports = await query.AsNoTracking()
                    .ProjectTo<ReportViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(reports.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetReports(PaginationRequestModel pagination, ReportFilterModel filter)
        {
            try
            {
                var query = _reportRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(pl => pl.Label.Name.Contains(filter.Name));
                }
                if (filter.Status != null)
                {
                    query = query.Where(pl => pl.Status.Equals(filter.Status));
                }
                if (filter.LabelId != null)
                {
                    query = query.Where(pl => pl.LabelId.Equals(filter.LabelId));
                }
                if (filter.ClassId != null)
                {
                    query = query.Where(pl => pl.Student.StudentClass != null ? pl.Student.StudentClass.ClassId.Equals(filter.ClassId) : false);
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var reports = await query.AsNoTracking()
                    .ProjectTo<ReportViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(report => report.Status.Equals(ReportStatuses.Pending))
                    .ThenByDescending(report => report.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(reports.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateReport(Guid studentId, ReportCreateModel model)
        {
            try
            {
                var id = Guid.NewGuid();
                var report = _mapper.Map<Report>(model);
                report.Id = id;
                report.Status = ReportStatuses.Pending;
                report.StudentId = studentId;
                report.ImageUrl = await _cloudStorageService.Upload(Guid.NewGuid(), model.Image.ContentType, model.Image.OpenReadStream());

                _reportRepository.Add(report);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    var response = await GetReportById(id);
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

        public async Task<IActionResult> UpdateReport(Guid id, ReportUpdateModel model)
        {
            try
            {
                var report = await _reportRepository.FirstOrDefaultAsync(rp => rp.Id == id);
                if (report == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                _mapper.Map(model, report);
                _reportRepository.Update(report);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    var response = await GetReportById(id);
                    return new ObjectResult(response)
                    {
                        StatusCode = StatusCodes.Status200OK
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
