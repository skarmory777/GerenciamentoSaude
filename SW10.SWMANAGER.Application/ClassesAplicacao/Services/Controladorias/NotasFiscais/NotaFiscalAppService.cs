using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using DFe.Utils;
using NFe.Classes;
using Sefaz.Entities;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais;
using SW10.SWMANAGER.ClassesAplicacao.Sefaz;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais
{
    public class NotaFiscalAppService : SWMANAGERAppServiceBase, INotaFiscalAppService
    {
        //private readonly IRepository<NotaFiscal, long> _notaFiscalRepository;
        //private readonly IListarNotasFiscaisExcelExporter _listarNotasFiscaisExcelExporter;
        //private readonly IAppNotifier _appNotifier;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IRepository<Empresa, long> _empresaRepository;

        //private ConfiguracaoApp _configuracoes = new ConfiguracaoApp();
        ////private readonly INotaFiscalManifestacaoDestinatarioAppService _notaFiscalManifestacaoDestinatarioAppService;


        //public NotaFiscalAppService(
        //    IRepository<NotaFiscal, long> notaFiscalRepository,
        //    IAppNotifier appNotifier,
        //    IListarNotasFiscaisExcelExporter listarNotasFiscaisExcelExporter,
        //    IUnitOfWorkManager unitOfWorkManager,
        //    IRepository<Empresa, long> empresaRepository
        //    //INotaFiscalManifestacaoDestinatarioAppService notaFiscalManifestacaoDestinatarioAppService
        //    )
        //{
        //    _notaFiscalRepository = notaFiscalRepository;
        //    _appNotifier = appNotifier;
        //    _listarNotasFiscaisExcelExporter = listarNotasFiscaisExcelExporter;
        //    _unitOfWorkManager = unitOfWorkManager;
        //    _empresaRepository = empresaRepository;
        //    //_notaFiscalManifestacaoDestinatarioAppService = notaFiscalManifestacaoDestinatarioAppService;
        //}

        //[UnitOfWork]
        //public async Task CriarOuEditar(NotaFiscal input)
        //{
        //    try
        //    {
        //        var notaFiscal = input;//.MapTo<NotaFiscal>();
        //        if (input.Id.Equals(0))
        //        {
        //            using (var unitOfWork = _unitOfWorkManager.Begin())
        //            {
        //                await _notaFiscalRepository.InsertAsync(notaFiscal);

        //                unitOfWork.Complete();
        //                _unitOfWorkManager.Current.SaveChanges();
        //                unitOfWork.Dispose();
        //            }
        //        }
        //        else
        //        {
        //            using (var unitOfWork = _unitOfWorkManager.Begin())
        //            {
        //                var _notaFiscal = _notaFiscalRepository.Get(notaFiscal.Id);
        //                _notaFiscal.Ambiente = input.Ambiente;
        //                _notaFiscal.CStat = input.CStat;
        //                _notaFiscal.Motivo = input.Motivo;
        //                _notaFiscal.XmlNota = input.XmlNota;
        //                _notaFiscal.Nsu = input.Nsu;
        //                _notaFiscal.Schema = input.Schema;
        //                _notaFiscal.VersaoAplicacao = input.VersaoAplicacao;
        //                _notaFiscal.VersaoServico = input.VersaoServico;
        //                _notaFiscal.ChaveAcesso = input.ChaveAcesso;
        //                _notaFiscal.Cnpj = input.Cnpj;
        //                _notaFiscal.Cpf = input.Cpf;
        //                _notaFiscal.DataEmissao = input.DataEmissao;
        //                _notaFiscal.DataRecebimento = input.DataRecebimento;
        //                _notaFiscal.DigitoValidacao = input.DigitoValidacao;
        //                _notaFiscal.InscricaoEstadual = input.InscricaoEstadual;
        //                _notaFiscal.Nome = input.Nome;
        //                _notaFiscal.NumeroProtocolo = input.NumeroProtocolo;
        //                _notaFiscal.ProxyDataEmissao = input.ProxyDataEmissao;
        //                _notaFiscal.Situacao = input.Situacao;
        //                _notaFiscal.TipoNota = input.TipoNota;
        //                _notaFiscal.ValorNota = input.ValorNota;
        //                _notaFiscal.VersaoNota = input.VersaoNota;
        //                _notaFiscal.Modelo = input.Modelo;
        //                _notaFiscal.Serie = input.Serie;
        //                _notaFiscal.Numero = input.Numero;
        //                _notaFiscal.IsManifestacaoDestinatario = input.IsManifestacaoDestinatario;

        //                await _notaFiscalRepository.UpdateAsync(_notaFiscal);
        //                unitOfWork.Complete();
        //                unitOfWork.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroSalvar"), ex);
        //    }

        //}

        //public async Task Excluir(CriarOuEditarNotaFiscal input)
        //{
        //    try
        //    {
        //        await _notaFiscalRepository.DeleteAsync(input.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExcluir"), ex);
        //    }

        //}

        //public async Task<PagedResultDto<NotaFiscalDto>> Listar(ListarNotasFiscaisInput input)
        //{
        //    var contarNotasFiscais = 0;
        //    List<NotaFiscal> notasFiscais;
        //    List<NotaFiscalDto> notasFiscaisDtos = new List<NotaFiscalDto>();
        //    try
        //    {
        //        var query = _notaFiscalRepository
        //            .GetAll()
        //            .Where(m => m.DataEmissao >= input.StartDate && m.DataEmissao <= input.EndDate)
        //            .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
        //            m.ChaveAcesso.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Cnpj.ToString().Contains(input.Filtro.ToUpper()) ||
        //            m.Motivo.ToUpper().Contains(input.Filtro.ToUpper()) ||
        //            m.Nsu.ToString().Contains(input.Filtro.ToUpper()) ||
        //            m.Numero.ToString().ToUpper().Contains(input.Filtro.ToUpper())
        //            );

        //        contarNotasFiscais = await query
        //            .CountAsync();

        //        notasFiscais = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //        notasFiscaisDtos = notasFiscais
        //            .MapTo<List<NotaFiscalDto>>();

        //        return new PagedResultDto<NotaFiscalDto>(
        //            contarNotasFiscais,
        //            notasFiscaisDtos
        //            );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<PagedResultDto<VMNotaFiscal>> ListarIndex(ListarNotasFiscaisInput input)
        {
            var contarNotasFiscais = 0;
            List<VMNotaFiscal> notasFiscais;
            try
            {
                using (var notaFiscalRepository = IocManager.Instance.ResolveAsDisposable<IRepository<NotaFiscal, long>>())
                {
                    var query = notaFiscalRepository.Object.GetAll()
                        .Where(m => m.DataEmissao >= input.StartDate && m.DataEmissao <= input.EndDate)
                        .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.ChaveAcesso.Contains(input.Filtro) ||
                        m.Cnpj.ToString().Contains(input.Filtro) ||
                        m.Numero.ToString().Contains(input.Filtro) ||
                        m.Nome.Contains(input.Filtro) ||
                        m.Nsu.ToString().Contains(input.Filtro));

                    contarNotasFiscais = await query.CountAsync();

                    notasFiscais = await query.Select(m => new VMNotaFiscal
                    {
                        Id = m.Id,
                        ChaveAcesso = m.ChaveAcesso,
                        Cnpj = m.Cnpj,
                        DataEmissao = (DateTime)m.DataEmissao,
                        DataRecebimento = m.DataRecebimento.HasValue ? m.DataRecebimento.Value : m.DataEmissao,
                        Nome = m.Nome,
                        Nsu = m.Nsu,
                        Numero = m.Numero,
                        ValorNota = m.ValorNota,
                        IsManifestacaoDestinatario = m.IsManifestacaoDestinatario,
                        CStat = m.CStat,
                        NomeEmpresa = m.Empresa.NomeFantasia
                    })
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    return new PagedResultDto<VMNotaFiscal>(contarNotasFiscais, notasFiscais);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<DefaultReturn<nfeProc>> ObterNFeReceita(string chNFe, long empresaId)
        {
            using (var sefazTecnospeedRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AbpSefazTecnoSpeedConfiguracoes, long>>())
            using (var empresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Empresa, long>>())
            {
                var _retornoPadrao = new DefaultReturn<nfeProc>();

                var empresa = empresaRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.Id == empresaId);

                if (empresa == null || (empresa != null && empresa.Id == 0))
                {
                    throw new UserFriendlyException("EmpresaNaoSelecionada");
                }

                try
                {
                    var cnpjConfig = await sefazTecnospeedRepository.Object.FirstOrDefaultAsync(x => x.Cnpj == empresa.Cnpj).ConfigureAwait(false);

                    using (var sefazConexao = global::Sefaz.SefazHelper.ConexaoSefaz(SefazTecnoSpeedConfiguracoes.MapToSefazConfig(cnpjConfig)))
                    {
                        if (!await sefazConexao.SincronizaNotaAsync(this.GetConnection(), chNFe).ConfigureAwait(false))
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { Descricao = "Oscilação na comuicação com o sefaz! Essa nota pode demorar para ser obtida, entrar em contato com o suporte" });
                            return _retornoPadrao;
                        }
                        _retornoPadrao.ReturnObject = (await sefazConexao.DownloadNfePorChaveAsync(chNFe).ConfigureAwait(false)).NfeProc;
                    }
                }
                catch (Exception e)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { Descricao = e.Message });
                }
                return _retornoPadrao;
            }
        }

        //public DefaultReturn<nfeProc> ObterNFeReceita(string chNFe, long empresaId)
        //{
        //    using (var empresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Empresa, long>>())
        //    {
        //        var _retornoPadrao = new DefaultReturn<nfeProc>
        //        {
        //            Errors = new List<ErroDto>()
        //        };

        //        nfeProc nfeProc = new nfeProc();
        //        var empresa = empresaRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.Id == empresaId);

        //        if (empresa == null || (empresa != null && empresa.Id == 0))
        //        {
        //            throw new UserFriendlyException("EmpresaNaoSelecionada");
        //        }

        //        var arquivoConfiguracao = $@"\config_{empresa.Cnpj}.xml";
        //        try
        //        {
        //            var configuracoes = this.CarregarConfiguracao(arquivoConfiguracao);


        //            if (!chNFe.IsNullOrEmpty())
        //            {
        //                chNFe = chNFe.Replace(" ", "");
        //            }

        //            if (configuracoes != null)
        //            {
        //                RetornoNfeDistDFeInt result = null;
        //                //CarregaDadosCertificado();
        //                using (var service = new ServicosNFe(configuracoes.CfgServico))
        //                {
        //                    result = service.NfeDistDFeInteresse("", empresa.Cnpj, chNFE: chNFe);
        //                }

        //                if (result.Retorno.loteDistDFeInt != null && result.Retorno.loteDistDFeInt.Count() > 0)
        //                {
        //                    string conteudo = Compressao.Unzip(result.Retorno.loteDistDFeInt[0].XmlNfe);

        //                    if (conteudo.StartsWith("<nfeProc"))
        //                    {
        //                        nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(conteudo);
        //                    }
        //                    else if (conteudo.StartsWith("<resNFe"))
        //                    {
        //                        //CarregaDadosCertificado();
        //                        using (var service = new ServicosNFe(configuracoes.CfgServico))
        //                        {
        //                            var manifest = service.RecepcaoEventoManifestacaoDestinatario(1, 1, chNFe, NFeTipoEvento.TeMdCienciaDaEmissao, configuracoes.Emitente.CNPJ);
        //                            var resultManifest = manifest.Retorno;
        //                            var breakPoint = resultManifest.retEvento;
        //                        }

        //                        return this.ObterNFeReceita(chNFe, empresaId);
        //                    }
        //                }
        //                else if (!string.IsNullOrEmpty(result.Retorno.xMotivo))
        //                {
        //                    _retornoPadrao.Errors.Add(new ErroDto { Descricao = result.Retorno.xMotivo });
        //                }

        //                _retornoPadrao.ReturnObject = nfeProc;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            _retornoPadrao.Errors.Add(new ErroDto { Descricao = e.Message });
        //        }
        //        return _retornoPadrao;
        //    }
        //}

        private ConfiguracaoApp CarregarConfiguracao(string arquivoConfiguracao)
        {
            var fullPath = @"c:\nfe" + arquivoConfiguracao; // /configuracao.xml");
            try
            {
                ConfiguracaoApp file = new ConfiguracaoApp();
                if (System.IO.File.Exists(fullPath))
                {
                    file = FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(fullPath);
                    //_configuracoes = file;
                    if (file.CfgServico.TimeOut == 0)
                    {
                        file.CfgServico.TimeOut = 999999; //mínimo
                    }

                    file.CfgServico.DiretorioSchemas = @"c:\nfe\Schemas";
                    file.CfgServico.DiretorioSalvarXml = @"c:\nfe\Xmls";
                    //throw new Exception("acessou pasta " + file.CfgServico.Certificado.Arquivo);


                    file.CfgServico.tpAmb = DFe.Classes.Flags.TipoAmbiente.Producao;

                    return file;
                }
                else
                {
                    throw new Exception("Não existe arquivo de configuração de Nota fiscal para esse CNPJ. Entre em contato  com o suporte para resolver o problema.");
                    return null;
                    // throw new Exception("sem acesso ao caminho " + fullPath);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }



        //public async Task<FileDto> ListarParaExcel(ListarNotasFiscaisInput input)
        //{
        //    try
        //    {
        //        var result = await Listar(input);
        //        var notasFiscais = result.Items;
        //        return _listarNotasFiscaisExcelExporter.ExportToFile(notasFiscais.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        //public async Task<NotaFiscal> Obter(long id)
        //{
        //    try
        //    {
        //        using (var query = await _notaFiscalRepository.FirstOrDefaultAsync(id))
        //        {
        //            return query;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        //public async Task<NotaFiscalDto> ObterIncluding(long id)
        //{
        //    var query = _notaFiscalRepository
        //        .GetAllIncluding(
        //            m => m.AutXml,
        //            m => m.Avulsa,
        //            m => m.Cana,
        //            m => m.Cobranca,
        //            m => m.Compra,
        //            m => m.Destinatario,
        //            m => m.DetalhesNota,
        //            m => m.Emitente,
        //            m => m.Entrega,
        //            m => m.Exporta,
        //            m => m.Ide,
        //            m => m.InformacaoAdicional,
        //            m => m.Pagamento,
        //            m => m.Retirada,
        //            m => m.TotalNota
        //        )
        //        .Where(m => m.Id == id);
        //    var result = await query.FirstOrDefaultAsync();
        //    return result.MapTo<NotaFiscalDto>();
        //}
        //[UnitOfWork]
        //public async Task<NotaFiscal> Obter(string chave)
        //{
        //    try
        //    {
        //        using (var unitOfWork = _unitOfWorkManager.Begin())
        //        {
        //            var result = await _notaFiscalRepository
        //                .GetAllListAsync(m => m.ChaveAcesso == chave);

        //            var notaFiscal = result
        //                .FirstOrDefault();
        //            //.MapTo<CriarOuEditarNotaFiscal>();
        //            unitOfWork.Complete();
        //            unitOfWork.Dispose();
        //            return notaFiscal;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        //public async Task<long> MaxNsu(long empresaId)
        //{

        //    var result = 0;
        //    var max = await _notaFiscalRepository.GetAllListAsync(m => m.EmpresaId == empresaId);
        //    if (max.Count() > 0)
        //    {
        //        result = max.Select(m => m.Nsu).Max();
        //    }
        //    return result;
        //}

        //public NotaFiscalDetalhe ObterDetalhe(long id)
        //{
        //    var query = _notaFiscalRepository
        //        .GetAll()
        //        .SelectMany(m => m.DetalhesNota)
        //        .Include(m => m.imposto)
        //        .Include(m => m.imposto.COFINS)
        //        .Include(m => m.imposto.COFINSST)
        //        .Include(m => m.imposto.ICMS)
        //        .Include(m => m.imposto.ICMSUFDest)
        //        .Include(m => m.imposto.IPI)
        //        .Include(m => m.imposto.ISSQN)
        //        .Include(m => m.imposto.PIS)
        //        .Include(m => m.imposto.PISST)
        //        .Include(m => m.prod)
        //        .Where(m => m.Id == id);

        //    var result = query
        //        .FirstOrDefault();

        //    return result;
        //}

        //public NotaFiscalTotal ObterTotal(long id)
        //{
        //    var query = _notaFiscalRepository
        //        .GetAll()
        //        .Select(m => m.TotalNota)
        //        .Include(m => m.ICMSTot)
        //        .Include(m => m.ISSQNtot)
        //        .Include(m => m.retTrib)
        //        .Where(m => m.Id == id);

        //    var result = query.FirstOrDefault();

        //    return result;
        //}

        //public async Task ManifestacaoDestinatario(NotaFiscalManifestacaoDestinatarioDto input)
        //{
        //    try
        //    {
        //        await _notaFiscalManifestacaoDestinatarioAppService.CriarOuEditar(input);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException("ErroSalvar", ex);
        //    }
        //}

        //public void Dispose()
        //{
        //    GC.SuppressFinalize(this);
        //}

        //public async Task<string> Sincronizar(string[] lines)
        //{
        //    try
        //    {
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            var line = lines[i].Split(',').ToArray();
        //            if (line.Contains("EXCEPTION")) { }
        //            CriarOuEditarNotaFiscal notaFiscal = new CriarOuEditarNotaFiscal();
        //            var _notaFiscal = await Obter(line[1]);
        //            if (_notaFiscal != null)
        //            {
        //                notaFiscal.Id = _notaFiscal.Id;
        //                notaFiscal.LastModificationTime = DateTime.Now;
        //                notaFiscal.LastModifierUserId = AbpSession.UserId;
        //            }
        //            notaFiscal.Handle = Convert.ToInt64(line[0]);
        //            notaFiscal.ChaveAcesso = Convert.ToString(line[1]);
        //            notaFiscal.Situacao = Convert.ToString(line[2]);
        //            notaFiscal.CodigoNotaFiscal = Convert.ToInt64(line[3]);
        //            if (!line[4].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroRecibo = Convert.ToInt64(line[4]);
        //            }
        //            if (!line[5].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroProtocoloEnvio = Convert.ToInt64(line[5]);
        //            }
        //            if (!line[6].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroProtocoloCancelamento = Convert.ToInt64(line[6]);
        //            }
        //            if (!line[7].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroProtocoloInutilizacao = Convert.ToInt64(line[7]);
        //            }
        //            if (!line[8].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroRegistroDpec = Convert.ToInt64(line[8]);
        //            }

        //            notaFiscal.ModoEntrada = Convert.ToString(line[9]);
        //            notaFiscal.ModoSaida = Convert.ToString(line[10]);
        //            notaFiscal.Cnpj = Convert.ToString(line[11]);
        //            notaFiscal.Motivo = Convert.ToString(line[12]);
        //            if (!line[13].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.DataAutorizacao = Convert.ToDateTime(line[13]);
        //            }
        //            if (!line[14].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.DataCadastro = Convert.ToDateTime(line[14]);
        //            }
        //            if (!line[15].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.DataCancelamento = Convert.ToDateTime(line[15]);
        //            }
        //            if (!line[16].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.DataEmissao = Convert.ToDateTime(line[16]);
        //            }
        //            notaFiscal.Impresso = Convert.ToString(line[17]);
        //            notaFiscal.EnviaEmail = Convert.ToString(line[18]);
        //            notaFiscal.Email = Convert.ToString(line[19]);
        //            notaFiscal.DocumentoDestinatario = Convert.ToString(line[20]);
        //            notaFiscal.NomeDestinatario = Convert.ToString(line[21]);
        //            if (!line[22].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.GrupoId = Convert.ToInt64(line[22]);
        //            }
        //            if (!line[23].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.IntegracaoId = Convert.ToInt64(line[23]);
        //            }
        //            if (!line[24].IsNullOrWhiteSpace())
        //            {
        //                notaFiscal.NumeroLote = Convert.ToInt64(line[24]);
        //            }
        //            notaFiscal.Numero = Convert.ToInt64(line[25]);
        //            notaFiscal.DhDpec = Convert.ToString(line[26]);
        //            notaFiscal.NomeGrupo = Convert.ToString(line[27]);
        //            notaFiscal.Eventos = Convert.ToInt32(line[28]);
        //            notaFiscal.Ambiente = Convert.ToInt32(line[29]);
        //            notaFiscal.Impressora = Convert.ToString(line[30]);
        //            notaFiscal.Origem = Convert.ToInt32(line[31]);
        //            notaFiscal.SincronizadoPm = Convert.ToInt32(line[32]);
        //            notaFiscal.CStat = Convert.ToString(line[33]);
        //            notaFiscal.Importado = Convert.ToInt32(line[34]);
        //            notaFiscal.Destinada = Convert.ToInt32(line[35]);
        //            notaFiscal.XmlDestinatario = Convert.ToString(line[36]);

        //            await CriarOuEditar(notaFiscal);
        //        }
        //        //enviar uma notificacao para o ícone de notificação.
        //        await _appNotifier
        //            .SendMessageAsync(
        //                AbpSession.ToUserIdentifier(),
        //                L("SincronizacaoConcluida"),
        //                "Success".ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
        //            );

        //        return "SincronizacaoConcluida";
        //    }
        //    catch (Exception ex)
        //    {
        //        await _appNotifier
        //            .SendMessageAsync(
        //                AbpSession.ToUserIdentifier(),
        //                L("FalhaSincronizacao") + " " + ex.Message.ToString(),
        //                "Error".ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
        //            );
        //        return "FalhaSincronizacao";
        //    }

        //}



    }
}
