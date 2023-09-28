namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoquePreMovimento5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "MedicoId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "MedicoId");
            AddForeignKey("dbo.EstoquePreMovimento", "MedicoId", "dbo.Medico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "MedicoId", "dbo.Medico");
            DropIndex("dbo.EstoquePreMovimento", new[] { "MedicoId" });
            DropColumn("dbo.EstoquePreMovimento", "MedicoId");
        }
    }
}
