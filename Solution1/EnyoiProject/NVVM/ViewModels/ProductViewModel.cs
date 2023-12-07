using EnyoiProject.NVVM.Models;
using EnyoiProject.NVVM.Views;
using EnyoiProject.Responses;
using EnyoiProject.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using Windows.Services.Maps;


namespace EnyoiProject.NVVM.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private ObservableCollection<Product> _products;
        private readonly IApiService _apiService;
        private static ProductViewModel _instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public ProductViewModel(INavigation navigation, IApiService apiService)
        {
            _instance = this;
            _navigation = navigation;
            _apiService = apiService;
            LoadProductAsync();

        }

        public ObservableCollection<Product> Inventory
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Inventory));
                }
            }
        }

        public ICommand NewProductCommand => new Command(async () => await ExecuteNewProductCommand());

        public static ProductViewModel GetInstance()
        {
            return _instance;
        }
        private async Task ExecuteNewProductCommand()
        {
            await _navigation.PushAsync(new CreateProductView());
        }
        private async void LoadProductAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Product>(url, "/api", "/products");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Inventory = new ObservableCollection<Product>((List<Product>)response.Result);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
