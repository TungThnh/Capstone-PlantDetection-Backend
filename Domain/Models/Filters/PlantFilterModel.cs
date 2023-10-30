namespace Domain.Models.Filters
{
    public class PlantFilterModel
    {
        public Guid? CategoryId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Status { get; set; }
    }
}
