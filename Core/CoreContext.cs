using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Core
{
    public class CoreContext: IdentityDbContext<User>
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
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = null!;
    public DbSet<PartnerStatus> PartnerStatuses { get; set; } = null!;
    public DbSet<PartnerType> PartnerTypes { get; set; } = null!;
    public DbSet<MyContractStatus> MyContractStatuses { get; set; } = null!;
    public DbSet<MySubTask> MySubTasks { get; set; } = null!;
    public DbSet<Letter> Letters { get; set; } = null!;
    public DbSet<ShippingMethod> ShippingMethods { get; set; } = null!;
    public DbSet<LetterType> LetterTypes { get; set; } = null!;


        public CoreContext(DbContextOptions<CoreContext> options)
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
                new LevelImportance {Id=1, Name="Низкий", ClassView = "btn btn-secondary"},
                new LevelImportance {Id=2, Name="Средний", ClassView = "btn btn-warning" },
                new LevelImportance {Id=3, Name="Высокий", ClassView = "btn btn-danger"}
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
                new DirectorType {Id=1, Name="Директор"},
                new DirectorType {Id=2, Name="Генеральный директор"},
                new DirectorType {Id=3, Name="Глава"}
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
                new TypeOfStateReg {Id=3, Name="ГК РФ"}
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
});

            modelBuilder.Entity<PartnerType>().HasData(
new PartnerType[]
{
                new PartnerType {Id=1, Name="Юридическое лицо"},
                new PartnerType {Id=2, Name="Индивидуальный предприниматель"},
                new PartnerType {Id=3, Name="Физическое лицо"},
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

            modelBuilder.Entity<LetterType>().HasData(
            new LetterType[]
            {
                new LetterType {Id=1, Name="Входящее"},
                new LetterType {Id=2, Name="Исходящее"}
            });
        }
    }
}
