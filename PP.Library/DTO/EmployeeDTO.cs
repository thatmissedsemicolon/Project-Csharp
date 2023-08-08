using PP.Library.Models;

namespace PP.Library.DTO
{
	public class EmployeeDTO
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Rate { get; set; }

        public List<TimeDTO> TimeRecords { get; set; }

        public EmployeeDTO()
        {
            Name = string.Empty;
            TimeRecords = new List<TimeDTO>();
        }

        public EmployeeDTO(Employee e)
        {
            this.Id = e.Id;
            this.Name = e.Name ?? string.Empty;
            this.Rate = e.Rate;
            this.TimeRecords = e.TimeRecords.Select(t => new TimeDTO(t)).ToList(); ;
        }
    }
}

