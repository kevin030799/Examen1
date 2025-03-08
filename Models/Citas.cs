namespace Examen1.Models
{
    public class Citas
    {
        public int Id_Cita { get; set; }
        public int Id_Paciente { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
    }
}
