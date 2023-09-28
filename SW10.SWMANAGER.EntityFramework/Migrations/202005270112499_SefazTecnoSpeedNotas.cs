namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SefazTecnoSpeedNotas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sefaz_TecnoSpeedConfiguracoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Encode = c.Boolean(nullable: false),
                        Producao = c.Boolean(nullable: false),
                        Grupo = c.String(),
                        Cnpj = c.String(),
                        User = c.String(),
                        Password = c.String(),
                        Delimitador = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sefaz_TecnoSpeed_Notas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Cnpj = c.String(),
                        ChaveNfe = c.String(),
                        Emitente = c.String(),
                        IdentificadorEmitente = c.String(),
                        IdentificadorTipoEmitente = c.String(),
                        Modelo = c.Int(nullable: false),
                        Serie = c.Int(nullable: false),
                        NumeroNota = c.Long(nullable: false),
                        ValorNota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        IsMDEConhecimento = c.Boolean(nullable: false),
                        IsMDEConfirmacao = c.Boolean(nullable: false),
                        IsNotaSefazTecnospeed = c.Boolean(nullable: false),
                        AttemptNotaSefazTecnospeedCount = c.Int(nullable: false),
                        DateNotaSefazTecnospeed = c.DateTime(),
                        DataEmissao = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sefaz_TecnoSpeed_Notas");
            DropTable("dbo.Sefaz_TecnoSpeedConfiguracoes");
        }
    }
}
