using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Workflow.Database.Migrations
{
    public partial class AddedTriggerIdToWorkflowSchemeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TriggerId",
                table: "WorkflowScheme",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowScheme_TriggerId",
                table: "WorkflowScheme",
                column: "TriggerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowScheme_Triggers_TriggerId",
                table: "WorkflowScheme");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowScheme_TriggerId",
                table: "WorkflowScheme");

            migrationBuilder.DropColumn(
                name: "TriggerId",
                table: "WorkflowScheme");
        }
    }
}
