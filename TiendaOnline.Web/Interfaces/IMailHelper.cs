using TiendaOnline.Web.Common;

namespace TiendaOnline.Web.Interfaces
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
