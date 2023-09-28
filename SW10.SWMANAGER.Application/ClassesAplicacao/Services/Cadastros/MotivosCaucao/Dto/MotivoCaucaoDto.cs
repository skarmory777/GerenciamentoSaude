using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCaucao;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto
{
    [AutoMap(typeof(MotivoCaucao))]
    public class MotivoCaucaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
