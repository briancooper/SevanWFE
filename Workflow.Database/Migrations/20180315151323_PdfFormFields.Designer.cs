﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Workflow.Core.Models;
using Workflow.Database;

namespace Workflow.Database.Migrations
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20180315151323_PdfFormFields")]
    partial class PdfFormFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Workflow.Core.Models.Projects.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessKey")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Workflow.Core.Models.Templates.FormTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Description");

                    b.Property<string>("FilePath")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PdfFormFields");

                    b.Property<Guid>("ProjectId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("FormTemplates");
                });

            modelBuilder.Entity("Workflow.Core.Models.Templates.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("ProjectId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Workflow.Core.Models.Triggers.ProjectTrigger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ProjectId");

                    b.Property<Guid>("TriggerId");

                    b.HasKey("Id");

                    b.HasIndex("TriggerId");

                    b.ToTable("ProjectTriggers");
                });

            modelBuilder.Entity("Workflow.Core.Models.Triggers.Trigger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Triggers");
                });

            modelBuilder.Entity("Workflow.Core.Models.Triggers.TriggerAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("ProjectTriggerId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProjectTriggerId");

                    b.ToTable("TriggerAttributes");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowAssociation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Entity");

                    b.Property<string>("SchemeCode")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("WorkflowAssociation");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowGlobalParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("WorkflowGlobalParameter");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowInbox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("IdentityId");

                    b.Property<Guid>("ProcessId");

                    b.HasKey("Id");

                    b.ToTable("WorkflowInbox");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessInstance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityName")
                        .IsRequired();

                    b.Property<bool>("IsDeterminingParametersChanged");

                    b.Property<Guid?>("ParentProcessId");

                    b.Property<string>("PreviousActivity");

                    b.Property<string>("PreviousActivityForDirect");

                    b.Property<string>("PreviousActivityForReverse");

                    b.Property<string>("PreviousState");

                    b.Property<string>("PreviousStateForDirect");

                    b.Property<string>("PreviousStateForReverse");

                    b.Property<Guid>("RootProcessId");

                    b.Property<Guid?>("SchemeId");

                    b.Property<string>("StateName");

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessInstance");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessInstancePersistence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ParameterName")
                        .IsRequired();

                    b.Property<Guid>("ProcessId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessInstancePersistence");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessInstanceStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Lock");

                    b.Property<byte>("Status");

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessInstanceStatus");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessScheme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AllowedActivities");

                    b.Property<string>("DefiningParameters")
                        .IsRequired();

                    b.Property<string>("DefiningParametersHash")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<bool>("IsObsolete");

                    b.Property<string>("RootSchemeCode");

                    b.Property<Guid?>("RootSchemeId");

                    b.Property<string>("Scheme")
                        .IsRequired();

                    b.Property<string>("SchemeCode")
                        .IsRequired();

                    b.Property<string>("StartingTransition");

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessScheme");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessTimer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ignore");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("NextExecutionDateTime");

                    b.Property<Guid>("ProcessId");

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessTimer");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowProcessTransitionHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActorIdentityId");

                    b.Property<string>("ExecutorIdentityId");

                    b.Property<string>("FromActivityName")
                        .IsRequired();

                    b.Property<string>("FromStateName");

                    b.Property<bool>("IsFinalised");

                    b.Property<Guid>("ProcessId");

                    b.Property<string>("ToActivityName")
                        .IsRequired();

                    b.Property<string>("ToStateName");

                    b.Property<string>("TransitionClassifier")
                        .IsRequired();

                    b.Property<DateTime>("TransitionTime");

                    b.Property<string>("TriggerName");

                    b.HasKey("Id");

                    b.ToTable("WorkflowProcessTransitionHistory");
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowScheme", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256);

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ProjectId");

                    b.Property<string>("Scheme")
                        .IsRequired();

                    b.Property<Guid?>("TriggerId");

                    b.HasKey("Code");

                    b.HasIndex("TriggerId");

                    b.ToTable("WorkflowScheme");
                });

            modelBuilder.Entity("Workflow.Core.Models.Triggers.ProjectTrigger", b =>
                {
                    b.HasOne("Workflow.Core.Models.Triggers.Trigger", "Trigger")
                        .WithMany()
                        .HasForeignKey("TriggerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Workflow.Core.Models.Triggers.TriggerAttribute", b =>
                {
                    b.HasOne("Workflow.Core.Models.Triggers.ProjectTrigger")
                        .WithMany("Attributes")
                        .HasForeignKey("ProjectTriggerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Workflow.Core.Models.WorkflowScheme", b =>
                {
                    b.HasOne("Workflow.Core.Models.Triggers.Trigger", "Trigger")
                        .WithMany()
                        .HasForeignKey("TriggerId");
                });
#pragma warning restore 612, 618
        }
    }
}
