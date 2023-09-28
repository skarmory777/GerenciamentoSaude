namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_chave_estrangeira_profissao_cbo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisProfissao", "SisCboId", c => c.Long());
            CreateIndex("dbo.SisProfissao", "SisCboId");
            AddForeignKey("dbo.SisProfissao", "SisCboId", "dbo.SisCbo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisProfissao", "SisCboId", "dbo.SisCbo");
            DropIndex("dbo.SisProfissao", new[] { "SisCboId" });
            DropColumn("dbo.SisProfissao", "SisCboId");
        }
    }
}
