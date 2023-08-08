using PP.Library.Models;

namespace PP.Library.DTO
{
	public class ProjectDTO
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsClosed { get; set; }
        public int ClientId { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }

        public List<BillDTO> Bills { get; set; }

        public ProjectDTO()
        {
            Name = string.Empty;
            ShortName = string.Empty;
            LongName = string.Empty;
            Bills = new List<BillDTO>();
        }

        public ProjectDTO(Project p)
        {
            this.Id = p.Id;
            this.Name = p.Name ?? string.Empty;
            this.Bills = p.Bills.Select(b => new BillDTO(b)).ToList();
            this.IsClosed = p.IsClosed;
            this.OpenDate = p.OpenDate;
            this.ClosedDate = p.ClosedDate;
            this.ShortName = p.ShortName ?? string.Empty;
            this.LongName = p.LongName ?? string.Empty;
        }
    }
}

