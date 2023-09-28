using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;

using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03
{
    public class CabecalhoLotesV3_03_03Service
    {
        //  DefaultReturn<cabecalhoTransacao> _retornoPadrao;

        public cabecalhoTransacao CriarCabecalho(FaturamentoEntregaLoteDto faturamentoEntregaLote)
        {
            //_retornoPadrao = new DefaultReturn<cabecalhoTransacao>();
            //_retornoPadrao.Warnings = new List<ErroDto>();
            //_retornoPadrao.Errors = new List<ErroDto>();

            var cabecalho = new cabecalhoTransacao();

            cabecalho.origem = CriarTrancaoOrigem(faturamentoEntregaLote);
            cabecalho.destino = CriarTransacaoDestino(faturamentoEntregaLote);
            cabecalho.identificacaoTransacao = CriarIdentificacaoTrancao(faturamentoEntregaLote);
            cabecalho.Padrao = dm_versao.Item30303;

            //_retornoPadrao.ReturnObject = cabecalho;

            return cabecalho;
        }

        private cabecalhoTransacaoIdentificacaoTransacao CriarIdentificacaoTrancao(FaturamentoEntregaLoteDto faturamentoEntregaLote)
        {
            cabecalhoTransacaoIdentificacaoTransacao cabecalhoTransacaoIdentificacaoTransacao = new cabecalhoTransacaoIdentificacaoTransacao();

            DateTime DateFromXmlSerializer = DateTime.Now;
            DateTime TimeFromXmlSerializer = DateFromXmlSerializer;


            DateTime dataHoraAtual = new DateTime(DateFromXmlSerializer.Year
                                                , DateFromXmlSerializer.Month
                                                , DateFromXmlSerializer.Day
                                                , DateFromXmlSerializer.Hour
                                                , DateFromXmlSerializer.Minute
                                                , DateFromXmlSerializer.Second
                                                , DateTimeKind.Local);





            cabecalhoTransacaoIdentificacaoTransacao.sequencialTransacao = faturamentoEntregaLote.CodEntregaLote;
            cabecalhoTransacaoIdentificacaoTransacao.dataRegistroTransacao = dataHoraAtual.Date;
            cabecalhoTransacaoIdentificacaoTransacao.HoraRegistroTransacao = dataHoraAtual;
            cabecalhoTransacaoIdentificacaoTransacao.tipoTransacao = dm_tipoTransacao.ENVIO_LOTE_GUIAS;

            return cabecalhoTransacaoIdentificacaoTransacao;
        }

        private cabecalhoTransacaoOrigem CriarTrancaoOrigem(FaturamentoEntregaLoteDto faturamentoEntregaLote)
        {
            cabecalhoTransacaoOrigem cabecalhoTransacaoOrigem = new cabecalhoTransacaoOrigem();
            cabecalhoTransacaoOrigemIdentificacaoPrestador cabecalhoTransacaoOrigemIdentificacaoPrestador = new cabecalhoTransacaoOrigemIdentificacaoPrestador();

            cabecalhoTransacaoOrigemIdentificacaoPrestador.ItemElementName = ItemChoiceType.codigoPrestadorNaOperadora;

            //if (!string.IsNullOrEmpty(faturamentoEntregaLote.IdentificacaoPrestadorNaOperadora))
            //{
            cabecalhoTransacaoOrigemIdentificacaoPrestador.Item = faturamentoEntregaLote.IdentificacaoPrestadorNaOperadora;
            //}
            //else
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTC0001", Parametros = new List<object> { faturamentoEntregaLote.Convenio.NomeFantasia } });
            //}

            cabecalhoTransacaoOrigem.Item = cabecalhoTransacaoOrigemIdentificacaoPrestador;

            return cabecalhoTransacaoOrigem;
        }

        private cabecalhoTransacaoDestino CriarTransacaoDestino(FaturamentoEntregaLoteDto faturamentoEntregaLote)
        {
            cabecalhoTransacaoDestino cabecalhoTransacaoDestino = new cabecalhoTransacaoDestino();

            //if (!string.IsNullOrEmpty(faturamentoEntregaLote.Convenio.RegistroANS))
            //{
            cabecalhoTransacaoDestino.Item = faturamentoEntregaLote.Convenio.RegistroANS;
            //}
            //else
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LTC0002", Parametros = new List<object> { faturamentoEntregaLote.Convenio.NomeFantasia } });
            //}

            return cabecalhoTransacaoDestino;
        }
    }
}
