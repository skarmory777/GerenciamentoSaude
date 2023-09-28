using Abp.Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao
{


    public class QuitacaoValidacaoService
    {
        private readonly IRepository<LancamentoQuitacao, long> _lancamentoQuitacaoRepository;
        private readonly IRepository<MeioPagamento, long> _meioPagamentoRepository;
        private readonly IRepository<Cheque, long> _chequeRepository;


        public QuitacaoValidacaoService(IRepository<LancamentoQuitacao, long> lancamentoQuitacaoRepository
                                      , IRepository<MeioPagamento, long> meioPagamentoRepository
                                      , IRepository<Cheque, long> chequeRepository)
        {
            _lancamentoQuitacaoRepository = lancamentoQuitacaoRepository;
            _meioPagamentoRepository = meioPagamentoRepository;
            _chequeRepository = chequeRepository;
        }

        DefaultReturn<QuitacaoDto> _retornoPadrao = new DefaultReturn<QuitacaoDto>();

        List<QuitacaoIndex> lancamentosQuitacoes;

        public DefaultReturn<QuitacaoDto> Validar(QuitacaoDto quitacaoDto)
        {
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            lancamentosQuitacoes = JsonConvert.DeserializeObject<List<QuitacaoIndex>>(quitacaoDto.LancamentosJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            ValidarChequeNaoUtilizado(quitacaoDto);

            foreach (var item in lancamentosQuitacoes)
            {
                ValidarValorQuitacaoObrigatorio(item);
                ValidarValorRestante(item);
            }

            return _retornoPadrao;
        }


        void ValidarValorQuitacaoObrigatorio(QuitacaoIndex lancamentoQuitacao)
        {
            if (lancamentoQuitacao.ValorQuitacao == null || lancamentoQuitacao.ValorQuitacao == 0)
            {
                _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "QTC0001", Parametros = new List<object> { string.Format("{0:dd/MM/yyyy}", lancamentoQuitacao.DataVencimento) } });
            }
        }

        void ValidarValorRestante(QuitacaoIndex lancamentoQuitacao)
        {
            var quitacoes = _lancamentoQuitacaoRepository.GetAll()
                                                            .Where(w => w.LancamentoId == lancamentoQuitacao.LancamentoId)
                                                            .ToList();

            if (quitacoes.Count > 0)
            {
                var totalQuitado = quitacoes.Sum(s => s.ValorEfetivo);

                if ((lancamentoQuitacao.ValorLancamento - totalQuitado) < lancamentoQuitacao.ValorEfetivo)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "QTC0002" });
                }
            }
        }

        void ValidarChequeNaoUtilizado(QuitacaoDto quitacao)
        {
            if (!quitacao.IsCredito)
            {
                var meioPagamento = _meioPagamentoRepository.GetAll()
                                                            .Where(w => w.Id == quitacao.MeioPagamentoId)
                                                            .FirstOrDefault();

                if (meioPagamento != null && meioPagamento.TipoMeioPagamentoId == (long)EnumTipoMeioPagamento.Cheque)
                {
                    if (quitacao.ChequeId == null)
                    {
                        _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "QTC0005" });
                    }
                    else
                    {
                        var cheque = _chequeRepository.GetAll()
                                                      .Where(w => w.Id == quitacao.ChequeId)
                                                     .FirstOrDefault();

                        if (cheque == null)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "QTC0003" });
                        }
                        else if (cheque.Data != null)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "QTC0004" });
                        }
                    }
                }
            }
        }
    }
}
