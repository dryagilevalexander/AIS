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

        public async Task<List<CommonContractTemplate>> GetCommonContractTemplates()
        {
            return await db.CommonContractTemplates.ToListAsync();
        }

        public async Task<CommonContractTemplate> GetCommonContractTemplateWithContractTemplatesById(int id)
        {
            return await db.CommonContractTemplates.Include(p => p.ContractTemplates).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ContractTemplate>> GetContractTemplates()
        {
            return await db.ContractTemplates.ToListAsync();
        }

        public async Task<List<TypeOfCondition>> GetTypesOfCondition()
        {
            return await db.TypesOfCondition.ToListAsync();
        }

        public async Task<bool> CreateCommonContractTemplate(CommonContractTemplateViewModel cctvm)
        {
            try
            {
               CommonContractTemplate commonContractTemplate = new CommonContractTemplate
                {
                    Name = cctvm.Name,
                    Description = cctvm.Description,
                    Title = cctvm.Title,
                    Preamble = cctvm.Preamble
                };

                db.CommonContractTemplates.Add(commonContractTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditCommonContractTemplate(CommonContractTemplateViewModel cctvm)
        {
            try
            {
                CommonContractTemplate commonContractTemplate = await db.CommonContractTemplates.FirstOrDefaultAsync(p => p.Id == cctvm.Id);
                commonContractTemplate.Name = cctvm.Name;
                commonContractTemplate.Description = cctvm.Description;
                commonContractTemplate.Title = cctvm.Title;
                commonContractTemplate.Preamble = cctvm.Preamble;

                db.CommonContractTemplates.Update(commonContractTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCommonContractTemplate(int? id)
        {
            if (id != null)
            {
                CommonContractTemplate commonContractTemplate = await db.CommonContractTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.CommonContractTemplates.Remove(commonContractTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> CreateContractTemplate(ContractTemplateViewModel ctvm)
        {

                ContractTemplate contractTemplate = new ContractTemplate
                {
                    Name = ctvm.Name,
                    Description = ctvm.Description,
                    TypeOfContractId = ctvm.TypeOfContractId,
                    CommonContractTemplateId = ctvm.CommonContractTemplateId
                };

                db.ContractTemplates.Add(contractTemplate);
                await db.SaveChangesAsync();
                return true;

        }

        public async Task<bool> EditContractTemplate(ContractTemplateViewModel ctvm)
        {
            try
            {
                ContractTemplate contractTemplate = await db.ContractTemplates.FirstOrDefaultAsync(p => p.Id == ctvm.Id);
                contractTemplate.Name = ctvm.Name;
                contractTemplate.Description = ctvm.Description;
                contractTemplate.TypeOfContractId = ctvm.TypeOfContractId;

                db.ContractTemplates.Update(contractTemplate);
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
                Condition condition = new Condition
                {
                    Name = cvm.Name,
                    Text = cvm.Text,
                    TypeOfConditionId = cvm.TypeOfConditionId,
                    TypeOfStateRegId = cvm.TypeOfStateRegId,
                    ContractTemplateId = cvm.ContractTemplateId
                };

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
                condition.Name = cvm.Name;
                condition.Text = cvm.Text;
                condition.TypeOfConditionId = cvm.TypeOfConditionId;
                condition.TypeOfStateRegId = cvm.TypeOfStateRegId;
                condition.ContractTemplateId = cvm.ContractTemplateId;

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
                ConditionId = scvm.ConditionId

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
                    SubConditionId = scpvm.SubConditionId

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

        public async Task<ContractTemplate?> GetContractTemplateById(int id)
        {
            try
            {
                ContractTemplate? contractTemplate = await db.ContractTemplates
                    .FirstOrDefaultAsync(p => p.Id == id);
                return contractTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ContractTemplate?> GetContractTemplateWithConditionsById(int id)
        {
            try
            {
                ContractTemplate? contractTemplate = await db.ContractTemplates
                    .Include(p => p.Conditions).ThenInclude(p => p.SubConditions).ThenInclude(p => p.SubConditionParagraphs)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return contractTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ContractTemplate?> GetContractTemplateWithTypeOfContractById(int id)
        {
            try
            {
                ContractTemplate? contractTemplate = await db.ContractTemplates
                    .Include(p => p.TypeOfContract)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return contractTemplate;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteContractTemplate(int? id)
        {
            if (id != null)
            {
                ContractTemplate contractTemplate = await db.ContractTemplates.FirstOrDefaultAsync(p => p.Id == id);
                db.ContractTemplates.Remove(contractTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<List<Condition>> GetConditions()
        {
            return await db.Conditions.Include(p => p.SubConditions).ToListAsync();
        }

        public async Task<bool> CreateCondition(Condition condition)
        {
            try
            {
                db.Conditions.Add(condition);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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
