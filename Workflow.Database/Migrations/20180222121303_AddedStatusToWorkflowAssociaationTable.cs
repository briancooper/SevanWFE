using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Workflow.Database.Migrations
{
    public partial class AddedStatusToWorkflowAssociaationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Entity",
                table: "WorkflowAssociation",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WorkflowAssociation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkflowAssociation");

            migrationBuilder.AlterColumn<string>(
                name: "Entity",
                table: "WorkflowAssociation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
