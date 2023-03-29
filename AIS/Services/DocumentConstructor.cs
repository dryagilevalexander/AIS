using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Core;
using AIS.Models;

namespace AIS.Services
{
    public class DocumentConstructor : IDocumentConstructor
    {
        private static Dictionary<string, string> _replacePatterns;
        private const string DocumentResourcesDirectory = "\\Files\\Templates\\";
        private const string DocumentOutputDirectory = "\\Files\\Output\\";

        private static string[] nums_1_9 = "ноль один два три четыре пять шесть семь восемь девять".Split();
        private static string[] nums_10_19 = "десять одиннадцать двенадцать тринадцать четырнадцать пятнадцать шестнадцать семнадцать восемнадцать девятнадцать".Split();
        private static string[] nums_20_90 = "ноль десять двадцать тридцать сорок пятьдесят шестьдесят семьдесят восемьдесят девяносто".Split();
        private static string[] nums_100_900 = "ноль сто двести триста четыреста пятьсот шестьсот семьсот восемьсот девятьсот".Split();
        private static string[] razrad = @" тысяч миллион миллиард триллион квадриллион квинтиллион секстиллион септиллион октиллион нониллион дециллион андециллион дуодециллион тредециллион кваттордециллион квиндециллион сексдециллион септемдециллион октодециллион новемдециллион вигинтиллион анвигинтиллион дуовигинтиллион тревигинтиллион кватторвигинтиллион квинвигинтиллион сексвигинтиллион септемвигинтиллион октовигинтиллион новемвигинтиллион тригинтиллион антригинтиллион".Split();


        public void SetReplacePatterns(Dictionary<string, string> replacePatterns)
        {
            _replacePatterns = replacePatterns;
        }


        public string Replacing(string appPath, string replacingTemplateName, string outputFileName)
        {
            using (var document = DocX.Load(appPath + DocumentConstructor.DocumentResourcesDirectory + replacingTemplateName))
            {
                // Check if some of the replace patterns are used in the loaded document.
                if (document.FindUniqueByPattern(@"<[\w \=]{4,}>", RegexOptions.IgnoreCase).Count > 0)
                {
                    // Do the replacement of all the found tags and with green bold strings.
                    var replaceTextOptions = new FunctionReplaceTextOptions()
                    {
                        FindPattern = "<(.*?)>",
                        RegexMatchHandler = DocumentConstructor.ReplaceFunc,
                        RegExOptions = RegexOptions.IgnoreCase,
                        NewFormatting = new Formatting() { Bold = true, FontColor = System.Drawing.Color.Black, FontFamily = new Xceed.Document.NET.Font("Times New Roman") }
                    };
                    document.ReplaceText(replaceTextOptions);

                    // Save this document to disk.
                    document.SaveAs(appPath + DocumentConstructor.DocumentOutputDirectory + outputFileName);
                }

            }
            return DocumentOutputDirectory + outputFileName;
        }

        private static string ReplaceFunc(string findStr)
        {
            if (_replacePatterns.ContainsKey(findStr))
            {
                return _replacePatterns[findStr];
            }
            return findStr;
        }

       public string CostInWords(decimal n)
        {
            string cost = Math.Truncate(n).ToString();
            string costInWords = "";


        foreach (var s in solve(splitIntoCategories(cost)))
            {
                costInWords = costInWords + s + " ";
            }
            return costInWords.Trim();
        }

