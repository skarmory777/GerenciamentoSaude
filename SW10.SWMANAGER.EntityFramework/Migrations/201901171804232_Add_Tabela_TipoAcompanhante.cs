namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Add_Tabela_TipoAcompanhante : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteTipoAcompanhante",
                c => new
                {
                    Id = c.Long(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
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
                    { "DynamicFilter_TipoAcompanhante_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AteAtendimento", "AteTipoAcompanhanteId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "AteTipoAcompanhanteId");
            AddForeignKey("dbo.AteAtendimento", "AteTipoAcompanhanteId", "dbo.AteTipoAcompanhante", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "AteTipoAcompanhanteId", "dbo.AteTipoAcompanhante");
            DropIndex("dbo.AteAtendimento", new[] { "AteTipoAcompanhanteId" });
            DropColumn("dbo.AteAtendimento", "AteTipoAcompanhanteId");
            DropTable("dbo.AteTipoAcompanhante",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAcompanhante_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
