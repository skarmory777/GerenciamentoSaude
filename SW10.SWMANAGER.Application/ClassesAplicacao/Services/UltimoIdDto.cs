using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(UltimoId))]
    public class UltimoIdDto : CamposPadraoCRUDDto
    {
        public string NomeTabela { get; set; }


        public string Codigo { get; set; }
    }
}
