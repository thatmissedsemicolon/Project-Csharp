using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PP.MAUI.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ClientService clientService;
        private ClientDTO clientDTOModel;
        public Client clientModel { get; set; }
        private ClientDTO selectedClient;
        private ObservableCollection<ClientDTO> clients;

        public ClientViewModel()
        {
            clientService = ClientService.Instance;
            clientDTOModel = new ClientDTO();
            clientModel = new Client(clientDTOModel);
            AddClientCommand = new Command(async () => await AddClient());
            UpdateClientCommand = new Command(async () => await UpdateClient(), () => SelectedClient != null);
            DeleteClientCommand = new Command(async () => await DeleteClient(), () => SelectedClient != null);
            ClientDetailCommand = new Command(ClientDetail, () => SelectedClient != null);
            LoadClientsCommand = new Command(async () => await LoadClients());
            //SearchCommand = new Command<string>(async (query) => await Search(query));
        }

        public async Task InitializeAsync()
        {
            Clients = new ObservableCollection<ClientDTO>(await clientService.GetAllClients());
        }

        public ObservableCollection<ClientDTO> Clients
        {
            get => clients;
            set
            {
                clients = value;
                OnPropertyChanged();
            }
        }

        public ClientDTO clientDTO
        {
            get => clientDTOModel;
            set
            {
                clientDTOModel = value;
                OnPropertyChanged();
            }
        }

        public ClientDTO SelectedClient
        {
            get => selectedClient;
            set
            {
                if (selectedClient != value)
                {
                    selectedClient = value;
                    OnPropertyChanged();
                    ((Command)UpdateClientCommand).ChangeCanExecute();
                    ((Command)DeleteClientCommand).ChangeCanExecute();
                    ((Command)ClientDetailCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand AddClientCommand { get; private set; }
        public ICommand UpdateClientCommand { get; private set; }
        public ICommand DeleteClientCommand { get; private set; }
        public ICommand LoadClientsCommand { get; private set; }
        public ICommand ClientDetailCommand { get; private set; }
        //public ICommand SearchCommand { get; set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Task.Run(() => Search(_searchText));
            }
        }

        public async Task Search(string searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchResults = await clientService.SearchClients(searchText);
                Clients = new ObservableCollection<ClientDTO>(searchResults);
            }
            else
            {
                await LoadClients();
            }
        }

        private async Task AddClient()
        {
            if (!string.IsNullOrEmpty(clientModel.Name))
            {
                try
                {
                    ClientDTO newClient = new ClientDTO
                    {
                        Name = clientModel.Name,
                        IsClosed = clientModel.IsClosed ?? false,
                        OpenDate = DateTime.Now,
                        Notes = clientModel.Notes
                    };

                    await clientService.AddClient(newClient);
                    await LoadClients();

                    clientDTO = new ClientDTO
                    {
                        IsClosed = false
                    };

                    clientModel = new Client(clientDTOModel);

                    SelectedClient = null;
                    OnPropertyChanged(nameof(clientDTO));
                    OnPropertyChanged(nameof(clientModel));
                }
                catch (Exception e)
                {
                    await LoadClients();
                    await Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                await LoadClients();
                await Shell.Current.DisplayAlert("Error", "Client name cannot be empty", "OK");
            }
        }

        private async Task UpdateClient()
        {
            try
            {
                ClientDTO newClient = new ClientDTO
                {
                    Id = selectedClient.Id,
                    Name = clientModel.Name,
                    IsClosed = clientModel.IsClosed ?? selectedClient.IsClosed,
                    Notes = !string.IsNullOrEmpty(clientModel.Notes) ? clientModel.Notes : selectedClient.Notes
                };

                await clientService.UpdateClient(newClient);
                await LoadClients();

                clientDTO = new ClientDTO
                {
                    IsClosed = false
                };

                clientModel = new Client(clientDTOModel);

                SelectedClient = null;
                OnPropertyChanged(nameof(clientDTO));
                OnPropertyChanged(nameof(clientModel));
            }
            catch (Exception e)
            {
                await LoadClients();
                await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            }
        }

        private async Task DeleteClient()
        { 
            try
            {
                await clientService.DeleteClient(SelectedClient.Id.ToString());
                await LoadClients();

                clientDTO = new ClientDTO();

                clientModel = new Client(clientDTOModel);

                SelectedClient = null;
                OnPropertyChanged(nameof(clientDTO));
                OnPropertyChanged(nameof(clientModel));
            }
            catch (Exception e)
            {
                await LoadClients();
                await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            }
        }

        private void ClientDetail()
        {
            if (SelectedClient != null)
            {
                Shell.Current.GoToAsync($"//ClientDetails?clientId={SelectedClient.Id}");
                clientDTO = new ClientDTO();
                clientModel = new Client(clientDTOModel);

                SelectedClient = null;
                OnPropertyChanged(nameof(clientDTO));
                OnPropertyChanged(nameof(clientModel));
            }
        }

        private async Task LoadClients()
        {
            SelectedClient = null;
            clientDTO = new ClientDTO();
            clientModel = new Client(clientDTOModel);

            OnPropertyChanged(nameof(clientDTO));
            OnPropertyChanged(nameof(clientModel));
            Clients = new ObservableCollection<ClientDTO>(await clientService.GetAllClients());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
