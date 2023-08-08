using PP.Library.DTO;

namespace PP.Library.Models
{
    public class Time
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Narrative { get; set; }
        public decimal Hours { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string? ProjectName { get; set; }

        public Time()
        {
            Narrative = string.Empty;
            ProjectName = string.Empty;
        }

        public Time(TimeDTO dto)
        {
            this.Id = dto.Id;
            this.Date = dto.Date;
            this.Narrative = dto.Narrative;
            this.Hours = dto.Hours;
            this.ProjectId = dto.ProjectId;
            this.EmployeeId = dto.EmployeeId;
            this.ProjectName = dto.ProjectName;
        }
    }
}