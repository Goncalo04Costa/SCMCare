﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Modelos
{
    public class UserFuncionario : IdentityUser
    {
        public int FuncionarioId { get; set; }
    }
}
