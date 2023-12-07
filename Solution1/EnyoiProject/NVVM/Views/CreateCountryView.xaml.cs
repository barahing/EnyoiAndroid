using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CreateCountryView : ContentPage
{
    public CreateCountryView()
    {
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();
        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new CreateCountryViewModel(_apiService, this.Navigation);
    }
}