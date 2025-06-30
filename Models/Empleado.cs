namespace MVCwebApi.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Documento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Contrato { get; set; } = null!;
        public string Jornada { get; set; } = null!;
    }
}