        //разбить на разряды
        private static IEnumerable<string> splitIntoCategories(string s)
        {
            s = s.PadLeft(s.Length + 3 - s.Length % 3, '0');
            return Enumerable.Range(0, s.Length / 3).Select(i => s.Substring(i * 3, 3));
        }
        //вывести название цифр в разряде
        private static IEnumerable<string> solve(IEnumerable<string> n)
        {
            var ii = 0;
            foreach (var s in n)
            {
                var countdown = n.Count() - ++ii;
                yield return
                    String.Format(@"{0} {1} {2} {3}",
                        s[0] == '0' ? "" : nums_100_900[getDigit(s[0])],
                        getE1(s[1], s[2]),
                        getE2(s[1], s[2], countdown),
                        s == "000" ? "" : getRankName(s, countdown)
                    );
            }

        }
        //вторая цифра разряда
        private static string getE1(char p1, char p2)
        {
            if (p1 != '0')
            {
                if (p1 == '1')
                    return nums_10_19[getDigit(p2)];
                return nums_20_90[getDigit(p1)];
            }
            return "";
        }
        //третья цифра разряда
        private static string getE2(char p1, char p2, int cd)
        {
            if (p1 != '1')
            {
                if (p2 == '0') return "";
                return (p2 == '2' && cd == 1) ? "две" : nums_1_9[getDigit(p2)];
            }
            return "";
        }

        private static int getDigit(char p1)
        {
            return Int32.Parse(p1.ToString());
        }
        //вывести название разрядов
        private static string getRankName(string s, int ii)
        {
            if (ii == 0) return "";
            var r = razrad[ii];
            //10 11 ...
            if (s[1] == '1') return r + (ii == 1 ? "" : "ов");

            if (new[] { '2', '3', '4' }.Contains(s[2]))
            {
                return r + (ii == 1 ? "и" : "а");
            }
            else
                return r + (ii == 1 ? "" : "ов");
        }

