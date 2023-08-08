using Microsoft.AspNetCore.Mvc;
using PP.API.EC;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ProjectDTO> Get(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new ProjectEC().GetAllProjects();
            }
            else
            {
                return new ProjectEC().Search(query);
            }
        }

        [HttpPost("/Project/add/{clientId}")]
        public ClientDTO AddProjectToClient(int clientId, [FromBody] ProjectDTO project)
        {
            return new ProjectEC().AddProjectToClient(clientId, project);
        }

        [HttpPost("/Project/update/{clientId}")]
        public ClientDTO UpdateProjectInClient(int clientId, [FromBody] ProjectDTO project)
        {
            return new ProjectEC().UpdateProject(clientId, project);
        }

        [HttpGet("/Project/{id}")]
        public ProjectDTO GetProjectById(int id)
        {
            return new ProjectEC().GetProjectById(id);
        }

        [HttpDelete("/Project/delete/{projectId}")]
        public bool DeleteProject(int projectId)
        {
            return new ProjectEC().DeleteById(projectId);
        }

        [HttpPost("/Project/search")]
        public IEnumerable<ProjectDTO> SearchProjects([FromBody] QueryMessage query)
        {
            return new ProjectEC().Search(query.Query);
        }
    }
}
