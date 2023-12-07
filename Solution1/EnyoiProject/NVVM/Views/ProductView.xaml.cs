using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class ProductView : ContentPage
{
    public ProductView()
    {
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();

        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new ProductViewModel(this.Navigation, _apiService);

    }


}