using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using UESAN.ExchangePro.CORE.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UESAN.ExchangePro.CORE.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ExchangeProDbContext _context;

        public AdminService(ExchangeProDbContext context)
        {
            _context = context;
        }

        public async Task<AdminEstadisticasDTO> GetEstadisticas()
        {
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var ofertasActivas = await _context.Ofertas.CountAsync(o => o.Estado == "ACTIVA");
            var transaccionesCompletadas = await _context.Transacciones.CountAsync(t => t.Estado == "COMPLETADA");
            var disputasPendientes = await _context.Disputas.CountAsync(d => d.Estado == "PENDIENTE" || d.Estado == "EN_REVISION");
            var feedbackPendientes = await _context.Set<UESAN.ExchangePro.CORE.Core.Entities.Feedback>()
                .CountAsync(f => f.Estado == "PENDIENTE");

            return new AdminEstadisticasDTO
            {
                TotalUsuarios = totalUsuarios,
                OfertasActivas = ofertasActivas,
                TransaccionesCompletadas = transaccionesCompletadas,
                DisputasPendientes = disputasPendientes,
                FeedbackPendientes = feedbackPendientes
            };
        }

        public async Task<ChatbotResponseDTO> ChatbotResponder(string mensaje)
        {
            var msg = mensaje.ToLower().Trim();

            var respuestas = new Dictionary<string, string>
            {
                { "hola", "¡Hola! Soy el asistente de ExchangePro. ¿En qué puedo ayudarte?" },
                { "buenos dias", "¡Buenos días! Soy el asistente de ExchangePro. ¿En qué puedo ayudarte?" },
                { "buenas tardes", "¡Buenas tardes! Soy el asistente de ExchangePro. ¿En qué puedo ayudarte?" },
                { "como funciona", "ExchangePro es una plataforma P2P donde puedes comprar y vender criptomonedas directamente con otros usuarios. Publica una oferta, espera que alguien la acepte, y realiza el pago de forma segura." },
                { "como comprar", "Para comprar: 1) Ve a la sección 'Ofertas', 2) Encuentra una oferta de venta que te interese, 3) Haz clic en 'Comprar', 4) Realiza el pago al vendedor, 5) Marca la transacción como pagada." },
                { "como vender", "Para vender: 1) Ve a 'Crear Oferta', 2) Indica el monto y método de pago, 3) Espera a que un comprador acepte tu oferta, 4) Confirma el pago cuando lo recibas." },
                { "disputa", "Si tienes un problema con una transacción, puedes abrir una disputa desde la página de detalles de la transacción. Un administrador revisará el caso y resolverá el conflicto." },
                { "como abrir disputa", "Ve a 'Transacciones', selecciona la transacción con problemas, y haz clic en 'Abrir Disputa'. Proporciona toda la evidencia necesaria para que podamos ayudarte." },
                { "wallet", "Tu wallet muestra tu saldo disponible y en retención. Puedes depositar fondos y retirarlos cuando lo desees." },
                { "retiro", "Para retirar fondos, ve a tu Wallet y selecciona 'Retirar'. Ingresa el monto y confirma la operación." },
                { "deposito", "Para depositar fondos, ve a tu Wallet y selecciona 'Depositar'. Sigue las instrucciones para completar el depósito." },
                { "comision", "ExchangePro cobra una comisión mínima por cada transacción completada. El porcentaje exacto se muestra antes de confirmar cada operación." },
                { "seguridad", "La seguridad es nuestra prioridad. Todas las transacciones están protegidas por nuestro sistema de retención de fondos y resolución de disputas." },
                { "tiempo", "El tiempo de cada transacción depende del vendedor y comprador. Generalmente las transacciones se completan en minutos u horas." },
                { "metodo de pago", "Puedes agregar múltiples métodos de pago desde 'Datos de Pago'. Soporte para transferencias bancarias, Yape, Plin y más." },
                { "gracias", "¡De nada! Si tienes más preguntas, estoy aquí para ayudarte." },
                { "adios", "¡Hasta luego! Gracias por usar ExchangePro." },
                { "chau", "¡Hasta luego! Gracias por usar ExchangePro." },
                { "quien eres", "Soy el asistente virtual de ExchangePro. Estoy aquí para responder tus dudas sobre la plataforma." },
                { "que eres", "Soy un asistente virtual creado para ayudarte a usar ExchangePro. Aunque no soy una IA avanzada, tengo respuestas para las preguntas más frecuentes." }
            };

            foreach (var kvp in respuestas)
            {
                if (msg.Contains(kvp.Key))
                {
                    await Task.Delay(500);
                    return new ChatbotResponseDTO { Respuesta = kvp.Value };
                }
            }

            await Task.Delay(500);
            return new ChatbotResponseDTO
            {
                Respuesta = "Lo siento, no tengo una respuesta para esa pregunta. Puedes intentar preguntar de otra forma o contactar al soporte técnico para más ayuda."
            };
        }
    }
}
