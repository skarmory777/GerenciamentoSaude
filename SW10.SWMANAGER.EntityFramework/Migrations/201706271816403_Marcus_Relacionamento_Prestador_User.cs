namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Marcus_Relacionamento_Prestador_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prestador", "UserId", c => c.Long());
            CreateIndex("dbo.Prestador", "UserId");
            AddForeignKey("dbo.Prestador", "UserId", "dbo.AbpUsers", "Id");
            DropColumn("dbo.Prestador", "DataCadastro");
            DropColumn("dbo.Prestador", "DataAtualizacao");
            DropColumn("dbo.Prestador", "UsuarioAlteracao");
        }

        public override void Down()
        {
            AddColumn("dbo.Prestador", "UsuarioAlteracao", c => c.String());
            AddColumn("dbo.Prestador", "DataAtualizacao", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prestador", "DataCadastro", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Prestador", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.Prestador", new[] { "UserId" });
            DropColumn("dbo.Prestador", "UserId");
        }
    }
}
