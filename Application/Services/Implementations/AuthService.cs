using Application.Services.Interfaces;
using Application.Settings;
using AutoMapper;
using Common.Constants;
using Common.Enums;
using Data;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Domain.Models.Firebase;
using Domain.Models.Systems;
using Domain.Models.Views;
using Domain.Specifications;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly new IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IStudentRepository _studentRepository;
        private readonly IManagerRepository _managerRepository;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _studentRepository = unitOfWork.Student;
            _managerRepository = unitOfWork.Manager;
        }

        public async Task<IActionResult> AuthenticateWithGoogleAsync(GoogleIdTokenModel model, AuthenticationType type)
        {
            try
            {
                var firebase = FirebaseAuth.DefaultInstance;
                FirebaseToken decodedToken = await firebase.VerifyIdTokenAsync(model.IdToken);
                string decodedTokenJson = JsonConvert.SerializeObject(decodedToken);
                FirebaseTokenModel firebaseTokenModel = JsonConvert.DeserializeObject<FirebaseTokenModel>(decodedTokenJson)!;
                if (type == AuthenticationType.ForStudent)
                {
                    return await GetOrCreateStudent(firebaseTokenModel);
                }
                return await GetOrCreateManager(firebaseTokenModel);
            }
            catch (FirebaseAuthException)
            {
                throw;
            }
        }

        private IActionResult GetStudentAccessToken(Student student, int statusCode)
        {
            var auth = _mapper.Map<AuthModel>(student);
            auth.Role = UserRoles.Student;
            if (auth != null)
            {
                var token = GenerateJwtToken(auth);
                return new ObjectResult(new AuthViewModel
                {
                    AccessToken = token,
                })
                {
                    StatusCode = statusCode
                };
            }
            return new ObjectResult(CustomErrors.UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }

        private async Task<IActionResult> GetOrCreateStudent(FirebaseTokenModel model)
        {
            // Return student information if email already exitsts
            if (_studentRepository.Any(sto => sto.Email.Equals(model.Claims.Email)))
            {
                var student = await _studentRepository.GetMany(sto => sto.Email.Equals(model.Claims.Email))
                    .FirstOrDefaultAsync();
                if (student != null)
                {
                    return GetStudentAccessToken(student, StatusCodes.Status200OK);
                }
            }
            // Create student if the email does not exist in the system
            try
            {
                // Create a student according to the object returned from firebase
                Student student = _mapper.Map<FirebaseTokenModel, Student>(model);

                // Add missing information to the object before adding it to the database
                student.Id = Guid.NewGuid();
                student.Status = StudentStatuses.Active;

                // Save to database
                _studentRepository.Add(student);
                var result = await _unitOfWork.SaveChangesAsync();

                // Return
                if (result > 0)
                {
                    return GetStudentAccessToken(student, StatusCodes.Status201Created);
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

        private IActionResult GetManagerAccessToken(Manager manager, int statusCode)
        {
            var auth = _mapper.Map<AuthModel>(manager);
            auth.Role = UserRoles.Manager;
            if (auth != null)
            {
                var token = GenerateJwtToken(auth);
                return new ObjectResult(new AuthViewModel
                {
                    AccessToken = token,
                })
                {
                    StatusCode = statusCode
                };
            }
            return new ObjectResult(CustomErrors.UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }

        private async Task<IActionResult> GetOrCreateManager(FirebaseTokenModel model)
        {
            // Return manager information if email already exitsts
            if (_managerRepository.Any(sto => sto.Email.Equals(model.Claims.Email)))
            {
                var manager = await _managerRepository.GetMany(sto => sto.Email.Equals(model.Claims.Email))
                    .FirstOrDefaultAsync();
                if (manager != null)
                {
                    return GetManagerAccessToken(manager, StatusCodes.Status200OK);
                }

            }
            // Create manager if the email does not exist in the system
            try
            {
                // Create a manager according to the object returned from firebase
                Manager manager = _mapper.Map<FirebaseTokenModel, Manager>(model);

                // Add missing information to the object before adding it to the database
                manager.Id = Guid.NewGuid();
                manager.Status = ManagerStatuses.Active;

                // Save to database
                _managerRepository.Add(manager);
                var result = await _unitOfWork.SaveChangesAsync();

                // Return
                if (result > 0)
                {
                    return GetManagerAccessToken(manager, StatusCodes.Status201Created);
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

        public async Task<AuthModel> GetUser(Guid id)
        {
            try
            {
                // Find the user with manager role
                var manager = await _managerRepository.FirstOrDefaultAsync(sto => sto.Id == id);
                if (manager != null)
                {
                    return new AuthModel()
                    {
                        Email = manager.Email,
                        Id = manager.Id,
                        Role = UserRoles.Manager,
                        Status = manager.Status
                    };
                }

                // If the user is not a manager
                var student = await _studentRepository.FirstOrDefaultAsync(sto => sto.Id == id);
                if (student != null)
                {
                    return new AuthModel()
                    {
                        Email = student.Email,
                        Id = student.Id,
                        Role = UserRoles.Student,
                        Status = student.Status
                    };
                }
                return null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),
                    new Claim("role", auth.Role.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}