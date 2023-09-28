using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ModelosAtestados
{
    [AutoMap(typeof(ModeloAtestadoDto))]
    public class CriarOuEditarModeloAtestadoViewModel : ModeloAtestadoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarModeloAtestadoViewModel(ModeloAtestadoDto output)
        {
            output.MapTo(this);
        }
    }
}