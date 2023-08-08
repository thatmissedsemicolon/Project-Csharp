using PP.Library.Models;

namespace PP.Library.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectDTO> Projects { get; set; }
        public bool? IsClosed { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string? Notes { get; set; }

        public ClientDTO()
        {
            Name = string.Empty;
            Notes = string.Empty;
            IsClosed = null;
            Projects = new List<ProjectDTO>();
        }

        public ClientDTO(Client c)
        {
            this.Id = c.Id;
            this.Name = c.Name ?? string.Empty;
            this.Projects = c.Projects.Select(p => new ProjectDTO(p)).ToList();
            this.IsClosed = c.IsClosed;
            this.OpenDate = c.OpenDate;
            this.ClosedDate = c.ClosedDate;
            this.Notes = c.Notes;
        }
    }
}
