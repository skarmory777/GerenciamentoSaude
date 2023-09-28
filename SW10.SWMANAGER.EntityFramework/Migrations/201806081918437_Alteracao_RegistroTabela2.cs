namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Alteracao_RegistroTabela2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisRegistroTabela",
                c => new
                {
                    Id = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RegistroTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.SisRegistroArquivo", "RegistroTabelaId", c => c.Long());
            CreateIndex("dbo.SisRegistroArquivo", "RegistroTabelaId");
            AddForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela");
            DropIndex("dbo.SisRegistroArquivo", new[] { "RegistroTabelaId" });
            DropColumn("dbo.SisRegistroArquivo", "RegistroTabelaId");
            DropTable("dbo.SisRegistroTabela",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RegistroTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
