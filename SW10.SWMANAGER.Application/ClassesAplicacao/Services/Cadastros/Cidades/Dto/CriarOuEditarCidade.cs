using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto
{
    [AutoMap(typeof(Cidade))]
    public class CriarOuEditarCidade : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }

        public bool Capital { get; set; }

        public long EstadoId { get; set; }
    }
}
