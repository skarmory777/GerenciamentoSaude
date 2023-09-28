using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    public class ListarProdutosInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public string GrupoId { get; set; }
        public string GrupoClasseId { get; set; }
        public string GrupoSubClasseId { get; set; }
        public string DCBId { get; set; }
        public string FiltroPrincipais { get; set; }
        public string FiltroStatus { get; set; }

        public void Normalize()
        {
            Sorting = "Descricao";
        }
    }
}
