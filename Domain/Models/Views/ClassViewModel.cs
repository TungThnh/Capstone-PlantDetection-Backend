using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.Views
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public string? Note { get; set; }

        public string ThumbnailUrl { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public int NumberOfMember { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<ClassLabelViewModel> ClassLabels { get; set; } = new List<ClassLabelViewModel>();

        public ManagerViewModel Manager { get; set; } = null!;
    }
}
