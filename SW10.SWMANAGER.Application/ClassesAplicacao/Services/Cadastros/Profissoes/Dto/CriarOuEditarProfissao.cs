using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto
{

    [AutoMap(typeof(Profissao))]
    public class CriarOuEditarProfissao : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
