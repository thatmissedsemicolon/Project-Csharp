using PP.Library.Models;

namespace PP.CLI
{
    internal class Program
    {
        static List<Client> clients = new List<Client>();
        static List<Project> projects = new List<Project>();

        static void Main(string[] args)
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine("Select an option below: ");
                Console.WriteLine("1. Create Client");
                Console.WriteLine("2. Read Clients");
                Console.WriteLine("3. Update Client");
                Console.WriteLine("4. Delete Client");
                Console.WriteLine("5. Create Project");
                Console.WriteLine("6. Read Projects");
                Console.WriteLine("7. Update Project");
                Console.WriteLine("8. Delete Project");
                Console.WriteLine("9. Get Associated Client For A Specific Project");
                Console.WriteLine("0. Exit");

                try {
                    int selection = Convert.ToInt32(Console.ReadLine());

                    switch (selection)
                    {
                    default:
                        break;
                    case 1:
                        CreateClient();
                        break;
                    case 2:
                        ReadClients();
                        break;
                    case 3:
                        UpdateClient();
                        break;
                    case 4:
                        DeleteClient();
                        break;
                    case 5:
                        CreateProject();
                        break;
                    case 6:
                        ReadProjects();
                        break;
                    case 7:
                        UpdateProject();
                        break;
                    case 8:
                        DeleteProject();
                        break;
                    case 9:
                        GetClientIDForASpecificProject();
                        break;
                    case 0:
                        exit = true;
                        break;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static public void CreateClient()
        {
            try {
                Console.WriteLine("Enter Client ID: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Client Name: ");
                string? name = Console.ReadLine();

                DateTime openDate = DateTime.Now;
                DateTime closedDate = DateTime.MinValue;

                Console.WriteLine("Is the client active? (yes/no)");
                string? isClosedInput = Console.ReadLine();
                bool isClosed = isClosedInput?.ToLower() == "yes" ? true : false;

                Console.WriteLine("Enter any notes for the client: ");
                string? notes = Console.ReadLine();

                Client newClient = new Client()
                {
                    Id = id,
                    Name = name,
                    OpenDate = openDate,
                    ClosedDate = closedDate,
                    IsClosed = isClosed,
                    Notes = notes
                };

                clients.Add(newClient);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        static public void ReadClients()
        {
            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id}, Name: {client.Name}, IsActive: {client.IsClosed}, OpenDate: {client.OpenDate}, ClosedDate: {client.ClosedDate}, Notes: {client.Notes}");
            }
        }

        static public void UpdateClient()
        {
            try {
                Console.WriteLine("Enter the Client ID to update: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Client? clientToUpdate = clients.Find(c => c.Id == id);

                if (clientToUpdate != null)
                {
                    Console.WriteLine("Enter new Client Name: ");
                    clientToUpdate.Name = Console.ReadLine();

                    Console.WriteLine("Is the client active? (yes/no)");
                    string? isClosedInput = Console.ReadLine();
                    clientToUpdate.IsClosed = isClosedInput?.ToLower() == "yes" ? true : false;

                    Console.WriteLine("Enter any new notes for the client: ");
                    clientToUpdate.Notes = Console.ReadLine();

                    if((bool)!clientToUpdate.IsClosed)
                    {
                       clientToUpdate.ClosedDate = DateTime.Now;
                    }

                    Console.WriteLine("Client successfully updated!");
                }
                else
                {
                    Console.WriteLine("Client not found!");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        static public void DeleteClient()
        {
            try {
                Console.WriteLine("Enter the Client ID to delete: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Client? clientToDelete = clients.Find(c => c.Id == id);

                if (clientToDelete != null)
                {
                    clients.Remove(clientToDelete);
                    Console.WriteLine("Client sucessfully deleted!");
                }
                else
                {
                    Console.WriteLine("Client not found!");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        static public void CreateProject()
        {
            try {
                Console.WriteLine("Enter Project ID: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Short Name: ");
                string? shortName = Console.ReadLine();

                Console.WriteLine("Enter Long Name: ");
                string? longName = Console.ReadLine();

                Console.WriteLine("Enter Client ID: ");
                int clientId = Convert.ToInt32(Console.ReadLine());

                DateTime openDate = DateTime.Now;
                DateTime closedDate = DateTime.MinValue;

                Console.WriteLine("Is the project active? (yes/no)");
                string? isClosedInput = Console.ReadLine();
                bool isClosed = isClosedInput?.ToLower() == "yes" ? true : false;

                Project newProject = new Project()
                {
                    Id = id,
                    ShortName = shortName,
                    LongName = longName,
                    ClientId = clientId,
                    OpenDate = openDate,
                    ClosedDate = closedDate,
                    IsClosed = isClosed
                };

                projects.Add(newProject);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        static public void ReadProjects()
        {
            foreach (var project in projects)
            {
                Console.WriteLine($"ID: {project.Id}, Short Name: {project.ShortName}, Long Name: {project.LongName}, IsActive: {project.IsClosed}, OpenDate: {project.OpenDate}, ClosedDate: {project.ClosedDate}, ClientID: {project.ClientId}");
            }
        }

        static public void UpdateProject()
        {
            try {
                Console.WriteLine("Enter the Project ID to update: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Project? projectToUpdate = projects.Find(p => p.Id == id);

                if (projectToUpdate != null)
                {
                    Console.WriteLine("Enter new Short Name: ");
                    projectToUpdate.ShortName = Console.ReadLine();

                    Console.WriteLine("Enter new Long Name: ");
                    projectToUpdate.LongName = Console.ReadLine();

                    Console.WriteLine("Is the project active? (yes/no)");
                    string? isActiveInput = Console.ReadLine();
                    projectToUpdate.IsClosed = isActiveInput?.ToLower() == "yes" ? true : false;

                    if(!projectToUpdate.IsClosed)
                    {
                       projectToUpdate.ClosedDate = DateTime.Now;
                    }
                    else
                    {
                    Console.WriteLine("Project not found!");
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }  

        static public void DeleteProject()
        {
            try {
                Console.WriteLine("Enter the Project ID to delete: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Project? projectToDelete = projects.Find(p => p.Id == id);

                if (projectToDelete != null)
                {
                    projects.Remove(projectToDelete);
                    Console.WriteLine("Project successfully deleted!");
                }
                else
                {
                    Console.WriteLine("Project not found!");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }    

        static public void GetClientIDForASpecificProject()
        {
            try {
                Console.WriteLine("Enter the Project ID to find the associated Client: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Project? project = projects.Find(p => p.Id == id);

                if (project != null)
                {
                    Client? associatedClient = clients.Find(client => client.Id == project.ClientId);

                    if (associatedClient != null)
                    {
                       Console.WriteLine($"The client for project {project.Id} is {associatedClient.Name} and associated Client ID is: {associatedClient.Id}");
                    }
                    else
                    {
                       Console.WriteLine("No client is associated with this project!");
                    }
                }
                else
                {
                    Console.WriteLine("Project not found!");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}