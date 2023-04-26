using Microsoft.EntityFrameworkCore;

namespace OpenXML
{
    public class ContragentsService
    {
        ApplicationContext db;
        public ContragentsService()
        {
            db = new ApplicationContext();
        }
        public Contragent GetContragentById(int id)
        {
            return db.Contragents.Include(p => p.DirectorType).FirstOrDefault(p => p.Id == id);
        }
        public Contragent GetMainOrganization()
        {
            return db.Contragents.Include(p => p.DirectorType).FirstOrDefault(p => p.IsMain == true); 
        }
    }
}
