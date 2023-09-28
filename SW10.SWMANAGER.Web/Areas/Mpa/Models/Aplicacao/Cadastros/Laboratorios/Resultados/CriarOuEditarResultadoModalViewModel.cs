using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Resultados
{
    [AutoMap(typeof(ResultadoDto))]
    public class CriarOuEditarResultadoModalViewModel : ResultadoDto
    {
        public SelectList ListaAmbulatorioInternacao { get; set; }
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarResultadoModalViewModel(ResultadoDto output)
        {
            output.MapTo(this);
        }

        public bool IsSolicitacao { get; set; }
    }
}