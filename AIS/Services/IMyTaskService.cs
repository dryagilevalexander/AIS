using AIS.ViewModels.TasksViewModels;
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
        Task DeleteMyTask(int id);
        Task CreateTask(User destinationUser, MyTaskViewModel mtvm);
        Task EditMyTask(User destinationUser, MyTaskViewModel mtvm);
        Task DeleteMyEnclosure(int id);
        Task<MyTask> GetMyTaskByIdEagerLoading(int id);
        Task CreateSubTask(MySubTaskViewModel mtvm);
        Task EditSubTask(MySubTaskViewModel mtvm);
        Task<MySubTask> GetMySubTaskByIdWithFiles(int id);
        Task<List<MyTaskStatus>> GetMyTaskStatusesToList();
        Task<List<LevelImportance>> GetMyTaskLevelsToList();
        Task DeleteMySubTask(MySubTask currentSubTask);

    }
}
