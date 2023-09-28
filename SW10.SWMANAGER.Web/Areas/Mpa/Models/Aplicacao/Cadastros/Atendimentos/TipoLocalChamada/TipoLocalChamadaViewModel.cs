using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.TipoLocalChamada
{
    [AutoMap(typeof(TipoLocalChamadaDto))]
    public class TipoLocalChamadaViewModel : TipoLocalChamadaDto
    {
        public TipoLocalChamadaViewModel(TipoLocalChamadaDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
    }
}