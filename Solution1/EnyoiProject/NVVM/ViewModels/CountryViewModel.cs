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
    public class CountryViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private ObservableCollection<Country> _countries;
        private readonly IApiService _apiService;
        private static CountryViewModel _instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public CountryViewModel(INavigation navigation, IApiService apiService)
        {
            _instance = this;
            _navigation = navigation;
            _apiService = apiService;
            LoadCountryAsync();
        }

        public ObservableCollection<Country> Nations
        {
            get { return _countries; }
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    OnPropertyChanged(nameof(Nations));
                }
            }
        }

        public ICommand NewCountryCommand => new Command(async () => await ExecuteNewCountryCommand());

        public static CountryViewModel GetInstance()
        {
            return _instance;
        }
        private async Task ExecuteNewCountryCommand()
        {
            await _navigation.PushAsync(new CreateCountryView());
        }
        private async void LoadCountryAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No tiene internet, verifique sus datos", "Aceptar");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Country>(url, "/api", "/countries");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Nations = new ObservableCollection<Country>((List<Country>)response.Result);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
