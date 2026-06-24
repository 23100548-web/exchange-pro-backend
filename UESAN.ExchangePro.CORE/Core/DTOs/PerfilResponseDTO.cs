namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class PerfilResponseDTO
    {
        public long IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public string? FotoPerfil { get; set; }
        public decimal? Reputacion { get; set; }
        public int? TotalCalificaciones { get; set; }
    }
}
