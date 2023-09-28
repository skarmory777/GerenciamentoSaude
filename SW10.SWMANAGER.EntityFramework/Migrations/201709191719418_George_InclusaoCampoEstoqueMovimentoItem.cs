namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class George_InclusaoCampoEstoqueMovimentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimentoItem", "PreMovimentoItemId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimentoItem", "PreMovimentoItemId");
            AddForeignKey("dbo.EstoqueMovimentoItem", "PreMovimentoItemId", "dbo.EstoquePreMovimentoItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoqueMovimentoItem", "PreMovimentoItemId", "dbo.EstoquePreMovimentoItem");
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "PreMovimentoItemId" });
            DropColumn("dbo.EstoqueMovimentoItem", "PreMovimentoItemId");
        }
    }
}
