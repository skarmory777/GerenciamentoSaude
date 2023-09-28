using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;

using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos
{
    public class TISSAppService : SWMANAGERAppServiceBase, ITISSAppService
    {
        private readonly IRepository<FaturamentoEntregaLote, long> _loteRepository;
        private readonly IRepository<FaturamentoEntregaConta, long> _faturamentoEntregaContaRepository;
        private readonly IOperacoesV3_03_03AppService _operacoesV3_03_03AppService;
        private readonly IRepository<RegistroArquivo, long> _registroArquivo;


        public TISSAppService(IRepository<FaturamentoEntregaLote, long> loteRepository
                            , IRepository<FaturamentoEntregaConta, long> faturamentoEntregaContaRepository
                            , IOperacoesV3_03_03AppService operacoesV3_03_03AppService
                            , IRepository<RegistroArquivo, long> registroArquivo)
        {
            _loteRepository = loteRepository;
            _faturamentoEntregaContaRepository = faturamentoEntregaContaRepository;
            _operacoesV3_03_03AppService = operacoesV3_03_03AppService;
            _registroArquivo = registroArquivo;
        }

        public DefaultReturn<string> GerarLoteXML(long loteId)
        {
            var _retornoPadrao = new DefaultReturn<string>();
            // _retornoPadrao.Warnings = new List<ErroDto>();
            // _retornoPadrao.Errors = new List<ErroDto>();

            var entregaConta = _faturamentoEntregaContaRepository.GetAll()
                                      .Where(w => w.EntregaLoteId == loteId)
                                      .Include(i => i.EntregaLote)
                                      .Include(i => i.EntregaLote.Convenio.VersaoTiss)
                                      .FirstOrDefault();

            var entregaContaDto = FaturamentoEntregaContaDto.Mapear(entregaConta);

            string xml = "";

            //  DefaultReturn<string> retorno=null;

            if (entregaConta != null)
            {
                var versao = entregaConta.EntregaLote.Convenio.VersaoTiss;

                switch (versao.Id)
                {
                    case (long)EnumVersaoTISS.V03_03_03:

                        _retornoPadrao = _operacoesV3_03_03AppService.GerarLoteXML(loteId);
                        break;
                }

                // byte[] arquivo = System.Text.Encoding.UTF8.GetBytes(xml);


                if (_retornoPadrao.Errors.Count == 0)
                {


                    xml = FuncoesGlobais.RemoveAccents(_retornoPadrao.ReturnObject);


                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utf8 = Encoding.UTF8;
                    byte[] utfBytes = utf8.GetBytes(xml);
                    byte[] arquivo = Encoding.Convert(utf8, iso, utfBytes);



                    string nomeArquivo = string.Concat(entregaConta.EntregaLote.CodEntregaLote, "_", FuncoesGlobais.CalculateMD5Hash(xml), ".xml");

                    _registroArquivo.Insert(new RegistroArquivo { RegistroTabelaId = (long)EnumArquivoTabela.LoteXML, RegistroId = loteId, Arquivo = arquivo, Descricao = nomeArquivo });

                    entregaConta.EntregaLote.IsLoteGerado = true;
                }
                else
                {
                    // entregaConta.
                }
            }

            return _retornoPadrao;
        }
    }
}

