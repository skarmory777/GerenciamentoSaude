namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_ProtocoloAtendimento_ClassificacaoAtendimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteClassificacaoAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Cor = c.String(),
                    Prioridade = c.Int(nullable: false),
                    PrazoAtendimento = c.Int(nullable: false),
                    Ativo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_ClassificacaoAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteProtocoloAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ClassificacaoAtendimentoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_ProtocoloAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteClassificacaoAtendimento", t => t.ClassificacaoAtendimentoId)
                .Index(t => t.ClassificacaoAtendimentoId);

            AddColumn("dbo.AteAtendimento", "ClassificacaoAtendimentoId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "ClassificacaoAtendimentoId");
            AddForeignKey("dbo.AteAtendimento", "ClassificacaoAtendimentoId", "dbo.AteClassificacaoAtendimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteProtocoloAtendimento", "ClassificacaoAtendimentoId", "dbo.AteClassificacaoAtendimento");
            DropForeignKey("dbo.AteAtendimento", "ClassificacaoAtendimentoId", "dbo.AteClassificacaoAtendimento");
            DropIndex("dbo.AteProtocoloAtendimento", new[] { "ClassificacaoAtendimentoId" });
            DropIndex("dbo.AteAtendimento", new[] { "ClassificacaoAtendimentoId" });
            DropColumn("dbo.AteAtendimento", "ClassificacaoAtendimentoId");
            DropTable("dbo.AteProtocoloAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProtocoloAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteClassificacaoAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ClassificacaoAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
