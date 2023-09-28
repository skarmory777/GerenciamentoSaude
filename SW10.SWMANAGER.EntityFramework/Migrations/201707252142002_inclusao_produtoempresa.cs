namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class inclusao_produtoempresa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Est_ProdutoEmpresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ProdutoEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: true)
                .Index(t => t.ProdutoId)
                .Index(t => t.EmpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Est_ProdutoEmpresa", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.Est_ProdutoEmpresa", "EmpresaId", "dbo.Empresa");
            DropIndex("dbo.Est_ProdutoEmpresa", new[] { "EmpresaId" });
            DropIndex("dbo.Est_ProdutoEmpresa", new[] { "ProdutoId" });
            DropTable("dbo.Est_ProdutoEmpresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
