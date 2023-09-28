using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio
{
    [AutoMapFrom(typeof(TabelaDominioDto))]
    public class CriarOuEditarTabelaDominioModalViewModel : TabelaDominioDto
    {
        //public TabelaDominioDto TabelaDominio { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public SelectList TiposTabela { get; set; }

        public SelectList GruposTipoTabelaDominio { get; set; }

        public ICollection<VersaoTissDto> VersoesTiss { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarTabelaDominioModalViewModel(TabelaDominioDto output)
        {
            output.MapTo(this);
        }
    }
}