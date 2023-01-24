using Shop.Common.Models;
using Shop.Common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace shop.UIFoms.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Product> products;
        private bool isRefreshing;
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing,value);
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        { // aqui le estoy diciendo que se me traiga los datos de la pagina
            this.IsRefreshing = true;
            var response = await this.apiService.GetListAsync<Product>(
                "https://postrejuanita.azurewebsites.net",
                "/api",
                "/Products");
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
            }
            //aqui estoy casteando result.. porque es de tipo objeto esto es para que pueda tener mi lista de memoria en memoria
            var myProducts = (List<Product>)response.Result;
            //aqui le estoy diciendo que me pinte mis productos.. recordar que observablecolletion es para que me pinte la lista
            this.Products = new ObservableCollection<Product>(myProducts);


        }
    }
}
