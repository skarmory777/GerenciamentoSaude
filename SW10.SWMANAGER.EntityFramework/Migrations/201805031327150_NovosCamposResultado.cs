namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NovosCamposResultado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultado", "ConvenioId", c => c.Long());
            AddColumn("dbo.LabResultado", "CentroCustoId", c => c.Long());
            AddColumn("dbo.LabResultado", "TipoAcomodacaoId", c => c.Long());
            AddColumn("dbo.LabResultado", "TurnoId", c => c.Long());
            CreateIndex("dbo.LabResultado", "ConvenioId");
            CreateIndex("dbo.LabResultado", "CentroCustoId");
            CreateIndex("dbo.LabResultado", "TipoAcomodacaoId");
            CreateIndex("dbo.LabResultado", "TurnoId");
            AddForeignKey("dbo.LabResultado", "CentroCustoId", "dbo.CentroCusto", "Id");
            AddForeignKey("dbo.LabResultado", "ConvenioId", "dbo.SisConvenio", "Id");
            AddForeignKey("dbo.LabResultado", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao", "Id");
            AddForeignKey("dbo.LabResultado", "TurnoId", "dbo.SisTurno", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultado", "TurnoId", "dbo.SisTurno");
            DropForeignKey("dbo.LabResultado", "TipoAcomodacaoId", "dbo.SisTipoAcomodacao");
            DropForeignKey("dbo.LabResultado", "ConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.LabResultado", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.LabResultado", new[] { "TurnoId" });
            DropIndex("dbo.LabResultado", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.LabResultado", new[] { "CentroCustoId" });
            DropIndex("dbo.LabResultado", new[] { "ConvenioId" });
            DropColumn("dbo.LabResultado", "TurnoId");
            DropColumn("dbo.LabResultado", "TipoAcomodacaoId");
            DropColumn("dbo.LabResultado", "CentroCustoId");
            DropColumn("dbo.LabResultado", "ConvenioId");
        }
    }
}
