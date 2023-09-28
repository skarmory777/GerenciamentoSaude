using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitoCaracteristicas
{
    [AutoMapFrom(typeof(CriarOuEditarLeitoCaracteristica))]
    public class CriarOuEditarLeitoCaracteristicaModalViewModel : CriarOuEditarLeitoCaracteristica
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarLeitoCaracteristicaModalViewModel(CriarOuEditarLeitoCaracteristica output)
        {
            output.MapTo(this);
        }
    }
}