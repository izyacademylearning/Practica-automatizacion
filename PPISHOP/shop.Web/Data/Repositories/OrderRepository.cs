using Microsoft.EntityFrameworkCore;
using shop.Web.Data.Entities;
using shop.Web.Helpers;
using shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data.Repositories
{
    //hereda de generic e implementa IORDER
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        //aqui lo primero que hace es validar que el usuario existe
        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            //aqui le decimos que si el usuario es administrador le mande todos los pedidos
            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //aqui le decimos que me incluya los productos es como decirle que me haga un join
                //y que me incluya todas las ordentes y me los ordena de forma descendente
                // el include es una herramiente de core que me permite enlazar las tablas para hacer consultas
                // y si quiero ir a consultar otra tabla que no este en la relacion le damos con theninclude
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            //devuelvame todos los detalles de los productos.. recordar relaciones include
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        //este metodo es para adicionar un producto a la tabla temporal
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {// el usuario existe?
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await this.context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            //si este usuario ya pidio el producto ... no saque otra linea con el mismo producto.
            // solo que aumente la cantidad en 
            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            this.context.OrderDetailTemps.Remove(orderDetailTemp);
            await this.context.SaveChangesAsync();
        }

        //confirmacion de pedido 
        public async Task<bool> ConfirmOrderAsync(string userName)
        {//buscamos el usuario que tenga los productos
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }
            //busqueme los productos que hayan en orderDetail pero me trae los productos de  la tabla product.. con include
            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            //aqui estamos mapeando un objeto a otro objeto.. 
            //investigar sobre programacion funcional!!!
            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                //aqui le decimos que la fecha es la de hoy en dia con la hora de londres UTC
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            //luego de agregar los pedidos.. que me remueva los registro del carrito de compras 
            this.context.Orders.Add(order);
            this.context.OrderDetailTemps.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await this.context.Orders.FindAsync(id);

            if (order == null)
            {
                return;
            }

            this.context.Orders.Remove(order);
            await this.context.SaveChangesAsync();

        }





        //  public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        public async Task DeleteOrderDetails(int id)
        {
            var prueba = context.OrderDetails.FromSql("SELECT * FROM dbo.OrderDetails WHERE OrderId = {0}", id);
            var IdPrueba = prueba.Select(o => o.Id);

            bool cont = true;

            while (cont != false)
            {

                int IdDetails = IdPrueba.First();
                var orderDetails = await this.context.OrderDetails.FindAsync(IdDetails);

                if (orderDetails == null)
                {
                    return;
                }

                this.context.OrderDetails.Remove(orderDetails);
                await this.context.SaveChangesAsync();

                if (!IdPrueba.Any() == true)

                {
                    cont = false;
                    break;
                }

            }

        }



    }

}

