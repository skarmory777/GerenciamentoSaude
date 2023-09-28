namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatItemTabela_Nullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FatItemTabela", "COCH", c => c.Single());
            AlterColumn("dbo.FatItemTabela", "HMCH", c => c.Single());
            AlterColumn("dbo.FatItemTabela", "ValorTotal", c => c.Single());
            AlterColumn("dbo.FatItemTabela", "Auxiliar", c => c.Int());
            AlterColumn("dbo.FatItemTabela", "Porte", c => c.Int());
            AlterColumn("dbo.FatItemTabela", "Filme", c => c.Single());
        }

        public override void Down()
        {
            AlterColumn("dbo.FatItemTabela", "Filme", c => c.Single(nullable: false));
            AlterColumn("dbo.FatItemTabela", "Porte", c => c.Int(nullable: false));
            AlterColumn("dbo.FatItemTabela", "Auxiliar", c => c.Int(nullable: false));
            AlterColumn("dbo.FatItemTabela", "ValorTotal", c => c.Single(nullable: false));
            AlterColumn("dbo.FatItemTabela", "HMCH", c => c.Single(nullable: false));
            AlterColumn("dbo.FatItemTabela", "COCH", c => c.Single(nullable: false));
        }
    }
}
