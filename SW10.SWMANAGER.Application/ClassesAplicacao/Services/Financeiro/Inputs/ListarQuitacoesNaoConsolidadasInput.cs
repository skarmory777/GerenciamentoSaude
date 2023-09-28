using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs
{
    public class ListarQuitacoesNaoConsolidadasInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long ContaCorrenteId { get; set; }
        public DateTime? DataMovimento { get; set; }

        public void Normalize()
        {
        }
    }
}
