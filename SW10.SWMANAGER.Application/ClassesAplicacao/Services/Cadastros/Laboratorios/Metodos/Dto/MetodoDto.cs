using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto
{
    [AutoMap(typeof(Metodo))]
    public class MetodoDto : CamposPadraoCRUDDto
    {
        public static Metodo Mapear(MetodoDto input)
        {
            return MapearBase<Metodo>(input);
            
        }

        public static MetodoDto Mapear(Metodo input)
        {
            return MapearBase<MetodoDto>(input);
            
        }
    }
}
