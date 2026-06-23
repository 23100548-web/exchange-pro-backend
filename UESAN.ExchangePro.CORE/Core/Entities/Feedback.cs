using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UESAN.ExchangePro.CORE.Core.Entities
{
    public class Feedback
    {
        public long IdFeedback { get; set; }
        public long IdUsuario { get; set; }
        public string Tipo { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = "PENDIENTE";
        public string? RespuestaAdmin { get; set; }
        public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
    }
}
