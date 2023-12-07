using EnyoiProject.NVVM.ViewModels;
using EnyoiProject.Services;

namespace EnyoiProject.NVVM.Views;

public partial class CreatePersonView : ContentPage
{
	public CreatePersonView()
	{
		InitializeComponent();
		var serviceProvider = new ServiceCollection()
			.AddSingleton<IApiService, ApiService > ()
			.BuildServiceProvider();		
		var _apiService = serviceProvider.GetRequiredService<IApiService>();
		BindingContext = new CreatePersonViewModel(_apiService, this.Navigation);
	}
}