using Infrastructure;
using Infrastructure.Models;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using AIS.ViewModels.ProcessViewModels;
using System.Net;
using AIS.ErrorManager;
using System.ComponentModel.DataAnnotations;

namespace AIS.Services
{
    public class LetterService: ILetterService
    {
        private AisDbContext db;
        IWebHostEnvironment _appEnvironment;
        public LetterService(AisDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;   
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllShippingMethods() 
        {
          return await db.ShippingMethods.ToListAsync();
        }

        public async Task<IEnumerable<LetterType>> GetAllletterTypes()
        {
            return await db.LetterTypes.ToListAsync();
        }

        public async Task CreateLetter(CreateLetterViewModel model)
        {
            try
            {
            List<string> enclosure = new List<string>();
            List<MyFile> myFiles = new List<MyFile>();

            Letter letter = new Letter
            {
                Number = model.Number,
                DepartureDate = model.DepartureDate,
                Name = model.Name,
                Destination = model.Destination,
                ShippingMethodId = model.ShippingMethodId,
                LetterTypeId = model.LetterTypeId
            };

                if (model.Enclosure is not null)
                {
                    foreach (var uploadedFile in model.Enclosure)
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
            letter.MyFiles = myFiles;
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

        public async Task EditLetter(EditLetterViewModel model)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();
                var currentLetter = await db.Letters.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == model.Id);

                currentLetter.Number = model.Number;
                currentLetter.Name = model.Name;
                currentLetter.DepartureDate = model.DepartureDate;
                currentLetter.Destination = model.Destination;
                currentLetter.ShippingMethodId = model.ShippingMethodId;
                currentLetter.LetterTypeId = model.LetterTypeId;

                if (model.Enclosure is not null)
                {
                    foreach (var uploadedFile in model.Enclosure)
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

                        currentLetter.MyFiles.Add(myFile);
                    }
                }


                db.Letters.Update(currentLetter);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось отредактировать документ", HttpStatusCode.BadRequest);
            }
        }
    }
}
