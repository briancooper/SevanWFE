using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Workflow.Database.Migrations
{
    public partial class ModifiedTriggerIdToNullableWorkflowScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme");

            migrationBuilder.AlterColumn<Guid>(
                name: "TriggerId",
                table: "WorkflowScheme",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme");

            migrationBuilder.AlterColumn<Guid>(
                name: "TriggerId",
                table: "WorkflowScheme",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
