using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class SefazTecnoSpeedNotasIndexViewModel : CamposPadraoCRUDDto
    {
        public string Cnpj { get; set; }

        public string ChaveNfe { get; set; }

        public string Emitente { get; set; }

        public string IdentificadorEmitente { get; set; }
        public string IdentificadorTipoEmitente { get; set; }

        public int Modelo { get; set; }

        public int Serie { get; set; }
        public long NumeroNota { get; set; }

        public decimal ValorNota { get; set; }

        public DateTimeOffset DataEmissao { get; set; }
    }

    public class SefazPendentesIndexFilter : ListarInput, IShouldNormalize
    {
        public string Filtro { get; set; }

        public string Fornecedor { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataEmissao Desc";
            }
        }
    }
}
