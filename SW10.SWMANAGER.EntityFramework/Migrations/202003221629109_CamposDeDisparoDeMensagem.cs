namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CamposDeDisparoDeMensagem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisDisparoDeMensagem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataProgramada = c.DateTime(nullable: false),
                        DataInicioDisparo = c.DateTime(),
                        DataFinalDisparo = c.DateTime(),
                        Mensagem = c.String(),
                        Total = c.Long(nullable: false),
                        TotalEnviado = c.Long(nullable: false),
                        TotalRecebido = c.Long(nullable: false),
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
                    { "DynamicFilter_DisparoDeMensagem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SisDisparoDeMensagemItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Origem = c.String(),
                        OrigemId = c.Long(),
                        PessoaId = c.Long(),
                        DisparoDeMensagemId = c.Long(),
                        DisparoDeMensagemItemTipoId = c.Long(nullable: false),
                        DataProgramada = c.DateTime(nullable: false),
                        DataInicioDisparo = c.DateTime(),
                        DataFinalDisparo = c.DateTime(),
                        DataRecebimento = c.DateTime(),
                        Mensagem = c.String(),
                        Valor = c.String(),
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
                    { "DynamicFilter_DisparoDeMensagemItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisDisparoDeMensagem", t => t.DisparoDeMensagemId)
                .ForeignKey("dbo.SisDisparoDeMensagemItemTipo", t => t.DisparoDeMensagemItemTipoId, cascadeDelete: true)
                .ForeignKey("dbo.SisPessoa", t => t.PessoaId)
                .Index(t => t.PessoaId)
                .Index(t => t.DisparoDeMensagemId)
                .Index(t => t.DisparoDeMensagemItemTipoId);
            
            CreateTable(
                "dbo.SisDisparoDeMensagemItemTipo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_DisparoDeMensagemItemTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SisDisparoDeMensagemItem", "PessoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.SisDisparoDeMensagemItem", "DisparoDeMensagemItemTipoId", "dbo.SisDisparoDeMensagemItemTipo");
            DropForeignKey("dbo.SisDisparoDeMensagemItem", "DisparoDeMensagemId", "dbo.SisDisparoDeMensagem");
            DropIndex("dbo.SisDisparoDeMensagemItem", new[] { "DisparoDeMensagemItemTipoId" });
            DropIndex("dbo.SisDisparoDeMensagemItem", new[] { "DisparoDeMensagemId" });
            DropIndex("dbo.SisDisparoDeMensagemItem", new[] { "PessoaId" });
            DropTable("dbo.SisDisparoDeMensagemItemTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DisparoDeMensagemItemTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisDisparoDeMensagemItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DisparoDeMensagemItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisDisparoDeMensagem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DisparoDeMensagem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
