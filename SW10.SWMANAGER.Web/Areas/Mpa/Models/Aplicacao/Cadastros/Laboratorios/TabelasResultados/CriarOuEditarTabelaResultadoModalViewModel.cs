using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.TabelasResultados
{
    [AutoMap(typeof(TabelaResultadoDto))]
    public class CriarOuEditarTabelaResultadoModalViewModel : TabelaResultadoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarTabelaResultadoModalViewModel(TabelaResultadoDto output)
        {
            output.MapTo(this);
        }
    }
}