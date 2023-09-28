namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ImportacaoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisPessoa", "ImportacaoId", c => c.Long());
            AddColumn("dbo.SisPessoa", "ImportacaoTabela", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisPessoa", "ImportacaoTabela");
            DropColumn("dbo.SisPessoa", "ImportacaoId");
        }
    }
}
