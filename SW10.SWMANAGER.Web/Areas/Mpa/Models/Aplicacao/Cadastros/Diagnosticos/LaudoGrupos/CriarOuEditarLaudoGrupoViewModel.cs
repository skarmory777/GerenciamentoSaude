using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.Laudos
{
    [AutoMap(typeof(LaudoGrupoDto))]
    public class CriarOuEditarLaudoGrupoViewModel : LaudoGrupoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarLaudoGrupoViewModel(LaudoGrupoDto output)
        {
            output.MapTo(this);
        }
    }
}