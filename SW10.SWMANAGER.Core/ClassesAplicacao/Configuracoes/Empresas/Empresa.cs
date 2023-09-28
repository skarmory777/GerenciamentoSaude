using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas
{
    [Table("SisEmpresa")]
    public class Empresa : PessoaJuridica
    {
        public byte[] Logotipo { get; set; }

        public string LogotipoMimeType { get; set; }

        public int CodigoSus { get; set; }

        public int Cnes { get; set; }

        public bool IsAtiva { get; set; }

        public bool IsComprasUnificadas { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }

        public Convenio Convenio { get; set; }

        [ForeignKey("Plano"), Column("SisPlanoId")]
        public long? PlanoId { get; set; }

        public Plano Plano { get; set; }

        [ForeignKey("Estoque"), Column("EstoqueId")]
        public long? EstoqueId { get; set; }

        public Estoque Estoque { get; set; }
    }
}
