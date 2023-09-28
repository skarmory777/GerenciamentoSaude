using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ProntuarioEletronico
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    [AutoMap(typeof(ProntuarioEletronicoDto))]
    public class CriarOuEditarProntuarioEletronicoViewModel : ProntuarioEletronicoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public long? FormConfigId { get; set; }

        public FormConfigDto FormConfig { get; set; }

        public long? AtendimentoLeitoId { get; set; }

        public CriarOuEditarProntuarioEletronicoViewModel(ProntuarioEletronicoDto output)
        {
            output.MapTo(this);
        }
    }
}