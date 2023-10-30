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
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class PlantService : BaseService, IPlantService
    {
        private readonly new IMapper _mapper;
        private readonly IPlantRepository _plantRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPlantCategoryRepository _plantCategoryRepository;
        private readonly ICloudStorageService _cloudStorageService;

        public PlantService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _plantRepository = unitOfWork.Plant;
            _plantCategoryRepository = unitOfWork.PlantCategory;
            _imageRepository = unitOfWork.Image;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> GetPlant(Guid id)
        {
            try
            {
                var plant = await _plantRepository.GetMany(pl => pl.Id.Equals(id))
                    .AsNoTracking()
                    .ProjectTo<PlantViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (plant == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(plant)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetPlants(PaginationRequestModel pagination, PlantFilterModel filter)
        {
            try
            {
                var query = _plantRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(pl => pl.Name.Contains(filter.Name));
                }
                if (filter.Code != null)
                {
                    query = query.Where(pl => pl.Name.Contains(filter.Code));
                }
                if (filter.Status != null)
                {
                    query = query.Where(pl => pl.Status.Equals(filter.Status));
                }
                if (filter.CategoryId != null)
                {
                    query = query.Where(pl => pl.PlantCategories.Any(pc => pc.CategoryId.Equals(filter.CategoryId)));
                }

                var totalRow = await query.AsNoTracking().CountAsync();
                var plants = await query.AsNoTracking()
                    .ProjectTo<PlantViewModel>(_mapper.ConfigurationProvider)
                    .Paginate(pagination)
                    .ToListAsync();

                return new ObjectResult(plants.ToPaged(pagination, totalRow))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> CreatePlant(PlantCreateModel model)
        {
            try
            {
                var id = Guid.NewGuid();
                var plant = _mapper.Map<Plant>(model);
                plant.Id = id;
                plant.Status = "Availabel";
                _unitOfWork.BeginTransaction();
                try
                {

                    _plantRepository.Add(plant);
                    var plantSaved = await _unitOfWork.SaveChangesAsync();
                    if (plantSaved > 0)
                    {
                        foreach (var plantCategoryId in model.CategoryIds)
                        {
                            var plantCategory = new PlantCategory
                            {
                                CategoryId = plantCategoryId,
                                PlantId = id
                            };
                            _plantCategoryRepository.Add(plantCategory);
                        }
                        var categoriesSaved = await _unitOfWork.SaveChangesAsync();
                        if (categoriesSaved > 0)
                        {
                            foreach (var image in model.Images)
                            {
                                var imageId = Guid.NewGuid();
                                var url = await _cloudStorageService.Upload(imageId, image.ContentType, image.OpenReadStream());
                                var plantImage = new Image
                                {
                                    Id = imageId,
                                    PlantId = id,
                                    Url = url
                                };
                                _imageRepository.Add(plantImage);
                            }
                            await _unitOfWork.SaveChangesAsync();
                        }
                        _unitOfWork.Commit();
                        return await GetPlant(id);
                    }
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
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
