using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05C
{
    internal class Cliente
    {
        public string IdCliente { get; set; }
        public string NombreCompañia { get; set; }
        public string NombreContacto { get; set; }
        public string CargoContacto { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Region { get; set; }
        public string CodPostal { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }

        private static string connectionString =
            @"Server=LAPTOP-D76ORAN2\SQLEXPRESS;
            Database=Neptuno;
            User Id=userTecsup;
            Password=123456;
            TrustServerCertificate=True;";

        public Cliente() { }

        public Cliente(string idCliente, string nombreCompañia, string nombreContacto,
                      string cargoContacto, string direccion, string ciudad, string region,
                      string codPostal, string pais, string telefono, string fax)
        {
            IdCliente = idCliente;
            NombreCompañia = nombreCompañia;
            NombreContacto = nombreContacto;
            CargoContacto = cargoContacto;
            Direccion = direccion;
            Ciudad = ciudad;
            Region = region;
            CodPostal = codPostal;
            Pais = pais;
            Telefono = telefono;
            Fax = fax;
        }

        public static List<Cliente> ListarClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("USP_ListarClientes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cliente cliente = new Cliente
                                {
                                    IdCliente = reader["idCliente"].ToString(),
                                    NombreCompañia = reader["NombreCompañia"].ToString(),
                                    NombreContacto = reader["NombreContacto"].ToString(),
                                    CargoContacto = reader["CargoContacto"].ToString(),
                                    Direccion = reader["Direccion"].ToString(),
                                    Ciudad = reader["Ciudad"].ToString(),
                                    Region = reader["Region"].ToString(),
                                    CodPostal = reader["CodPostal"].ToString(),
                                    Pais = reader["Pais"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Fax = reader["Fax"].ToString()
                                };
                                listaClientes.Add(cliente);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar clientes: {ex.Message}");
            }

            return listaClientes;
        }

        public static bool InsertarCliente(Cliente cliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("USP_InsertarCliente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@idCliente", cliente.IdCliente ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nombreCompañia", cliente.NombreCompañia ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nombreContacto", cliente.NombreContacto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@cargoContacto", cliente.CargoContacto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@direccion", cliente.Direccion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ciudad", cliente.Ciudad ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@region", cliente.Region ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@codPostal", cliente.CodPostal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@pais", cliente.Pais ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@telefono", cliente.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@fax", cliente.Fax ?? (object)DBNull.Value);

                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();

                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar cliente: {ex.Message}");
            }
        }

        public static bool ActualizarCliente(Cliente cliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("USP_ActualizarCliente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@idCliente", cliente.IdCliente ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nombreCompañia", cliente.NombreCompañia ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nombreContacto", cliente.NombreContacto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@cargoContacto", cliente.CargoContacto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@direccion", cliente.Direccion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ciudad", cliente.Ciudad ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@region", cliente.Region ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@codPostal", cliente.CodPostal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@pais", cliente.Pais ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@telefono", cliente.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@fax", cliente.Fax ?? (object)DBNull.Value);

                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();

                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar cliente: {ex.Message}");
            }
        }

        public static bool EliminarCliente(string idCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("USP_EliminarCliente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idCliente", idCliente);

                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();

                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar cliente: {ex.Message}");
            }
        }

        public static bool ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.IdCliente))
                return false;

            if (string.IsNullOrWhiteSpace(cliente.NombreCompañia))
                return false;

            if (string.IsNullOrWhiteSpace(cliente.NombreContacto))
                return false;

            return true;
        }

        public static bool ExisteCliente(string idCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM clientes WHERE idCliente = @idCliente";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCliente", idCliente);
                        connection.Open();

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia del cliente: {ex.Message}");
            }
        }

        public static void EstablecerCadenaConexion(string nuevaCadenaConexion)
        {
            connectionString = nuevaCadenaConexion;
        }
    }
}
