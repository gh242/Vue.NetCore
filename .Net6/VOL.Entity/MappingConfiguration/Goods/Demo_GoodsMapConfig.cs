using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Demo_GoodsMapConfig : EntityMappingConfiguration<Demo_Goods>
    {
        public override void Map(EntityTypeBuilder<Demo_Goods>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

