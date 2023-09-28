using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes.Dto
{
    public class ListarFaturamentoItemAutorizacaoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public string Grupo { get; set; }
        public string SubGrupo { get; set; }
        public long? GrupoId { get; set; }
        public long? SubGrupoId { get; set; }
        public long? ConvenioId { get; set; }

        public void Normalize()
        {
            Sorting = "Id";
        }
    }
}
