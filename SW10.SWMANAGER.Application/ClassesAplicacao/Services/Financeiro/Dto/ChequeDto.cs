using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(Cheque))]
    public class ChequeDto : CamposPadraoCRUDDto
    {
        public long TalaoChequeId { get; set; }

        public TalaoChequeDto TalaoCheque { get; set; }

        public long Numero { get; set; }
        public string Nominal { get; set; }
        public DateTime? Data { get; set; }

        public static Cheque Mapear(ChequeDto chequeDto)
        {
            Cheque cheque = new Cheque();

            cheque.Id = chequeDto.Id;
            cheque.Codigo = chequeDto.Codigo;
            cheque.Descricao = chequeDto.Descricao;
            cheque.TalaoChequeId = chequeDto.TalaoChequeId;
            cheque.Numero = chequeDto.Numero;

            return cheque;
        }

    }
}
