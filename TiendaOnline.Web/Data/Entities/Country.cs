using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Web.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }

        [DisplayName("Departments Number")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

    }
}
