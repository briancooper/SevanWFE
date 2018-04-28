using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Workflow.Database.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessKey = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowAssociation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Entity = table.Column<string>(nullable: false),
                    SchemeCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAssociation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowGlobalParameter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowGlobalParameter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdentityId = table.Column<Guid>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActivityName = table.Column<string>(nullable: false),
                    IsDeterminingParametersChanged = table.Column<bool>(nullable: false),
                    ParentProcessId = table.Column<Guid>(nullable: true),
                    PreviousActivity = table.Column<string>(nullable: true),
                    PreviousActivityForDirect = table.Column<string>(nullable: true),
                    PreviousActivityForReverse = table.Column<string>(nullable: true),
                    PreviousState = table.Column<string>(nullable: true),
                    PreviousStateForDirect = table.Column<string>(nullable: true),
                    PreviousStateForReverse = table.Column<string>(nullable: true),
                    RootProcessId = table.Column<Guid>(nullable: false),
                    SchemeId = table.Column<Guid>(nullable: true),
                    StateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessInstancePersistence",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParameterName = table.Column<string>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessInstancePersistence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessInstanceStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Lock = table.Column<Guid>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessInstanceStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessScheme",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllowedActivities = table.Column<string>(nullable: true),
                    DefiningParameters = table.Column<string>(nullable: false),
                    DefiningParametersHash = table.Column<string>(maxLength: 1024, nullable: false),
                    IsObsolete = table.Column<bool>(nullable: false),
                    RootSchemeCode = table.Column<string>(nullable: true),
                    RootSchemeId = table.Column<Guid>(nullable: true),
                    Scheme = table.Column<string>(nullable: false),
                    SchemeCode = table.Column<string>(nullable: false),
                    StartingTransition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessScheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessTimer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Ignore = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NextExecutionDateTime = table.Column<DateTime>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessTimer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProcessTransitionHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActorIdentityId = table.Column<string>(nullable: true),
                    ExecutorIdentityId = table.Column<string>(nullable: true),
                    FromActivityName = table.Column<string>(nullable: false),
                    FromStateName = table.Column<string>(nullable: true),
                    IsFinalised = table.Column<bool>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    ToActivityName = table.Column<string>(nullable: false),
                    ToStateName = table.Column<string>(nullable: true),
                    TransitionClassifier = table.Column<string>(nullable: false),
                    TransitionTime = table.Column<DateTime>(nullable: false),
                    TriggerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProcessTransitionHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowScheme",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 256, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Scheme = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowScheme", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTriggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    TriggerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTriggers_Triggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Triggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProjectTriggerId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriggerAttributes_ProjectTriggers_ProjectTriggerId",
                        column: x => x.ProjectTriggerId,
                        principalTable: "ProjectTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTriggers_TriggerId",
                table: "ProjectTriggers",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerAttributes_ProjectTriggerId",
                table: "TriggerAttributes",
                column: "ProjectTriggerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormTemplates");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TriggerAttributes");

            migrationBuilder.DropTable(
                name: "WorkflowAssociation");

            migrationBuilder.DropTable(
                name: "WorkflowGlobalParameter");

            migrationBuilder.DropTable(
                name: "WorkflowInbox");

            migrationBuilder.DropTable(
                name: "WorkflowProcessInstance");

            migrationBuilder.DropTable(
                name: "WorkflowProcessInstancePersistence");

            migrationBuilder.DropTable(
                name: "WorkflowProcessInstanceStatus");

            migrationBuilder.DropTable(
                name: "WorkflowProcessScheme");

            migrationBuilder.DropTable(
                name: "WorkflowProcessTimer");

            migrationBuilder.DropTable(
                name: "WorkflowProcessTransitionHistory");

            migrationBuilder.DropTable(
                name: "WorkflowScheme");

            migrationBuilder.DropTable(
                name: "ProjectTriggers");

            migrationBuilder.DropTable(
                name: "Triggers");
        }
    }
}
