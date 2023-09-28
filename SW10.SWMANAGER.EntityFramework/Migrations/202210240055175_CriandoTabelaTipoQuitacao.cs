namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class CriandoTabelaTipoQuitacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinTipoQuitacao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_TipoQuitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FinQuitacao", "TipoQuitacaoId", c => c.Long());
            CreateIndex("dbo.FinQuitacao", "TipoQuitacaoId");
            AddForeignKey("dbo.FinQuitacao", "TipoQuitacaoId", "dbo.FinTipoQuitacao", "Id");

            var command01 = "INSERT INTO [dbo].[FinTipoQuitacao] " +
           "([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime]" +
           ",[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[ImportaId]) VALUES " +
           "(1,'001','Quitação de Lançamento',0,null,null,null,null,GETDATE(),null,null) ;";

            Sql(command01);

            var command02 = "INSERT INTO [dbo].[FinTipoQuitacao] " +
           "([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime]" +
           ",[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[ImportaId]) VALUES " +
           "(1,'002','Tesouraria',0,null,null,null,null,GETDATE(),null,null) ;";

            Sql(command02);

            var command03 = "INSERT INTO [dbo].[FinTipoQuitacao] " +
           "([IsSistema],[Codigo],[Descricao],[IsDeleted],[DeleterUserId],[DeletionTime]" +
           ",[LastModificationTime],[LastModifierUserId],[CreationTime],[CreatorUserId],[ImportaId]) VALUES " +
           "(1,'003','Transferência',0,null,null,null,null,GETDATE(),null,null) ;";

            Sql(command03);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinQuitacao", "TipoQuitacaoId", "dbo.FinTipoQuitacao");
            DropIndex("dbo.FinQuitacao", new[] { "TipoQuitacaoId" });
            DropColumn("dbo.FinQuitacao", "TipoQuitacaoId");
            DropTable("dbo.FinTipoQuitacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoQuitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });            
        }
    }
}
