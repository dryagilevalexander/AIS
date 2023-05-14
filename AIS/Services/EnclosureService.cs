using AIS.ErrorManager;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AIS.Services
{
    public class EnclosureService:IEnclosureService
    {
        private AisDbContext db;
        public EnclosureService(AisDbContext context) 
        { 
            db = context;
        }

        public async Task<IEnumerable<MyFile>> GetMyEnclosuresBySubTaskId(int id)
        {
            return await (from myFile in db.MyFiles.Include(p => p.MySubTask) where myFile.MySubTaskId == id select myFile).ToListAsync();
        }

        public async Task<IEnumerable<MyFile>> GetMyEnclosuresByTaskId(int id)
        {
            return await (from myFile in db.MyFiles.Include(p => p.MyTask) where myFile.MyTaskId == id select myFile).ToListAsync();
        }

        public async Task<IEnumerable<MyFile>> GetMyEnclosuresByContractId(int id)
        {
            return await (from myFile in db.MyFiles.Include(p => p.Contract) where myFile.ContractId == id select myFile).ToListAsync();
        }

        public async Task<IEnumerable<MyFile>> GetMyEnclosuresByLetterId(int id)
        {
            return await (from myFile in db.MyFiles.Include(p => p.Letter) where myFile.LetterId == id select myFile).ToListAsync();
        }

        public async Task DeleteMyEnclosure(int id)
        {
            try
            {
                var currentMyFile = await db.MyFiles.FirstOrDefaultAsync(p => p.Id == id);
                db.Entry(currentMyFile).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить вложение", HttpStatusCode.BadRequest);
            }
        }
    }
}
