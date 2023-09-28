using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Parentescos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto
{
    [AutoMap(typeof(Parentesco))]
    public class ParentescoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

    }
}
