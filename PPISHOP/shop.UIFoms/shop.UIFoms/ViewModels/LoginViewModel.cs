
using GalaSoft.MvvmLight.Command;
using shop.UIFoms.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace shop.UIFoms.ViewModels
{
    public class LoginViewModel 
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(Login);
        public LoginViewModel()
        {
            this.Email = "deisy@gmail.com";
            this.Password = "123456";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email",
                    "Accept");
                return;

            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an password",
                    "Accept");
                return;
            }

            if (!this.Email.Equals("deisy@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "User or password wrong",
                    "Accept");
                return;

            }

            // cada que yo vaya a tirar una pagina a memoria.. primero hay que instanciar la productviewmodel
            // ademas de ligarla
            // aqui le estoy diciendo que me pinte los productos que estan siendo cargados en memoria
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());
       
                       
        }
    }

}
