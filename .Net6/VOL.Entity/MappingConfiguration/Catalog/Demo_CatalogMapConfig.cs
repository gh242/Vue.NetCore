using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Demo_CatalogMapConfig : EntityMappingConfiguration<Demo_Catalog>
    {
        public override void Map(EntityTypeBuilder<Demo_Catalog>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

