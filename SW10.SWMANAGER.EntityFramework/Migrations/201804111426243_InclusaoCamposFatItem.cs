namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposFatItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LabExame", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.LabExame", new[] { "FaturamentoItemId" });
            AddColumn("dbo.FatItem", "IsExameSimples", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsPeso", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsTesta100", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsAltura", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsCor", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsMestruacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsNacionalidade", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsNaturalidade", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsImpReferencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsCultura", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsPendente", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsRepete", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsLibera", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "Mneumonico", c => c.String());
            AddColumn("dbo.FatItem", "OrdemImp", c => c.Int());
            AddColumn("dbo.FatItem", "Prazo", c => c.Int());
            AddColumn("dbo.FatItem", "Interpretacao", c => c.Binary());
            AddColumn("dbo.FatItem", "Extra1", c => c.Binary());
            AddColumn("dbo.FatItem", "Extra2", c => c.Binary());
            AddColumn("dbo.FatItem", "QtdFatura", c => c.Int());
            AddColumn("dbo.FatItem", "MapaExame", c => c.String());
            AddColumn("dbo.FatItem", "OrdemResul", c => c.Int());
            AddColumn("dbo.FatItem", "OrdemResumo", c => c.Int());
            AddColumn("dbo.FatItem", "OrdemMapaResultado", c => c.Int());
            AddColumn("dbo.FatItem", "EquipamentoId", c => c.Long());
            AddColumn("dbo.FatItem", "ExameIncluiId", c => c.Long());
            AddColumn("dbo.FatItem", "SetorId", c => c.Long());
            AddColumn("dbo.FatItem", "MaterialId", c => c.Long());
            AddColumn("dbo.FatItem", "MetodoId", c => c.Long());
            AddColumn("dbo.FatItem", "UnidadeId", c => c.Long());
            AddColumn("dbo.FatItem", "FormataId", c => c.Long());
            AddColumn("dbo.FatItem", "MapaId", c => c.Long());
            CreateIndex("dbo.FatItem", "EquipamentoId");
            CreateIndex("dbo.FatItem", "ExameIncluiId");
            CreateIndex("dbo.FatItem", "SetorId");
            CreateIndex("dbo.FatItem", "MaterialId");
            CreateIndex("dbo.FatItem", "MetodoId");
            CreateIndex("dbo.FatItem", "UnidadeId");
            CreateIndex("dbo.FatItem", "FormataId");
            CreateIndex("dbo.FatItem", "MapaId");
            AddForeignKey("dbo.FatItem", "EquipamentoId", "dbo.LabEquipamento", "Id");
            AddForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.LabExame", "Id");
            AddForeignKey("dbo.FatItem", "FormataId", "dbo.LabFormata", "Id");
            AddForeignKey("dbo.FatItem", "MapaId", "dbo.LabMapa", "Id");
            AddForeignKey("dbo.FatItem", "MaterialId", "dbo.LabMaterial", "Id");
            AddForeignKey("dbo.FatItem", "MetodoId", "dbo.LabMetodo", "Id");
            AddForeignKey("dbo.FatItem", "SetorId", "dbo.LabSetor", "Id");
            AddForeignKey("dbo.FatItem", "UnidadeId", "dbo.LabUnidade", "Id");
            DropColumn("dbo.LabExame", "FaturamentoItemId");
        }

        public override void Down()
        {
            AddColumn("dbo.LabExame", "FaturamentoItemId", c => c.Long());
            DropForeignKey("dbo.FatItem", "UnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.FatItem", "SetorId", "dbo.LabSetor");
            DropForeignKey("dbo.FatItem", "MetodoId", "dbo.LabMetodo");
            DropForeignKey("dbo.FatItem", "MaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.FatItem", "MapaId", "dbo.LabMapa");
            DropForeignKey("dbo.FatItem", "FormataId", "dbo.LabFormata");
            DropForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.LabExame");
            DropForeignKey("dbo.FatItem", "EquipamentoId", "dbo.LabEquipamento");
            DropIndex("dbo.FatItem", new[] { "MapaId" });
            DropIndex("dbo.FatItem", new[] { "FormataId" });
            DropIndex("dbo.FatItem", new[] { "UnidadeId" });
            DropIndex("dbo.FatItem", new[] { "MetodoId" });
            DropIndex("dbo.FatItem", new[] { "MaterialId" });
            DropIndex("dbo.FatItem", new[] { "SetorId" });
            DropIndex("dbo.FatItem", new[] { "ExameIncluiId" });
            DropIndex("dbo.FatItem", new[] { "EquipamentoId" });
            DropColumn("dbo.FatItem", "MapaId");
            DropColumn("dbo.FatItem", "FormataId");
            DropColumn("dbo.FatItem", "UnidadeId");
            DropColumn("dbo.FatItem", "MetodoId");
            DropColumn("dbo.FatItem", "MaterialId");
            DropColumn("dbo.FatItem", "SetorId");
            DropColumn("dbo.FatItem", "ExameIncluiId");
            DropColumn("dbo.FatItem", "EquipamentoId");
            DropColumn("dbo.FatItem", "OrdemMapaResultado");
            DropColumn("dbo.FatItem", "OrdemResumo");
            DropColumn("dbo.FatItem", "OrdemResul");
            DropColumn("dbo.FatItem", "MapaExame");
            DropColumn("dbo.FatItem", "QtdFatura");
            DropColumn("dbo.FatItem", "Extra2");
            DropColumn("dbo.FatItem", "Extra1");
            DropColumn("dbo.FatItem", "Interpretacao");
            DropColumn("dbo.FatItem", "Prazo");
            DropColumn("dbo.FatItem", "OrdemImp");
            DropColumn("dbo.FatItem", "Mneumonico");
            DropColumn("dbo.FatItem", "IsLibera");
            DropColumn("dbo.FatItem", "IsRepete");
            DropColumn("dbo.FatItem", "IsPendente");
            DropColumn("dbo.FatItem", "IsCultura");
            DropColumn("dbo.FatItem", "IsImpReferencia");
            DropColumn("dbo.FatItem", "IsNaturalidade");
            DropColumn("dbo.FatItem", "IsNacionalidade");
            DropColumn("dbo.FatItem", "IsMestruacao");
            DropColumn("dbo.FatItem", "IsCor");
            DropColumn("dbo.FatItem", "IsAltura");
            DropColumn("dbo.FatItem", "IsTesta100");
            DropColumn("dbo.FatItem", "IsPeso");
            DropColumn("dbo.FatItem", "IsExameSimples");
            CreateIndex("dbo.LabExame", "FaturamentoItemId");
            AddForeignKey("dbo.LabExame", "FaturamentoItemId", "dbo.FatItem", "Id");
        }
    }
}
