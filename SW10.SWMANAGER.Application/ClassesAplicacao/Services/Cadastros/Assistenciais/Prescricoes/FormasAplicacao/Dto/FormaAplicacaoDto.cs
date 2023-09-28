using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto
{
    [AutoMap(typeof(FormaAplicacao))]
    public class FormaAplicacaoDto : CamposPadraoCRUDDto, IDescricao
    {

        public static FormaAplicacaoDto Mapear(FormaAplicacao input)
        {
            return MapearBase<FormaAplicacaoDto>(input);
        }

        public static FormaAplicacao Mapear(FormaAplicacaoDto input)
        {

            return MapearBase<FormaAplicacao>(input);
        }

        public static IEnumerable<FormaAplicacaoDto> Mapear(List<FormaAplicacao> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<FormaAplicacao> Mapear(List<FormaAplicacaoDto> input)
        {
            foreach (var item in input)
            {
                yield return Mapear(item);
            }
        }
    }
}
