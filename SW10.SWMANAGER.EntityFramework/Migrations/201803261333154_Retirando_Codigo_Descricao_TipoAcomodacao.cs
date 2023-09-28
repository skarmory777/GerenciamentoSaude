namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Retirando_Codigo_Descricao_TipoAcomodacao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisTipoAcomodacao", "Descricao", c => c.String());
            DropColumn("dbo.SisTipoAcomodacao", "CodTipoAcomodacao");
        }

        public override void Down()
        {
            AddColumn("dbo.SisTipoAcomodacao", "CodTipoAcomodacao", c => c.String(maxLength: 10));
            AlterColumn("dbo.SisTipoAcomodacao", "Descricao", c => c.String(maxLength: 255));
        }
    }
}
