using shop.Web.Data.Entities;
using shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data.Repositories
{

    //este utiliza el repositorio generico.. entonces puedo utilizar los metodos genericos como crear..eliminar..etc
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // con este metodo vamos a diferenciar los pedidos de los usuarios
        Task<IQueryable<Order>> GetOrdersAsync(string userName);


        //traigame los temporales de este usuario
        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);


        //agregar producto
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);


        //borar producto temporal
        Task DeleteDetailTempAsync(int id);

        //para confirmar el pedido.. 
        Task<bool> ConfirmOrderAsync(string userName);

        //borrar orden
        Task DeleteOrderAsync(int id);

        Task DeleteOrderDetails(int id);

      
    }

}