        public Dictionary<string, string> GetReplacePatterns(int partnerTypeId, Partner partner, Partner headOrganization, CurrentContractData? dcvm, DirectorType? headDirectorType, DirectorType? directorType)
        {
            string nameOfContract = "";
            string baseOfContract = "";
            string dateOfBirth = "";

            if (Convert.ToInt32(dcvm.TypeOfStateRegId) != 1)
            {
                nameOfContract = "Договор";
                if (Convert.ToInt32(dcvm.TypeOfStateRegId) == 2) //223-ФЗ
                    baseOfContract = "на основании Федерального закона 'О закупках товаров, работ, услуг отдельными видами юридических лиц' от 18.07.2011 N 223-ФЗ,";
                else if (Convert.ToInt32(dcvm.TypeOfStateRegId) == 3) //ГК
                {
                    baseOfContract = "";
                }
            }
            else //44-ФЗ
            {
                nameOfContract = "Контракт";
                if (dcvm.ArticleOfLawId is not null)
                {
                    if (Convert.ToInt32(dcvm.ArticleOfLawId) == 1)
                        baseOfContract = "на основании пункта 8 части 1 статьи 93 Федерального Закона от 05.04.2013 № 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд',";
                    else if (Convert.ToInt32(dcvm.ArticleOfLawId) == 2)
                        baseOfContract = "на основании части 4 статьи 93 Федерального Закона от 05.04.2013 № 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд',";

                }
            }


            Dictionary<string, string>? _replacePatterns = null;
            if (partnerTypeId == 1 || partnerTypeId == 2)
            {
                _replacePatterns = new Dictionary<string, string>()
            {
                { "PARTNERFNAME", partner.Name },
                { "PARTNERSHORTNAME", partner.ShortName },
                { "ADDRESS", partner.Address},
                { "INN", partner.INN },

                { "NAMEOFCONTRACT", nameOfContract },
                { "BASEOFCONTRACT", baseOfContract },
                { "NUMBER", dcvm.NumberOfContract},
                { "DATESTART", ((DateTime)dcvm.DateStart).ToString("dd.MM.yyyy") },
                { "DATEEND", ((DateTime)dcvm.DateEnd).ToString("dd.MM.yyyy") },
                { "SUBJECTOFCONTRACT", dcvm.SubjectOfContract},
                { "COST", Math.Truncate(Convert.ToDecimal(dcvm.Cost)).ToString() },
                { "COSTINWORDS", CostInWords(Convert.ToDecimal(dcvm.Cost)) },
                { "COIN", ((Convert.ToDecimal(dcvm.Cost)-Math.Truncate(Convert.ToDecimal(dcvm.Cost))).ToString()).Replace("0.","") },

                { "PARTNERDIR", directorType.Name },
                { "PARTNERDIRNAME", partner.DirectorName},
                { "KPP", partner.KPP },
                { "OGRN", partner.OGRN },
                { "ACCOUNT", partner.Account },
                { "CORRACCOUNT", partner.CorrespondentAccount},
                { "BANK", partner.Bank },
                { "BIK", partner.BIK },

                { "HEADFNAME", headOrganization.Name },
                { "HEADDIR", headDirectorType.Name },
                { "HEADDIRNAME", headOrganization.DirectorName},
                { "HEADSHORTNAME", headOrganization.ShortName },
                { "HEADADDRESS", headOrganization.Address},
                { "HEADINN", headOrganization.INN },
                { "HEADKPP", headOrganization.KPP },
                { "HEADOGRN", headOrganization.OGRN },
                { "HEADACCOUNT", headOrganization.Account },
                { "HEADCORRACCOUNT", headOrganization.CorrespondentAccount},
                { "HEADBANK", headOrganization.Bank },
                { "HEADBIK", headOrganization.BIK },

                { "PASSSERIES", "" },
                { "PASSNUMBER", "" },
                { "PASSDATEI", "" },
                { "DATEBIRTH", "" },
                { "PASSPLASEI", "" },
                { "PASSDIVCODE", "" }
            };
            }
            else if (partnerTypeId == 3)
            {
                if(partner.PassportDateOfBirth.HasValue)
                {
                    dateOfBirth = ((DateTime)partner.PassportDateOfBirth).ToString("dd.MM.yyyy");
                }
                _replacePatterns = new Dictionary<string, string>()
            {
                { "PARTNERFNAME", partner.Name },
                { "PARTNERSHORTNAME", partner.ShortName },
                { "ADDRESS", partner.Address},
                { "INN", partner.INN },

                { "NAMEOFCONTRACT", nameOfContract },
                { "BASEOFCONTRACT", baseOfContract },
                { "NUMBER", dcvm.NumberOfContract},
                { "DATESTART", ((DateTime)dcvm.DateStart).ToString("dd.MM.yyyy") },
                { "DATEEND", ((DateTime)dcvm.DateEnd).ToString("dd.MM.yyyy") },
                { "SUBJECTOFCONTRACT", dcvm.SubjectOfContract},
                { "COST", Math.Truncate(Convert.ToDecimal(dcvm.Cost)).ToString() },
                { "COSTINWORDS", CostInWords(Convert.ToDecimal(dcvm.Cost)) },
                { "COIN", ((Convert.ToDecimal(dcvm.Cost)-Math.Truncate(Convert.ToDecimal(dcvm.Cost))).ToString()).Replace("0.","") },

                { "PARTNERDIR", "" },
                { "PARTNERDIRNAME", ""},
                { "KPP", "" },
                { "OGRN", "" },
                { "ACCOUNT", "" },
                { "CORRACCOUNT", ""},
                { "BANK", "" },
                { "BIK", "" },

                { "HEADFNAME", headOrganization.Name },
                { "HEADDIR", headDirectorType.Name },
                { "HEADDIRNAME", headOrganization.DirectorName},
                { "HEADSHORTNAME", headOrganization.ShortName },
                { "HEADADDRESS", headOrganization.Address},
                { "HEADINN", headOrganization.INN },
                { "HEADKPP", headOrganization.KPP },
                { "HEADOGRN", headOrganization.OGRN },
                { "HEADACCOUNT", headOrganization.Account },
                { "HEADCORRACCOUNT", headOrganization.CorrespondentAccount},
                { "HEADBANK", headOrganization.Bank },
                { "HEADBIK", headOrganization.BIK },

                { "PASSSERIES", partner.PassportSeries},
                { "PASSNUMBER", partner.PassportNumber },
                { "PASSDATEI",((DateTime)partner.PassportDateOfIssue).ToString("dd.MM.yyyy") },
                { "DATEBIRTH", dateOfBirth },
                { "PASSPLASEI", partner.PassportPlaseOfIssue },
                { "PASSDIVCODE", partner.PassportDivisionCode}
                };
            }
            return _replacePatterns;
        }
    }
}

