using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao
{
    public class ContaAdministrativaValidacaoService
    {
        DefaultReturn<ContaAdministrativaDto> _retornoPadrao = new DefaultReturn<ContaAdministrativaDto>();

        List<RateioCentroCustoItemIndex> rateios;
        List<EmpresaIndex> empresas;


        public DefaultReturn<ContaAdministrativaDto> Validar(ContaAdministrativaDto contaAdministrativa)
        {
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            rateios = JsonConvert.DeserializeObject<List<RateioCentroCustoItemIndex>>(contaAdministrativa.CentrosCustos);
            empresas = JsonConvert.DeserializeObject<List<EmpresaIndex>>(contaAdministrativa.Empresas);

            ValidarSomaPecentual();
            ValidarCentroCustoNaoRepetido();
            ValidarEmpresaNaoRepetida();


            return _retornoPadrao;
        }

        void ValidarSomaPecentual()
        {

            var somaPercentual = rateios.Sum(s => s.PercentualRateio);
            if (somaPercentual != 100)
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "CTA0001" });
            }
        }

        void ValidarCentroCustoNaoRepetido()
        {
            // var rateios = JsonConvert.DeserializeObject<List<RateioCentroCustoItemIndex>>(contaAdministrativa.CentrosCustos);

            var centrosCusto = rateios.GroupBy(g => g.CentroCustoId);

            foreach (var item in centrosCusto)
            {
                if (item.Count() > 1)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "CTA0002", Descricao = string.Format("Centro de custo \"{0}\"  repetido.", item.First().CentroCustoDescricao) });
                }
            }


        }


        void ValidarEmpresaNaoRepetida()
        {
            // var rateios = JsonConvert.DeserializeObject<List<RateioCentroCustoItemIndex>>(contaAdministrativa.CentrosCustos);

            var lstEmpresas = empresas.GroupBy(g => g.EmpresaId);

            foreach (var item in lstEmpresas)
            {
                if (item.Count() > 1)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "CTA0003", Descricao = string.Format("Empresa \"{0}\"  repetida.", item.First().EmpresaDescricao) });
                }
            }


        }
    }
}
