using Api_Rest.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Api_Rest.Repository
{
    public class RepServiceApi : IAPIService
    {
        private static string _usuarioApi = "";
        private static string _passwordApi = "";
        private static string _urlBaseApi = "";
        private static string _tokenApi = "";

        private readonly HttpClient _httpClient;

        public RepServiceApi()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _usuarioApi = builder.GetSection("ConexionApi:UsuarioApi").Value;
            _passwordApi = builder.GetSection("ConexionApi:PasswordApi").Value;
            _urlBaseApi = builder.GetSection("ConexionApi:UrlBase").Value;

            _httpClient = new();
            _httpClient.BaseAddress = new Uri(_urlBaseApi);
        }

        public async Task LoginApi()
        {
            ModLoginApi modLogin = new();
            modLogin.Usuario = _usuarioApi;
            modLogin.Pass = _passwordApi;

            StringContent content = new(JsonConvert.SerializeObject(modLogin), Encoding.UTF8, "Application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_urlBaseApi + "Token", content);
            //var result = await response.Content.ReadAsStringAsync();
            _tokenApi = await response.Content.ReadAsStringAsync();
            //var token_res = JsonConvert.DeserializeObject<string>(result);
            //_tokenApi = result.ToString();
        }

        public async Task<List<ModClieClientes>> MostrarClientes()
        {
            List<ModClieClientes>? modClientes = new();

            await LoginApi();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            HttpResponseMessage response = await _httpClient.GetAsync("Clientes/MostrarClientes");

            if (response.IsSuccessStatusCode)
            {
                string respuesta = await response.Content.ReadAsStringAsync();
                modClientes = JsonConvert.DeserializeObject<List<ModClieClientes>>(respuesta);
                return modClientes;
            }
            else
            {
                return await Task.FromResult(modClientes);
            }
        }

        public async Task<ModClieClientes> BuscarCliente(string codigo)
        {
            ModClieClientes? modClientes = new();

            await LoginApi();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            HttpResponseMessage response = await _httpClient.GetAsync("Clientes/BuscarClientes/" + codigo);

            if (response.IsSuccessStatusCode) 
            {
                string respuesta = await response.Content.ReadAsStringAsync();
                modClientes = JsonConvert.DeserializeObject<ModClieClientes>(respuesta);
                return modClientes;
            }
            else
            {
                return await Task.FromResult(modClientes);
            }
        }

        public async Task<bool> RegistrarCliente(ModClieClientes modClientes)
        {
            
            await LoginApi();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            StringContent content = new(JsonConvert.SerializeObject(modClientes), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("Clientes/RegistrarCliente", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ActualizarCliente(ModClieClientes modClientes)
        {

            await LoginApi();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            StringContent content = new(JsonConvert.SerializeObject(modClientes), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync("Clientes/ActualizarCliente", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EliminarCliente(string codigo)
        {
            await LoginApi();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenApi);
            HttpResponseMessage response = await _httpClient.DeleteAsync("Clientes/EliminarCliente/" + codigo);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}
