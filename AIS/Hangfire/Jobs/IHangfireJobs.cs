namespace AIS.Hangfire.Jobs
{
    public interface IHangfireJobs
    {
        Task DeleteOldFiles();
    }
}
