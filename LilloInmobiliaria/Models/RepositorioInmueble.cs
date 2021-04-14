using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $" INSERT INTO Inmuebles (IdInmueble, PropietarioId, Direccion, CantAmbientes, Uso, Tipo, Precio, Estado) " +
                    $" VALUES (@idinmuebles, @propietarioid, @direccion, @cantambientes, @uso, @tipo, @precio, @estado);" +
                    $" SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@idinmueble", p.IdInmueble);
                    command.Parameters.AddWithValue("@propietarioid", p.PropietarioId);
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@cantambientes", p.CantAmbientes);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@estado", p.Estado);
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
                    $"PropietarioId=@propietarioid, Direccion=@direccion, CantAmbientes=@cantambientes, Uso=@uso, Tipo=@tipo, Precio=@precio, Estado=@estado " +
                    $"WHERE IdInmueble = @idinmueble";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@propietarioid", p.PropietarioId);
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@cantambientes", p.CantAmbientes);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@precio", p.Estado);
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
                string sql = $" SELECT IdInmueble, Direccion, CantAmbientes, Uso, Tipo, Precio, Estado, IdPropietario " +
                    "p.Nombre, p.Apellido " + 
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = i.Idpropietario " +
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
                            Direccion = reader.GetString(1),
                            CantAmbientes = reader.GetInt32(2),
                            Uso = reader.GetString(3),
                            Tipo = reader.GetString(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetString(6),
                            PropietarioId = reader.GetInt32(7),
                            Prop = new Propietario
                            {
                                IdPropietario = reader.GetInt32(8),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10)
                            }

                        };
                        return p;
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String sql = " SELECT IdInmueble, Direccion, CantAmbientes, Uso, Tipo, Precio, Estado, IdPropietario " +
                    "p.Nombre, p.Apellido " +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble p = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            CantAmbientes = reader.GetInt32(2),
                            Uso = reader.GetString(3),
                            Tipo = reader.GetString(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetString(6),
                            PropietarioId = reader.GetInt32(7),
                            Prop = new Propietario
                            {
                                IdPropietario = reader.GetInt32(8),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10)
                            }

                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

    }
}