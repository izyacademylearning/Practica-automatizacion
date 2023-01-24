using shop.UIFoms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace shop.UIFoms.Infrastructure
{
    class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        //este va a ser mi diccionario de recursos
        public InstanceLocator()
        {
            this.Main = new MainViewModel();

        }
    }
}
