namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class FaturamentoItemAtendimentoCampos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatItemAtendimento",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AteAtendimentoId = c.Long(nullable: false),
                        SisMedicoId = c.Long(),
                        AteLeitoId = c.Long(),
                        Data = c.DateTime(nullable: false),
                        FaturamentoItemId = c.Long(),
                        Quantidade = c.Decimal(precision: 18, scale: 2),
                        Entidade = c.String(),
                        EntidadeId = c.Long(nullable: false),
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
                    { "DynamicFilter_FaturamentoItemAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AteAtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId)
                .ForeignKey("dbo.AteLeito", t => t.AteLeitoId)
                .ForeignKey("dbo.SisMedico", t => t.SisMedicoId)
                .Index(t => t.AteAtendimentoId)
                .Index(t => t.SisMedicoId)
                .Index(t => t.AteLeitoId)
                .Index(t => t.Data, name: "Fat_Idx_Data")
                .Index(t => t.FaturamentoItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FatItemAtendimento", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.FatItemAtendimento", "AteLeitoId", "dbo.AteLeito");
            DropForeignKey("dbo.FatItemAtendimento", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemAtendimento", "AteAtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.FatItemAtendimento", new[] { "FaturamentoItemId" });
            DropIndex("dbo.FatItemAtendimento", "Fat_Idx_Data");
            DropIndex("dbo.FatItemAtendimento", new[] { "AteLeitoId" });
            DropIndex("dbo.FatItemAtendimento", new[] { "SisMedicoId" });
            DropIndex("dbo.FatItemAtendimento", new[] { "AteAtendimentoId" });
            DropTable("dbo.FatItemAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoItemAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
