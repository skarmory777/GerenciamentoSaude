using Abp.Extensions;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class FaturamentoResumoContaFilterDto : FaturamentoContaItemTableFilterDto
    {
        public List<long> GrupoIds { get; set; }

        public List<long> CentroDeCustoIds { get; set; }

        public List<long> LocalUtilizacaoIds { get; set; }

        public List<long> TerceirizadoIds { get; set; }

        public List<long> TurnoIds { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "FatContaItem.Data desc";
            }
        }
    }

    public class FaturamentoContaItemTableFilterDto : ListarInput
    {
        public long ContaMedicaId { get; set; }

        public bool EnablePaginate { get; set; } = true;

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public long? FatContaKitId { get; set; }

        public long? FatKitId { get; set; }

        public long? FatPacoteId { get; set; }

        public long? FatContaPacoteId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CentroCusto.Descricao, FatGrupo.Descricao, FatContaItem.Data desc";
            }
        }
    }

    public class FaturamentoContaKitFilterDto : FaturamentoContaItemTableFilterDto
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Data desc";
            }
        }
    }

    public class FaturamentoContaPacoteFilterDto : FaturamentoContaItemTableFilterDto
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "FatPacote.Inicio desc";
            }
        }
    }
}