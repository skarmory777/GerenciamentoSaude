using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto
{

    [AutoMap(typeof(Plano))]
    public class CriarOuEditarPlano : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public virtual ConvenioDto Convenio { get; set; }
        public long ConvenioId { get; set; }

        public bool IsDespesasAcompanhante { get; set; }

        public bool IsValidadeCarteiraIndeterminada { get; set; }

        //CONFIRMAR RELACIONAMENTO 
        //public long? TipoAcamodacaoId { get; set; }
        //[ForeignKey("TipoAcamodacaoId")]
        //public virtual CriarOuEditarTipoAcamodacao TipoAcamodacao { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsPlanoEmpresa { get; set; }

    }
}
