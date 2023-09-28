namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoCamposAgendamentoMaterial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAgendamentoMaterial", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.AteAgendamentoMaterial", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FornecedorId" });
            AlterColumn("dbo.AteAgendamentoMaterial", "FaturamentoItemId", c => c.Long());
            CreateIndex("dbo.AteAgendamentoMaterial", "FaturamentoItemId");
            AddForeignKey("dbo.AteAgendamentoMaterial", "FaturamentoItemId", "dbo.FatItem", "Id");
            DropColumn("dbo.AteAgendamentoMaterial", "FornecedorId");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAgendamentoMaterial", "FornecedorId", c => c.Long(nullable: false));
            DropForeignKey("dbo.AteAgendamentoMaterial", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.AteAgendamentoMaterial", new[] { "FaturamentoItemId" });
            AlterColumn("dbo.AteAgendamentoMaterial", "FaturamentoItemId", c => c.Long(nullable: false));
            CreateIndex("dbo.AteAgendamentoMaterial", "FornecedorId");
            CreateIndex("dbo.AteAgendamentoMaterial", "FaturamentoItemId");
            AddForeignKey("dbo.AteAgendamentoMaterial", "FaturamentoItemId", "dbo.FatItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AteAgendamentoMaterial", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
