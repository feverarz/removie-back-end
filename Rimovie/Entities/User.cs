using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rimovie.Entities
{
    public class User
    {
        public int UserId { get; set; }              // Clave primaria
        public string Username { get; set; }         // Nombre de usuario único
        public string Email { get; set; }            // Email único
        public string PasswordHash { get; set; }     // Contraseña encriptada
        public string Name { get; set; }             // Nombre real (opcional)
        public string Permission { get; set; }       // Rol o permisos
        public DateTime CreatedAt { get; set; }      // Fecha de creación
    }

}
