namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoTabelaAnexo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anexo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AnexoListaId = c.Guid(nullable: false),
                        FileName = c.String(),
                        BucketName = c.String(),
                        Key = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Anexo");
        }
    }
}
