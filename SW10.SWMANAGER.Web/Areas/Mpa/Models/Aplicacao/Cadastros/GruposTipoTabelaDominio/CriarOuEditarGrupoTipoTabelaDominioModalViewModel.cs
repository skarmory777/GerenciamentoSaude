using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposTipoTabelaDominio
{
    [AutoMapFrom(typeof(CriarOuEditarGrupoTipoTabelaDominio))]
    public class CriarOuEditarGrupoTipoTabelaDominioModalViewModel : CriarOuEditarGrupoTipoTabelaDominio
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList TiposTabela { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarGrupoTipoTabelaDominioModalViewModel(CriarOuEditarGrupoTipoTabelaDominio output)
        {
            output.MapTo(this);
        }
    }
}