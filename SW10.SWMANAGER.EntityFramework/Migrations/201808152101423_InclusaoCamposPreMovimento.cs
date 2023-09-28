namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposPreMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "HoraPrescrita", c => c.DateTime());
            AddColumn("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId");
            AddForeignKey("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", "dbo.AssPrescricaoItemHora", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", "dbo.AssPrescricaoItemHora");
            DropIndex("dbo.EstoquePreMovimento", new[] { "AssPrescricaoItemHoraId" });
            DropColumn("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId");
            DropColumn("dbo.EstoquePreMovimento", "HoraPrescrita");
        }
    }
}
