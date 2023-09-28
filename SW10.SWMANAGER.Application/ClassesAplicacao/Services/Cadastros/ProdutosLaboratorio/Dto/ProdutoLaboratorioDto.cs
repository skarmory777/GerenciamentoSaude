using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto
{
    [AutoMap(typeof(EstoqueLaboratorio))]
    public class ProdutoLaboratorioDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public long FornecedorId { get; set; }
        public long BrasLaboratorioId { get; set; }

        public static ProdutoLaboratorioDto Mapear(EstoqueLaboratorio estoqueLaboratorio)
        {
            if (estoqueLaboratorio == null)
            {
                return null;
            }

            var produtoLaboratorioDto = new ProdutoLaboratorioDto
            {
                Id = estoqueLaboratorio.Id,
                Codigo = estoqueLaboratorio.Codigo,
                Descricao = estoqueLaboratorio.Descricao,
                BrasLaboratorioId = estoqueLaboratorio.BrasLaboratorioId ?? 0
            };

            return produtoLaboratorioDto;
        }
    }
}
