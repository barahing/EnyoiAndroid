using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CategoryView : ContentPage
{
    public CategoryView()
    {
        InitializeComponent();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IApiService, ApiService>()
            .BuildServiceProvider();

        var _apiService = serviceProvider.GetRequiredService<IApiService>();
        BindingContext = new CategoryViewModel(this.Navigation, _apiService);


    }


}