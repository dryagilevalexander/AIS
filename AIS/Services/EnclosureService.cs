using Core;
using Microsoft.EntityFrameworkCore;

namespace AIS.Services
{
    public class EnclosureService:IEnclosureService
    {
        private CoreContext db;
        public EnclosureService(CoreContext coreContext) 
        { 
            db = coreContext;
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
    }
}
