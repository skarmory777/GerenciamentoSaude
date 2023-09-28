using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Kits
{
    [AutoMapFrom(typeof(FaturamentoKitDto))]
    public class CriarOuEditarFaturamentoKitModalViewModel : FaturamentoKitDto
    {
        public UserEditDto UpdateUser { get; set; }
        public FaturamentoItemDto Item { get; set; }
        public string Grupo { get; set; }
        public string SubGrupo { get; set; }
        public int Quantidade { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoKitModalViewModel(FaturamentoKitDto output)
        {
            output.MapTo(this);
        }
    }
}