using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais
{
    [Table("NotaFiscal")]
    public class NotaFiscal : CamposPadraoCRUD
    {
        public NotaFiscal()
        {
            //DetalhesNota = new List<NotaFiscalDetalhe>();
        }
        [MaxLength(500)]
        [Index("Idx_ChaveAcesso")]
        public string ChaveAcesso { get; set; }

        [Index("Idx_Cnpj")]
        public long Cnpj { get; set; }

        public long Cpf { get; set; }

        public byte Situacao { get; set; }

        [Index("Idx_DataEmissao")]
        public DateTime? DataEmissao { get; set; }
        [Index("Idx_DataRecebimento")]
        public DateTime? DataRecebimento { get; set; }

        public string DigitoValidacao { get; set; }

        public long InscricaoEstadual { get; set; }

        public long NumeroProtocolo { get; set; }

        public string ProxyDataEmissao { get; set; }

        public byte TipoNota { get; set; }

        public decimal VersaoNota { get; set; }

        public decimal ValorNota { get; set; }

        public string Nome { get; set; }

        public byte CStat { get; set; }

        public byte Ambiente { get; set; }

        public string VersaoAplicacao { get; set; }

        public decimal VersaoServico { get; set; }

        public string Motivo { get; set; }

        public short Nsu { get; set; }

        public string Schema { get; set; }

        public string XmlNota { get; set; }

        public long Numero { get; set; }

        public long Modelo { get; set; }

        public long Serie { get; set; }

        public bool IsManifestacaoDestinatario { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public long EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

    }
}
