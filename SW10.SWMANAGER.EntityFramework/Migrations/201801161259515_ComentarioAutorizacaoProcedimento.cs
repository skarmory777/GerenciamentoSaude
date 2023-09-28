namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ComentarioAutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteComentarioAutorizacaoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AutorizacaoProcedimentoId = c.Long(nullable: false),
                    DataRegistro = c.DateTime(nullable: false),
                    Conteudo = c.String(),
                    UsuarioId = c.Long(),
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
                    { "DynamicFilter_ComentarioAutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAutorizacaoProcedimento", t => t.AutorizacaoProcedimentoId, cascadeDelete: true)
                .Index(t => t.AutorizacaoProcedimentoId);

            AddColumn("dbo.AteAutorizacaoProcedimento", "NumerioGuia", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteComentarioAutorizacaoProcedimento", "AutorizacaoProcedimentoId", "dbo.AteAutorizacaoProcedimento");
            DropIndex("dbo.AteComentarioAutorizacaoProcedimento", new[] { "AutorizacaoProcedimentoId" });
            DropColumn("dbo.AteAutorizacaoProcedimento", "NumerioGuia");
            DropTable("dbo.AteComentarioAutorizacaoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ComentarioAutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
