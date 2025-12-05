using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.User
{
    public class PasswordResetForAdminDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
