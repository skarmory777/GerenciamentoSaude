namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracoesCamposRegistroExame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimento", "UnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.LauMovimento", "CentroCustoId", c => c.Long());
            AddColumn("dbo.LauMovimento", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.LauMovimento", "TurnoId", c => c.Long());
            AddColumn("dbo.LauMovimento", "VolumeContrasteTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimento", "VolumeContrasteVenoso", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimento", "VolumeContrasteOral", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimento", "VolumeContrasteRetal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimento", "IsIonico", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "IsBombaInsufora", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "IsContrasteVenoso", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "IsContrasteOral", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "IsContrasteRetal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "LoteContraste", c => c.String());
            CreateIndex("dbo.LauMovimento", "UnidadeOrganizacionalId");
            CreateIndex("dbo.LauMovimento", "CentroCustoId");
            CreateIndex("dbo.LauMovimento", "TipoAcomodacaoId");
            CreateIndex("dbo.LauMovimento", "TurnoId");
            AddForeignKey("dbo.LauMovimento", "CentroCustoId", "dbo.CentroCusto", "Id");
            AddForeignKey("dbo.LauMovimento", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.LauMovimento", "TurnoId", "dbo.SisTurno", "Id");
            AddForeignKey("dbo.LauMovimento", "UnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
            DropColumn("dbo.LauMovimentoItem", "IsContraste");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteTotal");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteVenoso");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteOral");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteRetal");
            DropColumn("dbo.LauMovimentoItem", "IsIonico");
            DropColumn("dbo.LauMovimentoItem", "IsBombaInsufora");
            DropColumn("dbo.LauMovimentoItem", "IsContrasteVenoso");
            DropColumn("dbo.LauMovimentoItem", "IsContrasteOral");
            DropColumn("dbo.LauMovimentoItem", "IsContrasteRetal");
        }

        public override void Down()
        {
            AddColumn("dbo.LauMovimentoItem", "IsContrasteRetal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContrasteOral", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContrasteVenoso", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsBombaInsufora", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsIonico", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteRetal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteOral", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteVenoso", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "IsContraste", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.LauMovimento", "UnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.LauMovimento", "TurnoId", "dbo.SisTurno");
            DropForeignKey("dbo.LauMovimento", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.LauMovimento", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.LauMovimento", new[] { "TurnoId" });
            DropIndex("dbo.LauMovimento", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.LauMovimento", new[] { "CentroCustoId" });
            DropIndex("dbo.LauMovimento", new[] { "UnidadeOrganizacionalId" });
            DropColumn("dbo.LauMovimento", "LoteContraste");
            DropColumn("dbo.LauMovimento", "IsContrasteRetal");
            DropColumn("dbo.LauMovimento", "IsContrasteOral");
            DropColumn("dbo.LauMovimento", "IsContrasteVenoso");
            DropColumn("dbo.LauMovimento", "IsBombaInsufora");
            DropColumn("dbo.LauMovimento", "IsIonico");
            DropColumn("dbo.LauMovimento", "VolumeContrasteRetal");
            DropColumn("dbo.LauMovimento", "VolumeContrasteOral");
            DropColumn("dbo.LauMovimento", "VolumeContrasteVenoso");
            DropColumn("dbo.LauMovimento", "VolumeContrasteTotal");
            DropColumn("dbo.LauMovimento", "TurnoId");
            DropColumn("dbo.LauMovimento", "TipoAcomodacaoId");
            DropColumn("dbo.LauMovimento", "CentroCustoId");
            DropColumn("dbo.LauMovimento", "UnidadeOrganizacionalId");
        }
    }
}
