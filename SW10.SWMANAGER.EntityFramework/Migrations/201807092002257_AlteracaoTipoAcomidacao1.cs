namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTipoAcomidacao1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatTaxa", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito");
            DropIndex("dbo.FatTaxa", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "TipoLeitoId" });
            AddColumn("dbo.SisTipoAcomodacao", "CodigoTiss", c => c.String(maxLength: 10));
            AddColumn("dbo.FatConta", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.FatContaKit", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.FatTaxa", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.FatTaxaTipoLeito", "TipoAcomodacaoId", c => c.Long());
            CreateIndex("dbo.FatConta", "TipoAcomodacaoId");
            CreateIndex("dbo.FatContaItem", "TipoAcomodacaoId");
            CreateIndex("dbo.FatContaKit", "TipoAcomodacaoId");
            CreateIndex("dbo.FatTaxa", "TipoAcomodacaoId");
            CreateIndex("dbo.FatTaxaTipoLeito", "TipoAcomodacaoId");
            AddForeignKey("dbo.FatConta", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.FatContaKit", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.FatContaItem", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.FatTaxa", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.FatTaxaTipoLeito", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            DropColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId");
        }

        public override void Down()
        {
            AddColumn("dbo.FatTaxaTipoLeito", "TipoLeitoId", c => c.Long());
            DropForeignKey("dbo.FatTaxaTipoLeito", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.FatTaxa", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.FatContaItem", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.FatContaKit", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.FatConta", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.FatTaxa", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.FatContaKit", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.FatContaItem", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.FatConta", new[] { "TipoAcomodacaoId" });
            DropColumn("dbo.FatTaxaTipoLeito", "TipoAcomodacaoId");
            DropColumn("dbo.FatTaxa", "TipoAcomodacaoId");
            DropColumn("dbo.FatContaKit", "TipoAcomodacaoId");
            DropColumn("dbo.FatContaItem", "TipoAcomodacaoId");
            DropColumn("dbo.FatConta", "TipoAcomodacaoId");
            DropColumn("dbo.SisTipoAcomodacao", "CodigoTiss");
            CreateIndex("dbo.FatTaxaTipoLeito", "TipoLeitoId");
            CreateIndex("dbo.FatTaxa", "TipoLeitoId");
            AddForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito", "Id");
            AddForeignKey("dbo.FatTaxa", "TipoLeitoId", "dbo.TipoLeito", "Id");
        }
    }
}
