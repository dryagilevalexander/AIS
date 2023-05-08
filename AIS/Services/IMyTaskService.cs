using AIS.ViewModels.ProcessViewModel;
using Core;

namespace AIS.Services
{
    public interface IMyTaskService
    {
        Task <IEnumerable<MyTask>> GetMyActiveTasks();
        Task<IEnumerable<MyTask>> GetMyActiveTasksWithCurrentUser(string currentUserId);
        Task<IEnumerable<MyTask>> GetMyArchiveTasks();
        Task<IEnumerable<MyTask>> GetMyArchiveTasksWithCurrentUser(string currentUserId);
        Task<IEnumerable<MyTaskStatus>> GetMyTaskStatuses();
        Task<IEnumerable<LevelImportance>> GetMyTaskLevels();
        Task<bool> DeleteMyTask(int? id);
        Task<bool> CreateTask(User destinationUser, MyTaskViewModel mtvm);
        Task<bool> EditMyTask(User destinationUser, MyTaskViewModel mtvm);
        Task<bool> DeleteMyEnclosure(int? id);
        Task<MyTask> GetMyTaskByIdEagerLoading(int id);
        Task<bool> CreateSubTask(MySubTaskViewModel mtvm);
        Task<bool> EditSubTask(MySubTaskViewModel mtvm);
        Task<MySubTask> GetMySubTaskByIdWithFiles(int id);
        Task<List<MyTaskStatus>> GetMyTaskStatusesToList();
        Task<List<LevelImportance>> GetMyTaskLevelsToList();
        Task<bool> DeleteMySubTask(int? id, MySubTask currentSubTask);

    }
}
