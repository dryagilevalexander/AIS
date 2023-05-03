using AIS.ViewModels;
using Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AIS.Services
{
    public class ConditionsService: IConditionsService
    {
        private CoreContext db;
        IWebHostEnvironment _appEnvironment;
        public ConditionsService(CoreContext coreContext, IWebHostEnvironment appEnvironment)
        {
            db = coreContext;
            _appEnvironment = appEnvironment;
        }

        public async Task<List<RootTemplate>> GetRootTemplates()
        {
            return await db.RootTemplates.ToListAsync();
        }

        public async Task<RootTemplate> GetRootTemplateById(int id)
        {
            return await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RootTemplate> GetRootTemplateWithDocumentTemplatesById(int id)
        {
            return await db.RootTemplates.Include(p => p.DocumentTemplates).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<DocumentTemplate>> GetDocumentTemplates()
        {
            return await db.DocumentTemplates.ToListAsync();
        }

        //Метод получения шаблона контракта с стандартными условиями для всех типов регулирования
        public DocumentTemplate GetDocumentTemplateEagerLoadingById(int id)
        {
            return db.DocumentTemplates.Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(c => c.SubConditionParagraphs).FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> CreateRootTemplate(RootTemplateViewModel cctvm)
        {
            try
            {
                RootTemplate RootTemplate = new RootTemplate();

                RootTemplate.Name = cctvm.Name;
                RootTemplate.Description = cctvm.Description;
                RootTemplate.TypeOfDocumentId = cctvm.TypeOfDocumentId;

                db.RootTemplates.Add(RootTemplate);
                await db.SaveChangesAsync();
                return true;
            }

            catch
            {
                return false;
            }
        }

        public async Task<bool> EditRootTemplate(RootTemplateViewModel cctvm)
        {
            try
            {
                RootTemplate RootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == cctvm.Id);
                RootTemplate.Name = cctvm.Name;
                RootTemplate.Description = cctvm.Description;
                RootTemplate.TypeOfDocumentId = cctvm.TypeOfDocumentId;

                db.RootTemplates.Update(RootTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRootTemplate(int? id)
        {
            if (id != null)
            {
                RootTemplate RootTemplate = await db.RootTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.RootTemplates.Remove(RootTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> CreateDocumentTemplate(DocumentTemplateViewModel ctvm)
        {

                DocumentTemplate documentTemplate = new DocumentTemplate
                {
                    Name = ctvm.Name,
                    Description = ctvm.Description,
                    RootTemplateId = ctvm.RootTemplateId,
                    TypeOfDocumentId = ctvm.TypeOfDocumentId
                };
            if (ctvm.TypeOfContractId != null) documentTemplate.TypeOfContractId = ctvm.TypeOfContractId;

                db.DocumentTemplates.Add(documentTemplate);
                await db.SaveChangesAsync();
                return true;
        }

        public async Task<bool> EditDocumentTemplate(DocumentTemplateViewModel ctvm)
        {
            try
            {
                DocumentTemplate DocumentTemplate = await db.DocumentTemplates.FirstOrDefaultAsync(p => p.Id == ctvm.Id);
                DocumentTemplate.Name = ctvm.Name;
                DocumentTemplate.Description = ctvm.Description;
                DocumentTemplate.TypeOfContractId = ctvm.TypeOfContractId;

                db.DocumentTemplates.Update(DocumentTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateCondition(ConditionViewModel cvm)
        {
            try
            {
                Condition condition = new Condition();

                    condition.Title = cvm.Title;
                    condition.Name = cvm.Name;
                    if (cvm.TypeOfDocumentId == 1) condition.TypeOfStateRegId = cvm.TypeOfStateRegId;
                    condition.DocumentTemplateId = cvm.DocumentTemplateId;
                    condition.NumLevelReference = cvm.NumLevelReference;
                    condition.Justification = cvm.Justification;
                    int numberOfConditions = await GetNumberOfConditionsInDocumentTemplate(condition.DocumentTemplateId);
                    condition.NumberInDocument = numberOfConditions + 1;

                db.Conditions.Add(condition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditCondition(ConditionViewModel cvm)
        {
            try
            {
                Condition condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == cvm.Id);
                condition.Title = cvm.Title;
                condition.Name = cvm.Name;
                if (cvm.TypeOfDocumentId == 1)  condition.TypeOfStateRegId = cvm.TypeOfStateRegId;
                condition.DocumentTemplateId = cvm.DocumentTemplateId;
                condition.NumLevelReference = cvm.NumLevelReference;
                condition.NumId = cvm.NumId;
                condition.Justification = cvm.Justification;
                db.Conditions.Update(condition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveCondition(Condition condition)
        {
            try
            {
                db.Conditions.Update(condition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateSubCondition(SubConditionViewModel scvm)
        {
            try
            {
                SubCondition subCondition = new SubCondition
                {
                Name = scvm.Name,
                Text = scvm.Text,
                ConditionId = scvm.ConditionId,
                NumLevelReference = scvm.NumLevelReference,
                NumId = scvm.NumId,
                Justification = scvm.Justification
                };

            db.SubConditions.Add(subCondition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditSubCondition(SubConditionViewModel scvm)
        {
            try
            {
                SubCondition subCondition = await db.SubConditions.FirstOrDefaultAsync(p => p.Id == scvm.Id);
                subCondition.Name = scvm.Name;
                subCondition.Text = scvm.Text;
                subCondition.ConditionId = scvm.ConditionId;
                subCondition.NumLevelReference = scvm.NumLevelReference;
                subCondition.NumId = scvm.NumId;
                subCondition.Justification = scvm.Justification;

                db.SubConditions.Update(subCondition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSubCondition(int? id)
        {
            if (id != null)
            {
                SubCondition subCondition = await db.SubConditions.FirstOrDefaultAsync(p => p.Id == id);
                db.SubConditions.Remove(subCondition);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CreateSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            try
            {
                SubConditionParagraph subConditionParagraph = new SubConditionParagraph
                {
                    Text = scpvm.Text,
                    SubConditionId = scpvm.SubConditionId,
                    NumLevelReference = scpvm.NumLevelReference,
                    NumId = scpvm.NumId,
                    Justification = scpvm.Justification
                };

                db.SubConditionParagraphs.Add(subConditionParagraph);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditSubConditionParagraph(SubConditionParagraphViewModel scpvm)
        {
            try
            {
                SubConditionParagraph subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == scpvm.Id);
                subConditionParagraph.Text = scpvm.Text;
                subConditionParagraph.SubConditionId = scpvm.SubConditionId;
                subConditionParagraph.NumLevelReference = scpvm.NumLevelReference;
                subConditionParagraph.NumId = scpvm.NumId;
                subConditionParagraph.Justification = scpvm.Justification;

                db.SubConditionParagraphs.Update(subConditionParagraph);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSubConditionParagraph(int? id)
        {
            if (id != null)
            {
                SubConditionParagraph subConditionParagraph = await db.SubConditionParagraphs.FirstOrDefaultAsync(p => p.Id == id);
                db.SubConditionParagraphs.Remove(subConditionParagraph);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<DocumentTemplate?> GetDocumentTemplateById(int id)
        {
            try
            {
                DocumentTemplate? DocumentTemplate = await db.DocumentTemplates
                    .FirstOrDefaultAsync(p => p.Id == id);
                return DocumentTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DocumentTemplate?> GetDocumentTemplateWithConditionsById(int id)
        {
            try
            {
                DocumentTemplate? DocumentTemplate = await db.DocumentTemplates
                    .Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return DocumentTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DocumentTemplate?> GetDocumentTemplateWithRootTemplateById(int id)
        {
            try
            {
                DocumentTemplate? DocumentTemplate = await db.DocumentTemplates
                    .Include(p => p.RootTemplate)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return DocumentTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DocumentTemplate?> GetDocumentTemplateWithTypeOfContractById(int id)
        {
            try
            {
                DocumentTemplate? DocumentTemplate = await db.DocumentTemplates
                    .Include(p => p.TypeOfContract)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return DocumentTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteDocumentTemplate(int? id)
        {
            if (id != null)
            {
                DocumentTemplate DocumentTemplate = await db.DocumentTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.DocumentTemplates.Remove(DocumentTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<List<Condition>> GetConditions()
        {
            return await db.Conditions.Include(p => p.SubConditions).ToListAsync();
        }

        public async Task<int> GetNumberOfConditionsInDocumentTemplate(int id)
        {
             DocumentTemplate DocumentTemplate = await db.DocumentTemplates.Include(p => p.Conditions).FirstOrDefaultAsync(p => p.Id == id);
             return DocumentTemplate.Conditions.Count();
        }

        public async Task<Condition?> GetCondition(int id)
        {
            try
            {
                Condition? condition = await db.Conditions
                    .Include(p=>p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return condition;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Condition?> GetConditionByNumberInDocument(int number)
        {
            try
            {
                Condition? condition = await db.Conditions
                    .FirstOrDefaultAsync(p => p.NumberInDocument == number);
                return condition;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SubCondition?> GetSubCondition(int id)
        {
            try
            {
                SubCondition? subCondition = await db.SubConditions
                    .Include(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return subCondition;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SubConditionParagraph?> GetSubConditionParagraph(int id)
        {
            try
            {
                SubConditionParagraph? subConditionParagraph = await db.SubConditionParagraphs
                    .FirstOrDefaultAsync(p => p.Id == id);
                return subConditionParagraph;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteCondition(int? id)
        {
            if (id != null)
            {
                Condition condition = await db.Conditions.FirstOrDefaultAsync(p => p.Id == id);               
                db.Conditions.Remove(condition);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
