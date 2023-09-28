using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Itens
{
    [AutoMapFrom(typeof(FaturamentoItemDto))]
    public class CriarOuEditarItemModalViewModel : FaturamentoItemDto
    {
        public UserEditDto UpdateUser { get; set; }
        public long FiltroSel2 { get; set; }

        // Material tem aqui no model mas ainda nao tem no core e dto
        public long? MaterialId { get; set; }
        public Material Material { get; set; }

        public string Sel2Config { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarItemModalViewModel(FaturamentoItemDto output)
        {
            output.MapTo(this);
        }
    }
}