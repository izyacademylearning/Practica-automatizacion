using shop.UIFoms.ViewModels;
using shop.UIFoms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace shop.UIFoms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainViewModel.GetInstance().Login = new LoginViewModel();
            this.MainPage = new NavigationPage( new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
