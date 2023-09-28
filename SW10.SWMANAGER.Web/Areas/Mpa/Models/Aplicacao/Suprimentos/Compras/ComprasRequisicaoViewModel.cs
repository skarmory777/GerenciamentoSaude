using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Compras
{
    [AutoMap(typeof(CompraRequisicaoDto))]
    public class ComprasRequisicaoViewModel : CompraRequisicaoDto
    {
        public ComprasRequisicaoViewModel(CompraRequisicaoDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList StatusRequisicao { get; set; }

        public SelectList StatusAprovacao { get; set; }

        public string Filtro { get; set; }
    }
}