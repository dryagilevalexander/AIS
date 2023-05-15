using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIS.Hangfire.Jobs
{
    public class HangfireJobs: IHangfireJobs
    {
        private readonly AisDbContext _db;
        private IWebHostEnvironment _appEnvironment;

        public HangfireJobs(AisDbContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
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
