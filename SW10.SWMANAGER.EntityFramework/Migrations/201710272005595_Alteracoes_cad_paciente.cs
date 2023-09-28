namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_cad_paciente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisCidade", "Descricao", c => c.String());
            AddColumn("dbo.SisPais", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoLogradouro", "Codigo", c => c.String(maxLength: 10));
        }

        public override void Down()
        {
            AlterColumn("dbo.SisTipoLogradouro", "Codigo", c => c.Int(nullable: false));
            DropColumn("dbo.SisPais", "Descricao");
            DropColumn("dbo.SisCidade", "Descricao");
        }
    }
}
