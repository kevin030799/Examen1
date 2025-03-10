using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Examen1.Models
{
    public class AccesoDatos
    {
            private readonly string _conexion;

            public AccesoDatos(IConfiguration configuration)
            {
                _conexion = configuration.GetConnectionString("DefaultConnection");
            }

            public Pacientes ObtenerPacientePorCedula(string cedula)
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_Obtener_Paciente_Por_Cedula", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cedula", cedula);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Pacientes
                        {
                            Id_Paciente = Convert.ToInt32(reader["Id"]),
                            Cedula = reader["Cedula"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"])
                        };
                    }
                }
                return null;
            }

            public void InsertarPaciente(Pacientes paciente)
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_Insertar_Paciente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cedula", paciente.Cedula);
                    cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                    cmd.Parameters.AddWithValue("@Correo", paciente.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", paciente.Direccion);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        public void ModificarPaciente(Pacientes paciente)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarPaciente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cedula", paciente.Cedula);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", paciente.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", paciente.Direccion);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
              
        
        public void InsertarCita(Citas cita)
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_Insertar_Cita", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPaciente", cita.Id_Paciente);
                    cmd.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
                    cmd.Parameters.AddWithValue("@Motivo", cita.Motivo);
                    cmd.Parameters.AddWithValue("@Estado", cita.Estado);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            public List<Citas> ObtenerCitasDelDia()
            {
                List<Citas> citas = new List<Citas>();
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_Obtener_Citas_Del_Dia", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        citas.Add(new Citas
                        {
                            Id_Cita = Convert.ToInt32(reader["Id"]),
                            Id_Paciente = Convert.ToInt32(reader["IdPaciente"]),
                            FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                            Motivo = reader["Motivo"].ToString(),
                            Estado = reader["Estado"].ToString()
                        });
                    }
                }
                return citas;
            }
        public void EliminarPacientePorCedula(string cedula)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("EliminarPacientePorCedula", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
    }
