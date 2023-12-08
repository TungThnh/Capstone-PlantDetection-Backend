﻿using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class PlantCreateModel
    {
        public string Name { get; set; } = null!;

        public ICollection<PlantCategoryCreateModel> PlantCategories { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public string? LivingCondition { get; set; }

        public string? DistributionArea { get; set; }

        public string? Ph { get; set; }

        public string? Uses { get; set; }

        public string? ScienceName { get; set; }

        public string? FruitTime { get; set; }

        public string? ConservationStatus { get; set; }

        public string? Size { get; set; }

        public string? Discoverer { get; set; }

        public string? Genus { get; set; }

        public string? Species { get; set; }

        public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
