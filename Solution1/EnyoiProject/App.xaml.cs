using EnyoiProject.NVVM.Views;

namespace EnyoiProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PersonView());
        }
    }
}