using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.MAUI.ViewModels
{
    public class ClientDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ClientService clientService;
        private ClientDTO clientDTOModel;
        private Client clientModel;
        private int clientId;

        public Client ClientModel
        {
            get => clientModel;
            set
            {
                if (clientModel != value)
                {
                    clientModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClientDTO ClientDTOModel
        {
            get => clientDTOModel;
            set
            {
                if (clientDTOModel != value)
                {
                    clientDTOModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ClientId
        {
            get => clientId;
            set
            {
                if (clientId != value)
                {
                    clientId = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClientDetailViewModel()
        {
            clientService = ClientService.Instance;
            clientDTOModel = new ClientDTO();
            clientModel = new Client(clientDTOModel);
        }

        public async Task GetClient()
        {
            ClientDTOModel = null;
            ClientModel = null;

            try
            {
                var clientDetails = await clientService.GetClientById(ClientId);

                if (clientDetails != null)
                {
                    ClientDTOModel = clientDetails;
                    ClientModel = new Client(ClientDTOModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while fetching client details: {e}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
