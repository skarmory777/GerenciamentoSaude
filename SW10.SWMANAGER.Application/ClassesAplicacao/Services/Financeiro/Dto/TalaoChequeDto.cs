using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(TalaoCheque))]
    public class TalaoChequeDto : CamposPadraoCRUDDto
    {
        public long ContaCorrenteId { get; set; }
        public ContaCorrenteDto ContaCorrente { get; set; }
        public DateTime DataAbertura { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }

        public static TalaoChequeDto Mapear(TalaoCheque talaoCheque)
        {
            TalaoChequeDto talaoChequeDto = new TalaoChequeDto();

            talaoChequeDto.Id = talaoCheque.Id;
            talaoChequeDto.Codigo = talaoCheque.Codigo;
            talaoChequeDto.Descricao = talaoCheque.Descricao;
            talaoChequeDto.ContaCorrenteId = talaoCheque.ContaCorrenteId;
            if (talaoCheque.ContaCorrente != null)
                talaoChequeDto.ContaCorrente = ContaCorrenteDto.Mapear(talaoCheque.ContaCorrente);
            talaoChequeDto.DataAbertura = talaoCheque.DataAbertura;
            talaoChequeDto.NumeroInicial = talaoCheque.NumeroInicial;
            talaoChequeDto.NumeroFinal = talaoCheque.NumeroFinal;

            return talaoChequeDto;
        }

        public static TalaoCheque Mapear(TalaoChequeDto talaoChequeDto)
        {
            TalaoCheque talaoCheque = new TalaoCheque();

            talaoCheque.Id = talaoChequeDto.Id;
            talaoCheque.Codigo = talaoChequeDto.Codigo;
            talaoCheque.Descricao = talaoChequeDto.Descricao;
            talaoCheque.ContaCorrenteId = talaoChequeDto.ContaCorrenteId;
            talaoCheque.DataAbertura = talaoChequeDto.DataAbertura;
            talaoCheque.NumeroInicial = talaoChequeDto.NumeroInicial;
            talaoCheque.NumeroFinal = talaoChequeDto.NumeroFinal;

            return talaoCheque;
        }
    }
}
