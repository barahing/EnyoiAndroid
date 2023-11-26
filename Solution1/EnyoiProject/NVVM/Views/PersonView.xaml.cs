using EnyoiProject.NVVM.ViewModels;

namespace EnyoiProject.NVVM.Views;

public partial class PersonView : ContentPage
{
	public PersonView()
	{
		InitializeComponent();
		BindingContext = new PersonViewModel(this.Navigation);
	}
}