using Domain.Models.Systems;

namespace Domain.Models.Views
{
    public class ResultViewModel
    {
        public Guid Id { get; set; }

        public PlantViewModel? Plant { get; set; }

        public string? Message { get; set; }

        public DateTime CreateAt { get; set; }

        public ICollection<EstimateViewModel> Estimate { get; set; } = new List<EstimateViewModel>();
    }
}
