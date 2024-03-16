using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace common_library.Base.Context
{
    public abstract class GenericContext<TContext> : DbContext
    {
        public abstract void DatabaseConfig(DbContextOptionsBuilder optionsBuilder);

        public virtual void IgnoreEntities(ModelBuilder modelBuilder)
        {
        }

        public virtual void SaveException(Exception ex)
        {
            throw ex;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DatabaseConfig(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
            }
            catch (InvalidOperationException ex)
            {
                SaveException(ex);
                throw ex;
            }

            IgnoreEntities(modelBuilder);
            Type mappingInterface = typeof(IEntityTypeConfiguration<>);
            IEnumerable<Type> enumerable = from x in typeof(TContext).GetTypeInfo().Assembly.GetTypes()
                                           where x.GetInterfaces().Any((Type y) => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface)
                                           select x;
            MethodInfo methodInfo = typeof(ModelBuilder).GetMethods().Single((MethodInfo x) => x.Name == "Entity" && x.IsGenericMethod && x.ReturnType.Name == "EntityTypeBuilder`1");
            foreach (Type item in enumerable)
            {
                try
                {
                    Type type = item.GetInterfaces().Single().GenericTypeArguments.Single();
                    object obj = methodInfo.MakeGenericMethod(type).Invoke(modelBuilder, null);
                    object obj2 = Activator.CreateInstance(item);
                    obj2.GetType().GetMethod("Configure")?.Invoke(obj2, new object[1] { obj });
                }
                catch (Exception ex2)
                {
                    Debugger.Break();
                    SaveException(ex2);
                }
            }
        }

        public void SetTrackingBehavior(QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking)
        {
            try
            {
                ChangeTracker.QueryTrackingBehavior = trackingBehavior;
            }
            catch (InvalidOperationException ex)
            {
                SaveException(ex);
            }
            catch (Exception ex2)
            {
                Debugger.Break();
                SaveException(ex2);
            }
        }
    }
}
