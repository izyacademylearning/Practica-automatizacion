﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Web.Data;
using shop.Web.Data.Entities;
using shop.Web.Helpers;
using shop.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Controllers
{// aqui le digo que necesita autorizacion
    
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly IUserHelper userHelper;
        private readonly DataContext context;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper,DataContext context)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
            this.context = context;
        }

        // GET: Products
        
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll().OrderBy(p=> p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProducViewModel view)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\Products", 
                        file); //aqui le estoy concatenando el nombre a la ruta para saber donde esta la imagen

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }
                     // aqui le estoy diciendo que tome la ruta relativa segun el ambiente 
                     //donde lo estoy ejecutando

                    path = $"~/images/Products/{file}";
                }

                // estoy conviritnedo el producto a una view para poder mandarlo con la ruta
                var product = this.ToProduct(view, path);

                // TODO: Pending to change to: this.User.Identity.Name CAMBIO HECHO!
                product.User = await this.userHelper.GetUserByEmailAsync("this.User.Identity.Name");
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        private Product ToProduct(ProducViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User

            };
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var view = this.ToProductViewModel(product);
            return View(view);
        }

        private ProducViewModel ToProductViewModel(Product product)
        {
            return new ProducViewModel
            {
                Id = product.Id,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User

            };
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProducViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // aqui le estoy diciendo que me guarde la imagen original
                    var path =view.ImageUrl;

                    // imagefile es la nueva foto.. si el usuario selecciono una nueva foto
                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            file); //aqui le estoy concatenando el nombre a la ruta para saber donde esta la imagen

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }
                        // aqui le estoy diciendo que tome la ruta relativa segun el ambiente 
                        //donde lo estoy ejecutando

                        path = $"~/images/Products/{file}";
                    }

                    // estoy conviritnedo el producto a una view para poder mandarlo con la ruta
                    var product = this.ToProduct(view, path);


                   // TODO: Pending to change to: this.User.Identity.Name
                    product.User = await this.userHelper.GetUserByEmailAsync("this.User.Identity.Name");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var prueba = context.OrderDetails.FromSql("SELECT * FROM dbo.OrderDetails WHERE ProductId = {0}", id);
            var IdPrueba = prueba.Select(o => o.Id);
           

            if (IdPrueba.Any() == true)
            {
                return new NotFoundViewResult("ProductExisting");
               
            }
                    
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult ProductNotFound()
        {
            return this.View();
        }


    }
}