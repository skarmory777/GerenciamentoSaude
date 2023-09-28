namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_ContaItem_Terceirizado_UnidOrg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Terceirizado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisPessoaId = c.Long(),
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
                    { "DynamicFilter_Terceirizado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisPessoa", t => t.SisPessoaId)
                .Index(t => t.SisPessoaId);

            AddColumn("dbo.FatContaItem", "UnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.FatContaItem", "TerceirizadoId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "UnidadeOrganizacionalId");
            CreateIndex("dbo.FatContaItem", "TerceirizadoId");
            AddForeignKey("dbo.FatContaItem", "TerceirizadoId", "dbo.Terceirizado", "Id");
            AddForeignKey("dbo.FatContaItem", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatContaItem", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.FatContaItem", "TerceirizadoId", "dbo.Terceirizado");
            DropForeignKey("dbo.Terceirizado", "SisPessoaId", "dbo.SisPessoa");
            DropIndex("dbo.Terceirizado", new[] { "SisPessoaId" });
            DropIndex("dbo.FatContaItem", new[] { "TerceirizadoId" });
            DropIndex("dbo.FatContaItem", new[] { "UnidadeOrganizacionalId" });
            DropColumn("dbo.FatContaItem", "TerceirizadoId");
            DropColumn("dbo.FatContaItem", "UnidadeOrganizacionalId");
            DropTable("dbo.Terceirizado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Terceirizado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
