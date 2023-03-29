using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleOfLaws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleOfLaws", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserNickName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DirectorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevelImportances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ClassView = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelImportances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyContractStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyContractStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTaskStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfContracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderUserId = table.Column<string>(type: "text", nullable: false),
                    SenderUserName = table.Column<string>(type: "text", nullable: false),
                    DestinationUserId = table.Column<string>(type: "text", nullable: false),
                    DestinationUserName = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MyTaskStatusId = table.Column<int>(type: "integer", nullable: true),
                    MyTaskLevelImportanceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyTasks_LevelImportances_MyTaskLevelImportanceId",
                        column: x => x.MyTaskLevelImportanceId,
                        principalTable: "LevelImportances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MyTasks_MyTaskStatuses_MyTaskStatusId",
                        column: x => x.MyTaskStatusId,
                        principalTable: "MyTaskStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfContract = table.Column<string>(type: "text", nullable: true),
                    DateStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PartnerId = table.Column<int>(type: "integer", nullable: true),
                    SubjectOfContract = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: true),
                    TypeOfContract = table.Column<int>(type: "integer", nullable: false),
                    MyContractStatusId = table.Column<int>(type: "integer", nullable: true),
                    projectContractLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_MyContractStatuses_MyContractStatusId",
                        column: x => x.MyContractStatusId,
                        principalTable: "MyContractStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MyFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    NameInServer = table.Column<string>(type: "text", nullable: false),
                    MyTaskId = table.Column<int>(type: "integer", nullable: true),
                    ContractId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyFiles_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MyFiles_MyTasks_MyTaskId",
                        column: x => x.MyTaskId,
                        principalTable: "MyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameOfTemplate = table.Column<string>(type: "text", nullable: false),
                    NameOutput = table.Column<string>(type: "text", nullable: false),
                    TypeOfContractId = table.Column<int>(type: "integer", nullable: true),
                    Test = table.Column<int>(type: "integer", nullable: false),
                    TemplateFileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_MyFiles_TemplateFileId",
                        column: x => x.TemplateFileId,
                        principalTable: "MyFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_TypeOfContracts_TypeOfContractId",
                        column: x => x.TypeOfContractId,
                        principalTable: "TypeOfContracts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PartnerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DocumentTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerTypes_DocumentTemplates_DocumentTemplateId",
                        column: x => x.DocumentTemplateId,
                        principalTable: "DocumentTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TypeOfStateRegs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DocumentTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfStateRegs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeOfStateRegs_DocumentTemplates_DocumentTemplateId",
                        column: x => x.DocumentTemplateId,
                        principalTable: "DocumentTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    INN = table.Column<string>(type: "text", nullable: true),
                    PartnerStatusId = table.Column<int>(type: "integer", nullable: true),
                    PartnerTypeId = table.Column<int>(type: "integer", nullable: false),
                    KPP = table.Column<string>(type: "text", nullable: true),
                    DirectorTypeId = table.Column<int>(type: "integer", nullable: true),
                    DirectorName = table.Column<string>(type: "text", nullable: true),
                    Bank = table.Column<string>(type: "text", nullable: true),
                    Account = table.Column<string>(type: "text", nullable: true),
                    CorrespondentAccount = table.Column<string>(type: "text", nullable: true),
                    BIK = table.Column<string>(type: "text", nullable: true),
                    OGRN = table.Column<string>(type: "text", nullable: true),
                    PassportSeries = table.Column<string>(type: "text", nullable: true),
                    PassportNumber = table.Column<string>(type: "text", nullable: true),
                    PassportDateOfIssue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PassportDateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PassportPlaseOfIssue = table.Column<string>(type: "text", nullable: true),
                    PassportDivisionCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partners_DirectorTypes_DirectorTypeId",
                        column: x => x.DirectorTypeId,
                        principalTable: "DirectorTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Partners_PartnerStatuses_PartnerStatusId",
                        column: x => x.PartnerStatusId,
                        principalTable: "PartnerStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Partners_PartnerTypes_PartnerTypeId",
                        column: x => x.PartnerTypeId,
                        principalTable: "PartnerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employeers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PartnerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employeers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employeers_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ArticleOfLaws",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "пункт 8 части 1 44-ФЗ (коммунальные услуги)" },
                    { 2, "часть 4 44-ФЗ (единственный поставщик)" }
                });

            migrationBuilder.InsertData(
                table: "DirectorTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Директор" },
                    { 2, "Генеральный директор" },
                    { 3, "Глава" }
                });

            migrationBuilder.InsertData(
                table: "LevelImportances",
                columns: new[] { "Id", "ClassView", "Name" },
                values: new object[,]
                {
                    { 1, "btn btn-secondary", "Низкий" },
                    { 2, "btn btn-warning", "Средний" },
                    { 3, "btn btn-danger", "Высокий" }
                });

            migrationBuilder.InsertData(
                table: "MyContractStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Подготовка" },
                    { 2, "Согласование" },
                    { 3, "Заключение" },
                    { 4, "Исполнение" },
                    { 5, "Завершен" },
                    { 6, "В архиве" }
                });

            migrationBuilder.InsertData(
                table: "MyTaskStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Поступила" },
                    { 2, "В работе" },
                    { 3, "Выполнено" },
                    { 4, "В архиве" }
                });

            migrationBuilder.InsertData(
                table: "PartnerStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Головная" },
                    { 2, "Дочерняя" },
                    { 3, "Контрагент" }
                });

            migrationBuilder.InsertData(
                table: "PartnerTypes",
                columns: new[] { "Id", "DocumentTemplateId", "Name" },
                values: new object[,]
                {
                    { 1, null, "Юридическое лицо" },
                    { 2, null, "Индивидуальный предприниматель" },
                    { 3, null, "Физическое лицо" }
                });

            migrationBuilder.InsertData(
                table: "TypeOfContracts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Договор подряда" },
                    { 2, "Договор оказания услуг" },
                    { 3, "Договор поставки" }
                });

            migrationBuilder.InsertData(
                table: "TypeOfStateRegs",
                columns: new[] { "Id", "DocumentTemplateId", "Name" },
                values: new object[,]
                {
                    { 1, null, "44-ФЗ" },
                    { 2, null, "223-ФЗ" },
                    { 3, null, "ГК РФ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_MyContractStatusId",
                table: "Contracts",
                column: "MyContractStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PartnerId",
                table: "Contracts",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_TemplateFileId",
                table: "DocumentTemplates",
                column: "TemplateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_TypeOfContractId",
                table: "DocumentTemplates",
                column: "TypeOfContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Employeers_PartnerId",
                table: "Employeers",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MyFiles_ContractId",
                table: "MyFiles",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MyFiles_MyTaskId",
                table: "MyFiles",
                column: "MyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MyTasks_MyTaskLevelImportanceId",
                table: "MyTasks",
                column: "MyTaskLevelImportanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MyTasks_MyTaskStatusId",
                table: "MyTasks",
                column: "MyTaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DirectorTypeId",
                table: "Partners",
                column: "DirectorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PartnerStatusId",
                table: "Partners",
                column: "PartnerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PartnerTypeId",
                table: "Partners",
                column: "PartnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerTypes_DocumentTemplateId",
                table: "PartnerTypes",
                column: "DocumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfStateRegs_DocumentTemplateId",
                table: "TypeOfStateRegs",
                column: "DocumentTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Partners_PartnerId",
                table: "Contracts",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_MyContractStatuses_MyContractStatusId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Partners_PartnerId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "ArticleOfLaws");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Employeers");

            migrationBuilder.DropTable(
                name: "TypeOfStateRegs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MyContractStatuses");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "DirectorTypes");

            migrationBuilder.DropTable(
                name: "PartnerStatuses");

            migrationBuilder.DropTable(
                name: "PartnerTypes");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "MyFiles");

            migrationBuilder.DropTable(
                name: "TypeOfContracts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "MyTasks");

            migrationBuilder.DropTable(
                name: "LevelImportances");

            migrationBuilder.DropTable(
                name: "MyTaskStatuses");
        }
    }
}
