using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto
{

    [AutoMap(typeof(Origem))]
    public class CriarOuEditarOrigem : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public bool IsAtivo { get; set; }
    }
}
