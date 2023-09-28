namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class DocRotulo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocRotulo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    Titulo = c.String(),
                    Ordem = c.Single(nullable: false),
                    IsCapitulo = c.Boolean(nullable: false),
                    IsSessao = c.Boolean(nullable: false),
                    IsAssunto = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_DocRotulo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.DocItem", "Sessao", c => c.Long());
            AddColumn("dbo.DocItem", "Assunto", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.DocItem", "Assunto");
            DropColumn("dbo.DocItem", "Sessao");
            DropTable("dbo.DocRotulo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DocRotulo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
