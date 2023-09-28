namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_FaturamentoTaxa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTaxa", "EmpresaId", c => c.Long());
            AddColumn("dbo.FatTaxa", "LocalUtilizacaoId", c => c.Long());
            AddColumn("dbo.FatTaxa", "FaturamentoGrupoId", c => c.Long());
            AddColumn("dbo.FatTaxa", "TipoLeitoId", c => c.Long());
            CreateIndex("dbo.FatTaxa", "EmpresaId");
            CreateIndex("dbo.FatTaxa", "LocalUtilizacaoId");
            CreateIndex("dbo.FatTaxa", "FaturamentoGrupoId");
            CreateIndex("dbo.FatTaxa", "TipoLeitoId");
            AddForeignKey("dbo.FatTaxa", "EmpresaId", "dbo.SisEmpresa", "Id");
            AddForeignKey("dbo.FatTaxa", "FaturamentoGrupoId", "dbo.FatGrupo", "Id");
            AddForeignKey("dbo.FatTaxa", "LocalUtilizacaoId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.FatTaxa", "TipoLeitoId", "dbo.TipoLeito", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTaxa", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatTaxa", "LocalUtilizacaoId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.FatTaxa", "FaturamentoGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatTaxa", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.FatTaxa", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatTaxa", new[] { "FaturamentoGrupoId" });
            DropIndex("dbo.FatTaxa", new[] { "LocalUtilizacaoId" });
            DropIndex("dbo.FatTaxa", new[] { "EmpresaId" });
            DropColumn("dbo.FatTaxa", "TipoLeitoId");
            DropColumn("dbo.FatTaxa", "FaturamentoGrupoId");
            DropColumn("dbo.FatTaxa", "LocalUtilizacaoId");
            DropColumn("dbo.FatTaxa", "EmpresaId");
        }
    }
}
