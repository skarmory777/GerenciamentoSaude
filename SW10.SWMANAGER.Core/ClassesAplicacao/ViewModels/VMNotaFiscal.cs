using System;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    public class VMNotaFiscal
    {
        public long Id { get; set; }

        public string ChaveAcesso { get; set; }

        public long Cnpj { get; set; }

        public DateTime DataEmissao { get; set; }

        public DateTime? DataRecebimento { get; set; }

        public decimal ValorNota { get; set; }

        public string Nome { get; set; }

        public short Nsu { get; set; }

        public long Numero { get; set; }

        public bool IsManifestacaoDestinatario { get; set; }

        public byte CStat { get; set; }

        public string NomeEmpresa { get; set; }
    }
}
