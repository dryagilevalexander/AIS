namespace AIS.Models
{
    public class CurrentContractData
    {
        string numberOfContract = "б/н";
        
        
        public string TypeContractId { get; set; }
        public string NumberOfContract {
            get
            {
                return numberOfContract;
            }
            set
            {
                if(value!="")
                {
                    numberOfContract = value;
                }
            }
        }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string PartnerId { get; set; }
        public string SubjectOfContract { get; set; }
        public string Cost { get; set; }
        public string TypeOfStateRegId { get; set; }
        public string? ArticleOfLawId { get; set; }
        public string DocumentTemplateId { get; set; }
    }
}
