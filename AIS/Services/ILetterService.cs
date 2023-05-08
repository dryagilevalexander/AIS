using AIS.ViewModels.ProcessViewModels;
using Core;

namespace AIS.Services
{
    public interface ILetterService
    {
        Task<IEnumerable<ShippingMethod>> GetAllShippingMethods();
        Task<IEnumerable<LetterType>> GetAllletterTypes();
        Task<bool> CreateLetter(LetterViewModel letterViewModel);
        Task<bool> DeleteLetter(int? id);
        Task<Letter> GetLetterById(int id);
        Task<bool> EditLetter(Letter letter);
        Task<List<Letter>> GetAllLettersEagerLoading();
    }
}
