using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Web.Data;
using shop.Web.Data.Entities;
using shop.Web.Data.Repositories;
using shop.Web.Helpers;
using shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Controllers
{
    [Authorize]
    public class OrdersController:Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly DataContext context;

        public OrdersController(IOrderRepository orderRepository,IProductRepository productRepository,DataContext context)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.context = context;
        }

        //aqui es donde le voy a decir que me pinte la tabla de ordener
        public async Task<IActionResult> Index()
        {
           
                var model = await orderRepository.GetOrdersAsync(this.User.Identity.Name);
                return View(model);
        
        }
              

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubmitOrders()
        {
            var model = await orderRepository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {//aqui me almacena los temporales que el usuario me haya cargado
            var model = await this.orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return this.View(model);
        }

        public IActionResult AddProduct()
        {//aqui me pinta el combobox
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = this.productRepository.GetComboProducts()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {//aqui me muestra los productos y que me agregue un items
            if (this.ModelState.IsValid)
            {
                await this.orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }

            //con este metodo confirmo la orden 
        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await this.orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return this.RedirectToAction("Index");
            }

            return this.RedirectToAction("Create");
        }



        // GET: Products/Delete/5
      
        public async Task<IActionResult> DeleteOrder(int? id)
        { 
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.orderRepository.GetByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // POST: Products/Delete/5
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.DeleteOrderDetails(id.Value);
            await this.orderRepository.DeleteOrderAsync(id.Value);
            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {

            var prueba = context.OrderDetails.FromSql("SELECT * FROM dbo.OrderDetails WHERE OrderId = {0}", id);

            var aux = prueba.Select(o => o.Product.Id);

        
         
                int? IdDetails = aux.First();
                var product = await this.productRepository.GetByIdAsync(IdDetails.Value);
                

                if (id == null)
                {
                    return new NotFoundViewResult("ProductNotFound");
                }
                //TODO IMPLEMENTAR UNA LISTA              
                                                                      
            return View(product);
        }




    }
}
