namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AgendamentoMaterialInclusaoCampoFornecedor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAgendamentoMaterial", "FornecedorId", c => c.Long(nullable: false));
            CreateIndex("dbo.AteAgendamentoMaterial", "FornecedorId");
            AddForeignKey("dbo.AteAgendamentoMaterial", "FornecedorId", "dbo.SisFornecedor", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoMaterial", "FornecedorId", "dbo.SisFornecedor");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FornecedorId" });
            DropColumn("dbo.AteAgendamentoMaterial", "FornecedorId");
        }
    }
}
