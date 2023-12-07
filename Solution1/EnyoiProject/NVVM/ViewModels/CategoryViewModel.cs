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
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private ObservableCollection<Category> _categories;
        private readonly IApiService _apiService;
        private static CategoryViewModel _instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public CategoryViewModel(INavigation navigation, IApiService apiService)
        {
            _instance = this;
            _navigation = navigation;
            _apiService = apiService;
            LoadCategoryAsync();
        }

        public ObservableCollection<Category> CategoryList
        {
            get { return _categories; }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(CategoryList));
                }
            }
        }

        public ICommand NewCategoryCommand => new Command(async () => await ExecuteNewCategoryCommand());

        public static CategoryViewModel GetInstance()
        {
            return _instance;
        }
        private async Task ExecuteNewCategoryCommand()
        {
            await _navigation.PushAsync(new CreateCategoryView());
        }
        private async void LoadCategoryAsync()
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

            CategoryList = new ObservableCollection<Category>((List<Category>)response.Result);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
