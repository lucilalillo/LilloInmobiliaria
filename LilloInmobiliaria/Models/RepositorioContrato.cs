using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class RepositorioContrato
		{
			private readonly IConfiguration configuration;
			private readonly string connectionString;



			public RepositorioContrato(IConfiguration configuration)
			{
				this.configuration = configuration;
				connectionString = configuration["ConnectionStrings:DefaultConnection"];
			}
			public int Alta(Contrato c)
			{
				int res = -1;
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"INSERT INTO Contratos (InmuebleId, InquilinoId, FecInicio, FecFin, Monto, Estado) " +
						$"VALUES (@idInmueble, @idInquilino, @fechaInicio, @fechaFin, @monto, @estado);" +
						$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@idInmueble", c.InmuebleId);
						command.Parameters.AddWithValue("@idInquilino", c.InquilinoId);
						command.Parameters.AddWithValue("@fechaInicio", c.FecInicio);
						command.Parameters.AddWithValue("@fechaFin", c.FecFin);
						command.Parameters.AddWithValue("@monto", c.Monto);
						command.Parameters.AddWithValue("@estado", c.Estado);
						connection.Open();
						res = Convert.ToInt32(command.ExecuteScalar());
						c.IdContrato = res;
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
					string sql = $"DELETE FROM Contratos WHERE IdContrato= @idContrato";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@idContrato", id);
						connection.Open();
						res = command.ExecuteNonQuery();
						connection.Close();
					}
				}
				return res;
			}
			public int Modificacion(Contrato c)
			{
				int res = -1;
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"UPDATE Contratos SET InmuebleId=@idInmueble, InquilinoId=@idInquilino, FecInicio=@fechaInicio, FecFin=@fechaFin, Monto=@monto, Estado=@estado " +
						$" WHERE IdContrato = @idContrato";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
                        command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@idInmueble", c.InmuebleId);
						command.Parameters.AddWithValue("@idInquilino", c.InquilinoId);
						command.Parameters.AddWithValue("@fechaInicio", c.FecInicio);
						command.Parameters.AddWithValue("@fechaFin", c.FecInicio);
						command.Parameters.AddWithValue("@monto", c.Monto);
						command.Parameters.AddWithValue("@estado", c.Estado);
						command.Parameters.AddWithValue("@idContrato", c.IdContrato);
						connection.Open();
						res = command.ExecuteNonQuery();
						connection.Close();
					}
				}
				return res;
			}



			public IList<Contrato> ObtenerTodos()
			{
				IList<Contrato> res = new List<Contrato>();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"SELECT IdContrato, InmuebleId, InquilinoId, FecInicio, FecFin, Monto, "+
					    " c.Estado, inq.Nombre, inq.Apellido , i.Direccion " +
						$" FROM Contratos c INNER JOIN Inmuebles i ON c.InmuebleId = i.IdInmueble " +
						$" INNER JOIN Inquilinos inq ON c.InquilinoId = inq.IdInquilinos ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						connection.Open();
					    var reader = command.ExecuteReader();
						while (reader.Read())
						{
							Contrato c = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								InmuebleId = reader.GetInt32(1),
								InquilinoId = reader.GetInt32(2),
								FecInicio = reader.GetDateTime(3),
								FecFin = reader.GetDateTime(4),
								Monto = reader.GetDecimal(5),
								Estado = reader.GetBoolean(6),
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(2),
									Nombre = reader.GetString(7),
									Apellido = reader.GetString(8),
								},
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(1),
									Direccion = reader.GetString(9),
								}
								};
							res.Add(c);
						}
						connection.Close();
					}
				}
				return res;
			}



			public Contrato ObtenerPorId(int id)
			{
				Contrato c = null;
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"SELECT IdContrato, InmuebleId, InquilinoId, FecInicio, FecFin, Monto, Estado FROM Contratos " +
						$" WHERE IdContrato=@idContrato";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.Add("@idContrato", SqlDbType.Int).Value = id;
						command.CommandType = CommandType.Text;
						connection.Open();
						var reader = command.ExecuteReader();
						if (reader.Read())
						{
							c = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								InmuebleId = reader.GetInt32(1),
								InquilinoId = reader.GetInt32(2),
								FecInicio = reader.GetDateTime(3),
								FecFin = reader.GetDateTime(4),
								Monto = reader.GetDecimal(5),
								Estado = reader.GetBoolean(6),
							};
						}
						connection.Close();
					}
				}
				return c;
			}
			public IList<Contrato> ObtenerPorInmuebleId(int id)
			{
				IList<Contrato> res = new List<Contrato>();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"SELECT IdContrato, InmuebleId, InquilinoId, FecInicio, FecFin, Monto, Estado, " +
						$"i.Direccion, inq.Nombre, inq.Apellido " +
						$" FROM Contratos c INNER JOIN Inmuebles i ON c.InmuebleId = i.IdInmueble " +
						$"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.IdInquilino " +
						$"WHERE InmuebleId = @idInmueble";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.Add("@idInmueble", SqlDbType.Int).Value = id;
						command.CommandType = CommandType.Text;
						connection.Open();
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							Contrato c = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								InmuebleId = reader.GetInt32(1),
								InquilinoId = reader.GetInt32(2),
								FecInicio = reader.GetDateTime(3),
								FecFin = reader.GetDateTime(4),
								Monto = reader.GetDecimal(5),
								Estado = reader.GetBoolean(6),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(1),
									Direccion = reader.GetString(7),



								},
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(2),
									Nombre = reader.GetString(8),
									Apellido = reader.GetString(9),



								}
							};
							res.Add(c);
						}
						connection.Close();
					}
				}
				return res;
			}



			public IList<Contrato> ObtenerPorContratoId(int id)
			{
				IList<Contrato> res = new List<Contrato>();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"SELECT c.IdContrato, InmuebleId, InquilinoId, FecInicio, FecFin, Monto, Estado, " +
						$"i.Direccion, inq.Nombre, inq.Apellido " +
						$" FROM Contratos c INNER JOIN Inmuebles i ON c.InmuebleId = i.IdInmueble " +
						$"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.IdInquilino " +
						$"WHERE InmuebleId = @idInmueble";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.Add("@idContrato", SqlDbType.Int).Value = id;
						command.CommandType = CommandType.Text;
						connection.Open();
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							Contrato c = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								InmuebleId = reader.GetInt32(1),
								InquilinoId = reader.GetInt32(2),
								FecInicio = reader.GetDateTime(3),
								FecFin = reader.GetDateTime(4),
								Monto = reader.GetDecimal(5),
								Estado = reader.GetBoolean(6),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(1),
									Direccion = reader.GetString(7),
								},
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(2),
									Nombre = reader.GetString(8),
									Apellido = reader.GetString(9),
								},

							};
							res.Add(c);
						}
						connection.Close();
					}
				}
				return res;
			}
			public IList<Contrato> ObtenerContratosVigentes(DateTime fechaInicio, DateTime fechaFin)
			{
				IList<Contrato> res = new List<Contrato>();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"SELECT c.IdContrato, InmuebleId, InquilinoId, FecInicio, FecFin, Monto, c.Estado, i.Direccion, inq.Nombre, inq.Apellido  " +
						$" FROM Contratos c INNER JOIN Inmuebles i ON c.InmuebleId = i.IdInmueble " +
						$"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.IdInquilino " +
						$"WHERE c.Estado = 1" +
						$"AND((FecInicio < @fechaInicio)AND(FecFin > @fechaFin))" +
						$"OR((FecInicio BETWEEN @fechaInicio AND @fechaFin)AND(FecFin BETWEEN @fechaInicio AND @fechaFin))" +
						$"OR((FecInicio < @fechaInicio)AND(FecFin BETWEEN @fechaInicio AND @fechaFin))" +
						$"OR((FecInicio BETWEEN @fechaInicio AND @fechaFin)AND(FecFin > @fechaFin));";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.Add("@fechaInicio", SqlDbType.DateTime).Value = fechaInicio;
						command.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaFin;
						command.CommandType = CommandType.Text;
						connection.Open();
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							Contrato c = new Contrato
							{
								IdContrato = reader.GetInt32(0),
								InmuebleId = reader.GetInt32(1),
								InquilinoId = reader.GetInt32(2),
								FecInicio = reader.GetDateTime(3),
								FecFin = reader.GetDateTime(4),
								Monto = reader.GetDecimal(5),
								Estado = reader.GetBoolean(6),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(1),
									Direccion = reader.GetString(7),
								},
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(2),
									Nombre = reader.GetString(8),
									Apellido = reader.GetString(9),
								},

							};
							res.Add(c);
						}
						connection.Close();
					}
				}
				return res;
			}



		}


	}

