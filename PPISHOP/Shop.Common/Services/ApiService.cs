
namespace Shop.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Models;
 
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class ApiService
    {
        public async Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller)
        {
            // recordar que es conexion.. entonces debe de ir en un try
            try
            {
                // el crea un objeto de conexion
                var client = new HttpClient
                {//arma una url con la url base
                    BaseAddress = new Uri(urlBase)
                };
                //despues concatena la peticion con el controlador
                var url = $"{servicePrefix}{controller}";
                // en un objeto responde.. anda y traeme lo que hay en la url
                var response = await client.GetAsync(url);

                // a la respuesta leala como un string
                var result = await response.Content.ReadAsStringAsync();

                // si el result fue no satisfactorio.. status OK
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                // resulta que esto cuando me lo jalo de postman.. se trae un string muy grande

                //dezserializar es coger un string y volverlo json
                //serializar coger un json y volverlo a string
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }

}
