using Microsoft.EntityFrameworkCore;

namespace OpenXML
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<SubCondition> SubConditions { get; set; }
        public DbSet<SubConditionParagraph> SubConditionParagraphs { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<ContractTemplate> ContractTemplates { get; set; }
        public DbSet<DirectorType> DirectorTypes { get; set; }

        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=openXML;Username=postgres;Password=12345");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractTemplate>().HasData(
            new ContractTemplate[]
            {
                new ContractTemplate {Id=1, Name = "Договор (общие)", ContractType = 0},
                new ContractTemplate {Id=2, Name = "Договор подряда", ContractType = 1},
                new ContractTemplate {Id=3, Name = "Договор оказания услуг", ContractType = 1}

            });

            modelBuilder.Entity<Contragent>().HasData(
            new Contragent[]
            {
                new Contragent {Id=1, Name = "Муниципальное унитарное предприятие \"Энергетический ресурс\" Некрасовского муниципального района", IsMain = true,  INN ="7701071", KPP="7701001", ShortName ="МУП \"Энергоресурс\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Советская, д. 175", OGRN = "1235550001", Bank ="ПАО \"Сбербанк\"", Account ="40301030000000065", CorrespondentAccount = "3010110000000022", BIK = "7752251",  DirectorName = "Иванов И.И.", DirectorNameR = "Иванова И.И.", DirectorTypeId = 1},
                new Contragent {Id=2, Name = "ООО \"Сервисное предприятие авторемонт\"", IsMain = false, INN ="7701051", KPP="7701001", ShortName ="ООО \"Авторемонт\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Пролетарская, д. 11", OGRN = "3315350022", Bank ="ПАО \"Сбербанк\"", Account ="403221030000010072", CorrespondentAccount = "3010150000000133", BIK = "7752251",DirectorName = "Капралов Д.М.", DirectorNameR = "Капралова Д.М.", DirectorTypeId = 2},
                new Contragent {Id=3, Name = "ООО Муниципальное образовательное учреждение \"Некрасовская средняя общеобразовательная школа\"", IsMain = false, INN ="7701031", KPP="7701002", ShortName ="МОУ \"Некрасовская средняя школа\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Матросова, д. 17", OGRN = "3411330222", Bank ="ПАО \"Сбербанк\"", Account ="403551030012010078", CorrespondentAccount = "3010250001000123", BIK = "7752251", DirectorName = "Сергеев А.Р.", DirectorNameR = "Сергеева А.Р.", DirectorTypeId = 1}
            });

            modelBuilder.Entity<Condition>().HasData(
            new Condition[]
            {
                new Condition {Id = 1, Name = "Договор contractType № __", TypeOfCondition = 1, ContractTemplateId = 1, RegulationType = 4},
                new Condition {Id = 2, Name = "Преамбула", Text = "customerName именуемое в дальнейшем \"Заказчик\", в лице customerDirectorTypeNameR customerDirectorNameR, действующего на основании Устава, с одной стороны, и executorName, именуемое в дальнейшем \"executor\", в лице executorDirectorTypeNameR executorDirectorNameR, действующего на основании Устава, с другой стороны, baseOfContract заключили настоящий договор о нижеследующем:", TypeOfCondition = 2, ContractTemplateId = 1, RegulationType = 4},

                new Condition {Id = 3, Name = "Предмет договора", TypeOfCondition = 3, ContractTemplateId =  2, RegulationType = 4},
                new Condition {Id = 4, Name = "Права и обязанности сторон", TypeOfCondition = 3, ContractTemplateId = 2, RegulationType = 4},
                new Condition {Id = 5, Name = "Ответственность сторон", TypeOfCondition = 3, ContractTemplateId = 2, RegulationType = 3},

                new Condition {Id = 6, Name = "Предмет договора", TypeOfCondition = 3, ContractTemplateId =  3, RegulationType = 4},
                new Condition {Id = 7, Name = "Права и обязанности сторон", TypeOfCondition = 3, ContractTemplateId = 3, RegulationType = 4},
                new Condition {Id = 8, Name = "Ответственность сторон", TypeOfCondition = 3, ContractTemplateId = 3, RegulationType = 3},

            });

            modelBuilder.Entity<SubCondition>().HasData(
            new SubCondition[]
            {
                new SubCondition {Id=1, Text="За неисполнение или ненадлежащее исполнение Контракта Стороны несут ответственность в соответствии с законодательством Российской Федерации и условиями Контракта.", ConditionId = 5},
                new SubCondition {Id=2, Text="В случае полного (частичного) неисполнения условий Контракта одной из Сторон эта Сторона обязана возместить другой Стороне причиненные убытки в части, непокрытой неустойкой.", ConditionId = 5},
                new SubCondition {Id=3, Text="В случае просрочки исполнения Подрядчиком обязательств, предусмотренных Контрактом, Подрядчик уплачивает Заказчику пени. Пеня начисляется за каждый день просрочки исполнения Подрядчиком обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства. Размер пени составляет одна трехсотая действующей на дату уплаты пени ключевой ставки Центрального банка Российской Федерации от цены Контракта (отдельного этапа исполнения Контракта), уменьшенной на сумму, пропорциональную объему обязательств, предусмотренных Контрактом (соответствующим отдельным этапом исполнения Контракта) и фактически исполненных Подрядчиком.", ConditionId = 5},
                new SubCondition {Id=4, Text="В случае просрочки исполнения Заказчиком обязательств, предусмотренных Контрактом, Подрядчик вправе потребовать уплату пени в размере одной трехсотой действующей на дату уплаты пеней ключевой ставки Центрального банка Российской Федерации от не уплаченной в срок суммы. Пеня начисляется за каждый день просрочки исполнения обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства.", ConditionId = 5},
                new SubCondition {Id=5, Text="Применение неустойки (штрафа, пени) не освобождает Стороны от исполнения обязательств по Контракту.", ConditionId = 5},
                new SubCondition {Id=6, Text="В случае расторжения Контракта в связи с односторонним отказом Стороны от исполнения Контракта другая Сторона вправе потребовать возмещения только фактически понесенного ущерба, непосредственно обусловленного обстоятельствами, являющимися основанием для принятия решения об одностороннем отказе от исполнения Контракта.", ConditionId = 5},
                new SubCondition {Id=7, Text="Подрядчик обязуется выполнить по заданию Заказчика работу, указанную в пункте 1.2 настоящего договора, и сдать ее результат Заказчику, а Заказчик обязуется принять результат работы и оплатить его.", ConditionId = 3},
                new SubCondition {Id=8, Text="Подрядчик обязуется выполнить следующую работу: subjectOfContract, именуемую в дальнейшем \"Работа\".", ConditionId = 3},
                new SubCondition {Id=9, Text="Подрядчик обязуется:", ConditionId = 4},

                new SubCondition {Id=10, Text="За неисполнение или ненадлежащее исполнение Контракта Стороны несут ответственность в соответствии с законодательством Российской Федерации и условиями Контракта.", ConditionId = 8},
                new SubCondition {Id=11, Text="В случае полного (частичного) неисполнения условий Контракта одной из Сторон эта Сторона обязана возместить другой Стороне причиненные убытки в части, непокрытой неустойкой.", ConditionId = 8},
                new SubCondition {Id=12, Text="В случае просрочки исполнения Исполнителем обязательств, предусмотренных Контрактом, Исполнитель уплачивает Заказчику пени. Пеня начисляется за каждый день просрочки исполнения Исполнителем обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства. Размер пени составляет одна трехсотая действующей на дату уплаты пени ключевой ставки Центрального банка Российской Федерации от цены Контракта (отдельного этапа исполнения Контракта), уменьшенной на сумму, пропорциональную объему обязательств, предусмотренных Контрактом (соответствующим отдельным этапом исполнения Контракта) и фактически исполненных Исполнителем.", ConditionId = 8},
                new SubCondition {Id=13, Text="В случае просрочки исполнения Заказчиком обязательств, предусмотренных Контрактом, Исполнитель вправе потребовать уплату пени в размере одной трехсотой действующей на дату уплаты пеней ключевой ставки Центрального банка Российской Федерации от не уплаченной в срок суммы. Пеня начисляется за каждый день просрочки исполнения обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства.", ConditionId = 8},
                new SubCondition {Id=14, Text="Применение неустойки (штрафа, пени) не освобождает Стороны от исполнения обязательств по Контракту.", ConditionId = 8},
                new SubCondition {Id=15, Text="В случае расторжения Контракта в связи с односторонним отказом Стороны от исполнения Контракта другая Сторона вправе потребовать возмещения только фактически понесенного ущерба, непосредственно обусловленного обстоятельствами, являющимися основанием для принятия решения об одностороннем отказе от исполнения Контракта.", ConditionId = 8},
                new SubCondition {Id=16, Text="Исполнитель обязуется оказать по заданию Заказчика услуги, указанные в пункте 1.2 настоящего договора, и сдать ее результат Заказчику, а Заказчик обязуется принять результат оказания услуг и оплатить его.", ConditionId = 6},
                new SubCondition {Id=17, Text="Исполнитель обязуется оказать следующие услуги: subjectOfContract, именуемые в дальнейшем \"Услуги\".", ConditionId = 6},
                new SubCondition {Id=18, Text="Исполнитель обязуется:", ConditionId = 7},

            });

            modelBuilder.Entity<SubConditionParagraph>().HasData(
            new SubConditionParagraph[]
            {
                new SubConditionParagraph {Id=1, Text="Подрядчик обязуется выполнить Работу с надлежащим качеством, из своих материалов, своими силами и средствами.", SubConditionId = 9},
                new SubConditionParagraph {Id=2, Text="Подрядчик обязуется выполнить Работу в срок до dateEnd г.", SubConditionId = 9},

                new SubConditionParagraph {Id=3, Text="Исполнитель обязуется оказать услуги с надлежащим качеством, своими силами и средствами.", SubConditionId = 18},
                new SubConditionParagraph {Id=4, Text="Исполнитель обязуется оказать услуги в срок до dateEnd г.", SubConditionId = 18}
            });

            modelBuilder.Entity<DirectorType>().HasData(
            new DirectorType[]
            {
                new DirectorType {Id=1, Name = "Директор", NameR = "Директора"},
                new DirectorType {Id=2, Name = "Генеральный директор", NameR = "Генерального директора"},
                new DirectorType {Id=3, Name = "Заведующий", NameR = "Заведующего"},
                new DirectorType {Id=4, Name = "Глава", NameR = "Главы"}
            });
        }
    }
}
