using System;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class FeedbackResponseDTO
    {
        public long IdFeedback { get; set; }
        public string Tipo { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = null!;
        public string? RespuestaAdmin { get; set; }
        public long IdUsuario { get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public string UsuarioEmail { get; set; } = null!;
    }
}