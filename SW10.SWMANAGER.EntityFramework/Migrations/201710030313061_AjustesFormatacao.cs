namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class AjustesFormatacao : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.LabEquipamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    DiretorioOrdem = c.String(),
                    DiretorioResultado = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    Informacao_Id = c.Long(),
                    Informacao_Id1 = c.Long(),
                },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_Equipamento_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.LabEquipamento", "IsSistema", c => c.Boolean(nullable: false));
            //AddColumn("dbo.LabEquipamento", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabEquipamento", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabEquipamento", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabEquipamento", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabEquipamento", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabEquipamento", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabEquipamento", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabEquipamento", "CreatorUserId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.LabEquipamento", "CreatorUserId");
            DropColumn("dbo.LabEquipamento", "CreationTime");
            DropColumn("dbo.LabEquipamento", "LastModifierUserId");
            DropColumn("dbo.LabEquipamento", "LastModificationTime");
            DropColumn("dbo.LabEquipamento", "DeletionTime");
            DropColumn("dbo.LabEquipamento", "DeleterUserId");
            DropColumn("dbo.LabEquipamento", "IsDeleted");
            DropColumn("dbo.LabEquipamento", "Codigo");
            DropColumn("dbo.LabEquipamento", "IsSistema");
            AlterTableAnnotations(
                "dbo.LabEquipamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    DiretorioOrdem = c.String(),
                    DiretorioResultado = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    Informacao_Id = c.Long(),
                    Informacao_Id1 = c.Long(),
                },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_Equipamento_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

        }
    }
}
