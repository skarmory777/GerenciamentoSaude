using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Entradas
{
    public class EntradasUnidadeTipoViewModel
    {
        public long EntradaId { get; set; }

        public ICollection<EntradaItemDto> EntradasItem { get; set; }
    }
}