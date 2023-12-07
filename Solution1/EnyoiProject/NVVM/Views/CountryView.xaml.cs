using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CountryView : ContentPage
{
    public CountryView()
    {
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();

        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new CountryViewModel(this.Navigation, _apiService);

    }


}