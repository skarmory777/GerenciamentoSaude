using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto
{
    [AutoMap(typeof(ProdutoUnidade))]
    public class CriarOuEditarUnidade : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }

        public string Fator { get; set; }

    }
}
