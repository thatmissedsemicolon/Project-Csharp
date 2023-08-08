using PP.API.Database;
using PP.Library.DTO;

namespace PP.API.EC
{
    public class ProjectEC
    {
        private readonly Filebase _filebase;

        public ProjectEC()
        {
            _filebase = Filebase.Current;
        }

        public ClientDTO AddProjectToClient(int clientId, ProjectDTO project)
        {
            var client = _filebase.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null) throw new Exception("Client not found");

            int maxId = _filebase.Clients
                        .SelectMany(c => c.Projects)
                        .DefaultIfEmpty()
                        .Max(p => p?.Id ?? 0);

            project.Id = maxId + 1;
            client.Projects.Add(project);
            return _filebase.UpdateClient(client);
        }

        public IEnumerable<ProjectDTO> GetAllProjects()
        {
            return _filebase.GetAllClients()
                            .SelectMany(client => client.Projects);
        }

        public ClientDTO UpdateProject(int clientId, ProjectDTO projectToUpdate)
        {
            var client = _filebase.Clients.FirstOrDefault(c => c.Projects.Any(p => p.Id == clientId));
            if (client == null) throw new Exception("Client not found");

            var project = client.Projects.FirstOrDefault(p => p.Id == projectToUpdate.Id);
            if (project == null) throw new Exception("Project not found");

            project.Name = projectToUpdate.Name;
            project.IsClosed = projectToUpdate.IsClosed;
            project.ClosedDate = DateTime.Now;

            _filebase.UpdateClient(client);

            return client;
        }

        public bool DeleteById(int projectId)
        {
            foreach (var client in _filebase.Clients)
            {
                var project = client.Projects.FirstOrDefault(p => p.Id == projectId);
                if (project != null)
                {
                    client.Projects.Remove(project);
                    _filebase.UpdateClient(client);
                    return true;
                }
            }

            return false;
        }

        public ProjectDTO GetProjectById(int projectId)
        {
            var clients = _filebase.GetAllClients();
            foreach (var client in clients)
            {
                var project = client.Projects.FirstOrDefault(p => p.Id == projectId);
                if (project != null)
                {
                    return project;
                }
            }
            throw new Exception("Project not found");
        }

        public IEnumerable<ProjectDTO> Search(string query)
        {
            return GetAllProjects()
                   .Where(p => p.Name.ToUpper().Contains(query.ToUpper()))
                   .Take(1000);
        }
    }
}
