using Api_Rest.Models;
using Api_Rest.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Api_Rest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAPIService _apiService;

        public HomeController(IAPIService ApiService)
        {
            _apiService = ApiService;
        }

        public async Task<IActionResult> Index()
        {
            List<ModClieClientes> modClientes = await _apiService.MostrarClientes();

            return View(modClientes);
        }

        [HttpGet]
        public async Task<IActionResult> Regresar()
        {
            List<ModClieClientes> modClientes = await _apiService.MostrarClientes();

            return View("_listaClientes", modClientes);
        }

        public async Task<IActionResult> BuscarClientes(int codigo)
        {
            ModClieClientes modClientes = await _apiService.BuscarCliente(Convert.ToString(codigo));

            return View("ActualizarCliente", modClientes);
        }

        [HttpGet]
        public IActionResult NuevoCliente()
        {
            return View("RegistrarCliente");
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente( ModClieClientes modClie)
        {
            bool respuesta = await _apiService.RegistrarCliente(modClie);
            string[] msj = new string[2];

            if (respuesta)
            {
                msj[0] = "1";
                msj[1] = "CLiente registrado con éxito.";
            }
            else
            {
                msj[0] = "0";
                msj[1] = "Error inesperado al intentar registrar el nuevo cliente.";
            }
            return Json(msj);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarCliente(ModClieClientes modClie)
        {
            bool respuesta = await _apiService.ActualizarCliente(modClie);
            string[] msj = new string[2];

            if (respuesta)
            {
                msj[0] = "1";
                msj[1] = "Cliente actualizado con éxito.";
            }
            else
            {
                msj[0] = "0";
                msj[1] = "Error inesperado al intentar actualizar el cliente.";
            }
            return Json(msj);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCliente(string codigoCliente)
        {
            string[] msj = new string[2];
            bool respuesta = await _apiService.EliminarCliente(codigoCliente);
            if (respuesta)
            {
                msj[0] = "1";
                msj[1] = "Cliente Eliminado con éxito";
            }
            else
            {
                msj[0] = "0";
                msj[1] = "Error inesperado al intentar Eliminar el cliente.";
            }
            return Json(msj);
        }

    }
}