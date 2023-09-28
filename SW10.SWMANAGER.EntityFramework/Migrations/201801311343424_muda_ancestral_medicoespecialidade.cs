namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class muda_ancestral_medicoespecialidade : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.SisMedicoEspecialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisMedicoId = c.Long(nullable: false),
                    SisEspecialidadeId = c.Long(nullable: false),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_MedicoEspecialidade_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.SisMedicoEspecialidade", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMedicoEspecialidade", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.SisMedicoEspecialidade", "Descricao", c => c.String());
            AddColumn("dbo.SisMedicoEspecialidade", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMedicoEspecialidade", "DeleterUserId", c => c.Long());
            AddColumn("dbo.SisMedicoEspecialidade", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.SisMedicoEspecialidade", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.SisMedicoEspecialidade", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.SisMedicoEspecialidade", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SisMedicoEspecialidade", "CreatorUserId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.SisMedicoEspecialidade", "CreatorUserId");
            DropColumn("dbo.SisMedicoEspecialidade", "CreationTime");
            DropColumn("dbo.SisMedicoEspecialidade", "LastModifierUserId");
            DropColumn("dbo.SisMedicoEspecialidade", "LastModificationTime");
            DropColumn("dbo.SisMedicoEspecialidade", "DeletionTime");
            DropColumn("dbo.SisMedicoEspecialidade", "DeleterUserId");
            DropColumn("dbo.SisMedicoEspecialidade", "IsDeleted");
            DropColumn("dbo.SisMedicoEspecialidade", "Descricao");
            DropColumn("dbo.SisMedicoEspecialidade", "Codigo");
            DropColumn("dbo.SisMedicoEspecialidade", "IsSistema");
            AlterTableAnnotations(
                "dbo.SisMedicoEspecialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisMedicoId = c.Long(nullable: false),
                    SisEspecialidadeId = c.Long(nullable: false),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_MedicoEspecialidade_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

        }
    }
}
