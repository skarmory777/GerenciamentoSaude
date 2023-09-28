using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto
{
    [AutoMap(typeof(Setor))]
    public class SetorDto : CamposPadraoCRUDDto
    {
        public int OrdemSetor { get; set; }


        public static SetorDto Mapear(Setor input)
        {
            var result = MapearBase<SetorDto>(input);
            result.OrdemSetor = input.OrdemSetor;
            return result;
        }

        public static Setor Mapear(SetorDto input)
        {
            var result = MapearBase<Setor>(input);
            result.OrdemSetor = input.OrdemSetor;
            return result;
        }
    }
}
