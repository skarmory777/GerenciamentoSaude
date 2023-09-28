using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas
{
    [AutoMap(typeof(ConsultorTabelaCampoDto))]
    public class CriarOuEditarConsultorTabelaCampoModalViewModel : ConsultorTabelaCampoDto
    {
        public SelectList Campos { get; set; }

        public SelectList ConsultorTabelas { get; set; }

        public SelectList ConsultorTiposDadoNF { get; set; }

        public SelectList ConsultorOcorrencias { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarConsultorTabelaCampoModalViewModel(ConsultorTabelaCampoDto output)
        {
            output.MapTo(this);
        }
    }
}