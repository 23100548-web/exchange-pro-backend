using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class DisputaResponseDTO
    {
        public long IdDisputa { get; set; }
        public long IdTransaccion { get; set; }
        public long UsuarioReporta { get; set; }
        public string UsuarioReportaNombre { get; set; } = null!;
        public string? Motivo { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public List<string> Evidencias { get; set; } = new List<string>();
        public decimal TransaccionMonto { get; set; }
        public string TransaccionEstado { get; set; } = null!;
        public string CompradorNombre { get; set; } = null!;
        public string VendedorNombre { get; set; } = null!;
    }
}