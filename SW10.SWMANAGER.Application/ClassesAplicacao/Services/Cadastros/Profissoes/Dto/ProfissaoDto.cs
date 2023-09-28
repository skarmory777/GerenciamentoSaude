using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto
{
    [AutoMap(typeof(Profissao))]
    public class ProfissaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static ProfissaoDto Mapear(Profissao entity)
        {
            return MapearBase<ProfissaoDto>(entity);
        }

        public static Profissao Mapear(ProfissaoDto dto)
        {
            return MapearBase<Profissao>(dto);
        }
    }
}
