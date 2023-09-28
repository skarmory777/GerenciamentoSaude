namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Tabela_parametros : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisParametro",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EmpresaId = c.Long(),
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
                    { "DynamicFilter_Parametro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId)
                .Index(t => t.EmpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisParametro", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.SisParametro", new[] { "EmpresaId" });
            DropTable("dbo.SisParametro",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Parametro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
