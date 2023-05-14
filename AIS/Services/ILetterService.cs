using AIS.ViewModels.ProcessViewModels;
using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface ILetterService
    {
        Task<IEnumerable<ShippingMethod>> GetAllShippingMethods();
        Task<IEnumerable<LetterType>> GetAllletterTypes();
        Task CreateLetter(CreateLetterViewModel letterViewModel);
        Task DeleteLetter(int id);
        Task<Letter> GetLetterById(int id);
        Task EditLetter(Letter letter);
        Task<List<Letter>> GetAllLettersEagerLoading();
    }
}