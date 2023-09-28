namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTipoAcomidacao2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId", c => c.Long());
            CreateIndex("dbo.FatTaxaTipoLeito", "TipoLeitoId");
            AddForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito");
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "TipoLeitoId" });
            DropColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId");
        }
    }
}
