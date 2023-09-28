using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Tabelas
{
    [AutoMap(typeof(TabelaDto))]
    public class CriarOuEditarTabelaModalViewModel : TabelaDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarTabelaModalViewModel(TabelaDto output)
        {
            output.MapTo(this);
        }
    }
}