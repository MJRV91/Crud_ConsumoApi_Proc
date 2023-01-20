using MI_API_REST.Entities;
using MI_API_REST.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MI_API_REST.Controllers
{
    [ApiController]
    [Route("api/Clientes")]
    public class CliClientesController : ControllerBase
    {
        //private readonly ILogger<CliClientesController> _logger;
        private RepCliClientes repCliClientes;

        //public CliClientesController(ILogger<CliClientesController> logger)
        //{
        //    _logger = logger;
        //}

        public CliClientesController()
        {
            repCliClientes = new RepCliClientes();
        }

        /// <summary>
        /// Método paa mostrar lista de clientes.
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("MostrarClientes")]
        public ActionResult MostrarClientes()
        {
            List<ModClieClientes>? modClie = new();
            modClie = repCliClientes.MostrarClientes();
            if (modClie != null) 
            { 
                return Ok(modClie);
            }
            else
            {
                return BadRequest("No se encontraron clientes registrados.");
            }
        }

        /// <summary>
        /// Método para buscar cliente en específico.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("BuscarClientes/{codigo}")]
        public ActionResult BuscarCliente(string codigo)
        {
            ModClieClientes? modClie = new();
            modClie = repCliClientes.BuscarCliente(codigo);

            if (modClie != null)
            {
                return Ok(modClie);
            }
            else
            {
                return BadRequest("El cliente con el código" + codigo + " no está registrado.");
            }
        }

        /// <summary>
        /// Método para registrar un nuevo cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("RegistrarCliente")]
        public ActionResult RegistrarCliente(ModClieClientes modClie) 
        {
            //if (modClie.CliNombres.Trim().ToString().Count() == 0)
            if(String.IsNullOrEmpty(modClie.CliNombres))
            {
                return BadRequest("El Nombre del cliente es obligatorio.");
            }

            int resultado = repCliClientes.RegistrarCliente(modClie);
            if (resultado <= 0)
            {
                return BadRequest("Error inesperado al intentar registrar los datos del cliente.");
            }
            else
            {
                return Ok("Cliente registrado con éxito.");
            }
        }

        /// <summary>
        /// Método para actualizar datos de un cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ActualizarCliente")]
        public ActionResult ActualizarCliente(ModClieClientes modClie)
        {
            if (String.IsNullOrEmpty(modClie.CliNombres))
            {
                return BadRequest("El Nombre del cliente es obligatorio.");
            }

            int resultado = repCliClientes.ActualizarCliente(modClie);
            if (resultado <= 0)
            {
                return BadRequest("Error inesperado al intentar guardar los datos del cliente.");
            }
            else
            {
                return Ok("Cliente actualizado con éxito.");
            }
        }

        /// <summary>
        /// Método para Eliminar un cliente.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("EliminarCliente/{codigo}")]
        public ActionResult EliminarCliente(string codigo)
        {
            if (codigo.Trim().Count() == 0)
            {
                return BadRequest("El código del cliente es obligatorio.");
            }

            int resultado = repCliClientes.EliminarCliente(codigo);
            if (resultado <= 0)
            {
                return BadRequest("Error inesperado al intentar eliminar el cliente.");
            }
            else
            {
                return Ok("Cliente eliminado con éxito.");
            }
        }
    }
}