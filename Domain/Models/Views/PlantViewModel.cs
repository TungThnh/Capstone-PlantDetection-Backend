namespace Domain.Models.Views
{
    public class PlantViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Code { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public virtual ICollection<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

        public virtual ICollection<PlantCategoryViewModel> PlantCategories { get; set; } = new List<PlantCategoryViewModel>();
    }
}
