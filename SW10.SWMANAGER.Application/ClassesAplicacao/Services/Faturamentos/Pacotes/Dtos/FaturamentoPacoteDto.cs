using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos
{
    public class FaturamentoPacoteDto : CamposPadraoCRUDDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public long? FaturamentoContaId { get; set; }
        public FaturamentoContaDto FaturamentoConta { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public long? TerceirizadoId { get; set; }
        public long? TurnoId { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFim { get; set; }
        public float? Quantidade { get; set; } = 1;


        public static FaturamentoPacoteDto Mapear(FaturamentoPacote faturamentoPacote)
        {
            var faturamentoPacoteDto = new FaturamentoPacoteDto()
            {
                Id = faturamentoPacote.Id,
                Codigo = faturamentoPacote.Codigo,
                Descricao = faturamentoPacote.Descricao,
                Inicio = faturamentoPacote.Inicio,
                Final = faturamentoPacote.Final,
                FaturamentoItemId = faturamentoPacote.FaturamentoItemId,
                FaturamentoContaId = faturamentoPacote.FaturamentoContaId,
                Quantidade = faturamentoPacote.Qtde
            };

            if (faturamentoPacote.FaturamentoItem != null)
            {
                faturamentoPacoteDto.FaturamentoItem = FaturamentoItemDto.Mapear(faturamentoPacote.FaturamentoItem);
            }

            if (faturamentoPacote.FaturamentoConta != null)
            {
                faturamentoPacoteDto.FaturamentoConta = FaturamentoContaDto.Mapear(faturamentoPacote.FaturamentoConta);
            }

            return faturamentoPacoteDto;
        }

        public static FaturamentoPacote Mapear(FaturamentoPacoteDto faturamentoPacoteDto)
        {
            var faturamentoPacote = new FaturamentoPacote()
            {
                Id = faturamentoPacoteDto.Id,
                Codigo = faturamentoPacoteDto.Codigo,
                Descricao = faturamentoPacoteDto.Descricao,
                Inicio = faturamentoPacoteDto.Inicio,
                Final = faturamentoPacoteDto.Final,
                FaturamentoItemId = faturamentoPacoteDto.FaturamentoItemId,
                FaturamentoContaId = faturamentoPacoteDto.FaturamentoContaId,
                Qtde = faturamentoPacoteDto.Quantidade
            };

            if (faturamentoPacote.FaturamentoItem != null)
            {
                faturamentoPacote.FaturamentoItem = FaturamentoItemDto.Mapear(faturamentoPacoteDto.FaturamentoItem);
            }

            return faturamentoPacote;
        }

    }
}
