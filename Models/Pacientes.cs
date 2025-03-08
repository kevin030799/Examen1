namespace Examen1.Models
{
    public class Pacientes
    {
        // Modelo para Pacientes
            public int Id_Paciente { get; set; }
            public string Cedula { get; set; }
            public string Nombre { get; set; }
            public string Telefono { get; set; }
            public string Correo { get; set; }
            public string Direccion { get; set; }
            public DateTime FechaNacimiento { get; set; }
    }
}
