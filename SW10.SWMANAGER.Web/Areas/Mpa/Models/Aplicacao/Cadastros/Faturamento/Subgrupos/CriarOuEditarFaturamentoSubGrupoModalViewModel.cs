using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.SubGrupos
{
    [AutoMapFrom(typeof(FaturamentoSubGrupoDto))]
    public class CriarOuEditarFaturamentoSubGrupoModalViewModel : FaturamentoSubGrupoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Grupos { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoSubGrupoModalViewModel(FaturamentoSubGrupoDto output)
        {
            output.MapTo(this);
        }
    }
}