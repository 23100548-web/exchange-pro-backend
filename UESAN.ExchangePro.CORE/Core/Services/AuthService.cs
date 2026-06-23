using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.CORE.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
        }

        public async Task<bool> Registrar(RegistroDTO registroDTO)
        {
            if (registroDTO.Password != registroDTO.ConfirmarPassword)
                throw new Exception("Las contraseñas no coinciden.");

            bool existe = await _usuarioRepository.CorreoExiste(registroDTO.Correo);
            if (existe)
                throw new Exception("El correo ya está registrado.");

            var nuevoUsuario = new Usuarios
            {
                IdRol = 1,
                NombreCompleto = $"{registroDTO.Nombres} {registroDTO.Apellidos}".Trim(),
                Correo = registroDTO.Correo,
                Telefono = registroDTO.Telefono,
                DocumentoIdentidad = registroDTO.DocumentoIdentidad,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registroDTO.Password),
                Estado = "ACTIVO",
                Reputacion = 5.00m,
                TotalCalificaciones = 0,
                FechaRegistro = DateTime.UtcNow
            };

            return await _usuarioRepository.Insert(nuevoUsuario);
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.GetByCorreo(loginDTO.Correo);
            if (usuario == null)
                throw new Exception("Credenciales incorrectas.");

            bool passwordValido = BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.PasswordHash);
            if (!passwordValido)
                throw new Exception("Credenciales incorrectas.");

            // GENERACIÓN DEL TOKEN JWT
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Falta la clave JWT en appsettings")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim("Rol", usuario.IdRol.ToString()),
                new Claim("Nombres", usuario.Nombres),
                new Claim("Apellidos", usuario.Apellidos),
                new Claim("NombreCompleto", $"{usuario.Nombres} {usuario.Apellidos}")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> SolicitarReset(SolicitarResetDTO dto)
        {
            var usuario = await _usuarioRepository.GetByCorreo(dto.Correo);
            if (usuario == null)
                return null;

            var token = Guid.NewGuid().ToString("N");
            usuario.ResetToken = token;
            usuario.ResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _usuarioRepository.Update(usuario);

            // EmailService será implementado por Persona 5

            return token;
        }

        public async Task<bool> RestablecerPassword(RestablecerPasswordDTO dto)
        {
            var usuario = await _usuarioRepository.GetByResetToken(dto.ResetToken);
            if (usuario == null || usuario.ResetTokenExpiry == null || usuario.ResetTokenExpiry < DateTime.UtcNow)
                throw new Exception("Token inválido o expirado.");

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);
            usuario.ResetToken = null;
            usuario.ResetTokenExpiry = null;
            return await _usuarioRepository.Update(usuario);
        }
    }
}