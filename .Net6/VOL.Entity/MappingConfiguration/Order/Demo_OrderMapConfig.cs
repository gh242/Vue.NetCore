using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Demo_OrderMapConfig : EntityMappingConfiguration<Demo_Order>
    {
        public override void Map(EntityTypeBuilder<Demo_Order>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

