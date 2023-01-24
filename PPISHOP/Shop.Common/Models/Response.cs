using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
   public class Response
    {
        //su pude consumir la api aqui dice si pudo
        public bool IsSuccess { get; set; }

        // si no pudo en message me dice porque no puede
        public string Message { get; set; }

        // y si pudo lo mete en resul de tipo object que peude almacenar todo
        public object Result { get; set; }

    }
}
