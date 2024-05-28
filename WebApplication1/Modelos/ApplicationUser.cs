using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Modelos
{
    /// <summary>
    /// This class represents a User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's Name
        /// </summary>
        public string Name { get; set; } = null!;
    }
}