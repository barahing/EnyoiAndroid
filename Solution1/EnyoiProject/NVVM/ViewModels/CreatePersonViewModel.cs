using EnyoiProject.NVVM.Models;
using EnyoiProject.Responses;
using EnyoiProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace EnyoiProject.NVVM.ViewModels
{
    public class CreatePersonViewModel : INotifyPropertyChanged
    {
        private string _document;
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;
        private bool _isRunning;
        private bool _isEnabled;
        private string _addres;
        private readonly IApiService _apiService;
        private readonly INavigation _navigation;

        public CreatePersonViewModel(IApiService apiService, INavigation navigation)
        {
            _apiService = apiService;
            _navigation = navigation;
            _isEnabled = true;
        }

        public string Document 
        {
            get { return _document; }
            
            set
            {
                if (_document !=value)
                {
                    _document = value;
                    OnPropertyChanged(nameof(Document));
                }
            }
        }


        public string FirstName
        {
            get { return _firstName; }

            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }

            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string Addres
        {
            get { return _addres; }

            set
            {
                if (_addres != value)
                {
                    _addres = value;
                    OnPropertyChanged(nameof(Addres));
                }
            }
        }

        public string Phone
        {
            get { return _phone; }

            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public string Email
        {
            get { return _email; }

            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }

            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }

            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public ICommand SavePersonCommand => new Command(async () => await ExecuteSavePersonCommand());
        private async Task ExecuteSavePersonCommand()
        {
            bool isValid = await ValidateDateAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "No tienes conexión a Internet, por favor enciende el acceso a Internet de tu móvil", "Aceptar");
                return;
            }

            Person person = new Person 
            { 
                Document = Document,
                FirstName = FirstName,
                LastName = LastName,
                Addres = Addres,
                Phone = Phone,
                Email = Email
            };

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PostAsync(url,"/api", "/People", person);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("OK", "Persona creada", "Aceptar");
        }

        private async Task<bool> ValidateDateAsync()
        {
            if (string.IsNullOrEmpty(Document))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Documento no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Nombres no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Apellidos no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Addres))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Dirección no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Teléfono no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Email no puede estar vacío", "Aceptar");
                return false;
            }

            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
