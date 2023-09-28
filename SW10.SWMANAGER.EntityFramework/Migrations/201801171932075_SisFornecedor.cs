namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class SisFornecedor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisFornecedor",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisPessoaId = c.Long(),
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
                    { "DynamicFilter_SisFornecedor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisPessoa", t => t.SisPessoaId)
                .Index(t => t.SisPessoaId);

            AddColumn("dbo.SisPessoa", "FisicaJuridica", c => c.String(maxLength: 1));
            AddColumn("dbo.EstoqueMovimento", "SisFornecedorId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "Frete_SisFornecedorId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "SisFornecedorId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "Frete_SisFornecedorId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "SisFornecedorId");
            CreateIndex("dbo.EstoqueMovimento", "Frete_SisFornecedorId");
            CreateIndex("dbo.EstoquePreMovimento", "SisFornecedorId");
            CreateIndex("dbo.EstoquePreMovimento", "Frete_SisFornecedorId");
            AddForeignKey("dbo.EstoquePreMovimento", "Frete_SisFornecedorId", "dbo.SisFornecedor", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "SisFornecedorId", "dbo.SisFornecedor", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "Frete_SisFornecedorId", "dbo.SisFornecedor", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "SisFornecedorId", "dbo.SisFornecedor", "Id");
            DropColumn("dbo.SisPessoa", "FisicaJuricada");
        }

        public override void Down()
        {
            AddColumn("dbo.SisPessoa", "FisicaJuricada", c => c.String(maxLength: 1));
            DropForeignKey("dbo.EstoqueMovimento", "SisFornecedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.EstoqueMovimento", "Frete_SisFornecedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.EstoquePreMovimento", "SisFornecedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.EstoquePreMovimento", "Frete_SisFornecedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.SisFornecedor", "SisPessoaId", "dbo.SisPessoa");
            DropIndex("dbo.SisFornecedor", new[] { "SisPessoaId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "Frete_SisFornecedorId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "SisFornecedorId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "Frete_SisFornecedorId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "SisFornecedorId" });
            DropColumn("dbo.EstoquePreMovimento", "Frete_SisFornecedorId");
            DropColumn("dbo.EstoquePreMovimento", "SisFornecedorId");
            DropColumn("dbo.EstoqueMovimento", "Frete_SisFornecedorId");
            DropColumn("dbo.EstoqueMovimento", "SisFornecedorId");
            DropColumn("dbo.SisPessoa", "FisicaJuridica");
            DropTable("dbo.SisFornecedor",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisFornecedor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
