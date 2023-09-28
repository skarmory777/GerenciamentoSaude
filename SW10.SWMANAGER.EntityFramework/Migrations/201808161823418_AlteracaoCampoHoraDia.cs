namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoCampoHoraDia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", "dbo.AssPrescricaoItemHora");
            DropIndex("dbo.EstoquePreMovimento", new[] { "AssPrescricaoItemHoraId" });
            AddColumn("dbo.EstoquePreMovimento", "SisHoraDiaId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "SisHoraDiaId");
            AddForeignKey("dbo.EstoquePreMovimento", "SisHoraDiaId", "dbo.SisHoraDia", "Id");
            DropColumn("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", c => c.Long());
            DropForeignKey("dbo.EstoquePreMovimento", "SisHoraDiaId", "dbo.SisHoraDia");
            DropIndex("dbo.EstoquePreMovimento", new[] { "SisHoraDiaId" });
            DropColumn("dbo.EstoquePreMovimento", "SisHoraDiaId");
            CreateIndex("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId");
            AddForeignKey("dbo.EstoquePreMovimento", "AssPrescricaoItemHoraId", "dbo.AssPrescricaoItemHora", "Id");
        }
    }
}
