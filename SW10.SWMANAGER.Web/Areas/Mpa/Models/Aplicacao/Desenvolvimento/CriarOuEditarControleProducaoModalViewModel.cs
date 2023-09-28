using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.ControleProducoes
{
    [AutoMapFrom(typeof(CriarOuEditarControleProducao))]
    public class CriarOuEditarControleProducaoModalViewModel : CriarOuEditarControleProducao
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList TabelasSistema { get; set; }

        public SelectList Usuarios { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarControleProducaoModalViewModel(CriarOuEditarControleProducao output)
        {
            output.MapTo(this);
        }
    }
}