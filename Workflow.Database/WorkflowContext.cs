using Workflow.Core.Models;
using Microsoft.EntityFrameworkCore;
using Workflow.Core.Models.Templates;
using Workflow.Core.Models.Triggers;
using Workflow.Core.Models.Projects;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading.Tasks;
using System.Threading;
using Workflow.Core.Interfaces;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Workflow.Abstractions.Models;

namespace Workflow.Database
{
    public class WorkflowContext : DbContext
    {
        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<WorkflowAssociation> WorkflowAssociation { get; set; }

        public virtual DbSet<WorkflowGlobalParameter> WorkflowGlobalParameter { get; set; }

        public virtual DbSet<WorkflowInbox> WorkflowInbox { get; set; }

        public virtual DbSet<WorkflowProcessInstance> WorkflowProcessInstance { get; set; }

        public virtual DbSet<WorkflowProcessInstancePersistence> WorkflowProcessInstancePersistence { get; set; }

        public virtual DbSet<WorkflowProcessInstanceStatus> WorkflowProcessInstanceStatus { get; set; }

        public virtual DbSet<WorkflowProcessScheme> WorkflowProcessScheme { get; set; }

        public virtual DbSet<WorkflowProcessTimer> WorkflowProcessTimer { get; set; }

        public virtual DbSet<WorkflowProcessTransitionHistory> WorkflowProcessTransitionHistory { get; set; }

        public virtual DbSet<WorkflowScheme> WorkflowScheme { get; set; }

       // public virtual DbSet<Scheme> Scheme { get; set; }

        public virtual DbSet<Template> Templates { get; set; }

        public virtual DbSet<FormTemplate> FormTemplates { get; set; }

        public virtual DbSet<Trigger> Triggers { get; set; }

        public virtual DbSet<TriggerAttribute> TriggerAtrributes { get; set; }

        public virtual DbSet<ProjectTrigger> ProjectTriggers { get; set; }

        private readonly IBag _bag;


        public WorkflowContext(DbContextOptions<WorkflowContext> options, IBag bag) : base(options)
        {
            _bag = bag;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects");

            modelBuilder.Entity<WorkflowAssociation>().ToTable("WorkflowAssociation");

            modelBuilder.Entity<WorkflowGlobalParameter>().ToTable("WorkflowGlobalParameter");

            modelBuilder.Entity<WorkflowInbox>().ToTable("WorkflowInbox");

            modelBuilder.Entity<WorkflowProcessInstance>().ToTable("WorkflowProcessInstance");

            modelBuilder.Entity<WorkflowProcessInstancePersistence>().ToTable("WorkflowProcessInstancePersistence");

            modelBuilder.Entity<WorkflowProcessInstanceStatus>().ToTable("WorkflowProcessInstanceStatus");

            modelBuilder.Entity<WorkflowProcessScheme>().ToTable("WorkflowProcessScheme");

            modelBuilder.Entity<WorkflowProcessTimer>().ToTable("WorkflowProcessTimer");

            modelBuilder.Entity<WorkflowProcessTransitionHistory>().ToTable("WorkflowProcessTransitionHistory");

            modelBuilder.Entity<WorkflowScheme>().ToTable("WorkflowScheme");

           // modelBuilder.Entity<Scheme>().ToTable("Scheme");
            
            modelBuilder.Entity<Template>().ToTable("Templates");

            modelBuilder.Entity<FormTemplate>().ToTable("FormTemplates");

            modelBuilder.Entity<Trigger>().ToTable("Triggers");

            modelBuilder.Entity<TriggerAttribute>().ToTable("TriggerAttributes");

            modelBuilder.Entity<ProjectTrigger>().ToTable("ProjectTriggers");


            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e=>typeof(ISoftDelete).IsAssignableFrom(e.ClrType)))
            {
                AddProperty<bool>(modelBuilder, entityType, "IsDeleted", false);
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(IProjectRelated).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<Guid>("ProjectId");
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<IProjectRelated>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if ((Guid)entry.CurrentValues["ProjectId"] == Guid.Empty)
                        {
                            entry.CurrentValues["ProjectId"] = _bag.CurrentProjectId;
                        }
                        break;
                }
            }
        }

        private void AddProperty<T>(ModelBuilder modelBuilder, IMutableEntityType entityType, string name, T value)
        {
            entityType.GetOrAddProperty(name, typeof(T));

            var parameter = Expression.Parameter(entityType.ClrType);

            var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(T));

            var propertyExpression = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(name));

            var compareExpression = Expression.MakeBinary(ExpressionType.Equal, propertyExpression, Expression.Constant(value));

            var lambda = Expression.Lambda(compareExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda); 
        }
    }
}
