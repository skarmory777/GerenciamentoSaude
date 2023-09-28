using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Guias
{
    [AutoMapFrom(typeof(CriarOuEditarGuia))]
    public class CriarOuEditarGuiaModalViewModel : CriarOuEditarGuia
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Propriedades { get; set; }

        public SelectList Guias { get; set; }

        public int Contador { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarGuiaModalViewModel(CriarOuEditarGuia output)
        {
            output.MapTo(this);
        }
    }
}