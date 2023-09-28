using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs
{
    public class ListarRequisicoesCompraInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public string OrdenarPor { get; set; }
        public string EmpresaId { get; set; }
        public string EstoqueId { get; set; }
        public string UnidadeOrganizacionalId { get; set; }
        public string AprovacaoStatusId { get; set; }
        public string MotivoPedidoId { get; set; }
        public string Codigo { get; set; }
        public string Id { get; set; }
        public string StatusRequisicao { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsUrgente { get; set; }
        public string StatusAprovacao { get; set; }

        public void Normalize()
        {
            //Sorting = "Descricao";
            Sorting = OrdenarPor;
        }

    }
}
