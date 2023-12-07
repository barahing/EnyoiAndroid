using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CreateCategoryView : ContentPage
{
    public CreateCategoryView()
    {
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();
        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new CreateCategoryViewModel(_apiService, this.Navigation);
    }
}