using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Modelos
{
    [Table("token")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idtoken")]
        public int IdToken { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("token")]
        public string TokenValue { get; set; }
    }
}
