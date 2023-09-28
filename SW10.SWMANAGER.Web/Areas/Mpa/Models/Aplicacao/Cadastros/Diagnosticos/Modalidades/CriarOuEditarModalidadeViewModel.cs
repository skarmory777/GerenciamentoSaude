using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.Modalidades
{
    [AutoMap(typeof(ModalidadeDto))]
    public class CriarOuEditarModalidadeViewModel : ModalidadeDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarModalidadeViewModel(ModalidadeDto output)
        {
            output.MapTo(this);
        }
    }
}
