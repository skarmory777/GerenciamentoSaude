namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_Resultado_Exame_Incluindo_Relacionamentos_Campo_Mneumonico : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LabResultadoExame", name: "FormataId", newName: "LabFormataId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "ExameId", newName: "LabExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "ResultadoId", newName: "LabResultadoId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "KitId", newName: "LabKitExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioConferidoExameId", newName: "SisUsuarioConferidoId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioDigitadoExameId", newName: "SisUsuarioDigitadoid");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioPendenteExameId", newName: "SisUsuarioPendenteExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioImpressoExameId", newName: "SisUsuarioImpressoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "MaterialExameId", newName: "LabMaterialId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioCienteExameId", newName: "SisUsuarioCienteExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioImpSolicitaId", newName: "SisUsuarioImpSolicitaId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "UsuarioAlteradoExameId", newName: "SisUsuarioAlteradoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "TabelaId", newName: "LabTabelaId");
            RenameIndex(table: "dbo.LabResultadoExame", name: "IX_TabelaId", newName: "IX_LabTabelaId");
            AddColumn("dbo.LabResultadoExame", "Mneumonico", c => c.String());
            CreateIndex("dbo.LabResultadoExame", "LabFormataId");
            CreateIndex("dbo.LabResultadoExame", "LabExameId");
            CreateIndex("dbo.LabResultadoExame", "LabResultadoId");
            CreateIndex("dbo.LabResultadoExame", "FatContaItemId");
            CreateIndex("dbo.LabResultadoExame", "LabKitExameId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioConferidoId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioDigitadoid");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioPendenteExameId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioImpressoExameId");
            CreateIndex("dbo.LabResultadoExame", "LabMaterialId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioCienteExameId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioImpSolicitaId");
            CreateIndex("dbo.LabResultadoExame", "SisUsuarioAlteradoExameId");
            AddForeignKey("dbo.LabResultadoExame", "LabExameId", "dbo.LabExame", "Id");
            AddForeignKey("dbo.LabResultadoExame", "FatContaItemId", "dbo.FatContaItem", "Id");
            AddForeignKey("dbo.LabResultadoExame", "LabFormataId", "dbo.LabFormata", "Id");
            AddForeignKey("dbo.LabResultadoExame", "LabKitExameId", "dbo.LabKitExame", "Id");
            AddForeignKey("dbo.LabResultadoExame", "LabMaterialId", "dbo.LabMaterial", "Id");
            AddForeignKey("dbo.LabResultadoExame", "LabResultadoId", "dbo.LabResultado", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioAlteradoExameId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioCienteExameId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioConferidoId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioDigitadoid", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioImpressoExameId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioImpSolicitaId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.LabResultadoExame", "SisUsuarioPendenteExameId", "dbo.AbpUsers", "Id");
            DropColumn("dbo.LabResultadoExame", "UsuarioExclusaoId");
            DropColumn("dbo.LabResultadoExame", "UsuarioAlteracaoId");
            DropColumn("dbo.LabResultadoExame", "UsuarioInclusaoId");
        }

        public override void Down()
        {
            AddColumn("dbo.LabResultadoExame", "UsuarioInclusaoId", c => c.Long());
            AddColumn("dbo.LabResultadoExame", "UsuarioAlteracaoId", c => c.Long());
            AddColumn("dbo.LabResultadoExame", "UsuarioExclusaoId", c => c.Long());
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioPendenteExameId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioImpSolicitaId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioImpressoExameId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioDigitadoid", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioConferidoId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioCienteExameId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "SisUsuarioAlteradoExameId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultadoExame", "LabResultadoId", "dbo.LabResultado");
            DropForeignKey("dbo.LabResultadoExame", "LabMaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.LabResultadoExame", "LabKitExameId", "dbo.LabKitExame");
            DropForeignKey("dbo.LabResultadoExame", "LabFormataId", "dbo.LabFormata");
            DropForeignKey("dbo.LabResultadoExame", "FatContaItemId", "dbo.FatContaItem");
            DropForeignKey("dbo.LabResultadoExame", "LabExameId", "dbo.LabExame");
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioAlteradoExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioImpSolicitaId" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioCienteExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "LabMaterialId" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioImpressoExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioPendenteExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioDigitadoid" });
            DropIndex("dbo.LabResultadoExame", new[] { "SisUsuarioConferidoId" });
            DropIndex("dbo.LabResultadoExame", new[] { "LabKitExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "FatContaItemId" });
            DropIndex("dbo.LabResultadoExame", new[] { "LabResultadoId" });
            DropIndex("dbo.LabResultadoExame", new[] { "LabExameId" });
            DropIndex("dbo.LabResultadoExame", new[] { "LabFormataId" });
            DropColumn("dbo.LabResultadoExame", "Mneumonico");
            RenameIndex(table: "dbo.LabResultadoExame", name: "IX_LabTabelaId", newName: "IX_TabelaId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabTabelaId", newName: "TabelaId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioAlteradoExameId", newName: "UsuarioAlteradoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioImpSolicitaId", newName: "UsuarioImpSolicitaId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioCienteExameId", newName: "UsuarioCienteExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabMaterialId", newName: "MaterialExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioImpressoExameId", newName: "UsuarioImpressoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioPendenteExameId", newName: "UsuarioPendenteExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioDigitadoid", newName: "UsuarioDigitadoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SisUsuarioConferidoId", newName: "UsuarioConferidoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabKitExameId", newName: "KitId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabResultadoId", newName: "ResultadoId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabExameId", newName: "ExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "LabFormataId", newName: "FormataId");
        }
    }
}
