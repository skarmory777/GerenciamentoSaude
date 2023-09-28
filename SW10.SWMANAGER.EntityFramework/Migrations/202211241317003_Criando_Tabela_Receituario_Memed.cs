namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Criando_Tabela_Receituario_Memed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssReceituario",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AtendimentoId = c.Long(nullable: false),
                        DataReceituario = c.DateTime(nullable: false),
                        SisMedicoId = c.Long(nullable: false),
                        PrescricaoMemedId = c.String(),
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
                    { "DynamicFilter_ReceituarioMedico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.SisMedico", t => t.SisMedicoId, cascadeDelete: true)
                .Index(t => t.AtendimentoId)
                .Index(t => t.SisMedicoId);
            
            AddColumn("dbo.SisMedico", "PrescritorMemedToken", c => c.String());
            AddColumn("dbo.SisPaciente", "MemedId", c => c.String());

            DropForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "EstProdutoId" });
            DropColumn("dbo.CmpOrdemCompraItem", "EstProdutoId");
            AddColumn("dbo.CmpOrdemCompraItem", "EstProdutoId", c => c.Long(nullable: false));

            AddColumn("dbo.SisRegistroTabela", "TabelaPrincipal", c => c.String());
            CreateIndex("dbo.CmpOrdemCompraItem", "EstProdutoId");
            AddForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssReceituario", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssReceituario", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.AssReceituario", new[] { "SisMedicoId" });
            DropIndex("dbo.AssReceituario", new[] { "AtendimentoId" });
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "EstProdutoId" });
            DropColumn("dbo.SisRegistroTabela", "TabelaPrincipal");
            DropColumn("dbo.CmpOrdemCompraItem", "EstProdutoId");
            DropColumn("dbo.SisPaciente", "MemedId");
            DropColumn("dbo.SisMedico", "PrescritorMemedToken");
            DropTable("dbo.AssReceituario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ReceituarioMedico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
