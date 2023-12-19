using Application.Services.Interfaces;
using AutoMapper;
using Common.Constants;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Implementations
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IClassRepository _classRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IStudentRepository _studentRepository;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _classRepository = unitOfWork.Class;
            _plantRepository = unitOfWork.Plant;
            _reportRepository = unitOfWork.Report;
            _studentRepository = unitOfWork.Student;
        }

        public IActionResult GetDashboardData()
        {
            try
            {
                var totalPlants = _plantRepository.Count();
                var availablePlants = _plantRepository.GetMany(pl => pl.Status.Equals(PlantStatuses.Available)).Count();
                var totalClasses = _classRepository.Count();
                var confirmedClasses = _classRepository.GetMany(cl => cl.Status != ClassStatuses.PendingApproval).Count();
                var totalReports = _reportRepository.Count();
                var confirmedReports = _reportRepository.GetMany(cl => cl.Status != ReportStatuses.Pending).Count();
                var totalStudents = _studentRepository.Count();
                var availableStudents = _studentRepository.GetMany(cl => cl.Status != StudentStatuses.Inactive).Count();

                var dashboard = new DashboardViewModel
                {
                    TotalClasses = totalClasses,
                    TotalReports = totalReports,
                    AvailablePlants = availablePlants,
                    AvailableStudents = availableStudents,
                    ConfirmedClasses = confirmedClasses,
                    ConfirmedReports = confirmedReports,
                    TotalStudents = totalStudents,
                    TotalPlants = totalPlants,
                };

                return new ObjectResult(dashboard)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetManagerDashboardData(Guid id)
        {
            try
            {
                var totalPlants = _plantRepository.Count();
                var availablePlants = _plantRepository.GetMany(pl => pl.Status.Equals(PlantStatuses.Available)).Count();
                var totalClasses = _classRepository.GetMany(cl => cl.ManagerId.Equals(id)).Count();
                var confirmedClasses = _classRepository.GetMany(cl => cl.ManagerId.Equals(id) && cl.Status != ClassStatuses.PendingApproval).Count();
                var totalReports = _reportRepository.GetMany(rp => rp.Class.ManagerId.Equals(id)).Count();
                var confirmedReports = _reportRepository.GetMany(cl => cl.Class.ManagerId.Equals(id) && cl.Status != ReportStatuses.Pending).Count();
                var totalStudents = 0;
                var availableStudents = 0;
                if (_studentRepository.Any(st => st.StudentClass != null && st.StudentClass.Class.ManagerId.Equals(id)))
                {
                    totalStudents = _studentRepository.GetMany(st => st.StudentClass != null && st.StudentClass.Class.ManagerId.Equals(id)).Count();
                    availableStudents = _studentRepository.GetMany(st => st.StudentClass != null && st.StudentClass.Status != ClassQueueStatuses.Enrolled).Count();
                }

                var dashboard = new DashboardViewModel
                {
                    TotalClasses = totalClasses,
                    TotalReports = totalReports,
                    AvailablePlants = availablePlants,
                    AvailableStudents = availableStudents,
                    ConfirmedClasses = confirmedClasses,
                    ConfirmedReports = confirmedReports,
                    TotalStudents = totalStudents,
                    TotalPlants = totalPlants,
                };

                return new ObjectResult(dashboard)
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
