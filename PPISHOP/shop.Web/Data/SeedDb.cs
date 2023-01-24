using Microsoft.AspNetCore.Identity;
using shop.Web.Data.Entities;
using shop.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            //aqui me dice que si la base de datos no esta creada
            //recordar , esto es Code First
            await this.context.Database.EnsureCreatedAsync();

            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");


            // vamos a crear el usuario con el que se va a iniciar siempre 
            // var user = await this.userManager.FindByEmailAsync("DeisyOssa@gmail.com");
            var user = await this.userHelper.GetUserByEmailAsync("DeisyOssa@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Deisy",
                    LastName = "Ossa",
                    Email = "DeisyOssa@gmail.com",
                    UserName = "DeisyOssa@gmail.com"

                };

                // aqui me va a crear el usuario.. y ademas le asigno la contraseña
                //var result = await this.userManager.CreateAsync(user, "123456");
                var result = await this.userHelper.AddUserAsync(user, "123456");

                // si el resultado da.. no entra en este bucle
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
                await this.userHelper.ConfirmEmailAsync(user, token);

            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            // aqui estoy preguntando si no hay datos en la base de datos, me meta productos en la base de datos
            if (!this.context.Products.Any())
            {
                // le estos diciendo ese producto va asociado a ese usuario
                this.AddProduct("Iphone z",user);
                this.AddProduct("Magic mouse",user);
                this.AddProduct("La veladora para ganar la carrera",user);
                // aqui le digo que me guarde los cambios, gracias al entiti framework
                await this.context.SaveChangesAsync();
            }
        }

        // como ya lo tengo que me cree el usuario.. hay que asignarle esos productos a algun usuario
        // y eso se hace arriba en products.any
        private void AddProduct(string name, User user )
        {
            this.context.Products.Add(new Product
            {
                //aqui le estoy diciendo que el al nombre del producto me lo ponga un valor aleatorio
                Name = name,
                Price = this.random.Next(100),
                // si esta disponible, y cuantos hay en stock
                IsAvailabe = true,
                Stock = this.random.Next(100)
            });
        }

    }
}
