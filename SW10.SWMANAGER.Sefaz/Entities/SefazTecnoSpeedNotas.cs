namespace Sefaz.Entities
{
    using Dapper.Contrib.Extensions;
    using NFe.Classes;
    using NFe.Utils;
    using System;

    [Table(SefazTecnoSpeedNotasTable)]
    public class SefazTecnoSpeedNotas
    {
        public const string SefazTecnoSpeedNotasTable = "Sefaz_TecnoSpeed_Notas";
        public SefazTecnoSpeedNotas()
        {

        }

        public SefazTecnoSpeedNotas(string cnpj, string chNFE, DateTime dataEmissao)
        {
            this.Cnpj = cnpj;
            this.ChaveNfe = chNFE;
            this.DataEmissao = dataEmissao;
        }

        public SefazTecnoSpeedNotas(string cnpj, nfeProc nfe, string chNFE, DateTime dataEmissao)
        {
            this.Cnpj = cnpj;
            this.ChaveNfe = chNFE;
            this.DataEmissao = dataEmissao;
            DadosNfe(nfe);
        }

        public void DadosNfe(nfeProc nfe)
        {
            if (nfe != null)
            {
                this.Emitente = $"{nfe.NFe.infNFe.emit.xNome}";
                this.IdentificadorEmitente = !nfe.NFe.infNFe.emit.CNPJ.CheckIfIsNullOrEmpty() ? nfe.NFe.infNFe.emit.CNPJ : nfe.NFe.infNFe.emit.CPF;
                this.IdentificadorTipoEmitente = !nfe.NFe.infNFe.emit.CNPJ.CheckIfIsNullOrEmpty() ? "CNPJ" : "CPF";
                this.ValorNota = nfe.NFe.infNFe.total.ICMSTot.vProd;
                this.Modelo = nfe.NFe.infNFe.ide.mod.ModeloDocumentoParaInt();
                this.Serie = nfe.NFe.infNFe.ide.serie;
                this.NumeroNota = nfe.NFe.infNFe.ide.nNF;

            }
        }

        [Key]
        public long Id { get; set; }

        public string Cnpj { get; set; }

        public string ChaveNfe { get; set; }

        public string Emitente { get; set; }

        public string IdentificadorEmitente { get; set; }
        public string IdentificadorTipoEmitente { get; set; }

        public int Modelo { get; set; }

        public int Serie { get; set; }
        public long NumeroNota { get; set; }

        public decimal ValorNota { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsMDEConhecimento { get; set; }

        public bool IsMDEConfirmacao { get; set; }

        public bool IsNotaSefazTecnospeed { get; set; }

        public int AttemptNotaSefazTecnospeedCount { get; set; }
        public DateTime? LastAttemptSefazTecnospeed { get; set; }

        public DateTime? DateNotaSefazTecnospeed { get; set; }

        public DateTimeOffset DataEmissao { get; set; }
    }
}
