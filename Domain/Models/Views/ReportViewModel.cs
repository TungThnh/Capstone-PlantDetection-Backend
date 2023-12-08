using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class ReportViewModel
    {
        public Guid Id { get; set; }

        public StudentViewModel Student { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public LabelViewModel Label { get; set; } = null!;

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public string? Note { get; set; } = null!;

        public ClassViewModel Class { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
