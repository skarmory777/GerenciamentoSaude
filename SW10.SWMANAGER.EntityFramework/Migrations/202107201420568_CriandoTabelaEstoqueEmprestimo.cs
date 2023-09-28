namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoTabelaEstoqueEmprestimo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstoqueEmprestimo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SisPessoaId = c.Long(nullable: false),
                        ContatoNome = c.String(maxLength: 100),
                        ContatoTelefone = c.String(maxLength: 80),
                        ContatoEmail = c.String(maxLength: 100),
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
                    { "DynamicFilter_EstoqueEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisPessoa", t => t.SisPessoaId, cascadeDelete: true)
                .Index(t => t.SisPessoaId);
            
            AddColumn("dbo.EstoqueMovimento", "EstoqueEmprestimoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "EstoqueEmprestimoId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "EstoqueEmprestimoId");
            CreateIndex("dbo.EstoquePreMovimento", "EstoqueEmprestimoId");
            AddForeignKey("dbo.EstoqueMovimento", "EstoqueEmprestimoId", "dbo.EstoqueEmprestimo", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstoqueEmprestimoId", "dbo.EstoqueEmprestimo", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueEmprestimoId", "dbo.EstoqueEmprestimo");
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueEmprestimoId", "dbo.EstoqueEmprestimo");
            DropForeignKey("dbo.EstoqueEmprestimo", "SisPessoaId", "dbo.SisPessoa");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueEmprestimoId" });
            DropIndex("dbo.EstoqueEmprestimo", new[] { "SisPessoaId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueEmprestimoId" });
            DropColumn("dbo.EstoquePreMovimento", "EstoqueEmprestimoId");
            DropColumn("dbo.EstoqueMovimento", "EstoqueEmprestimoId");
            DropTable("dbo.EstoqueEmprestimo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
