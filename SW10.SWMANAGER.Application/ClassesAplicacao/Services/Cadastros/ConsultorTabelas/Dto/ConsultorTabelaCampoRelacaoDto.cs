using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas.Dto
{
    [AutoMap(typeof(ConsultorTabelaCampoRelacao))]
    public class ConsultorTabelaCampoRelacaoDto : EntityDto<long>
    {
        public long ConsultorTabelaId { get; set; }

        public long ConsultorTabelaCampoId { get; set; }

        public virtual ConsultorTabela ConsultorTabela { get; set; }

        public virtual ConsultorTabelaCampo ConsultorTabelaCampo { get; set; }
    }
}
