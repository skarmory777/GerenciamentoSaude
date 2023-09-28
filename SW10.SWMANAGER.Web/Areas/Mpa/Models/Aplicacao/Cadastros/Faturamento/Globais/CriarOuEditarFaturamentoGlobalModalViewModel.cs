using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.ItensTabela;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Tabelas
{
    [AutoMapFrom(typeof(FaturamentoGlobalDto))]
    public class CriarOuEditarFaturamentoGlobalModalViewModel : FaturamentoGlobalDto
    {
        public UserEditDto UpdateUser { get; set; }

        public FaturamentoItensTabelaViewModel ItensModel { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoGlobalModalViewModel(FaturamentoGlobalDto output)
        {
            output.MapTo(this);
        }
    }
}