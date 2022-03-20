using Microsoft.EntityFrameworkCore;

namespace TiendaOnline.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
