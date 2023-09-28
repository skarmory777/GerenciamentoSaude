namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Ajustes_ElementoHtml : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisElementoHtmlTipo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    HtmlHelper = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_ElementoHtmlTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssTipoRespostaConfigElementoHtml", "Rotulo", c => c.String());
            AddColumn("dbo.AssTipoRespostaConfigElementoHtml", "RotuloPosElemento", c => c.String());
            AddColumn("dbo.SisElementoHtml", "SisElementoHtmlTipoId", c => c.Long(nullable: false));
            AddColumn("dbo.SisElementoHtml", "Tamanho", c => c.Int(nullable: false));
            AddColumn("dbo.SisElementoHtml", "IsDesativado", c => c.Boolean(nullable: false));
            CreateIndex("dbo.SisElementoHtml", "SisElementoHtmlTipoId");
            AddForeignKey("dbo.SisElementoHtml", "SisElementoHtmlTipoId", "dbo.SisElementoHtmlTipo", "Id", cascadeDelete: false);
            DropColumn("dbo.SisElementoHtml", "Rotulo");
            DropColumn("dbo.SisElementoHtml", "RotuloPosElemento");
        }

        public override void Down()
        {
            AddColumn("dbo.SisElementoHtml", "RotuloPosElemento", c => c.String());
            AddColumn("dbo.SisElementoHtml", "Rotulo", c => c.String());
            DropForeignKey("dbo.SisElementoHtml", "SisElementoHtmlTipoId", "dbo.SisElementoHtmlTipo");
            DropIndex("dbo.SisElementoHtml", new[] { "SisElementoHtmlTipoId" });
            DropColumn("dbo.SisElementoHtml", "IsDesativado");
            DropColumn("dbo.SisElementoHtml", "Tamanho");
            DropColumn("dbo.SisElementoHtml", "SisElementoHtmlTipoId");
            DropColumn("dbo.AssTipoRespostaConfigElementoHtml", "RotuloPosElemento");
            DropColumn("dbo.AssTipoRespostaConfigElementoHtml", "Rotulo");
            DropTable("dbo.SisElementoHtmlTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ElementoHtmlTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
