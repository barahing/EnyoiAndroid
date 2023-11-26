using EnyoiProject.NVVM.ViewModels;

namespace EnyoiProject.NVVM.Views;

public partial class CreatePersonView : ContentPage
{
	public CreatePersonView()
	{
		InitializeComponent();
		BindingContext = new CreatePersonViewModel();
	}
}