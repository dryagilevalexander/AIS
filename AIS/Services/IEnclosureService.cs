using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IEnclosureService
    {
        Task<IEnumerable<MyFile>> GetMyEnclosuresBySubTaskId(int id);
        Task<IEnumerable<MyFile>> GetMyEnclosuresByTaskId(int id);
        Task<IEnumerable<MyFile>> GetMyEnclosuresByContractId(int id);
    }
}
