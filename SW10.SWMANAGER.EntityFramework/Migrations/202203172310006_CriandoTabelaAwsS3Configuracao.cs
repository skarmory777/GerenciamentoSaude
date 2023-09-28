namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoTabelaAwsS3Configuracao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AwsS3Configuracao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmpresaId = c.Long(nullable: false),
                        AcessKey = c.String(),
                        SecretKey = c.String(),
                        BucketName = c.String(),
                        BucketRegion = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .Index(t => t.EmpresaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AwsS3Configuracao", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.AwsS3Configuracao", new[] { "EmpresaId" });
            DropTable("dbo.AwsS3Configuracao");
        }
    }
}
