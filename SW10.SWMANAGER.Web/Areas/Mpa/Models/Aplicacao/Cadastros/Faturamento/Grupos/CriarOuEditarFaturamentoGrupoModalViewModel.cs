using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Grupos
{
    [AutoMapFrom(typeof(FaturamentoGrupoDto))]
    public class CriarOuEditarFaturamentoGrupoModalViewModel : FaturamentoGrupoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList SubGrupos { get; set; }

        public SelectList TiposGrupo { get; set; }

        public SelectList Sexos { get; set; }

        public long? Sel2Convenio { get; set; }

        public bool ConvenioIsOutraDespesa { get; set; }

        public string ConvenioCodigo { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoGrupoModalViewModel(FaturamentoGrupoDto output)
        {
            output.MapTo(this);
        }
    }
}