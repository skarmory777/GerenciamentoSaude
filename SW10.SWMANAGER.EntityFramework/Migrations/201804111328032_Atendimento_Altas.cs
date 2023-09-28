namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Atendimento_Altas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAlta", "AteGrupoCIDId", "dbo.GrupoCID");
            DropForeignKey("dbo.AteAlta", "AteMotivoAltaId", "dbo.AssMotivoAlta");
            DropForeignKey("dbo.AteAltaMedica", "AteLeitoId", "dbo.AteLeito");
            DropIndex("dbo.AteAlta", new[] { "AteGrupoCIDId" });
            DropIndex("dbo.AteAlta", new[] { "AteMotivoAltaId" });
            DropIndex("dbo.AteAltaMedica", new[] { "AteLeitoId" });
            RenameColumn(table: "dbo.AteAtendimento", name: "SisLeitoId", newName: "AteLeitoId");
            RenameColumn(table: "dbo.AteAtendimento", name: "AssMotivoAltaId", newName: "AteMotivoAltaId");
            RenameIndex(table: "dbo.AteAtendimento", name: "IX_AssMotivoAltaId", newName: "IX_AteMotivoAltaId");
            RenameIndex(table: "dbo.AteAtendimento", name: "IX_SisLeitoId", newName: "IX_AteLeitoId");
            AddColumn("dbo.AteAtendimento", "AteGrupoCIDId", c => c.Long());
            AddColumn("dbo.AteAtendimento", "DataAltaMedica", c => c.DateTime());
            CreateIndex("dbo.AteAtendimento", "AteGrupoCIDId");
            AddForeignKey("dbo.AteAtendimento", "AteGrupoCIDId", "dbo.GrupoCID", "Id");
            DropTable("dbo.AteAlta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Alta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteAltaMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AltaMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.AteAltaMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    AteLeitoId = c.Long(),
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
                    { "DynamicFilter_AltaMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteAlta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    AteGrupoCIDId = c.Long(),
                    AteMotivoAltaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Alta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            DropForeignKey("dbo.AteAtendimento", "AteGrupoCIDId", "dbo.GrupoCID");
            DropIndex("dbo.AteAtendimento", new[] { "AteGrupoCIDId" });
            DropColumn("dbo.AteAtendimento", "DataAltaMedica");
            DropColumn("dbo.AteAtendimento", "AteGrupoCIDId");
            RenameIndex(table: "dbo.AteAtendimento", name: "IX_AteLeitoId", newName: "IX_SisLeitoId");
            RenameIndex(table: "dbo.AteAtendimento", name: "IX_AteMotivoAltaId", newName: "IX_AssMotivoAltaId");
            RenameColumn(table: "dbo.AteAtendimento", name: "AteMotivoAltaId", newName: "AssMotivoAltaId");
            RenameColumn(table: "dbo.AteAtendimento", name: "AteLeitoId", newName: "SisLeitoId");
            CreateIndex("dbo.AteAltaMedica", "AteLeitoId");
            CreateIndex("dbo.AteAlta", "AteMotivoAltaId");
            CreateIndex("dbo.AteAlta", "AteGrupoCIDId");
            AddForeignKey("dbo.AteAltaMedica", "AteLeitoId", "dbo.AteLeito", "Id");
            AddForeignKey("dbo.AteAlta", "AteMotivoAltaId", "dbo.AssMotivoAlta", "Id");
            AddForeignKey("dbo.AteAlta", "AteGrupoCIDId", "dbo.GrupoCID", "Id");
        }
    }
}
