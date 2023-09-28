using Abp.Dependency;
using Abp.Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.Collections.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao
{
    public class DocumentoValidacaoService: SWMANAGERAppServiceBase
    {

        DefaultReturn<DocumentoDto> _retornoPadrao = new DefaultReturn<DocumentoDto>();
        List<LancamentoIndex> lancamentosDto;
        List<DocumentoRateioIndex> rateiosDto;

        public DefaultReturn<DocumentoDto> Validar(DocumentoDto documento)
        {
            lancamentosDto = JsonConvert.DeserializeObject<List<LancamentoIndex>>(documento.LancamentosJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            rateiosDto = JsonConvert.DeserializeObject<List<DocumentoRateioIndex>>(documento.RateioJson);

            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            ValidarTotalDocumento(documento);
            ValidarRateio(documento);

            return _retornoPadrao;
        }

        private void ValidarTotalDocumento(DocumentoDto documento)
        {
            if (lancamentosDto.IsNullOrEmpty())
            {
                _retornoPadrao.Errors.Add(ErroDto.Criar("", "Não foram geradas parcelas."));
                return;
            }
            var totalParcelas = lancamentosDto.Sum(s => s.ValorLancamento);
            var totalJuros = lancamentosDto.Sum(s => s.Juros);
            var valorDocumento = (documento.ValorDocumento ?? 0) + (documento.ValorAcrescimoDecrescimo ?? 0) - (documento.ValorDesconto ?? 0);

            if (totalParcelas == valorDocumento)
            {
                return;
            }
            if (MathHelper.TruncateTwoDigits(totalParcelas) != MathHelper.TruncateTwoDigits(valorDocumento))
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "FIN0001" });
            }
        }

        void ValidarTotalRateio(DocumentoDto documento)
        {
            var totalRateio = rateiosDto.Sum(s => s.Valor); 
            var totalRateioArredondado = decimal.Round((decimal)(totalRateio), 2, System.MidpointRounding.AwayFromZero);
            var valorDoc = (documento.ValorDocumento ?? 0) + (documento.ValorAcrescimoDecrescimo ?? 0);

            var valorDocumento = decimal.Round((decimal)valorDoc, 2, System.MidpointRounding.AwayFromZero);

            if (totalRateioArredondado != valorDocumento)
            {
                if (MathHelper.TruncateTwoDigits(totalRateio) != MathHelper.TruncateTwoDigits(valorDocumento))
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "FIN0002" });
                }
            }
        }

        void ValidarRateio(DocumentoDto documento)
        {
            //ValidarTotalRateio(documento);

            foreach (var item in rateiosDto)
            {
                ValidarContaAdministrativa(item, documento.IsCredito);
                if (!item.CentroCustoId.HasValue || item.CentroCustoId.Value.Equals(0))
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "FIN0004", Parametros = new List<object> { item.CentroCustoDescricao } });
                }
            }

        }

        void ValidarContaAdministrativa(DocumentoRateioIndex documentoRateio, bool isCredito)
        {
            using (var contaAdministrativaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ContaAdministrativa, long>>())
            {
                var contaAdministrativa = contaAdministrativaRepository.Object.GetAll().AsNoTracking()
                                          .Any(w => w.ContaAdministrativaEmpresas.Any(a => a.EmpresaId == documentoRateio.EmpresaId)
                                                  && w.Id == documentoRateio.ContaAdministrativaId
                                                  && w.IsReceita == isCredito
                                                  && w.IsDespesa == !isCredito);

                if (contaAdministrativa == false)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "FIN0003", Parametros = new List<object> { documentoRateio.ContaAdministrativaDescricao } });
                }                
            }
        }
    }
}
