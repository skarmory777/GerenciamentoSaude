namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UltimoId_Tamanho : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisUltimoId", "TamanhoCampo", c => c.Int());
            AddColumn("dbo.SisUltimoId", "complementoEsquerda", c => c.String(maxLength: 1));
        }

        public override void Down()
        {
            DropColumn("dbo.SisUltimoId", "complementoEsquerda");
            DropColumn("dbo.SisUltimoId", "TamanhoCampo");
        }
    }
}
