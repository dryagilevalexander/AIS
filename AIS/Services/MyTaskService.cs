using AIS.ViewModels;
using Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AIS.Services
{
    public class MyTaskService : IMyTaskService
    {
        private CoreContext db;
        IWebHostEnvironment _appEnvironment;

        public MyTaskService(CoreContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IEnumerable<MyTask>> GetMyActiveTasks()
        {
            IEnumerable<MyTask> myTasksWithTaskStatus = await db.MyTasks
                                                               .Include(u => u.MyTaskStatus)
                                                               .Include(r => r.MyTaskLevelImportance).Where(p => p.MyTaskStatusId != 4).OrderByDescending(p => p.Id).ToListAsync();
            return myTasksWithTaskStatus;
        }

        public async Task<IEnumerable<MyTask>> GetMyActiveTasksWithCurrentUser(string currentUserId)
        {
            IEnumerable<MyTask> myTasks = await db.MyTasks
                .Include(u => u.MyTaskStatus)
                .Include(r => r.MyTaskLevelImportance).Where(p => p.MyTaskStatusId != 4).Where(p => p.SenderUserId == currentUserId || p.DestinationUserId == currentUserId).OrderByDescending(p => p.Id).ToListAsync();
            return myTasks;
        }

        public async Task<IEnumerable<MyTask>> GetMyArchiveTasks()
        {
            IEnumerable<MyTask> myTasksWithTaskStatus = await db.MyTasks
                                                               .Include(u => u.MyTaskStatus)
                                                               .Include(r => r.MyTaskLevelImportance).Where(p => p.MyTaskStatusId == 4).OrderByDescending(p => p.Id).ToListAsync();
            return myTasksWithTaskStatus;
        }

        public async Task<IEnumerable<MyTask>> GetMyArchiveTasksWithCurrentUser(string currentUserId)
        {
            IEnumerable<MyTask> myTasks = await db.MyTasks
                .Include(u => u.MyTaskStatus)
                .Include(r => r.MyTaskLevelImportance).Where(p => p.MyTaskStatusId == 4).Where(p => p.SenderUserId == currentUserId || p.DestinationUserId == currentUserId).OrderByDescending(p => p.Id).ToListAsync();
            return myTasks;
        }

        public async Task<IEnumerable<MyTaskStatus>> GetMyTaskStatuses()
        {
            IEnumerable<MyTaskStatus> taskStatuses = db.MyTaskStatuses.ToList();
            return taskStatuses;
        }

        public async Task<IEnumerable<LevelImportance>> GetMyTaskLevels()
        {
            IEnumerable<LevelImportance> taskLevel = db.LevelImportances.ToList();
            return taskLevel;
        }

        public async Task<List<MyTaskStatus>> GetMyTaskStatusesToList()
        {
            List<MyTaskStatus> taskStatuses = db.MyTaskStatuses.ToList();
            return taskStatuses;
        }

        public async Task<List<LevelImportance>> GetMyTaskLevelsToList()
        {
            List<LevelImportance> taskLevel = db.LevelImportances.ToList();
            return taskLevel;
        }

        public async Task<bool> DeleteMyTask(int? id)
        {
            if (id != null)
            {
                MyTask myTask = new MyTask { Id = id.Value };
                db.Entry(myTask).State = EntityState.Deleted;
                await db.SaveChangesAsync();

                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CreateTask(User destinationUser, MyTaskViewModel mtvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();
                MyTask myTask = new MyTask
                {
                    Name = mtvm.Name,
                    Description = mtvm.Description,
                    DateStart = mtvm.DateStart,
                    DateEnd = mtvm.DateEnd,
                    MyTaskStatusId = mtvm.MyTaskStatusId,
                    MyTaskLevelImportanceId = mtvm.MyTaskLevelImportanceId,
                    SenderUserId = mtvm.SenderUserId,
                    SenderUserName = mtvm.SenderUserName,
                    DestinationUserId = mtvm.DestinationUserId,
                    DestinationUserName = destinationUser.UserNickName
                };

                if (mtvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mtvm.Enclosure)
                    {
                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        myFiles.Add(myFile);
                    }
                }
                myTask.MyFiles = myFiles;
                db.MyTasks.Update(myTask);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> EditMyTask(User destinationUser, MyTaskViewModel mtvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();
                var currentTask = await db.MyTasks.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == mtvm.Id);

                currentTask.Name = mtvm.Name;
                currentTask.Description = mtvm.Description;
                currentTask.DateStart = mtvm.DateStart;
                currentTask.DateEnd = mtvm.DateEnd;
                currentTask.MyTaskStatusId = mtvm.MyTaskStatusId;
                currentTask.MyTaskLevelImportanceId = mtvm.MyTaskLevelImportanceId;
                currentTask.DestinationUserId = mtvm.DestinationUserId;
                currentTask.DestinationUserName = destinationUser.UserNickName;

                if (mtvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mtvm.Enclosure)
                    {
                        // путь к папке Files

                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        currentTask.MyFiles.Add(myFile);
                    }
                }

                db.MyTasks.Update(currentTask);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMyEnclosure(int? id)
        {
            try
            {
                if (id != null)
                {
                    var currentMyFile = await db.MyFiles.FirstOrDefaultAsync(p => p.Id == id);
                    db.Entry(currentMyFile).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<MyTask> GetMyTaskByIdEagerLoading(int id)
        {
            MyTask? myTask = await db.MyTasks.Include(u => u.MyFiles).Include(u => u.MySubTasks).FirstOrDefaultAsync(p => p.Id == id);
            return myTask;
        }

        public async Task<bool> CreateSubTask(MySubTaskViewModel mtvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();
                MySubTask mySubTask = new MySubTask
                {
                    MyTaskId = mtvm.MyTaskId,
                    Name = mtvm.Name,
                    Description = mtvm.Description,
                    DateStart = mtvm.DateStart,
                    DateEnd = mtvm.DateEnd,
                    MyTaskStatusId = mtvm.MyTaskStatusId,
                    MyTaskLevelImportanceId = mtvm.MyTaskLevelImportanceId,
                };

                if (mtvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mtvm.Enclosure)
                    {
                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        myFiles.Add(myFile);
                    }
                }
                mySubTask.MyFiles = myFiles;
                db.MySubTasks.Update(mySubTask);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditSubTask(MySubTaskViewModel mtvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();
                var currentSubTask = await db.MySubTasks.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == mtvm.Id);


                currentSubTask.Name = mtvm.Name;
                currentSubTask.Description = mtvm.Description;
                currentSubTask.DateStart = mtvm.DateStart;
                currentSubTask.DateEnd = mtvm.DateEnd;
                currentSubTask.MyTaskStatusId = mtvm.MyTaskStatusId;
                currentSubTask.MyTaskLevelImportanceId = mtvm.MyTaskLevelImportanceId;

                if (mtvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mtvm.Enclosure)
                    {
                        // путь к папке Files

                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        currentSubTask.MyFiles.Add(myFile);
                    }
                }

                db.MySubTasks.Update(currentSubTask);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMySubTask(int? id, MySubTask currentSubTask)
        {
            try
            {
                if (id != null)
                {
                    db.MySubTasks.Remove(currentSubTask);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

            public async Task<MySubTask> GetMySubTaskByIdWithFiles(int id)
        {
            return await db.MySubTasks.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
