using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(RelacaoGuiaCampo))]
    public class CriarOuEditarRelacaoGuiaCampo : CamposPadraoCRUDDto
    {
        public long GuiaId { get; set; }

        public long GuiaCampoId { get; set; }

        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }
    }
}