﻿using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Common.Extensions;
using Common.Helpers;
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
    public class ClassService : BaseService, IClassService
    {
        private readonly new IMapper _mapper;
        private readonly IClassRepository _classRepository;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly ICloudStorageService _cloudStorageService;

        public ClassService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _classRepository = unitOfWork.Class;
            _studentClassRepository = unitOfWork.StudentClass;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> GetClass(Guid id)
        {
            try
            {
                var iClass = await _classRepository.GetMany(cl => cl.Id.Equals(id))
                    .AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (iClass == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(iClass)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetManagerClasses(Guid id, ClassFilterModel filter, PaginationRequestModel pagination)
        {
            try
            {
                var query = _classRepository.GetMany(cl => cl.ManagerId.Equals(id) && !cl.Status.Equals(ClassStatuses.PendingApproval));
                if (filter.Name != null)
                {
                    query = query.Where(cl => cl.Name.Contains(filter.Name));
                }
                if (filter.ManagerId != null)
                {
                    query = query.Where(cl => cl.ManagerId.Equals(filter.ManagerId));
                }
                if (filter.Status != null)
                {
                    query = query.Where(cl => cl.Status.Equals(filter.Status));
                }
                var totalRow = query.Count();
                var classes = await query.AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .OrderByDescending(cl => cl.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(classes.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<StudentClassViewModel> GetStudentInClass(Guid classId, Guid studentId)
        {
            try
            {
                var studentClass = await _studentClassRepository.GetMany(st => st.ClassId.Equals(classId) && st.StudentId.Equals(studentId))
                    .ProjectTo<StudentClassViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return studentClass != null ? studentClass : null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudentClass(Guid id)
        {
            try
            {
                var iClass = await _classRepository.GetMany(cl => cl.StudentClasses.Any(st => st.StudentId.Equals(id)))
                    .AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (iClass == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(iClass)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetAvailableClasses(PaginationRequestModel pagination, ClassFilterModel filter)
        {
            try
            {
                var query = _classRepository.GetMany(cl => !cl.Status.Equals(ClassStatuses.PendingApproval));
                if (filter.Name != null)
                {
                    query = query.Where(cl => cl.Name.Contains(filter.Name));
                }
                if (filter.ManagerId != null)
                {
                    query = query.Where(cl => cl.ManagerId.Equals(filter.ManagerId));
                }
                if (filter.Status != null)
                {
                    query = query.Where(cl => cl.Status.Equals(filter.Status));
                }
                var totalRow = await query.AsNoTracking().CountAsync();
                var classes = await query.AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .OrderByDescending(cl => cl.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(classes.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetClasses(PaginationRequestModel pagination, ClassFilterModel filter)
        {
            try
            {
                var query = _classRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(cl => cl.Name.Contains(filter.Name));
                }
                if (filter.ManagerId != null)
                {
                    query = query.Where(cl => cl.ManagerId.Equals(filter.ManagerId));
                }
                if (filter.Status != null)
                {
                    query = query.Where(cl => cl.Status.Equals(filter.Status));
                }
                var totalRow = await query.AsNoTracking().CountAsync();
                var classes = await query.AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .OrderByDescending(cl => cl.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(classes.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreateClass(ClassCreateModel model, Guid managerId)
        {
            try
            {
                if(_classRepository.Any(cl => cl.Code.Equals(model.Code)))
                {
                    return new ObjectResult(CustomErrors.ClassCodeConflict)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

                if (_classRepository.Any(cl => cl.Name.Equals(model.Name)))
                {
                    return new ObjectResult(CustomErrors.ClassNameConflict)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

                var iClass = _mapper.Map<Class>(model);
                iClass.Id = Guid.NewGuid();
                iClass.ManagerId = managerId;
                iClass.Status = ClassStatuses.Closed;
                _classRepository.Add(iClass);
                iClass.ThumbnailUrl = await _cloudStorageService
                    .Upload(Guid.NewGuid(), model.Thumbnail.ContentType, model.Thumbnail);

                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetClass(iClass.Id);
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

        public async Task<IActionResult> UpdateClass(Guid id, ClassUpdateModel model)
        {
            try
            {
                var iClass = await _classRepository.FirstOrDefaultAsync(cl => cl.Id.Equals(id));
                if (iClass == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                _mapper.Map(model, iClass);
                _classRepository.Update(iClass);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetClass(iClass.Id);
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

        public async Task<IActionResult> ListOfStudents(Guid id, PaginationRequestModel pagination)
        {
            try
            {
                var query = _studentClassRepository.GetMany(st => st.ClassId.Equals(id));
                var totalRow = await query.AsNoTracking().CountAsync();
                var students = await query.AsNoTracking()
                    .ProjectTo<StudentClassViewModel>(_mapper.ConfigurationProvider)
                    .OrderByDescending(st => st.CreateAt)
                    .Paginate(pagination)
                    .ToListAsync();
                return new ObjectResult(students.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> FindClassByCode(string code)
        {
            try
            {
                var iClass = await _classRepository.GetMany(cl => cl.Code.Equals(code))
                    .AsNoTracking()
                    .ProjectTo<ClassViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (iClass == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(iClass)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckFolderExists(string directoryPath, string folderName)
        {
            string folderPath = Path.Combine(directoryPath, folderName);

            return Directory.Exists(folderPath);
        }

        private bool CheckModelFileExists(string folderPath)
        {
            string modelFilePath = Path.Combine(folderPath, "best.pt");

            return File.Exists(modelFilePath);
        }

        public async Task<IActionResult> IsClassModelExists(Guid id)
        {
            try
            {
                var classCode = await _classRepository.GetMany(cl => cl.Id.Equals(id)).Select(cl => cl.Code).FirstOrDefaultAsync();

                if (classCode == null)
                {
                    return new NotFoundResult();
                }

                string classDirectory = @"C:\Users\Janglee\Desktop\plant_classfify\classes\";

                return new ObjectResult(CheckFolderExists(classDirectory, classCode) && CheckModelFileExists(classDirectory + classCode))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> RequestToJoinClass(Guid studentId, Guid classId)
        {
            try
            {
                var iClass = await _classRepository.GetMany(cl => cl.Id.Equals(classId)).FirstOrDefaultAsync();

                if(iClass == null)
                {
                    return new NotFoundResult();
                }

                if(iClass.Status.Equals(ClassStatuses.Closed))
                {
                    return new ObjectResult(CustomErrors.ClassClosed)
                    {
                        StatusCode = StatusCodes.Status423Locked
                    };
                }

                // Return 409 if the student is already in the class
                if (_studentClassRepository.Any(sc => sc.ClassId.Equals(classId) && sc.StudentId.Equals(studentId)))
                {
                    // Returns 409 if the student is already in class and waiting for confirmation
                    if (_studentClassRepository.Any(sc => sc.ClassId.Equals(classId)
                        && sc.StudentId.Equals(studentId) && sc.Status.Equals(ClassQueueStatuses.PendingApproval)))
                    {
                        return new ObjectResult(CustomErrors.RequestSubmitted)
                        {
                            StatusCode = StatusCodes.Status409Conflict
                        };
                    }

                    // Returns 409 if the student is already in the class and has been invited
                    if (_studentClassRepository.Any(sc => sc.ClassId.Equals(classId)
                        && sc.StudentId.Equals(studentId) && sc.Status.Equals(ClassQueueStatuses.Invited)))
                    {
                        return new ObjectResult(CustomErrors.YouHasBeenInvited)
                        {
                            StatusCode = StatusCodes.Status409Conflict
                        };
                    }

                    return new ObjectResult(CustomErrors.StudentAlreadyInClass)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }
                var studentClass = new StudentClass
                {
                    ClassId = classId,
                    StudentId = studentId
                };
                studentClass.CreateAt = DateTimeHelper.Now();
                studentClass.Status = ClassQueueStatuses.Enrolled;
                _studentClassRepository.Add(studentClass);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetClass(classId);
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

        public async Task<IActionResult> LeaveClass(Guid classId, Guid studentId)
        {
            try
            {
                var studentClass = await _studentClassRepository
                    .GetMany(st => st.ClassId.Equals(classId) && st.StudentId.Equals(studentId)).FirstOrDefaultAsync();

                if (studentClass != null)
                {
                    _studentClassRepository.Remove(studentClass);
                }

                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return new NoContentResult();
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

        public async Task<IActionResult> InviteStudentIntoClass(InviteStudentModel model)
        {
            try
            {
                // Return 409 if the student is already in the class
                if (_studentClassRepository.Any(sc => sc.ClassId.Equals(model.ClassId) && sc.StudentId.Equals(model.StudentId)))
                {
                    // Returns 409 if the student is already in class and waiting for confirmation
                    if (_studentClassRepository.Any(sc => sc.ClassId.Equals(model.ClassId)
                        && sc.StudentId.Equals(model.StudentId) && sc.Status.Equals(ClassQueueStatuses.PendingApproval)))
                    {
                        return new ObjectResult(CustomErrors.RequestHasNotBeenConfirmed)
                        {
                            StatusCode = StatusCodes.Status409Conflict
                        };
                    }

                    // Returns 409 if the student is already in the class and has been invited
                    if (_studentClassRepository.Any(sc => sc.ClassId.Equals(model.ClassId)
                        && sc.StudentId.Equals(model.StudentId) && sc.Status.Equals(ClassQueueStatuses.Invited)))
                    {
                        return new ObjectResult(CustomErrors.StudentHasBeenInvited)
                        {
                            StatusCode = StatusCodes.Status409Conflict
                        };
                    }

                    return new ObjectResult(CustomErrors.StudentAlreadyInClass)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

                var studentClass = _mapper.Map<StudentClass>(model);
                studentClass.CreateAt = DateTimeHelper.Now();
                studentClass.Status = ClassQueueStatuses.Invited;
                _studentClassRepository.Add(studentClass);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetClass(model.ClassId);
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

        public async Task<IActionResult> ApproveStudentToJoinClass(Guid classId, Guid studentId)
        {
            try
            {
                var studentClass = await _studentClassRepository.GetMany(st => st.ClassId.Equals(classId) && st.StudentId.Equals(studentId))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (studentClass == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                studentClass.Status = ClassQueueStatuses.Enrolled;

                _studentClassRepository.Update(studentClass);

                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    var response = await GetStudentInClass(classId, studentId);
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
