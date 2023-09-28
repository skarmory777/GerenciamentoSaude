using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.ItensTabela;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Tabelas
{
    [AutoMapFrom(typeof(FaturamentoTabelaDto))]
    public class CriarOuEditarFaturamentoTabelaModalViewModel : FaturamentoTabelaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public FaturamentoItensTabelaViewModel ItensModel { get; set; }

        public string FiltroSel2 { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoTabelaModalViewModel(FaturamentoTabelaDto output)
        {
            output.MapTo(this);
        }
    }
}