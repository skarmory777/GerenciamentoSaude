namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Incluindo_Campos_Tiss_Convenio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisConvenioURLServico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Url = c.String(),
                    SisConvenioId = c.Long(),
                    SisVersaoTissId = c.Long(),
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
                    { "DynamicFilter_ConvenioURLServico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.SisVersaoTiss", t => t.SisVersaoTissId)
                .Index(t => t.SisConvenioId)
                .Index(t => t.SisVersaoTissId);

            AddColumn("dbo.SisConvenio", "IsPreencheCodigoCredenciadoCodigoOperadora", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsImprimeTratamento", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsImprimeObsConta", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsAgrupaGuiaXml", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "Is09e10", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsFatorMultiplicador", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsEquipeMedicaBranco", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsObrigaEspecialidade", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "Is41a45BrancoPJ", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsSomarFilmeTaxa", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsImprimeObsContaGuiaConsulta", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsImportaAgudaCronica", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "Is38e39GuiaConsulta", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "Is86e89GuiaSPSADT", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsFilmeGuiaOutrasDespesas", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "Is22GuiaSPSADT", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "XmlUltimosDigitosMatricula", c => c.Int(nullable: false));
            AddColumn("dbo.SisConvenio", "XmlPrimeirosDititosMatricula", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisConvenioURLServico", "SisVersaoTissId", "dbo.SisVersaoTiss");
            DropForeignKey("dbo.SisConvenioURLServico", "SisConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.SisConvenioURLServico", new[] { "SisVersaoTissId" });
            DropIndex("dbo.SisConvenioURLServico", new[] { "SisConvenioId" });
            DropColumn("dbo.SisConvenio", "XmlPrimeirosDititosMatricula");
            DropColumn("dbo.SisConvenio", "XmlUltimosDigitosMatricula");
            DropColumn("dbo.SisConvenio", "Is22GuiaSPSADT");
            DropColumn("dbo.SisConvenio", "IsFilmeGuiaOutrasDespesas");
            DropColumn("dbo.SisConvenio", "Is86e89GuiaSPSADT");
            DropColumn("dbo.SisConvenio", "Is38e39GuiaConsulta");
            DropColumn("dbo.SisConvenio", "IsImportaAgudaCronica");
            DropColumn("dbo.SisConvenio", "IsImprimeObsContaGuiaConsulta");
            DropColumn("dbo.SisConvenio", "IsSomarFilmeTaxa");
            DropColumn("dbo.SisConvenio", "Is41a45BrancoPJ");
            DropColumn("dbo.SisConvenio", "IsObrigaEspecialidade");
            DropColumn("dbo.SisConvenio", "IsEquipeMedicaBranco");
            DropColumn("dbo.SisConvenio", "IsFatorMultiplicador");
            DropColumn("dbo.SisConvenio", "Is09e10");
            DropColumn("dbo.SisConvenio", "IsAgrupaGuiaXml");
            DropColumn("dbo.SisConvenio", "IsImprimeObsConta");
            DropColumn("dbo.SisConvenio", "IsImprimeTratamento");
            DropColumn("dbo.SisConvenio", "IsPreencheCodigoCredenciadoCodigoOperadora");
            DropTable("dbo.SisConvenioURLServico",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConvenioURLServico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
