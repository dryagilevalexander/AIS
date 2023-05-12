using Infrastructure;
using Infrastructure.Models;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using AIS.ViewModels.ProcessViewModels;
using System.Net;
using AIS.ErrorManager;

namespace AIS.Services
{
    public class LetterService: ILetterService
    {
        private AisDbContext db;
        public LetterService(AisDbContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllShippingMethods() 
        {
          return await db.ShippingMethods.ToListAsync();
        }

        public async Task<IEnumerable<LetterType>> GetAllletterTypes()
        {
            return await db.LetterTypes.ToListAsync();
        }

        public async Task CreateLetter(LetterViewModel letterViewModel)
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
            }
            catch
            {
                throw new AisException("Не удалось создать документ", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteLetter(int id)
        {
            try
            {
                    Letter letter = new Letter { Id = id };
                    db.Entry(letter).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить документ", HttpStatusCode.BadRequest);
            }
        }
        
        public async Task<Letter> GetLetterById(int id)
        {
            Letter? letter = await db.Letters.FirstOrDefaultAsync(p => p.Id == id);
            if(letter == null) throw new AisException("Документ не найден", HttpStatusCode.BadRequest);
            return letter;
        }

        public async Task<List<Letter>> GetAllLettersEagerLoading()
        {
            return await db.Letters.Include(u => u.ShippingMethod).Include(u => u.LetterType).ToListAsync();
        }

        public async Task EditLetter(Letter letter)
        {
            try
            {
                db.Letters.Update(letter);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось отредактировать документ", HttpStatusCode.BadRequest);
            }
        }
    }
}
