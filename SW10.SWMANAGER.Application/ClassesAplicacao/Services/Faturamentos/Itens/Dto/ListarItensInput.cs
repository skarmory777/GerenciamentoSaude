using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto
{
    public class ListarFaturamentoItensInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public string Tipo { get; set; }

        public string Grupo { get; set; }

        public string SubGrupo { get; set; }

        public long GrupoId { get; set; }
        public long SubGrupoId { get; set; }
        public long TipoId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
