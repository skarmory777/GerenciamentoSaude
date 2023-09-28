namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUserNotificationMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Long(nullable: false),
                        Source = c.String(),
                        SourceId = c.String(),
                        Title = c.String(),
                        Message = c.String(nullable: false),
                        ReadState = c.Int(nullable: false),
                        UserBlockConfirmation = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        ReadTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SisAvisoGrupos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AvisoId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
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
                    { "DynamicFilter_AvisoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisAvisos", t => t.AvisoId, cascadeDelete: true)
                .ForeignKey("dbo.AbpRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AvisoId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SisAvisos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Titulo = c.String(),
                        Mensagem = c.String(),
                        DataProgramada = c.DateTime(nullable: false),
                        DataInicioDisparo = c.DateTime(),
                        DataFinalDisparo = c.DateTime(),
                        Bloquear = c.Boolean(nullable: false),
                        DisparoAtivo = c.Boolean(nullable: false),
                        TotalEnviado = c.Long(nullable: false),
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
                    { "DynamicFilter_Aviso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SisAvisoGrupos", "RoleId", "dbo.AbpRoles");
            DropForeignKey("dbo.SisAvisoGrupos", "AvisoId", "dbo.SisAvisos");
            DropIndex("dbo.SisAvisoGrupos", new[] { "RoleId" });
            DropIndex("dbo.SisAvisoGrupos", new[] { "AvisoId" });
            DropTable("dbo.SisAvisos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Aviso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisAvisoGrupos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AvisoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AppUserNotificationMessages");
        }
    }
}
