using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    [AutoMap(typeof(DivisaoDto))]
    public class CriarOuEditarDivisaoViewModel : DivisaoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        //public ICollection<TipoRespostaDto> TiposRespostasDisponiveis { get; set; }

        public CriarOuEditarDivisaoViewModel(DivisaoDto output)
        {
            output.MapTo(this);
        }
    }
}