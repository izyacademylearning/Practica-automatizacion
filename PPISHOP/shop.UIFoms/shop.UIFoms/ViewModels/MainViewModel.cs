using System;
using System.Collections.Generic;
using System.Text;

namespace shop.UIFoms.ViewModels
{
    class MainViewModel
    {
        private static MainViewModel instance;
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public MainViewModel()
        { // aqui le estoy diciendo que me instancie la primera pagina
            instance = this;
        }
    
        public static MainViewModel GetInstance()
        {
            if(instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
    }

}
