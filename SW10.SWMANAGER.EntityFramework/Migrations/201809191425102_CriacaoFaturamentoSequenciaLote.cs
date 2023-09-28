namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class CriacaoFaturamentoSequenciaLote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatSequenciaLote",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Sequencia = c.Long(nullable: false),
                    ConvenioId = c.Long(nullable: false),
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
                    { "DynamicFilter_FaturamentoSequenciaLote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId, cascadeDelete: true)
                .Index(t => t.ConvenioId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatSequenciaLote", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatSequenciaLote", new[] { "ConvenioId" });
            DropTable("dbo.FatSequenciaLote",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoSequenciaLote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
