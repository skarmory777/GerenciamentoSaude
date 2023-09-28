using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposCentroCusto
{
    [AutoMapFrom(typeof(CriarOuEditarGrupoCentroCusto))]
    public class CriarOuEditarGruposCentroCustoModalViewModel : CriarOuEditarGrupoCentroCusto
    {

        public SelectList TiposGrupoCentroCusto { get; set; }

        public bool IsEditMode


        {
            get { return Id > 0; }
        }
        public CriarOuEditarGruposCentroCustoModalViewModel(CriarOuEditarGrupoCentroCusto output)
        {
            output.MapTo(this);
        }
    }
}