using PP.Library.DTO;

namespace PP.Library.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Project> Projects { get; set; }
        public bool? IsClosed { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string? Notes { get; set; }


        public Client(ClientDTO dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Projects = dto.Projects.Select(p => new Project(p)).ToList();
            this.IsClosed = dto.IsClosed;
            this.OpenDate = dto.OpenDate;
            this.ClosedDate = dto.ClosedDate;
            this.Notes = dto.Notes;
        }

        public Client()
        {
            Name = string.Empty;
            Notes = string.Empty;
            IsClosed = null;
            Projects = new List<Project>();
        }
    }
}
