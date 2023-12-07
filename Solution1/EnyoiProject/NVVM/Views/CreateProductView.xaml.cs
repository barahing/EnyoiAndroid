using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CreateProductView : ContentPage
{
	public CreateProductView()
	{
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();
        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new CreateProductViewModel(_apiService, this.Navigation);
    }
}