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
    public class CreateCategoryViewModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _isRunning;
        private bool _isEnabled;
        private readonly IApiService _apiService;
        private readonly INavigation _navigation;

        public CreateCategoryViewModel(IApiService apiService, INavigation navigation)
        {
            _apiService = apiService;
            _navigation = navigation;
            _isEnabled = true;
        }

        public string Name
        {
            get { return _name; }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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

        public ICommand SaveCategoryCommand => new Command(async () => await ExecuteSaveCategoryCommand());
        private async Task ExecuteSaveCategoryCommand()
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

            Category category = new Category
            {
                Name = Name
            };

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PostAsync(url, "/api", "/categories", category);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("OK", "Categoría creada", "Aceptar");
            await _navigation.PopAsync();
        }

        private async Task<bool> ValidateDateAsync()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Nombre no puede estar vacío", "Aceptar");
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
