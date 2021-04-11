using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class RepositorioInmbueble : RepositorioBase, IRepositorio<Inmueble>
    {
        public RepositorioInmbueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmuebles (Prop, Direccion, CantAmbientes, Uso, Tipo, Precio) " +
                    $"VALUES (@prop, @direccion, @cantambientes, @uso, @tipo, @precio);" +
                    $"SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@prop", p.Prop.IdPropietario);
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@cantambientes",p.CantAmbientes);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@tipo",p.Tipo);
                    command.Parameters.AddWithValue("@precio",p.Precio);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdInmueble = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inmuebles WHERE IdInmueble = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET " +
                    $"Prop=@prop, Direccion=@direccion, CantAmbientes=@cantambientes, Uso=@uso, Tipo=@tipo, Precio=@precio " +
                    $"WHERE IdInmueble = @idinmueble";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@prop", p.Prop.IdPropietario);
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@cantambientes",p.Direccion);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@idinmueble", p.IdInmueble);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Prop, Direccion, CantAmbientes, Uso, Tipo, Precio FROM Inmuebles" +
                    $" WHERE IdInmueble=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Prop = reader.GetString(1),
                            Direccion = reader.GetString(2),
                            Uso = reader.GetString(3),
                            Tipo = reader.GetString(4),
                            Precio = (double)reader.GetDecimal(5),
                        };
                        return p;
                    }
                    connection.Close();
                }
            }
            return p;
        }
    }


        public IList<Inmueble> ObtenerTodos()
        {
            
        }
    }

