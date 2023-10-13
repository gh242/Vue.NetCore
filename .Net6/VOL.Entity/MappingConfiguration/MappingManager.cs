using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VOL.Entity.MappingConfiguration
{

    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder b);
    }

    public interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }

    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration<T> where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> b);

        public void Map(ModelBuilder b)
        {
            Map(b.Entity<T>());
        }
    }

    public static class ModelBuilderExtenions
    {
        private static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes().Where(x => !x.IsAbstract && x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));
        }

        public static void AddEntityConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityMappingConfiguration<>));
            foreach (var config in mappingTypes.Select(Activator.CreateInstance).Cast<IEntityMappingConfiguration>())
            {
                config.Map(modelBuilder);
            }
        }
    }
}

//字符串默認長度
//public class StringDefaultLengthConvention : IPropertyAddedConvention
//{
//    public InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder)
//    {
//        if (propertyBuilder.Metadata.ClrType == typeof(string))
//            propertyBuilder.HasMaxLength(32, ConfigurationSource.Convention);
//        return propertyBuilder;
//    }
//}
////attribute方式設置decimal精度
//public class DecimalPrecisionAttributeConvention : PropertyAttributeConvention<DecimalPrecisionAttribute>
//{
//    public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, DecimalPrecisionAttribute attribute, MemberInfo clrMember)
//    {
//        if (propertyBuilder.Metadata.ClrType == typeof(decimal))
//            propertyBuilder.HasPrecision(attribute.Precision, attribute.Scale);
//        return propertyBuilder;
//    }

//    /// <summary>
//    /// decimal類型設置精度
//    /// </summary>
//    /// <param name="propertyBuilder"></param>
//    /// <param name="precision">精度</param>
//    /// <param name="scale">小數位數</param>
//    public static PropertyBuilder<TProperty> HasPrecision<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, int precision = 18, int scale = 4)
//    {
//        //fluntapi方式設置精度  
//        ((IInfrastructure<InternalPropertyBuilder>)propertyBuilder).Instance.HasPrecision(precision, scale);

//        return propertyBuilder;
//    }

//    public static InternalPropertyBuilder HasPrecision(this InternalPropertyBuilder propertyBuilder, int precision, int scale)
//    {
//        propertyBuilder.Relational(ConfigurationSource.Explicit).HasColumnType($"decimal({precision},{scale})");

//        return propertyBuilder;
//    }

////servie,DI注入替換.
//services.AddSingleton<IModelCustomizer, MyRelationalModelCustomizer>();
//services.AddSingleton<ICoreConventionSetBuilder, MyCoreConventionSetBuilder>();

////實現entity的自動發現和mapper設置
//public class MyRelationalModelCustomizer : RelationalModelCustomizer
//{
//    public MyRelationalModelCustomizer(ModelCustomizerDependencies dependencies)
//        : base(dependencies) { }

//    public override void Customize(ModelBuilder modelBuilder, DbContext dbContext)
//    {
//        base.Customize(modelBuilder, dbContext);
//        var sp = (IInfrastructure<IServiceProvider>)dbContext;
//        var dbOptions = sp.Instance.GetServices<DbContextOptions>();
//        foreach (var item in dbOptions)
//        {
//            if (item.ContextType == dbContext.GetType())
//                ConfigureDbContextEntityService.Configure(modelBuilder, item, dbContext);
//        }
//    }
//}

//public class MyCoreConventionSetBuilder : CoreConventionSetBuilder
//{
//    public MyCoreConventionSetBuilder(CoreConventionSetBuilderDependencies dependencies) : base(dependencies) { }
//    public override ConventionSet CreateConventionSet()
//    {
//        var conventionSet = base.CreateConventionSet();
//        //默認字符串長度，而不是nvarchar(max).
//        //為什麼要insert(0,obj),則是因為這個默認規則要最優先處理，如果後續有規則的話就直接覆蓋了。
//        //propertyBuilder.HasMaxLength(32, ConfigurationSource.Convention);
//        //理论上我指定了規則的級別為.Convention，應該和順序就沒有關系了。but，還沒有完成測試，所以我也不知道
//        conventionSet.PropertyAddedConventions.Insert(0, new StringDefaultLengthConvention());
//        //decimal設置精度
//        conventionSet.PropertyAddedConventions.Add(new DecimalPrecisionAttributeConvention());
//        return conventionSet;
//    }
//}

