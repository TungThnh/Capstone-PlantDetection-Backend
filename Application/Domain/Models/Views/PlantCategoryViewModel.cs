namespace Domain.Models.Views
{
    public class PlantCategoryViewModel
    {
        public string? Description { get; set; }

        public CategoryViewModel Category { get; set; } = null!;
    }
}
