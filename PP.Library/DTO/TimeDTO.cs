using PP.Library.Models;
		
namespace PP.Library.DTO
{
	public class TimeDTO
	{
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Narrative { get; set; }
        public decimal Hours { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string? ProjectName { get; set; }

        public TimeDTO()
		{
            Narrative = string.Empty;
            ProjectName = string.Empty;
        }

        public TimeDTO(Time t)
        {
            this.Id = t.Id;
            this.Date = t.Date;
            this.Narrative = t.Narrative;
            this.Hours = t.Hours;
            this.ProjectId = t.ProjectId;
            this.EmployeeId = t.EmployeeId;
            this.ProjectName = t.ProjectName;
        }
    }
}

