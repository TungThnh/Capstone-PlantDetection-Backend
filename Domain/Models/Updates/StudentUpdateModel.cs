using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Updates
{
    public class StudentUpdateModel
    {
        public string? FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string? College { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? DayOfBirth { get; set; }
    }
}
