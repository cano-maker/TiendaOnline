using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TiendaOnline.Web.Data.Entities;

namespace TiendaOnline.Web.Models
{
    public class CategoryViewModel : Category
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
