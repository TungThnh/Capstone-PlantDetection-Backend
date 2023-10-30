using AutoMapper;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Firebase;
using Domain.Models.Systems;
using Domain.Models.Views;
using Common.Constants;
using Domain.Models.Updates;
using Domain.Specifications;

namespace Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student
            CreateMap<Student, AuthModel>();

            CreateMap<Student, StudentViewModel>();

            CreateMap<FirebaseTokenModel, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Claims.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                (src.Claims != null && !string.IsNullOrEmpty(src.Claims.Name))
                    ? (src.Claims.Name.LastIndexOf(' ') != -1
                        ? src.Claims.Name.Substring(src.Claims.Name.LastIndexOf(' ') + 1)
                        : src.Claims.Name)
                    : ""))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                (src.Claims != null && !string.IsNullOrEmpty(src.Claims.Name))
                   ? (src.Claims.Name.LastIndexOf(' ') != -1
                       ? src.Claims.Name.Substring(0, src.Claims.Name.LastIndexOf(' '))
                       : src.Claims.Name) : ""))
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Claims.Picture));

            CreateMap<StudentUpdateModel, Student>()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Manager
            CreateMap<Manager, AuthModel>();

            CreateMap<Manager, ManagerViewModel>();

            CreateMap<FirebaseTokenModel, Manager>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Claims.Email))
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                 (src.Claims != null && !string.IsNullOrEmpty(src.Claims.Name))
                     ? (src.Claims.Name.LastIndexOf(' ') != -1
                         ? src.Claims.Name.Substring(src.Claims.Name.LastIndexOf(' ') + 1)
                         : src.Claims.Name) : ""))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                 (src.Claims != null && !string.IsNullOrEmpty(src.Claims.Name))
                    ? (src.Claims.Name.LastIndexOf(' ') != -1
                        ? src.Claims.Name.Substring(0, src.Claims.Name.LastIndexOf(' '))
                        : src.Claims.Name) : ""))
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Claims.Picture));

            // Class
            CreateMap<Class, ClassViewModel>();

            CreateMap<ClassUpdateModel, Class>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) => srcMember != null));

            CreateMap<ClassCreateModel, Class>();

            // Student Class
            CreateMap<StudentClass, StudentClassViewModel>();

            CreateMap<StudentClassViewModel, StudentClass>();

            CreateMap<InviteStudentModel, StudentClass>();

            // Image
            CreateMap<Image, ImageViewModel>();

            // Category
            CreateMap<Category, CategoryViewModel>();

            // Plant Category
            CreateMap<PlantCategory, PlantCategoryViewModel>();

            // Plant
            CreateMap<Plant, PlantViewModel>();
            CreateMap<PlantCreateModel, Plant>()
                               .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}