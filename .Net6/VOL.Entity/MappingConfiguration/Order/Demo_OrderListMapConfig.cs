using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Demo_OrderListMapConfig : EntityMappingConfiguration<Demo_OrderList>
    {
        public override void Map(EntityTypeBuilder<Demo_OrderList>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

