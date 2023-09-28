namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_foreign_comprarequisicao_modo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "CmpModoId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "CmpModoId");
            AddForeignKey("dbo.CmpRequisicao", "CmpModoId", "dbo.CmpRequisicaoModo", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicao", "CmpModoId", "dbo.CmpRequisicaoModo");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpModoId" });
            DropColumn("dbo.CmpRequisicao", "CmpModoId");
        }
    }
}
