using Newtonsoft.Json;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.Library.Services
{
    public class ClientService
    {
        private static ClientService? _instance;
        public static ClientService Instance => _instance ??= new ClientService();

        //private List<Client> Clients { get; set; }

        private List<ClientDTO> clients;
        public List<ClientDTO> Clients
        {
            get
            {
                return clients ?? new List<ClientDTO>();
            }
        }

        private ClientService()
        {
            var response = new WebRequestHandler()
                    .Get("/Client")
                    .Result;

            clients = JsonConvert
                .DeserializeObject<List<ClientDTO>>(response)
                ?? new List<ClientDTO>();
        }

        public async Task AddClient(ClientDTO client)
        {
            if (client != null)
            {
                var response = new WebRequestHandler()
                     .Post("/Client/add", client)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdClient = JsonConvert.DeserializeObject<ClientDTO>(response);
                        clients.Add(createdClient);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public async Task<List<ClientDTO>> GetAllClients()
        {
            string response = await new WebRequestHandler().Get("/Client");

            if (response != null)
            {
                clients = JsonConvert.DeserializeObject<List<ClientDTO>>(response);
                Clients.Sort((client1, client2) => client1.Id.CompareTo(client2.Id));
                return Clients;
            }

            else
            {
                return new List<ClientDTO>();
            }
        }

        public async Task UpdateClient(ClientDTO client)
        {
            var response = new WebRequestHandler()
                        .Post("/Client/update", client)
                        .Result;

            if (response == "ERROR")
            {
                throw new Exception("Error occurred while updating the client.");
            }
        }

        public async Task DeleteClient(string id)
        {
            string result = await new WebRequestHandler().Delete($"/Client/delete/{id}");

            if (result == "ERROR")
            {
                throw new Exception("Failed to delete the client on the server.");
            }
        }

        public async Task<List<ClientDTO>> SearchClients(string queryText)
        {
            QueryMessage queryMessage = new QueryMessage
            {
                Query = queryText
            };

            string result = await new WebRequestHandler().Post($"/Client/search", queryMessage);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<List<ClientDTO>>(result);
            }

            return null;
        }

        public async Task<ClientDTO> GetClientById(int id)
        {
            string result = await new WebRequestHandler().Get($"/Client/{id}");

            if (!string.IsNullOrEmpty(result))
            {
                var clients = JsonConvert.DeserializeObject<List<ClientDTO>>(result);

                return clients.FirstOrDefault();
            }

            return null;
        }

        public async Task AddProjectToClient(int clientId, ProjectDTO project)
        {
            if (project != null)
            {
                var response = new WebRequestHandler()
                     .Post($"/Project/add/{clientId}", project)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdProject = JsonConvert.DeserializeObject<ClientDTO>(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        public async Task UpdateProjectInClient(int clientId, ProjectDTO project)
        {
            if (project != null)
            {
                var response = new WebRequestHandler()
                     .Post($"/Project/update/{clientId}", project)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdProject = JsonConvert.DeserializeObject<ClientDTO>(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        public async Task DeleteProjectFromClient(int projectId)
        {
            if (projectId != null)
            {
                var response = new WebRequestHandler()
                     .Delete($"/Project/delete/{projectId}")
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdProject = JsonConvert.DeserializeObject<ClientDTO>(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        public async Task<ProjectDTO> GetProject(int projectId)
        {
            string result = await new WebRequestHandler().Get($"/Project/{projectId}");

            if (!string.IsNullOrEmpty(result))
            {
                var projects = JsonConvert.DeserializeObject<List<ProjectDTO>>(result);
                return projects.FirstOrDefault();
            }

            return null;
        }

        public async Task<List<ProjectDTO>> SearchProjects(string name)
        {
            QueryMessage queryMessage = new QueryMessage
            {
                Query = name
            };

            string result = await new WebRequestHandler().Post($"/Project/search", queryMessage);

            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<List<ProjectDTO>>(result);
            }

            return null;
        }

        public async Task<List<ProjectDTO>> GetAllProjects()
        {
            string response = await new WebRequestHandler().Get("/Project");

            if (response != null)
            {
                var projects = JsonConvert.DeserializeObject<List<ProjectDTO>>(response);
                projects.Sort((project1, project2) => project1.Id.CompareTo(project2.Id));
                return projects;
            }

            else
            {
                return new List<ProjectDTO>();
            }
        }
    }
}
