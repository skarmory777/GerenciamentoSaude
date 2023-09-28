using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Equipamentos
{
    [AutoMap(typeof(EquipamentoDto))]
    public class CriarOuEditarEquipamentoModalViewModel : EquipamentoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarEquipamentoModalViewModel(EquipamentoDto output)
        {
            output.MapTo(this);
        }
    }
}