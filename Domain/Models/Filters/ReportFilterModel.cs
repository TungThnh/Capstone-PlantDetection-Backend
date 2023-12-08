namespace Domain.Models.Filters
{
    public class ReportFilterModel
    {
        public string? Status { get; set; }
        public string? Name { get; set; }
        public bool? Latest { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? LabelId { get; set; }
    }
}
