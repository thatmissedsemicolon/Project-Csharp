using PP.API.Database;
using PP.Library.DTO;

namespace PP.API.EC
{
    public class ClientEC
    {
        private readonly Filebase _filebase;

        public ClientEC()
        {
            _filebase = Filebase.Current;
        }

        public ClientDTO Add(ClientDTO client)
        {
            return _filebase.AddClient(client);
        }

        public IEnumerable<ClientDTO> GetAllClients()
        {
            return _filebase.GetAllClients();
        }

        public ClientDTO Update(ClientDTO clientToUpdate)
        {
            var existingClient = _filebase.Clients.FirstOrDefault(c => c.Id == clientToUpdate.Id);

            if (existingClient == null)
            {
                return null;
            }

            existingClient.Name = string.IsNullOrWhiteSpace(clientToUpdate.Name) ? existingClient.Name : clientToUpdate.Name;

            if (existingClient.Projects == null || existingClient.Projects.All(p => p.IsClosed))
            {
                existingClient.IsClosed = clientToUpdate.IsClosed ?? existingClient.IsClosed;
                existingClient.ClosedDate = DateTime.Now;
                existingClient.Notes = clientToUpdate.Notes;
            }
            else
            {
                if (clientToUpdate.IsClosed.HasValue && clientToUpdate.IsClosed.Value != existingClient.IsClosed)
                {
                    throw new Exception("Cannot update IsClosed as not all projects are closed");
                }
                else if (!clientToUpdate.IsClosed.HasValue)
                {
                    existingClient.IsClosed = existingClient.IsClosed;
                    existingClient.ClosedDate = DateTime.Now;
                    existingClient.Notes = clientToUpdate.Notes;
                }
            }

            return Add(existingClient);
        }

        public bool DeleteById(string id)
        {
            var client = _filebase.Clients.FirstOrDefault(c => c.Id == Convert.ToInt32(id));

            if (client != null)
            {
                if (client.Projects == null || client.Projects.All(p => p.IsClosed))
                {
                    if (_filebase.DeleteClient(id))
                    {
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Cannot delete client because they have open projects.");
                }
            }

            throw new Exception("Client not found");
        }

        public IEnumerable<ClientDTO> GetById(int id)
        {
            var clients = _filebase.Clients.Where(c => c.Id == Convert.ToInt32(id));

            return clients;
        }

        public IEnumerable<ClientDTO> Search(string query)
        {
            return _filebase.Clients.Where(c => c.Name.ToUpper().Contains(query.ToUpper())).Take(1000);
        }
    }
}
