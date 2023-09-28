using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto
{
    public class ModeloTextoGuiaDto : CamposPadraoCRUD
    {
        public long? TextoId { get; set; }

        public TextoModelo TextoModelo { get; set; }

        public long? FatGuiaId { get; set; }

        public FaturamentoGuia FatGuia { get; set; }
    }
}
