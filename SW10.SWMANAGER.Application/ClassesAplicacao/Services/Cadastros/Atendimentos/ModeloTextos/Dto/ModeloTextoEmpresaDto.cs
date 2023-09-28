using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto
{
    public class ModeloTextoEmpresaDto : CamposPadraoCRUD
    {
        public long? TextoId { get; set; }

        public TextoModeloDto TextoModelo { get; set; }

        public long? EmpresaId { get; set; }

        public Empresa Empresa { get; set; }
    }
}
