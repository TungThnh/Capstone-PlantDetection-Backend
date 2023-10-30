using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Views
{
    public class EstimateViewModel
    {
        public PlantViewModel Plant { get; set; } = null!;

        public double Confidence { get; set; }
    }
}
