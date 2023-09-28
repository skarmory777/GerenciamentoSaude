namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class SisComentario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisComentario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    UsuarioId = c.Long(),
                    SisTarefaId = c.Long(nullable: false),
                    DataRegistro = c.DateTime(),
                    Conteudo = c.String(),
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
                    { "DynamicFilter_Comentario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisTarefa", t => t.SisTarefaId, cascadeDelete: false)
                .Index(t => t.SisTarefaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisComentario", "SisTarefaId", "dbo.SisTarefa");
            DropIndex("dbo.SisComentario", new[] { "SisTarefaId" });
            DropTable("dbo.SisComentario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Comentario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
