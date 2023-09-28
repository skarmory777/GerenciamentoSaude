using Abp.AutoMapper;
using Abp.Extensions;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto
{
    [AutoMap(typeof(FaturamentoEntregaLote))]
    public class FaturamentoEntregaLoteDto : CamposPadraoCRUDDto
    {
        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public string CodEntregaLote { get; set; }
        public string NumeroProcesso { get; set; }

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataEntrega { get; set; }

        public float ValorFatura { get; set; }
        public float ValorTaxas { get; set; }

        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool Desativado { get; set; }

        public long? UsuarioLoteId { get; set; }
        public User UsuarioLote { get; set; }

        public string IdentificacaoPrestadorNaOperadora { get; set; }

        public bool IsLoteGerado { get; set; }


        public static FaturamentoEntregaLoteDto Mapear(FaturamentoEntregaLote faturamentoEntregaLote)
        {
            var dto = MapearBase<FaturamentoEntregaLoteDto>(faturamentoEntregaLote);
            dto.EmpresaId = faturamentoEntregaLote.EmpresaId;
            dto.ConvenioId = faturamentoEntregaLote.ConvenioId;
            dto.CodEntregaLote = faturamentoEntregaLote.CodEntregaLote;
            dto.NumeroProcesso = faturamentoEntregaLote.NumeroProcesso;
            dto.DataInicial = faturamentoEntregaLote.DataInicial;
            dto.DataFinal = faturamentoEntregaLote.DataFinal;
            dto.DataEntrega = faturamentoEntregaLote.DataEntrega;
            dto.ValorFatura = faturamentoEntregaLote.ValorFatura;
            dto.ValorTaxas = faturamentoEntregaLote.ValorTaxas;
            dto.IsAmbulatorio = faturamentoEntregaLote.IsAmbulatorio;
            dto.IsInternacao = faturamentoEntregaLote.IsInternacao;
            dto.Desativado = faturamentoEntregaLote.Desativado;
            dto.UsuarioLoteId = faturamentoEntregaLote.UsuarioLoteId;
            dto.IsLoteGerado = faturamentoEntregaLote.IsLoteGerado;


            if (faturamentoEntregaLote.Convenio != null)
            {
                dto.Convenio = ConvenioDto.Mapear(faturamentoEntregaLote.Convenio);
            }

            if (faturamentoEntregaLote.Empresa != null)
            {
                dto.Empresa = EmpresaDto.Mapear(faturamentoEntregaLote.Empresa);
            }

            return dto;
        }

    }

    public class FaturamentoEntregaLoteListOutputDto : CamposPadraoCRUDDto
    {
        public long? EmpresaId { get; set; }
        public string EmpresaNomeFantasia { get; set; }

        public long? ConvenioId { get; set; }
        public string ConvenioNomeFantasia { get; set; }

        public string CodEntregaLote { get; set; }
        public string NumeroProcesso { get; set; }

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataEntrega { get; set; }

        public float ValorFatura { get; set; }
        public float ValorTaxas { get; set; }

        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }

        public long TotalContas { get; set; }
        //public bool Desativado { get; set; }

        //public long? UsuarioLoteId { get; set; }
        //public User UsuarioLote { get; set; }

        //public string IdentificacaoPrestadorNaOperadora { get; set; }

        //public bool IsLoteGerado { get; set; }
    }


    public class FaturamentoEntregaLoteInputDto : ListarInput
    {
        public long? ConvenioId { get; set; }

        public long? PacienteId { get; set; }

        public long? TipoInternacao { get; set; }

        public string Periodo { get; set; }


        public DateTime? StartDateEntrega { get; set; }

        public DateTime? EndDateEntrega { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataEntrega DESC";
            }
        }
    }

    public class FaturamentoEntregaLoteListarContasPorLoteInputDto:ListarInput
    {
        public long LoteId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataInicio desc";
            }
        }
    }
}
