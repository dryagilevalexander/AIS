using Core;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using AIS.ViewModels;

namespace AIS.Services
{
    public class LetterService: ILetterService
    {
        private CoreContext db;
        public LetterService(CoreContext coreContext)
        {
            db = coreContext;
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllShippingMethods() 
        {
          return await db.ShippingMethods.ToListAsync();
        }

        public async Task<IEnumerable<LetterType>> GetAllletterTypes()
        {
            return await db.LetterTypes.ToListAsync();
        }

        public async Task<bool> CreateLetter(LetterViewModel letterViewModel)
        {
            try
            { 
            Letter letter = new Letter
            {
                Number = letterViewModel.Number,
                DepartureDate = letterViewModel.DepartureDate,
                Name = letterViewModel.Name,
                Destination = letterViewModel.Destination,
                ShippingMethodId = letterViewModel.ShippingMethodId,
                LetterTypeId = letterViewModel.LetterTypeId
            };

            db.Letters.Add(letter);
            await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLetter(int? id)
        {
            try
            {
                if (id != null)
                {
                    Letter letter = new Letter { Id = id.Value };
                    db.Entry(letter).State = EntityState.Deleted;
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
        
        public async Task<Letter> GetLetterById(int id)
        {
            return await db.Letters.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Letter>> GetAllLettersEagerLoading()
        {
            return await db.Letters.Include(u => u.ShippingMethod).Include(u => u.LetterType).ToListAsync();
        }

        public async Task<bool> EditLetter(Letter letter)
        {
            try
            {
                db.Letters.Update(letter);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
