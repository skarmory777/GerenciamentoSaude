using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Atestados
{
    [AutoMap(typeof(AtestadoDto))]
    public class CriarOuEditarAtestadoViewModel : AtestadoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public SelectList TiposAtestados { get; set; }

        public SelectList ModelosAtestados { get; set; }

        public CriarOuEditarAtestadoViewModel(AtestadoDto output)
        {
            output.MapTo(this);
        }
    }
}