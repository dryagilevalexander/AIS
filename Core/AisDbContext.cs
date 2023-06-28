using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure
{
    public class AisDbContext: IdentityDbContext<User>
    {
    public DbSet<Employee> Employeers { get; set; } = null!;
    public DbSet<Partner> Partners { get; set; } = null!;
    public DbSet<MyTask> MyTasks { get; set; } = null!;
    public DbSet<MyTaskStatus> MyTaskStatuses { get; set; } = null!;
    public DbSet<MyFile> MyFiles { get; set; } = null!;
    public DbSet<LevelImportance> LevelImportances { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<DirectorType> DirectorTypes { get; set; } = null!;
    public DbSet<TypeOfStateReg> TypeOfStateRegs { get; set; } = null!;
    public DbSet<ArticleOfLaw> ArticleOfLaws { get; set; } = null!;
    public DbSet<TypeOfContract> TypeOfContracts { get; set; } = null!;
    public DbSet<PartnerStatus> PartnerStatuses { get; set; } = null!;
    public DbSet<PartnerType> PartnerTypes { get; set; } = null!;
    public DbSet<MyContractStatus> MyContractStatuses { get; set; } = null!;
    public DbSet<MySubTask> MySubTasks { get; set; } = null!;
    public DbSet<Letter> Letters { get; set; } = null!;
    public DbSet<ShippingMethod> ShippingMethods { get; set; } = null!;
    public DbSet<LetterType> LetterTypes { get; set; } = null!;
    public DbSet<Condition> Conditions { get; set; }
    public DbSet<SubCondition> SubConditions { get; set; }
    public DbSet<SubConditionParagraph> SubConditionParagraphs { get; set; }
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
    public DbSet<RootTemplate> RootTemplates { get; set; }
    public DbSet<TypeOfDocument> TypesOfDocument { get; set; }
    public DbSet<PartnerCategory> PartnerCategories { get; set; }
        public AisDbContext(DbContextOptions<AisDbContext> options)
        : base(options)
        {            
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyFile>()
                .HasOne(p => p.MyTask)
                .WithMany(t => t.MyFiles)
                .OnDelete(DeleteBehavior.Cascade);
            //.WillCascadeOnDelete();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MySubTask>()
                .HasOne(p => p.MyTask)
                .WithMany(t => t.MySubTasks)
                .OnDelete(DeleteBehavior.Cascade);
            //.WillCascadeOnDelete();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LevelImportance>().HasData(
            new LevelImportance[]
            {
                new LevelImportance {Id=1, Name="Низкий", ClassView = "btn btn-secondary btn-block"},
                new LevelImportance {Id=2, Name="Средний", ClassView = "btn btn-warning btn-block" },
                new LevelImportance {Id=3, Name="Высокий", ClassView = "btn btn-danger btn-block"}
            });

            modelBuilder.Entity<MyTaskStatus>().HasData(
            new MyTaskStatus[]
            {
                new MyTaskStatus {Id=1, Name="Поступила"},
                new MyTaskStatus {Id=2, Name="В работе"},
                new MyTaskStatus {Id=3, Name="Выполнено"},
                new MyTaskStatus {Id=4, Name="В архиве"}
            });

            modelBuilder.Entity<DirectorType>().HasData(
            new DirectorType[]
            {
                new DirectorType {Id=1, Name="Директор", NameR = "Директора"},
                new DirectorType {Id=2, Name="Генеральный директор", NameR = "Генерального директора"},
                new DirectorType {Id=3, Name="Глава", NameR = "Главы"},
                new DirectorType {Id=4, Name="Заведующий", NameR = "Заведующего"},
            });

            modelBuilder.Entity<PartnerStatus>().HasData(
new PartnerStatus[]
{
                new PartnerStatus {Id=1, Name="Головная"},
                new PartnerStatus {Id=2, Name="Дочерняя"},
                new PartnerStatus {Id=3, Name="Контрагент"}
});

            modelBuilder.Entity<TypeOfStateReg>().HasData(
new TypeOfStateReg[]
{
                new TypeOfStateReg {Id=1, Name="44-ФЗ"},
                new TypeOfStateReg {Id=2, Name="223-ФЗ"},
                new TypeOfStateReg {Id=3, Name="ГК РФ"},
                new TypeOfStateReg {Id=4, Name="Универсальный"},

});
           
            modelBuilder.Entity<ArticleOfLaw>().HasData(
new ArticleOfLaw[]
{
                new ArticleOfLaw {Id=1, Name="пункт 8 части 1 44-ФЗ (коммунальные услуги)"},
                new ArticleOfLaw {Id=2, Name="часть 4 44-ФЗ (единственный поставщик)"},
});

            modelBuilder.Entity<TypeOfContract>().HasData(
new TypeOfContract[]
{
                new TypeOfContract {Id=1, Name="Договор подряда"},
                new TypeOfContract {Id=2, Name="Договор оказания услуг"},
                new TypeOfContract {Id=3, Name="Договор поставки"},
                new TypeOfContract {Id=4, Name="Договор аренды"}
});

            modelBuilder.Entity<PartnerType>().HasData(
            new PartnerType[]
            {
                new PartnerType {Id=1, Name="Юридическое лицо"},
                new PartnerType {Id=2, Name="Индивидуальный предприниматель"},
                new PartnerType {Id=3, Name="Физическое лицо"},
            });

            modelBuilder.Entity<PartnerCategory>().HasData(
            new PartnerCategory[]
            {
                new PartnerCategory {Id=1, Name="Государственный орган"},
                new PartnerCategory {Id=2, Name="Судебный орган"},
                new PartnerCategory {Id=3, Name="Организация"}
            });

            modelBuilder.Entity<MyContractStatus>().HasData(
            new MyContractStatus[]
            {
                new MyContractStatus {Id=1, Name="Подготовка"},
                new MyContractStatus {Id=2, Name="Согласование"},
                new MyContractStatus {Id=3, Name="Заключение"},
                new MyContractStatus {Id=4, Name="Исполнение"},
                new MyContractStatus {Id=5, Name="Завершен"},
                new MyContractStatus {Id=6, Name="В архиве"}
            });

            modelBuilder.Entity<ShippingMethod>().HasData(
            new ShippingMethod[]
            {
                new ShippingMethod {Id=1, Name="Нарочное"},
                new ShippingMethod {Id=2, Name="Email"},
                new ShippingMethod {Id=3, Name="Почтовое отправление"},
                new ShippingMethod {Id=4, Name="Электронный документооборот"}
            });

            modelBuilder.Entity<TypeOfDocument>().HasData(
            new TypeOfDocument[]
            {
                new TypeOfDocument {Id=1, Name="Контракт"},
                new TypeOfDocument {Id=2, Name="Заявление"},
                new TypeOfDocument {Id=3, Name="Исковое заявление"}
            });

            modelBuilder.Entity<LetterType>().HasData(
            new LetterType[]
            {
                new LetterType {Id=1, Name="Входящее"},
                new LetterType {Id=2, Name="Исходящее"}
            });

            modelBuilder.Entity<RootTemplate>().HasData(
            new RootTemplate[]
            {
                new RootTemplate {Id=1, Name="Общий шаблон договора", Description = "Определяет корневой шаблон для всех договоров", TypeOfDocumentId = 1},
                new RootTemplate {Id=2, Name="Заявления в судебные органы", Description = "Корневой шаблон заявлений в судебные органы", TypeOfDocumentId = 2}
            });

            modelBuilder.Entity<DocumentTemplate>().HasData(
            new DocumentTemplate[]
            {
                new DocumentTemplate {Id=1, Name = "Договор подряда", Description = "", TypeOfDocumentId = 1, TypeOfContractId = 1, RootTemplateId = 1},
                new DocumentTemplate {Id=2, Name = "Договор оказания услуг", Description = "", TypeOfDocumentId = 1, TypeOfContractId = 2, RootTemplateId = 1},
                new DocumentTemplate {Id=3, Name = "Заявление об отмене судебного приказа", Description = "", TypeOfDocumentId = 2, RootTemplateId = 2}
            });

            modelBuilder.Entity<Partner>().HasData(
            new Partner[]
            {
                new Partner {Id=1, Name = "Муниципальное унитарное предприятие \"Энергетический ресурс\" Некрасовского муниципального района", INN ="7701071", KPP="7701001", ShortName ="МУП \"Энергоресурс\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Советская, д. 175", OGRN = "1235550001", Bank ="ПАО \"Сбербанк\"", Account ="40301030000000065", CorrespondentAccount = "3010110000000022", BIK = "7752251",  DirectorName = "Иванов И.И.", DirectorNameR = "Иванова И.И.", DirectorTypeId = 1, PartnerStatusId = 1, PartnerTypeId = 1, PartnerCategoryId = 3, Email = "test@test.ru", PhoneNumber="111"},
                new Partner {Id=2, Name = "ООО \"Сервисное предприятие авторемонт\"", INN ="7701051", KPP="7701001", ShortName ="ООО \"Авторемонт\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Пролетарская, д. 11", OGRN = "3315350022", Bank ="ПАО \"Сбербанк\"", Account ="403221030000010072", CorrespondentAccount = "3010150000000133", BIK = "7752251",DirectorName = "Капралов Д.М.", DirectorNameR = "Капралова Д.М.", DirectorTypeId = 2, PartnerStatusId = 2, PartnerTypeId = 1, PartnerCategoryId = 3, Email = "test1@test.ru", PhoneNumber="222"},
                new Partner {Id=3, Name = "ООО Муниципальное образовательное учреждение \"Некрасовская средняя общеобразовательная школа\"", INN ="7701031", KPP="7701002", ShortName ="МОУ \"Некрасовская средняя школа\"", Address = "Ярославская обл., Некрасовский р-н, рп. Некрасовское, ул. Матросова, д. 17", OGRN = "3411330222", Bank ="ПАО \"Сбербанк\"", Account ="403551030012010078", CorrespondentAccount = "3010250001000123", BIK = "7752251", DirectorName = "Сергеев А.Р.", DirectorNameR = "Сергеева А.Р.", DirectorTypeId = 1, PartnerStatusId = 2, PartnerTypeId = 1, PartnerCategoryId = 3, Email = "test2@test.ru", PhoneNumber="333"},
                new Partner {Id=4, Name = "Некрасовский районный суд Ярославской области", INN ="", KPP="", ShortName ="Некрасовский районный суд Ярославской области", Address = "152260, п. Некрасовское, ул. Некрасова, д. 30", OGRN = "", Bank ="", Account ="", CorrespondentAccount = "", BIK = "", DirectorName = "", DirectorNameR = "", DirectorTypeId = 1, PartnerStatusId = 2, PartnerTypeId = 1, PartnerCategoryId = 2, Email = "test3@test.ru", PhoneNumber="555"},
                new Partner {Id=5, Name = "Арбитражный суд Ярославской области", INN ="", KPP="", ShortName ="Арбитражный суд Ярославской области", Address = "150054, г. Ярославль, пр-т Ленина, д. 28", OGRN = "", Bank ="", Account ="", CorrespondentAccount = "", BIK = "", DirectorName = "", DirectorNameR = "", DirectorTypeId = 1, PartnerStatusId = 2, PartnerTypeId = 1, PartnerCategoryId = 2, Email = "test4@test.ru", PhoneNumber="777"},
                new Partner {Id=6, Name = "Муниципальное унитарное предприятие «Некрасовский водохозяйственный комплекс»", INN ="", KPP="", ShortName ="МУП «НВК»", Address = "152260, Ярославская обл., рп. Некрасовское, ул. Советская, д. 178", OGRN = "", Bank ="", Account ="", CorrespondentAccount = "", BIK = "", DirectorName = "", DirectorNameR = "", DirectorTypeId = 1, PartnerStatusId = 2, PartnerTypeId = 1, PartnerCategoryId = 3, Email = "test5@test.ru", PhoneNumber="888"}

            });

            modelBuilder.Entity<Condition>().HasData(
            new Condition[]
            {
                new Condition {Id = 1, NumberInDocument = 1, Name = "Заголовок договора", Title = "\"Договор contractType № __\"", DocumentTemplateId = 1, TypeOfStateRegId = 4, NumLevelReference = 0, NumId = 0, Justification = "center" },
                new Condition {Id = 2, NumberInDocument = 3, Name = "Предмет договора", Title = "Предмет договора", DocumentTemplateId =  1, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},
                new Condition {Id = 3, NumberInDocument = 4, Name = "Права и обязанности сторон", Title = "Права и обязанности сторон", DocumentTemplateId = 1, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},
                new Condition {Id = 4, NumberInDocument = 6, Name = "Ответственность сторон", Title = "Ответственность сторон", DocumentTemplateId = 1, TypeOfStateRegId = 1, NumLevelReference = 1, NumId = 1, Justification = "center"},

                new Condition {Id = 5, NumberInDocument = 1, Name = "Заголовок договора", Title = "\"Договор contractType № __\"", DocumentTemplateId = 2, TypeOfStateRegId = 4, NumLevelReference = 0, NumId = 0, Justification = "center" },
                new Condition {Id = 6, NumberInDocument = 3, Name = "Предмет договора", Title = "Предмет договора", DocumentTemplateId =  2, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},
                new Condition {Id = 7, NumberInDocument = 4, Name = "Права и обязанности сторон", Title = "Права и обязанности сторон", DocumentTemplateId = 2, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},
                new Condition {Id = 8, NumberInDocument = 6, Name = "Ответственность сторон", Title = "Ответственность сторон", DocumentTemplateId = 2, TypeOfStateRegId = 1, NumLevelReference = 1, NumId = 1, Justification = "center"},

                new Condition {Id = 10, NumberInDocument = 5, Name = "Цена договора и порядок расчетов", Title = "Цена договора и порядок расчетов", DocumentTemplateId = 1, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},
                new Condition {Id = 11, NumberInDocument = 5, Name = "Цена договора и порядок расчетов", Title = "Цена договора и порядок расчетов", DocumentTemplateId = 2, TypeOfStateRegId = 4, NumLevelReference = 1, NumId = 1, Justification = "center"},

                new Condition {Id = 12, NumberInDocument = 2, Name = "Преамбула", DocumentTemplateId = 1, TypeOfStateRegId = 4, NumLevelReference = 0, NumId = 0, Justification = "center"},
                new Condition {Id = 13, NumberInDocument = 2, Name = "Преамбула", DocumentTemplateId = 2, TypeOfStateRegId = 4, NumLevelReference = 0, NumId = 0, Justification = "center"}

            }); ;

            modelBuilder.Entity<SubCondition>().HasData(
            new SubCondition[]
            {
                new SubCondition {Id=1, Text="customerName именуемое в дальнейшем \"Заказчик\", в лице customerDirectorTypeNameR customerDirectorNameR, действующего на основании Устава, с одной стороны, и executorName, именуемое в дальнейшем \"executor\", в лице executorDirectorTypeNameR executorDirectorNameR, действующего на основании Устава, с другой стороны, baseOfContract заключили настоящий договор о нижеследующем:", ConditionId = 12, NumLevelReference = 0, NumId = 0, Justification = "both"},
                new SubCondition {Id=2, Text="За неисполнение или ненадлежащее исполнение Контракта Стороны несут ответственность в соответствии с законодательством Российской Федерации и условиями Контракта.", ConditionId = 4, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=3, Text="В случае полного (частичного) неисполнения условий Контракта одной из Сторон эта Сторона обязана возместить другой Стороне причиненные убытки в части, непокрытой неустойкой.", ConditionId = 4, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=4, Text="В случае просрочки исполнения Подрядчиком обязательств, предусмотренных Контрактом, Подрядчик уплачивает Заказчику пени. Пеня начисляется за каждый день просрочки исполнения Подрядчиком обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства. Размер пени составляет одна трехсотая действующей на дату уплаты пени ключевой ставки Центрального банка Российской Федерации от цены Контракта (отдельного этапа исполнения Контракта), уменьшенной на сумму, пропорциональную объему обязательств, предусмотренных Контрактом (соответствующим отдельным этапом исполнения Контракта) и фактически исполненных Подрядчиком.", ConditionId = 4, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=5, Text="В случае просрочки исполнения Заказчиком обязательств, предусмотренных Контрактом, Подрядчик вправе потребовать уплату пени в размере одной трехсотой действующей на дату уплаты пеней ключевой ставки Центрального банка Российской Федерации от не уплаченной в срок суммы. Пеня начисляется за каждый день просрочки исполнения обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства.", ConditionId = 4, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=6, Text="Применение неустойки (штрафа, пени) не освобождает Стороны от исполнения обязательств по Контракту.", ConditionId = 4, NumLevelReference = 2, NumId = 1 , Justification = "both"},
                new SubCondition {Id=7, Text="В случае расторжения Контракта в связи с односторонним отказом Стороны от исполнения Контракта другая Сторона вправе потребовать возмещения только фактически понесенного ущерба, непосредственно обусловленного обстоятельствами, являющимися основанием для принятия решения об одностороннем отказе от исполнения Контракта.", ConditionId = 4, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=8, Text="Подрядчик обязуется выполнить по заданию Заказчика работу, указанную в пункте 1.2 настоящего договора, и сдать ее результат Заказчику, а Заказчик обязуется принять результат работы и оплатить его.", ConditionId = 2, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=9, Text="Подрядчик обязуется выполнить следующую работу: subjectOfContract, именуемую в дальнейшем \"Работа\".", ConditionId = 2, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=10, Text="Подрядчик обязуется:", ConditionId = 3, NumLevelReference = 2, NumId = 1, Justification = "both"},

                new SubCondition {Id=11, Text="customerName именуемое в дальнейшем \"Заказчик\", в лице customerDirectorTypeNameR customerDirectorNameR, действующего на основании Устава, с одной стороны, и executorName, именуемое в дальнейшем \"executor\", в лице executorDirectorTypeNameR executorDirectorNameR, действующего на основании Устава, с другой стороны, baseOfContract заключили настоящий договор о нижеследующем:", ConditionId = 13, NumLevelReference = 0, NumId = 0, Justification = "both"},
                new SubCondition {Id=12, Text="За неисполнение или ненадлежащее исполнение Контракта Стороны несут ответственность в соответствии с законодательством Российской Федерации и условиями Контракта.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=13, Text="В случае полного (частичного) неисполнения условий Контракта одной из Сторон эта Сторона обязана возместить другой Стороне причиненные убытки в части, непокрытой неустойкой.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=14, Text="В случае просрочки исполнения Исполнителем обязательств, предусмотренных Контрактом, Исполнитель уплачивает Заказчику пени. Пеня начисляется за каждый день просрочки исполнения Исполнителем обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства. Размер пени составляет одна трехсотая действующей на дату уплаты пени ключевой ставки Центрального банка Российской Федерации от цены Контракта (отдельного этапа исполнения Контракта), уменьшенной на сумму, пропорциональную объему обязательств, предусмотренных Контрактом (соответствующим отдельным этапом исполнения Контракта) и фактически исполненных Исполнителем.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=15, Text="В случае просрочки исполнения Заказчиком обязательств, предусмотренных Контрактом, Исполнитель вправе потребовать уплату пени в размере одной трехсотой действующей на дату уплаты пеней ключевой ставки Центрального банка Российской Федерации от не уплаченной в срок суммы. Пеня начисляется за каждый день просрочки исполнения обязательства, предусмотренного Контрактом, начиная со дня, следующего после дня истечения установленного Контрактом срока исполнения обязательства.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=16, Text="Применение неустойки (штрафа, пени) не освобождает Стороны от исполнения обязательств по Контракту.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=17, Text="В случае расторжения Контракта в связи с односторонним отказом Стороны от исполнения Контракта другая Сторона вправе потребовать возмещения только фактически понесенного ущерба, непосредственно обусловленного обстоятельствами, являющимися основанием для принятия решения об одностороннем отказе от исполнения Контракта.", ConditionId = 8, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=18, Text="Исполнитель обязуется оказать по заданию Заказчика услуги, указанные в пункте 1.2 настоящего договора, и сдать ее результат Заказчику, а Заказчик обязуется принять результат оказания услуг и оплатить его.", ConditionId = 6, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=19, Text="Исполнитель обязуется оказать следующие услуги: subjectOfContract, именуемые в дальнейшем \"Услуги\".", ConditionId = 6, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=20, Text="Исполнитель обязуется:", ConditionId = 7, NumLevelReference = 2, NumId = 1, Justification = "both"},

                new SubCondition {Id=21, Text="place                                                 __.__.202_ г.", ConditionId = 1, NumLevelReference = 0, NumId = 0, Justification = "center"},
                new SubCondition {Id=22, Text="place                                                 __.__.202_ г.", ConditionId = 5, NumLevelReference = 0, NumId = 0, Justification = "center"},

                new SubCondition {Id=23, Text="Цена настоящего договора составляет cost руб.", ConditionId = 10, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=24, Text="Уплата Заказчиком Исполнителю цены договора осуществляется путем перечисления средств на расчетный счет Исполнителя, указанный в настоящем договоре.", ConditionId = 10, NumLevelReference = 2, NumId = 1, Justification = "both"},

                new SubCondition {Id=25, Text="Цена настоящего договора составляет cost руб.", ConditionId = 11, NumLevelReference = 2, NumId = 1, Justification = "both"},
                new SubCondition {Id=26, Text="Уплата Заказчиком Исполнителю цены договора осуществляется путем перечисления средств на расчетный счет Исполнителя, указанный в настоящем договоре.", ConditionId = 11, NumLevelReference = 2, NumId = 1, Justification = "both"}

            });

            modelBuilder.Entity<SubConditionParagraph>().HasData(
            new SubConditionParagraph[]
            {
                new SubConditionParagraph {Id=1, Text="Подрядчик обязуется выполнить Работу с надлежащим качеством, из своих материалов, своими силами и средствами.", SubConditionId = 10, NumLevelReference = 3, NumId = 1, Justification = "both"},
                new SubConditionParagraph {Id=2, Text="Подрядчик обязуется выполнить Работу в срок до dateEnd г.", SubConditionId = 10, NumLevelReference = 3, NumId = 1 , Justification = "both"},

                new SubConditionParagraph {Id=3, Text="Исполнитель обязуется оказать услуги с надлежащим качеством, своими силами и средствами.", SubConditionId = 20, NumLevelReference = 3, NumId = 1, Justification = "both"},
                new SubConditionParagraph {Id=4, Text="Исполнитель обязуется оказать услуги в срок до dateEnd г.", SubConditionId = 20, NumLevelReference = 3, NumId = 1, Justification = "both"}
            });
        }
    }
}
