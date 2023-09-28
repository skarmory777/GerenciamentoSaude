namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Projeto_Tarefa_Intervalo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SisTarefa", name: "ProjetoId", newName: "SisProjetoId");
            RenameColumn(table: "dbo.SisTarefa", name: "ModuloId", newName: "SisModuloId");
            RenameColumn(table: "dbo.SisTarefa", name: "StatusId", newName: "SisStatusId");
            CreateTable(
                "dbo.SisTarefaIntervalo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    SisTarefaId = c.Long(nullable: false),
                    Inicio = c.DateTime(nullable: false),
                    Fim = c.DateTime(nullable: false),
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
                    { "DynamicFilter_TarefaIntervalo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisTarefa", t => t.SisTarefaId, cascadeDelete: false)
                .Index(t => t.SisTarefaId);

            AddColumn("dbo.SisTarefa", "SisPrioridadeId", c => c.Long());
            AddColumn("dbo.SisTarefa", "SisTipoTarefaId", c => c.Long());
            AddColumn("dbo.SisTarefa", "SisNivel1Id", c => c.Long());
            AddColumn("dbo.SisTarefa", "SisNivel2Id", c => c.Long());
            AddColumn("dbo.SisTarefa", "SisNivel3Id", c => c.Long());
            AlterColumn("dbo.SisTarefa", "DataRegistro", c => c.DateTime());
            AlterColumn("dbo.SisTarefa", "DataPrevistaInicio", c => c.DateTime());
            AlterColumn("dbo.SisTarefa", "DataPrevistaTermino", c => c.DateTime());
            AlterColumn("dbo.SisTarefa", "DataInicio", c => c.DateTime());
            AlterColumn("dbo.SisTarefa", "DataTermino", c => c.DateTime());
            CreateIndex("dbo.SisTarefa", "SisProjetoId");
            CreateIndex("dbo.SisTarefa", "SisModuloId");
            CreateIndex("dbo.SisTarefa", "SisStatusId");
            CreateIndex("dbo.SisTarefa", "SisPrioridadeId");
            CreateIndex("dbo.SisTarefa", "SisTipoTarefaId");
            CreateIndex("dbo.SisTarefa", "SisNivel1Id");
            CreateIndex("dbo.SisTarefa", "SisNivel2Id");
            CreateIndex("dbo.SisTarefa", "SisNivel3Id");
            AddForeignKey("dbo.SisTarefa", "SisModuloId", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisNivel1Id", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisNivel2Id", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisNivel3Id", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisPrioridadeId", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisProjetoId", "dbo.SisProjeto", "Id");
            AddForeignKey("dbo.SisTarefa", "SisStatusId", "dbo.DocRotulo", "Id");
            AddForeignKey("dbo.SisTarefa", "SisTipoTarefaId", "dbo.DocRotulo", "Id");
            DropColumn("dbo.SisTarefa", "Prioridade");
        }

        public override void Down()
        {
            AddColumn("dbo.SisTarefa", "Prioridade", c => c.Int(nullable: false));
            DropForeignKey("dbo.SisTarefaIntervalo", "SisTarefaId", "dbo.SisTarefa");
            DropForeignKey("dbo.SisTarefa", "SisTipoTarefaId", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisStatusId", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisProjetoId", "dbo.SisProjeto");
            DropForeignKey("dbo.SisTarefa", "SisPrioridadeId", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisNivel3Id", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisNivel2Id", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisNivel1Id", "dbo.DocRotulo");
            DropForeignKey("dbo.SisTarefa", "SisModuloId", "dbo.DocRotulo");
            DropIndex("dbo.SisTarefa", new[] { "SisNivel3Id" });
            DropIndex("dbo.SisTarefa", new[] { "SisNivel2Id" });
            DropIndex("dbo.SisTarefa", new[] { "SisNivel1Id" });
            DropIndex("dbo.SisTarefa", new[] { "SisTipoTarefaId" });
            DropIndex("dbo.SisTarefa", new[] { "SisPrioridadeId" });
            DropIndex("dbo.SisTarefa", new[] { "SisStatusId" });
            DropIndex("dbo.SisTarefa", new[] { "SisModuloId" });
            DropIndex("dbo.SisTarefa", new[] { "SisProjetoId" });
            DropIndex("dbo.SisTarefaIntervalo", new[] { "SisTarefaId" });
            AlterColumn("dbo.SisTarefa", "DataTermino", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisTarefa", "DataInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisTarefa", "DataPrevistaTermino", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisTarefa", "DataPrevistaInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SisTarefa", "DataRegistro", c => c.DateTime(nullable: false));
            DropColumn("dbo.SisTarefa", "SisNivel3Id");
            DropColumn("dbo.SisTarefa", "SisNivel2Id");
            DropColumn("dbo.SisTarefa", "SisNivel1Id");
            DropColumn("dbo.SisTarefa", "SisTipoTarefaId");
            DropColumn("dbo.SisTarefa", "SisPrioridadeId");
            DropTable("dbo.SisTarefaIntervalo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TarefaIntervalo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameColumn(table: "dbo.SisTarefa", name: "SisStatusId", newName: "StatusId");
            RenameColumn(table: "dbo.SisTarefa", name: "SisModuloId", newName: "ModuloId");
            RenameColumn(table: "dbo.SisTarefa", name: "SisProjetoId", newName: "ProjetoId");
        }
    }
}
