using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio
{
    [AutoMap(typeof(TabelaDominioVersaoTissDto))]
    public class CriarOuEditarTabelaDominioVersaoTissModalViewModel : TabelaDominioVersaoTissDto
    {
        public SelectList VersoesTiss { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarTabelaDominioVersaoTissModalViewModel(TabelaDominioVersaoTissDto output)
        {
            output.MapTo(this);
        }
    }
}