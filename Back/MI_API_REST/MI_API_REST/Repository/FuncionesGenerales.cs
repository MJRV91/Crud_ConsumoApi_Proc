
using System.Data;
using System.Data.SqlClient;

namespace MI_API_REST.Repository
{
    public class FuncionesGenerales
    {
        private readonly IConfiguration _configuration;
        public FuncionesGenerales()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        /// <summary>
        /// Devuelve datos de cualquier Procedimiento Almavcenado
        /// Trabaja exclusivamente con SP
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="parametros"></param>
        /// <returns>DataTable</returns>
        public DataTable CargarDatos(string sp, Dictionary<string, string>? parametros = null)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ConexionBD").ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand(sp, con)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parametros != null)
            {
                foreach (KeyValuePair<string, string> item in parametros)
                {
                    cmd.Parameters.AddWithValue(Convert.ToString(item.Key), Convert.ToString(item.Value));
                }
            }
            DataTable dt = new();
            try
            {
                new SqlDataAdapter(cmd).Fill(dt);
            }
            catch (Exception) 
            {
                throw;
            }
            con.Close();
            return dt;
        }

        /// <summary>
        /// Ejecuta Acciones de Insertar, Actualizar y Eliminar
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="parametros"></param>
        /// <returns>Int</returns>
        public int EjecutarInsDelUpd(string sp, Dictionary<string, string>? parametros = null)
        {
            using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ConexionBD").ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand(sp, con)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parametros != null)
            {
                foreach (KeyValuePair<string, string> item in parametros)
                {
                    cmd.Parameters.AddWithValue(Convert.ToString(item.Key), Convert.ToString(item.Value));
                }
            }
            int resultado = 0;
            try
            {
                resultado = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            con.Close();
            return resultado;
        }
    }
}
