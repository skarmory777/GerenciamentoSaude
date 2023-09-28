using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cep
{
    [AutoMapFrom(typeof(CriarOuEditarCep))]
    public class CriarOuEditarCepModalViewModel : CriarOuEditarCep
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Paises { get; set; }

        public SelectList Estados { get; set; }
        public SelectList Cidades { get; set; }

        public SelectList TiposLogradouro { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarCepModalViewModel(CriarOuEditarCep output)
        {
            output.MapTo(this);
        }
    }
}