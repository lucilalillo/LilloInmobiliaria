using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LilloInmobiliaria.Models
{
    public class RepositorioPago
    {

		private readonly IConfiguration configuration;
		private readonly string connectionString;

        public RepositorioPago(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}
		
		public int Alta(Pago pa)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pago (NroPago, FechaPago, Importe, ContratoId) " +
					$" VALUES (@nroPago, @fechaPago, @importe, @idcontrato);" +
					$" SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nroPago", pa.NroPago);
					command.Parameters.AddWithValue("@fechaPago", pa.FechaPago);
					command.Parameters.AddWithValue("@importe", pa.Importe);
					command.Parameters.AddWithValue("@idContrato", pa.ContratoId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					pa.IdPago = res;
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
				string sql = $"DELETE FROM Pago WHERE IdPago = @idPago";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idPago", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Pago pa)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Pago SET NroPago=@nroPago, FechaPago=@fechaPago, " +
					$"Importe=@importe, ContratoId=@idContrato " +
					$"WHERE IdPago = @idPago";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nroPago", pa.NroPago);
					command.Parameters.AddWithValue("@fechaPago", pa.FechaPago);
					command.Parameters.AddWithValue("@importe", pa.Importe);
					command.Parameters.AddWithValue("@idContrato", pa.ContratoId);
					command.Parameters.AddWithValue("@idPago", pa.IdPago);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

        public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                string sql = $"SELECT IdPago, NroPago, FechaPago, Importe, ContratoId, " +
					"c.InmuebleId, i.Direccion, c.InquilinoId, inq.Apellido,  " +
					"c.FechaInicio, c.FechaFin, c.Monto, c.Estado " +
					"FROM Pago pa, Contrato c, Inmueble i, Inquilino inq " +
					"WHERE pa.ContratoId = c.IdContrato AND c.InmuebleId = i.IdInmueble " +
					"AND c.InquilinoId = inq.IdInquilino; ";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pa = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								InmuebleId = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(5),
									Direccion = reader.GetString(6),
								},
								InquilinoId = reader.GetInt32(7),
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(7),
									Apellido = reader.GetString(8)
								},
								FecInicio = reader.GetDateTime(9),
								FecFin = reader.GetDateTime(10),
								Monto = reader.GetDecimal(11),
								Estado = reader.GetBoolean(12),

							}
						};
						res.Add(pa);
					}
					connection.Close();
				}
			}
			return res;
		}

        public Pago ObtenerPorId(int id)
		{
			Pago pa = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT pa.IdPago, NroPago, FechaPago, Importe, ContratoId, c.InmuebleId, c.InquilinoId, " +
					" c.FechaInicio, c.FechaFin, c.Monto, c.Estado  " +
					$" FROM Pago pa INNER JOIN Contrato c ON pa.IdCon = c.IdContrato" +
					$" WHERE pa.IdPago=@idPago";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idPago", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						pa = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								InmuebleId = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(5),

								},
								InquilinoId = reader.GetInt32(6),
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(6),

								},
								FecInicio = reader.GetDateTime(7),
								FecFin = reader.GetDateTime(8),
								Monto = reader.GetDecimal(9),
								Estado = reader.GetBoolean(10),
							}
						};
					}
					connection.Close();
				}
			}
			return pa;
		}
		public IList<Pago> ObtenerTodosPorIdContrato(int Idcontrato)
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, NroPago, FechaPago, Importe, IdCon, " +
					$" c.IdInmu, i.Direccion, c.IdInqui, inq.Apellido, c.FechaInicio, c.FechaFin, c.Monto, c.Estado " +
					$" FROM Pago pa, Contrato c, " +
					"Inmueble i, Inquilino inq WHERE pa.IdCon = c.IdContrato " +
					"AND c.IdInmu = i.IdInmueble " +
					"AND c.IdInqui = inq.IdInquilino AND pa.IdCon = @idContrato; ";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idContrato", SqlDbType.Int).Value = Idcontrato;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pa = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								InmuebleId = reader.GetInt32(5),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(5),
									Direccion = reader.GetString(6),
								},
								InquilinoId = reader.GetInt32(7),
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(7),
									Apellido = reader.GetString(8)
								},
								FecInicio = reader.GetDateTime(9),
								FecFin = reader.GetDateTime(10),
								Monto = reader.GetDecimal(11),
								Estado = reader.GetBoolean(12),
                            }
                        };
						res.Add(pa);
					}
					connection.Close();
				}
			}
			return res;
		}


	}
}
