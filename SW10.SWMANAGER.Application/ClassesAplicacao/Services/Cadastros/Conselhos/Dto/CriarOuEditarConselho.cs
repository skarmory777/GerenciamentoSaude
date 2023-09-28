using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos.Dto
{
    [AutoMap(typeof(Conselho))]
    public class CriarOuEditarConselho : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public string Uf { get; set; }

        public string NomeEStado { get; set; }
    }
}
