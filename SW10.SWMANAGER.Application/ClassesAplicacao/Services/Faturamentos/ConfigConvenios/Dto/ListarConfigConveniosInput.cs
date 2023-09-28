using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto
{
    public class ListarFaturamentoConfigConveniosInput : PagedAndSortedInputDto, IShouldNormalize
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

        public long? ConvenioId { get; set; }
        public long? EmpresaId { get; set; }
        public long? PlanoId { get; set; }
        public long? GrupoId { get; set; }
        public long? SubGrupoId { get; set; }
        public long? ItemId { get; set; }



        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ConvenioId";
            }
        }
    }
}
