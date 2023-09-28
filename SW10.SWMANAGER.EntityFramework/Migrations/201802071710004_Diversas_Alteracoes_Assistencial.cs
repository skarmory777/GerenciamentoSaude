namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Diversas_Alteracoes_Assistencial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssProntuario", "SisPrestadorId", "dbo.SisPrestador");
            DropIndex("dbo.AssProntuario", new[] { "SisPrestadorId" });
            RenameColumn(table: "dbo.AssProntuario", name: "AtendimentoId", newName: "AteAtendimentoId");
            RenameIndex(table: "dbo.AssProntuario", name: "IX_AtendimentoId", newName: "IX_AteAtendimentoId");
            CreateTable(
                "dbo.SisProfissionalSaude",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisPessoaId = c.Long(),
                    SisTipoVinculoEmpregaticioId = c.Long(),
                    SisParticipacaoId = c.Long(),
                    IsCorpoClinico = c.Boolean(nullable: false),
                    DataNascimento = c.DateTime(nullable: false),
                    CNS = c.String(),
                    SisTipoPrestadorId = c.Long(),
                    SisConselhoId = c.Long(),
                    NumeroConselho = c.Int(nullable: false),
                    Faculdade = c.String(),
                    IsAtivo = c.Boolean(nullable: false),
                    SisUserId = c.Long(),
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
                    { "DynamicFilter_ProfissionalSaude_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConselho", t => t.SisConselhoId)
                .ForeignKey("dbo.SisPessoa", t => t.SisPessoaId)
                .ForeignKey("dbo.SisTipoParticipacao", t => t.SisParticipacaoId)
                .ForeignKey("dbo.SisTipoPrestador", t => t.SisTipoPrestadorId)
                .ForeignKey("dbo.TipoVinculoEmpregaticio", t => t.SisTipoVinculoEmpregaticioId)
                .ForeignKey("dbo.AbpUsers", t => t.SisUserId)
                .Index(t => t.SisPessoaId)
                .Index(t => t.SisTipoVinculoEmpregaticioId)
                .Index(t => t.SisParticipacaoId)
                .Index(t => t.SisTipoPrestadorId)
                .Index(t => t.SisConselhoId)
                .Index(t => t.SisUserId);

            CreateTable(
                "dbo.AssProntuarioLog",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AssProntuarioId = c.Long(),
                    ApbUserId = c.Long(),
                    Anterior = c.String(),
                    Lancamento = c.String(),
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
                    { "DynamicFilter_ProntuarioLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssProntuario", t => t.AssProntuarioId)
                .ForeignKey("dbo.AbpUsers", t => t.ApbUserId)
                .Index(t => t.AssProntuarioId)
                .Index(t => t.ApbUserId);

            AddColumn("dbo.AssProntuario", "SisProfissionalSaudeId", c => c.Long());
            CreateIndex("dbo.AssProntuario", "SisProfissionalSaudeId");
            AddForeignKey("dbo.AssProntuario", "SisProfissionalSaudeId", "dbo.SisProfissionalSaude", "Id");
            DropColumn("dbo.AssProntuario", "SisPrestadorId");
        }

        public override void Down()
        {
            AddColumn("dbo.AssProntuario", "SisPrestadorId", c => c.Long(nullable: false));
            DropForeignKey("dbo.AssProntuarioLog", "ApbUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AssProntuarioLog", "AssProntuarioId", "dbo.AssProntuario");
            DropForeignKey("dbo.AssProntuario", "SisProfissionalSaudeId", "dbo.SisProfissionalSaude");
            DropForeignKey("dbo.SisProfissionalSaude", "SisUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.SisProfissionalSaude", "SisTipoVinculoEmpregaticioId", "dbo.TipoVinculoEmpregaticio");
            DropForeignKey("dbo.SisProfissionalSaude", "SisTipoPrestadorId", "dbo.SisTipoPrestador");
            DropForeignKey("dbo.SisProfissionalSaude", "SisParticipacaoId", "dbo.SisTipoParticipacao");
            DropForeignKey("dbo.SisProfissionalSaude", "SisPessoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.SisProfissionalSaude", "SisConselhoId", "dbo.SisConselho");
            DropIndex("dbo.AssProntuarioLog", new[] { "ApbUserId" });
            DropIndex("dbo.AssProntuarioLog", new[] { "AssProntuarioId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisUserId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisConselhoId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisTipoPrestadorId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisParticipacaoId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisTipoVinculoEmpregaticioId" });
            DropIndex("dbo.SisProfissionalSaude", new[] { "SisPessoaId" });
            DropIndex("dbo.AssProntuario", new[] { "SisProfissionalSaudeId" });
            DropColumn("dbo.AssProntuario", "SisProfissionalSaudeId");
            DropTable("dbo.AssProntuarioLog",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProntuarioLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisProfissionalSaude",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProfissionalSaude_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameIndex(table: "dbo.AssProntuario", name: "IX_AteAtendimentoId", newName: "IX_AtendimentoId");
            RenameColumn(table: "dbo.AssProntuario", name: "AteAtendimentoId", newName: "AtendimentoId");
            CreateIndex("dbo.AssProntuario", "SisPrestadorId");
            AddForeignKey("dbo.AssProntuario", "SisPrestadorId", "dbo.SisPrestador", "Id", cascadeDelete: true);
        }
    }
}
