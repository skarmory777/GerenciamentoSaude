namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class IncluindoReferenciaTISS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConselho", "TabelaItemTissId", c => c.Long());
            AddColumn("dbo.SisCbo", "TabelaItemTissId", c => c.Long());
            AddColumn("dbo.SisTipoLogradouro", "TabelaItemTissId", c => c.Long());
            AddColumn("dbo.AteTipoAtendimento", "TabelaItemTissId", c => c.Long());
            AddColumn("dbo.SisTipoAcomodacao", "TabelaItemTissId", c => c.Long());
            CreateIndex("dbo.SisConselho", "TabelaItemTissId");
            CreateIndex("dbo.SisCbo", "TabelaItemTissId");
            CreateIndex("dbo.SisTipoLogradouro", "TabelaItemTissId");
            CreateIndex("dbo.AteTipoAtendimento", "TabelaItemTissId");
            CreateIndex("dbo.SisTipoAcomodacao", "TabelaItemTissId");
            AddForeignKey("dbo.SisConselho", "TabelaItemTissId", "dbo.SisTabelaDominio", "Id");
            AddForeignKey("dbo.SisCbo", "TabelaItemTissId", "dbo.SisTabelaDominio", "Id");
            AddForeignKey("dbo.SisTipoLogradouro", "TabelaItemTissId", "dbo.SisTabelaDominio", "Id");
            AddForeignKey("dbo.AteTipoAtendimento", "TabelaItemTissId", "dbo.SisTabelaDominio", "Id");
            AddForeignKey("dbo.SisTipoAcomodacao", "TabelaItemTissId", "dbo.SisTabelaDominio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisTipoAcomodacao", "TabelaItemTissId", "dbo.SisTabelaDominio");
            DropForeignKey("dbo.AteTipoAtendimento", "TabelaItemTissId", "dbo.SisTabelaDominio");
            DropForeignKey("dbo.SisTipoLogradouro", "TabelaItemTissId", "dbo.SisTabelaDominio");
            DropForeignKey("dbo.SisCbo", "TabelaItemTissId", "dbo.SisTabelaDominio");
            DropForeignKey("dbo.SisConselho", "TabelaItemTissId", "dbo.SisTabelaDominio");
            DropIndex("dbo.SisTipoAcomodacao", new[] { "TabelaItemTissId" });
            DropIndex("dbo.AteTipoAtendimento", new[] { "TabelaItemTissId" });
            DropIndex("dbo.SisTipoLogradouro", new[] { "TabelaItemTissId" });
            DropIndex("dbo.SisCbo", new[] { "TabelaItemTissId" });
            DropIndex("dbo.SisConselho", new[] { "TabelaItemTissId" });
            DropColumn("dbo.SisTipoAcomodacao", "TabelaItemTissId");
            DropColumn("dbo.AteTipoAtendimento", "TabelaItemTissId");
            DropColumn("dbo.SisTipoLogradouro", "TabelaItemTissId");
            DropColumn("dbo.SisCbo", "TabelaItemTissId");
            DropColumn("dbo.SisConselho", "TabelaItemTissId");
        }
    }
}
