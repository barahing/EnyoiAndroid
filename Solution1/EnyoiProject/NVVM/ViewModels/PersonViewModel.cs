﻿using EnyoiProject.NVVM.Models;
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
    public class PersonViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private ObservableCollection<Person> _people;
        private readonly IApiService _apiService;
        private static PersonViewModel _instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public PersonViewModel(INavigation navigation, IApiService apiService)
        {
            _instance = this;
            _navigation = navigation;
            _apiService = apiService;
            LoadPersonAsync();
        }

        public ObservableCollection<Person> People
        {
            get { return _people; }
            set
            {
                if (_people != value)
                {
                    _people = value;
                    OnPropertyChanged(nameof(People));
                }
            }
        }

        public ICommand NewPersonCommand => new Command(async () => await ExecuteNewPersonCommand());

        public static PersonViewModel GetInstance()
        {
            return _instance;
        }
        private async Task ExecuteNewPersonCommand()
        {
            await _navigation.PushAsync(new CreatePersonView());
        }
        private async void LoadPersonAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No tiene internet, verifique sus datos", "Aceptar");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Person>(url, "/api", "/people");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            People = new ObservableCollection<Person>((List<Person>)response.Result);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
