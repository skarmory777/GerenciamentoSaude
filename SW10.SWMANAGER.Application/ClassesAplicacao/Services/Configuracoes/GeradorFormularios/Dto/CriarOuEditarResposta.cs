using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    public class CriarOuEditarResposta
    {
        public FormConfig FormConfig { get; set; }

        public long IdDadosResposta { get; set; }

        public string NomeClasse { get; set; }

        public string RegistroClasseId { get; set; }
    }
}
