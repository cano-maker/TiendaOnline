using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Common.Entities
{
    public class Country
    {
        public int id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string name { get; set; }
    }
}
