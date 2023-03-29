using Core;
using AIS.Models;
using static AIS.Controllers.ProcessController;

namespace AIS.Services
{
    public interface IDocumentConstructor
    {
        string Replacing(string appPath, string replacingTemplateName, string outputFileName);
        void SetReplacePatterns(Dictionary<string, string> replacePatterns);
        string CostInWords(decimal n);
        Dictionary<string, string> GetReplacePatterns(int partnerTypeId, Partner partner, Partner headOrganization, CurrentContractData? dcvm, DirectorType? headDirectorType, DirectorType? directorType);
    }
}
