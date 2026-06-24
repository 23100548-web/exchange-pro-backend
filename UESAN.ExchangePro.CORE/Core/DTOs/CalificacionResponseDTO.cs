namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class CalificacionResponseDTO
    {
        public long IdCalificacion { get; set; }
        public long IdTransaccion { get; set; }
        public long UsuarioCalificado { get; set; }
        public int Puntuacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaCalificacion { get; set; }
    }
}