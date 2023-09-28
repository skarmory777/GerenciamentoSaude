namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class criacao_tabela_estoqueempresa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Est_EstoqueEmpresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueId = c.Long(),
                    EmpresaId = c.Long(),
                    ProcessoCota = c.Long(),
                    IsCotaLimitarTransferencia = c.Boolean(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_EstoqueEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueId)
                .Index(t => t.EstoqueId)
                .Index(t => t.EmpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Est_EstoqueEmpresa", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.Est_EstoqueEmpresa", "EmpresaId", "dbo.Empresa");
            DropIndex("dbo.Est_EstoqueEmpresa", new[] { "EmpresaId" });
            DropIndex("dbo.Est_EstoqueEmpresa", new[] { "EstoqueId" });
            DropTable("dbo.Est_EstoqueEmpresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
