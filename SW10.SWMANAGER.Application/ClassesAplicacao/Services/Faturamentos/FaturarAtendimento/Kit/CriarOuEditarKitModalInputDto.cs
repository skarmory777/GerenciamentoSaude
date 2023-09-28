using System;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Kit
{
    public class CriarOuEditarKitModalInputDto : CriarOuEditarContaBaseModalInputDto
    {

        public DateTime Data { get; set; }

        public long? KitId { get; set; }
        public FaturamentoKitDto Kit { get; set; }

        public decimal? ValorItem { get; set; }

    }
}