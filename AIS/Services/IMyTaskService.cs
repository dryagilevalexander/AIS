using AIS.ViewModels.TasksViewModels;
using Infrastructure;
using Infrastructure.Models;

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
        Task CreateTask(User destinationUser, CreateTaskViewModel model);
        Task EditMyTask(User destinationUser, EditTaskViewModel model);
        Task<MyTask> GetMyTaskByIdEagerLoading(int id);
        Task CreateSubTask(CreateSubTaskViewModel model);
        Task EditSubTask(EditSubTaskViewModel model);
        Task<MySubTask> GetMySubTaskByIdWithFiles(int id);
        Task<List<MyTaskStatus>> GetMyTaskStatusesToList();
        Task<List<LevelImportance>> GetMyTaskLevelsToList();
        Task DeleteMySubTask(MySubTask currentSubTask);
        Task<IEnumerable<MyTask>> GetRequiredDateTasks(string currentUserId, string date);

    }
}
