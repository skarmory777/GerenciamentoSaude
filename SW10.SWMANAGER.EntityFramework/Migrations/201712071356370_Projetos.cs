namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Projetos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisProjeto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    DataCriacao = c.DateTime(nullable: false),
                    Nivel1 = c.String(),
                    Nivel2 = c.String(),
                    Nivel3 = c.String(),
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
                    { "DynamicFilter_Projeto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SisTarefa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ProjetoId = c.Long(),
                    DataRegistro = c.DateTime(nullable: false),
                    DataPrevistaInicio = c.DateTime(nullable: false),
                    DataPrevistaTermino = c.DateTime(nullable: false),
                    DataInicio = c.DateTime(nullable: false),
                    DataTermino = c.DateTime(nullable: false),
                    ResponsavelId = c.Long(),
                    Modulo = c.Long(),
                    Status = c.Int(nullable: false),
                    Prioridade = c.Int(nullable: false),
                    ClienteId = c.Long(),
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
                    { "DynamicFilter_Tarefa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.SisTarefa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tarefa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisProjeto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Projeto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
