using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto
{
    [AutoMap(typeof(LaudoModeloLaudo))]
    public class ModeloLaudoDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public long LaudoGrupoId { get; set; }
        public LaudoGrupo LaudoGrupo { get; set; } // Deve ser LaudoGrupoDto
        public string Modelo { get; set; }
    }
}
