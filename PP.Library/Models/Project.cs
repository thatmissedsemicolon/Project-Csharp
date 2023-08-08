using PP.Library.DTO;

namespace PP.Library.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsClosed { get; set; }
        public int ClientId { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }

        public List<Bill> Bills { get; set; }

        public Project()
        {
            Name = string.Empty;
            ShortName = string.Empty;
            LongName = string.Empty;
            Bills = new List<Bill>();
        }

        public Project(ProjectDTO dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name ?? string.Empty;
            this.Bills = dto.Bills.Select(b => new Bill(b)).ToList();
            this.IsClosed = dto.IsClosed;
            this.OpenDate = dto.OpenDate;
            this.ClosedDate = dto.ClosedDate;
            this.ShortName = dto.ShortName ?? string.Empty;
            this.LongName = dto.LongName ?? string.Empty;
        }
    }
}
