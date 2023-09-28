using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens
{

    [AutoMap(typeof(LaudoMovimentoDto))]
    public class LaudoMovimentoViewModel : LaudoMovimentoDto
    {
        public SelectList ListaAmbulatorioInternacao { get; set; }
        public SelectList ListaIonico { get; set; }
        public SelectList ListaAplicacao { get; set; }

        public long? EmpresaId { get; set; }
        public string NomeEmpresa { get; set; }

        public LaudoMovimentoViewModel(LaudoMovimentoDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }


    }
}