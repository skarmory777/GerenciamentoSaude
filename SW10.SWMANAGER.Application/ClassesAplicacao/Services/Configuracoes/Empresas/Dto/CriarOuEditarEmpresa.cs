using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto
{
    [AutoMap(typeof(Empresa))]
    public class CriarOuEditarEmpresa : PessoaJuridicaDto
    {

        public byte[] Logotipo { get; set; }

        public string LogotipoMimeType { get; set; }

        public int CodigoSus { get; set; }

        public int Cnes { get; set; }

        public bool IsAtiva { get; set; }

        public bool IsComprasUnificadas { get; set; }

        //ACERTAR REFERENCIA
        //public long? EstoqueMasterId { get; set; }
        //[ForeignKey("EstoqueMasterId")]
        //public virtual EstoqueMaster EstoqueMaster { get; set; }

        public long? ConvenioId { get; set; }
        public virtual ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public virtual PlanoDto Plano { get; set; }

        public long? EstoqueId { get; set; }
        public virtual EstoqueDto Estoque { get; set; }

        //public long NumeroRegistroAns { get; set; }
        //public long CodigoCredenciadoEmpresa { get; set; }
    }
}
