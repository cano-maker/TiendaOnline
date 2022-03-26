using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Web.Models
{
    public class Country
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

    }
}
