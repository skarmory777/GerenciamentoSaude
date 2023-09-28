namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Marcus_Adicionando_Referencia_Classe_Id_FormResposta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormResposta", "NomeClasse", c => c.String());
            AddColumn("dbo.FormResposta", "RegistroClasseId", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FormResposta", "RegistroClasseId");
            DropColumn("dbo.FormResposta", "NomeClasse");
        }
    }
}
