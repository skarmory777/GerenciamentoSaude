namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTipoAcomodacao2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatConta", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatContaKit", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatContaItem", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito");
            DropIndex("dbo.FatConta", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatContaItem", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatContaKit", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "TipoLeitoId" });
            DropColumn("dbo.FatConta", "TipoLeitoId");
            DropColumn("dbo.FatContaItem", "TipoLeitoId");
            DropColumn("dbo.FatContaKit", "TipoLeitoId");
            DropColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId");
            DropTable("dbo.TipoLeito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.TipoLeito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodigoTiss = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId", c => c.Long());
            AddColumn("dbo.FatContaKit", "TipoLeitoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "TipoLeitoId", c => c.Long());
            AddColumn("dbo.FatConta", "TipoLeitoId", c => c.Long());
            CreateIndex("dbo.FatTaxaTipoLeito", "TipoLeitoId");
            CreateIndex("dbo.FatContaKit", "TipoLeitoId");
            CreateIndex("dbo.FatContaItem", "TipoLeitoId");
            CreateIndex("dbo.FatConta", "TipoLeitoId");
            AddForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatContaItem", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatContaKit", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatConta", "TipoLeitoId", "dbo.TipoLeito", "Id");
        }
    }
}
