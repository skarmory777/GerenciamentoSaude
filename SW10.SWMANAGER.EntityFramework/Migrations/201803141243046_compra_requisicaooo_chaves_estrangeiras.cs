namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class compra_requisicaooo_chaves_estrangeiras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "CmpEmpresaId", c => c.Long(nullable: false));
            AddColumn("dbo.CmpRequisicao", "SisMedicoId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "SisPacienteId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "SisUnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.CmpRequisicao", "CmpMotivoPedidoId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "CmpEmpresaId");
            CreateIndex("dbo.CmpRequisicao", "SisMedicoId");
            CreateIndex("dbo.CmpRequisicao", "SisPacienteId");
            CreateIndex("dbo.CmpRequisicao", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.CmpRequisicao", "CmpMotivoPedidoId");
            AddForeignKey("dbo.CmpRequisicao", "CmpEmpresaId", "dbo.SisEmpresa", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CmpRequisicao", "SisMedicoId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.CmpRequisicao", "CmpMotivoPedidoId", "dbo.CmpMotivoPedido", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CmpRequisicao", "SisPacienteId", "dbo.SisPaciente", "Id");
            AddForeignKey("dbo.CmpRequisicao", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicao", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.CmpRequisicao", "SisPacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.CmpRequisicao", "CmpMotivoPedidoId", "dbo.CmpMotivoPedido");
            DropForeignKey("dbo.CmpRequisicao", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.CmpRequisicao", "CmpEmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpMotivoPedidoId" });
            DropIndex("dbo.CmpRequisicao", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.CmpRequisicao", new[] { "SisPacienteId" });
            DropIndex("dbo.CmpRequisicao", new[] { "SisMedicoId" });
            DropIndex("dbo.CmpRequisicao", new[] { "CmpEmpresaId" });
            DropColumn("dbo.CmpRequisicao", "CmpMotivoPedidoId");
            DropColumn("dbo.CmpRequisicao", "SisUnidadeOrganizacionalId");
            DropColumn("dbo.CmpRequisicao", "SisPacienteId");
            DropColumn("dbo.CmpRequisicao", "SisMedicoId");
            DropColumn("dbo.CmpRequisicao", "CmpEmpresaId");
        }
    }
}
