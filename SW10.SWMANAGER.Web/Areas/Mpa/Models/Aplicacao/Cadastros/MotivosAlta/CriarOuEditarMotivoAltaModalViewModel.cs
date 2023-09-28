using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosAlta
{
    [AutoMapFrom(typeof(CriarOuEditarMotivoAlta))]
    public class CriarOuEditarMotivoAltaModalViewModel : CriarOuEditarMotivoAlta
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList TiposAlta { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarMotivoAltaModalViewModel(CriarOuEditarMotivoAlta output)
        {
            output.MapTo(this);
        }
    }
}