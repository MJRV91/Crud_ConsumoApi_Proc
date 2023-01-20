using Api_Rest.Models;

namespace Api_Rest.Repository
{
    public interface IAPIService
    {
        Task<List<ModClieClientes>> MostrarClientes();
        Task<ModClieClientes> BuscarCliente(string codigo);
        Task<bool> RegistrarCliente(ModClieClientes modClie);
        Task<bool> ActualizarCliente(ModClieClientes modClie);
        Task<bool> EliminarCliente(string codigo);

    }
}
