using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Planos
{
    [AutoMapFrom(typeof(CriarOuEditarPlano))]
    public class CriarOuEditarPlanoModalViewModel : CriarOuEditarPlano
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Convenios { get; set; }

        public CriarOuEditarPlanoModalViewModel(CriarOuEditarPlano output)
        {
            output.MapTo(this);
        }
    }
}