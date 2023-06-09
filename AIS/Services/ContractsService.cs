﻿using Infrastructure;
using Infrastructure.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIS.ErrorManager;
using System.Net;
using AIS.ViewModels.ContractsViewModels;

namespace AIS.Services
{
    public class ContractsService: IContractsService
    {
        private AisDbContext db;
        IWebHostEnvironment _appEnvironment;
        public ContractsService(AisDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }
        public async Task<List<Contract>> GetActiveContractsEagerLoading()
        {
            return await db.Contracts
                           .Include(u => u.Partner)
                           .Include(u => u.MyContractStatus)
                           .Where(p => p.MyContractStatusId != 6)
                           .ToListAsync();
        }

        public async Task<List<Contract>> GetArchiveContractsEagerLoading()
        {
            return await db.Contracts
                           .Include(u => u.Partner)
                           .Include(u => u.MyContractStatus)
                           .Where(p => p.MyContractStatusId == 6)
                           .ToListAsync();
        }

        public async Task<IEnumerable<TypeOfContract>> GetTypeOfContracts()
        { 
            return await db.TypeOfContracts.ToListAsync();
        }

        public async Task<IEnumerable<TypeOfStateReg>> GetTypeOfStateRegs()
        {
            return await db.TypeOfStateRegs.ToListAsync();
        }
        public async Task<TypeOfStateReg> GetTypeOfStateRegById(int id)
        {
            return await db.TypeOfStateRegs.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ArticleOfLaw>> GetArticleOfLaws()
        {
            return await db.ArticleOfLaws.ToListAsync();
        }

        public async Task<IEnumerable<MyContractStatus>> GetMyContractStatuses()
        {
            return await db.MyContractStatuses.ToListAsync();
        }

        public async Task CreateContract(CreateContractViewModel model, int typeOfContract)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();

                Contract myContract = new Contract
                {
                    TypeOfContract = typeOfContract,
                    NumberOfContract = model.NumberOfContract,
                    MyContractStatusId = model.MyContractStatusId,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd,
                    PartnerOrganizationId = model.PartnerOrganizationId,
                    SubjectOfContract = model.SubjectOfContract,
                    Cost = model.Cost
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
                myContract.MyFiles = myFiles;
                db.Contracts.Update(myContract);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать контракт", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditContract(EditContractViewModel model)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();

                Contract? contract = await db.Contracts.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == model.Id);

                contract.NumberOfContract = model.NumberOfContract;
                contract.DateStart = model.DateStart;
                contract.DateEnd = model.DateEnd;
                contract.PartnerOrganizationId = model.PartnerOrganizationId;
                contract.MyContractStatusId = model.MyContractStatusId;
                contract.SubjectOfContract = model.SubjectOfContract;
                contract.Cost = model.Cost;

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
                contract.MyFiles = myFiles;
                db.Contracts.Update(contract);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось сохранить контракт", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteContract(int id)
        {
            try
            {
                    Contract myContract = db.Contracts.Include(r => r.MyFiles).FirstOrDefault(p => p.Id == id);
                    List<MyFile> myFiles = myContract.MyFiles;
                    foreach (var myFile in myFiles)
                    {
                        db.MyFiles.Remove(myFile);
                        await db.SaveChangesAsync();
                    }
                    db.Contracts.Remove(myContract);
                    await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось удалить контракт", HttpStatusCode.BadRequest);
            }
        }

        public async Task <IEnumerable<TypeOfDocument>> GetTypesOfDocument()
        {
            return await db.TypesOfDocument.ToListAsync();
        }

        public async Task<Contract> GetContractByIdWithMyFiles(int id)
        {
            return await db.Contracts.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == id);
        }

        public RootTemplate GetRootTemplateById(int id)
        {
            try
            {
                RootTemplate? RootTemplate = db.RootTemplates
                    .FirstOrDefault(p => p.Id == id);
                return RootTemplate;
            }
            catch
            {
                throw new AisException("Не удалось получить корневой шаблон документа", HttpStatusCode.BadRequest);
            }
        }

        //Метод установки реквизитов контрагентов контракта
        public DocumentModel SetContractRequisites(DocumentModel contract, bool isCustomer, Partner mainOrganization, Partner contragent)
        {
            if (isCustomer == true)
            {
                contract.CustomerProp = GetRequisites(mainOrganization);
                contract.ExecutorProp = GetRequisites(contragent);
            }
            else
            {
                contract.CustomerProp = GetRequisites(contragent);
                contract.ExecutorProp = GetRequisites(mainOrganization);
            }

            return contract;
        }

        //Метод получения реквизитов
        private Dictionary<string, string> GetRequisites(Partner contragent)
        {
            Dictionary<string, string> props = new Dictionary<string, string>()
            {
                    {contragent.ShortName,""},
                    {"ИНН", contragent.INN},
                    {"КПП", contragent.KPP},
                    {"ОГРН", contragent.OGRN},
                    {"Адрес", contragent.Address},
                    {"Банк", contragent.Bank},
                    {"БИК", contragent.BIK},
                    {"р/с", contragent.Account},
                    {"к/с", contragent.CorrespondentAccount},
                    {contragent.DirectorType.Name + " _________ ", contragent.DirectorName}
            };
            return props;
        }
    }
}
