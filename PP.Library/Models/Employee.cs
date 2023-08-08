using PP.Library.DTO;

namespace PP.Library.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Rate { get; set; }

        public List<Time> TimeRecords { get; set; }

        public Employee()
        {
            Name = string.Empty;
            TimeRecords = new List<Time>();
        }

        public Employee(EmployeeDTO dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name ?? string.Empty;
            this.Rate = dto.Rate;
            this.TimeRecords = dto.TimeRecords.Select(t => new Time(t)).ToList(); ;
        }
    }
}
