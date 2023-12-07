//using Android.Locations;
using EnyoiProject.NVVM.Models;
using EnyoiProject.Responses;
using EnyoiProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using static Android.Util.EventLogTags;

namespace EnyoiProject.NVVM.ViewModels
{
    public class CreateProductViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        private readonly IApiService _apiService;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private string _name;
        private string _description;
        private string _price;
        private string _isActive;
        private string _category;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isToggled;

        public CreateProductViewModel(IApiService apiService, INavigation navigation)
        {
            _navigation = navigation;
            _apiService = apiService;
            LoadCategoriesAsync();
            _isEnabled = true;
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
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


        public string Description
        {
            get { return _description; }

            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Price
        {
            get { return _price; }

            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string IsActive
        {
            get { return _isActive; }

            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public string Category
        {
            get { return _category; }

            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
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

        public bool IsToggled
        {
            get { return _isToggled; }

            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    OnPropertyChanged(nameof(IsToggled));
                }
            }
        }

        public ICommand SaveProductCommand => new Command(async () => await ExecuteSaveProductCommand());
        private async Task ExecuteSaveProductCommand()
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

            Product product = new Product
            {
                Name = Name,
                Description = Description,
                Price = Price,
                isActive = IsToggled,
                categoryId = SelectedCategory.Id,
                category = new Category { Id = SelectedCategory.Id, Name = SelectedCategory.Name }
            };

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PostAsync(url, "/api", "/Products", product);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("OK", "Producto creado", "Aceptar");
            await _navigation.PopAsync();
        }

        private async Task<bool> ValidateDateAsync()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Nombre no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Descripción no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Price))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Precio no puede estar vacío", "Aceptar");
                return false;
            }
            if (string.IsNullOrEmpty(Price))
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Precio no puede estar vacío", "Aceptar");
                return false;
            }
            if (SelectedCategory==null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "El campo Categoría no puede estar vacío", "Aceptar");
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void LoadCategoriesAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No tiene internet, verifique sus datos", "Aceptar");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Category>(url, "/api", "/categories");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Categories = new ObservableCollection<Category>((List<Category>)response.Result);
        }
    }
}
