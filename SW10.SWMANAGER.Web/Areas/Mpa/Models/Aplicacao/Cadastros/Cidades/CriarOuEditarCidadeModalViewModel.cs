using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cidades
{
    [AutoMap(typeof(CidadeDto))]
    public class CriarOuEditarCidadeModalViewModel : CidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        // Modelo antigo
        public SelectList Estados { get; set; }

        // Modelo novo (select2)
        public EstadoDto Estado { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarCidadeModalViewModel(CidadeDto output)
        {
            output.MapTo(this);
        }
    }
}