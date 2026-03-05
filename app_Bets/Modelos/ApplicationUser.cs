using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Modelos
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
