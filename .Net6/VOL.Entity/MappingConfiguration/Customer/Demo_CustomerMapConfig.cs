using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Demo_CustomerMapConfig : EntityMappingConfiguration<Demo_Customer>
    {
        public override void Map(EntityTypeBuilder<Demo_Customer>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

