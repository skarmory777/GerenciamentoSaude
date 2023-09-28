using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto
{
    public class ListarFaturamentoConfigConvenioGlobaisInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public bool Ativos { get; set; }

        public bool PlanoGlobal { get; set; }
        public bool PlanoEspecifico { get; set; }
        public bool PlanoTodos { get; set; }

        public bool Grupo { get; set; }
        public bool GrupoSubGrupo { get; set; }
        public bool GrupoItem { get; set; }
        public bool GrupoTodos { get; set; }



        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ConvenioId";
            }
        }
    }
}
