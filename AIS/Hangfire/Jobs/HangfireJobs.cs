using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AIS.Hangfire.Jobs
{
    public class HangfireJobs: IHangfireJobs
    {
        private readonly AisDbContext _db;
        private IWebHostEnvironment _appEnvironment;
        IHubContext<AisHub> _hubContext;


        public HangfireJobs(AisDbContext db, IWebHostEnvironment appEnvironment, IHubContext<AisHub> hubContext)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _hubContext = hubContext;
        }

        public async Task DeleteOldFiles()
        {
            List<MyFile> myFiles = await  _db.MyFiles.ToListAsync();
            string path = "/Files/";
            string[] allFilesInFolder = Directory.GetFiles(_appEnvironment.WebRootPath + path);
            foreach (string file in allFilesInFolder) 
            {
                if(myFiles.Find(p => (p.FilePath + p.NameInServer) == file)==null)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
