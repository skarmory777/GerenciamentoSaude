using Dapper;
using Dapper.Contrib.Extensions;
using FastMember;
using Newtonsoft.Json;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;
using RestSharp;
using RestSharp.Authenticators;
using Sefaz.Dto;
using Sefaz.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using nfeProc = NFe.Classes.nfeProc;

namespace Sefaz
{
    /// <summary>
    /// Faz toda a camada de integração com o tecnospeed
    /// </summary>
    public partial class SefazConnection : IDisposable
    {
       
        /// <summary>
        /// CNPJ cadastrado no sistema e responsavel pelo arquivo de configuração
        /// </summary>
        private readonly string Cnpj;

        protected string StringConnection { get; set; }

        /// <summary>
        /// Configuracao no tecnospeed
        /// </summary>
        private SefazConfig Config;

        /// <summary>
        /// Tecnospeed homolog url
        /// </summary>
        private const string HomologUrl = "https://managersaashom.tecnospeed.com.br:7071/ManagerAPIWeb";

        /// <summary>
        /// Tecnospeed producao url
        /// </summary>
        private const string ProducaoUrl = "https://managersaas.tecnospeed.com.br:8081/ManagerAPIWeb";

        /// <summary>
        /// Faz toda a camada de integração com o tecnospeed
        /// </summary>
        /// <param name="cnpj"> CNPJ cadastrado no sistema e responsavel pelo arquivo de configuração</param>
        public SefazConnection(string cnpj)
        {
            this.Cnpj = cnpj;
            this.Config = CarregaConfiguracao(this.Cnpj);
            SefazLogHelper.Init(cnpj);
        }

        public SefazConnection(SefazConfig config)
        {
            this.Config = config;
            this.Cnpj = this.Config.Cnpj;
            SefazLogHelper.Init(config.Cnpj);
        }


        public async Task<bool> SincronizaNotasCronAsync(string stringConnection)
        {
            return await this.SincronizaNotasAsync(stringConnection).ConfigureAwait(false);
        }

        public async Task<bool> SincronizaNotaAsync(string stringConnection, string chaveNFE)
        {
            return await this.SincronizaNotasAsync(stringConnection, chaveNFE).ConfigureAwait(false);
        }

        

        private async Task<List<NotasDataEmissaoEChaveDto>> BuscaTodasNotas()
        {
            var notas = new List<NotasDataEmissaoEChaveDto>();
            var limite = 100;
            var dto = new BuscaNotasDto
            {
                Limite = limite.ToString(),
                Origem = "2",
                Campos = "dtemissao,chave",
                Ordem = "dtemissao DESC"
            };

            await SincronizaAsync().ConfigureAwait(false);
            var result = await BuscaNotasAsync<NotasDataEmissaoEChaveDto>(dto).ConfigureAwait(false);
            notas.AddRange(result);

            if (result.Count() == limite)
            {
                do
                {
                    var minDataEmissao = notas.Min(x => x.DataEmissao);
                    dto.Filtro = $"dtemissao < {minDataEmissao:yyyy/MM/dd HH:mm:ss}";
                    result = await BuscaNotasAsync<NotasDataEmissaoEChaveDto>(dto).ConfigureAwait(false);

                    if (result != null)
                    {
                        notas.AddRange(result);
                    }
                } while (result.Count() == limite);
            }

            return notas;
        }

        private async Task<List<NotasDataEmissaoEChaveDto>> BuscaNota(string chaveNFE)
        {
            var limite = 100;
            var dto = new BuscaNotasDto
            {
                Limite = limite.ToString(),
                Origem = "2",
                Campos = "dtemissao,chave",
                Ordem = "dtemissao DESC"
            };

            dto.Filtro = $"chave={chaveNFE.Replace(" ","")}";

            await SincronizaAsync().ConfigureAwait(false);
            return await BuscaNotasAsync<NotasDataEmissaoEChaveDto>(dto).ConfigureAwait(false);
        }

        /// <summary>
        /// Carrega configuração baseada no path o Identificador do arquivo é o CNPJ do sistema.
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static SefazConfig CarregaConfiguracao(string cnpj)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var sefazPath = ConfigurationManager.AppSettings["sefazPath"].ToString();

            var configPath = Path.Combine(baseDir, sefazPath, "Configs", $"{cnpj}.json");

            if (File.Exists(configPath))
            {
                var file = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<SefazConfig>(file);
            }
            else
            {
                throw new SefazException("Arquivo de configuração não existente");
            }
        }

        public async Task<bool> SincronizaSefazAsync(string chNFE)
        {
            var client = CriaCliente(this.Config);
            var request = CriaRequisicao("/nfe/envia", Method.POST, this.Config);
            var arquivoBuilder = new StringBuilder();
            arquivoBuilder.AppendLine("DOCUMENTO=DNF");
            arquivoBuilder.AppendLine($"CHAVENOTA={chNFE}");
            var arquivo = arquivoBuilder.ToString();

            request.AddParameter("arquivo", arquivo);

            var result = await client.ExecuteAsync(request).ConfigureAwait(false);
            if (!SefazHelper.CheckIfHasError(result.Content))
            {
                return SefazHelper.CaseInsensitiveContains(result.Content, "Autorizado o uso da NF-e")
                    && SefazHelper.CaseInsensitiveContains(result.Content, "Importação disparada na pasta importa");
            }

            var exception = ExceptionHandler(result.Content, this.Config);
            if (exception != null)
            {
                throw exception;
            }
            SefazLogHelper.Info(this.Config.Cnpj, result.Content);
            return false;
        }

        public async Task<IEnumerable<string[]>> SincronizaAsync()
        {
            var client = CriaCliente(this.Config);
            var request = CriaRequisicao("/nfe/envia", Method.POST, this.Config);
            var arquivo = "DOCUMENTO=DFE";

            request.AddParameter("arquivo", arquivo);

            var result = await client.ExecuteAsync(request).ConfigureAwait(false);
            if (!SefazHelper.CheckIfHasError(result.Content))
            {
                return result.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList().Select(x => x.Split(this.Config.Delimitador.ToCharArray(), StringSplitOptions.None)).ToList();
            }

            var exception = ExceptionHandler(result.Content, this.Config);
            if (exception != null)
            {
                throw exception;
            }
            SefazLogHelper.Info(this.Config.Cnpj, result.Content);
            return null;
        }


        public IEnumerable<string[]> Sincroniza()
        {
            return this.SincronizaAsync().GetAwaiter().GetResult();
        }

        public bool SincronizaSefaz(string chNFE)
        {
            return this.SincronizaSefazAsync(chNFE).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<string[]>> BuscaNotasAsync(BuscaNotasDto dto)
        {
            var client = CriaCliente(this.Config);
            var request = CriaRequisicao("/nfe/consulta", Method.GET, this.Config);
            request.AddParameter("campos", dto.Campos);
            request.AddParameter("filtro", dto.Filtro);
            request.AddParameter("Origem", dto.Origem);
            request.AddParameter("Ordem", dto.Ordem);
            request.AddParameter("Limite", dto.Limite);

            var result = await client.ExecuteAsync(request).ConfigureAwait(false);


            if (!SefazHelper.CheckIfHasError(result.Content))
            {
                return result.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList().Select(x => x.Split(this.Config.Delimitador.ToCharArray(), StringSplitOptions.None)).ToList();
            }


            var exception = ExceptionHandler(result.Content, this.Config);
            if (exception != null)
            {
                throw exception;
            }
            SefazLogHelper.Info(this.Config.Cnpj, result.Content);
            return null;
        }

        public IEnumerable<string[]> BuscaNotas(BuscaNotasDto dto)
        {
            return this.BuscaNotasAsync(dto).GetAwaiter().GetResult();

        }

        public async Task<List<TResultType>> BuscaNotasAsync<TResultType>(BuscaNotasDto dto) where TResultType : IOutputBuscaNotasDto
        {
            var result = await this.BuscaNotasAsync(dto).ConfigureAwait(false);
            var accessor = TypeAccessor.Create(typeof(TResultType));
            var item = (TResultType)accessor.CreateNew();
            return result.Select(x => (TResultType)item.MapArgs(x)).Where(x=> x != null).ToList();

        }

        public List<TResultType> BuscaNotas<TResultType>(BuscaNotasDto dto) where TResultType : IOutputBuscaNotasDto
        {
            var result = this.BuscaNotasAsync(dto).GetAwaiter().GetResult();
            var accessor = TypeAccessor.Create(typeof(TResultType));
            var item = (TResultType)accessor.CreateNew();
            return result.Select(x => (TResultType)item.MapArgs(x)).Where(x => x != null).ToList();

        }

        /// <summary>
        /// Retorna XML a partir de uma chave NFE com base no CNPJ
        /// </summary>
        /// <param name="cnpj"> CNPJ do destinatario ou emitente cadastrado no sistema</param>
        /// <param name="chNFE">Chave NFE </param>
        /// <returns> Retorna XML da Nota </returns>
        public async Task<XmlDocument> DownloadXmlPorChaveAsync(string cnpj, string chNFE)
        {
            if (this.Config == null)
            {
                this.Config = CarregaConfiguracao(cnpj);
            }
            return await this.DownloadXmlPorChaveAsync(chNFE).ConfigureAwait(false);
        }


        public async Task<nfeProc> DownloadNfePorChaveManisfestacaoAsync(string chNFE)
        {
            var xmlRetorno = await this.DownloadXmlPorChaveAsync(chNFE).ConfigureAwait(false);
            if (SefazHelper.CheckNotaCompleta(xmlRetorno))
            {
                return DFe.Utils.FuncoesXml.XmlStringParaClasse<nfeProc>(xmlRetorno.InnerXml);
            }
            else if (SefazHelper.CheckNotaResumida(xmlRetorno))
            {
                //await this.EnviarManifestacaoDoDestinatario(chNFE).ConfigureAwait(false);
                return await this.DownloadNfePorChaveManisfestacaoAsync(chNFE).ConfigureAwait(false);
            }
            return null;
        }

        public async Task<RetornoNFeDto> DownloadNfePorChaveAsync(string chNFE)
        {
            var retorno = new RetornoNFeDto();
            var xmlRetorno = await this.DownloadXmlPorChaveAsync(chNFE).ConfigureAwait(false);
            if (xmlRetorno == null)
            {
                return null;
            }

            if (SefazHelper.CheckNotaCompleta(xmlRetorno))
            {
                retorno.NfeProc = DFe.Utils.FuncoesXml.XmlStringParaClasse<nfeProc>(xmlRetorno.InnerXml);
            }
            else if (SefazHelper.CheckNotaResumida(xmlRetorno))
            {
                retorno.ResNFe = DFe.Utils.FuncoesXml.XmlStringParaClasse<resNFe>(xmlRetorno.InnerXml);
            }
            return retorno;
        }

        /// <summary>
        /// Retorna XML a partir de uma chave NFE
        /// </summary>
        /// <param name="chNFE">Chave NFE</param>
        /// <returns>Retorna XML da Nota</returns>
        public async Task<XmlDocument> DownloadXmlPorChaveAsync(string chNFE)
        {
            var result = await this.DownloadStringPorChaveAsync(chNFE).ConfigureAwait(false);
            if (SefazHelper.CheckIfIsNullOrEmpty(result))
            {
                return null;
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);

            return xmlDoc;
        }

        /// <summary>
        /// Retorna string a partir de uma chave NFE
        /// </summary>
        /// <param name="chNFE">Chave NFE</param>
        /// <returns>String Da Nota</returns>
        public async Task<string> DownloadStringPorChaveAsync(string chNFE)
        {
            return await DownloadPorChaveAsync(chNFE).ConfigureAwait(false);
        }

        private async Task<string> DownloadPorChaveAsync(string chNFE, int tentativa = 0)
        {
            if (this.Config == null)
            {
                throw new SefazException("Configuração inexistente");
            }

            var client = CriaCliente(this.Config);
            var request = CriaRequisicao("/nfe/xml", Method.GET, this.Config);

            request.AddParameter("chavenota", SefazHelper.LimpaChNFE(chNFE));

            //await this.SincronizaAsync(chNFE).ConfigureAwait(false);
            var result = await client.ExecuteAsync(request).ConfigureAwait(false);
            if (SefazHelper.CheckIfIsXml(result.Content))
            {
                return result.Content;
            }

            var exception = ExceptionHandler(result.Content, this.Config);

            if (exception is SefazNenhumDocumentoEncontradoException && tentativa <= 2)
            {
                await this.EnviarManifestacaoDoDestinatario(chNFE).ConfigureAwait(false);
                return await DownloadPorChaveAsync(chNFE, tentativa + 1).ConfigureAwait(false);
            }
            else if (exception is SefazNenhumDocumentoEncontradoException && tentativa >= 3)
            {
                return null;
            }
            if (exception != null)
            {
                throw exception;
            }
            return null;
        }

        public async Task<string> EnviarManifestacaoDoDestinatario(string chNFE, long tipoEventoManifestacao = 2, string justificativa = "Tenho conhecimento desta operação")
        {
            var client = CriaCliente(this.Config);
            var request = CriaRequisicao("/nfe/envia", Method.POST, this.Config);
            var arquivo = new StringBuilder();

            arquivo.AppendLine("DOCUMENTO=MDE");
            arquivo.AppendLine($"TIPOEVENTO={tipoEventoManifestacao}");
            arquivo.AppendLine($"CHAVENOTA={SefazHelper.LimpaChNFE(chNFE)}");
            arquivo.AppendLine($"DHEVENTO={DateTime.Now.ToString("s", CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat)}");
            arquivo.AppendLine($"FUSO={DateTime.Now:zzz}");
            arquivo.AppendLine($"JUSTIFICATIVA={justificativa}");
            request.AddParameter("arquivo", arquivo.ToString());

            var result = await client.ExecuteAsync(request).ConfigureAwait(false);

            var exception = ExceptionHandler(result.Content, this.Config);
            if (exception != null)
            {
                throw exception;
            }

            return result.Content;
        }

        public SefazException ExceptionHandler(string content, SefazConfig config)
        {
            SefazLogHelper.Error(config.Cnpj, content);
            if (SefazHelper.CaseInsensitiveContains(content, "EspdManChaveJaCadastradaException") || SefazHelper.CaseInsensitiveContains(content, "já esta cadastrada"))
            {
                return null;
            }
            if (SefazHelper.CaseInsensitiveContains(content, "Rejeicao: Duplicidade de evento"))
            {
                return new SefazDuplicidadeEventoException(string.Join(" ", content.Split(config.Delimitador.ToCharArray())));
            }
            if (SefazHelper.CheckIfIsException(content))
            {
                return new SefazException(string.Join(" ", content.Split(config.Delimitador.ToCharArray())));
            }
            else if (SefazHelper.CheckIfIsNenhumRegistroEncontrado(content))
            {
                return new SefazNenhumDocumentoEncontradoException(content);
            }

            return null;
        }


        /// <summary>
        /// Cria Cliente no RestSharp
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="config">Config</param>
        /// <returns> RestClient Objeto</returns>
        private static RestClient CriaCliente(SefazConfig config)
        {
            if (config == null)
            {
                throw new Exception("Configuração inexistente");
            }

            var url = config.Producao ? ProducaoUrl : HomologUrl;

            var client = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(config.User, config.Password)
            };

            client.Timeout = -1;
            client.ReadWriteTimeout = -1;

            return client;
        }

        /// <summary>
        /// Cria Requisição no RestSharp
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="method">METODO</param>
        /// <param name="config">CONFIG</param>
        /// <returns></returns>
        private static RestRequest CriaRequisicao(string url, Method method, SefazConfig config)
        {
            if (config == null)
            {
                throw new SefazException("Configuração inexistente");
            }

            var request = new RestRequest(url, method);
            request.AddParameter("encode", true);
            request.AddParameter("grupo", config.Grupo);
            request.AddParameter("cnpj", config.Cnpj);


            return request;
        }



        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Config = null;

            GC.SuppressFinalize(this);
        }


        public const int IntervalTecnoSpeed = 181;

        public static ConcurrentDictionary<string, int> LockChavesNfe = new ConcurrentDictionary<string, int>();

        private async Task<bool> SincronizaNotasAsync(string stringConnection, string chaveNFE = null)
        {
            this.StringConnection = stringConnection;
            var filtro = string.Empty;
            var sefazNotasChaveNFE = new List<SefazTecnoSpeedNotas>();

            SefazLogHelper.Debug(this.Cnpj, "Inicio SincronizaNotasCron");

            //if (sqlConn.State != ConnectionState.Open)
            //{
            //    await sqlConn.OpenAsync();
            //}

            var query = $@"SELECT DISTINCT * FROM {SefazTecnoSpeedNotas.SefazTecnoSpeedNotasTable} WITH (NOLOCK) WHERE IsDeleted = @isFalse AND Cnpj = @Cnpj";
            if (!chaveNFE.CheckIfIsNullOrEmpty())
            {
                query += " AND ChaveNfe = @chaveNFE ";
            }
            else
            {
                query += " AND IsNotaSefazTecnospeed = @isFalse";
            }
            using (var sqlConn = new SqlConnection(this.StringConnection))
            {
                sefazNotasChaveNFE = (await sqlConn.QueryAsync<SefazTecnoSpeedNotas>(query, new { isFalse = false, this.Config.Cnpj, chaveNFE }).ConfigureAwait(false)).ToList();
            }

            List<NotasDataEmissaoEChaveDto> notas = null;

            if (chaveNFE.CheckIfIsNullOrEmpty())
            {
                notas = await BuscaTodasNotas().ConfigureAwait(false);
            }
            else
            {
                if (sefazNotasChaveNFE.Any())
                {
                    notas = sefazNotasChaveNFE.Select(x => new NotasDataEmissaoEChaveDto { DataEmissao = x.DataEmissao.Date, NfeChave = x.ChaveNfe }).ToList();
                }
                else
                {
                    notas = await BuscaNota(chaveNFE).ConfigureAwait(false);
                }
            }

            notas = notas.Where(x => !sefazNotasChaveNFE.Where(s=> s.IsNotaSefazTecnospeed).Select(s => s.ChaveNfe).Contains(x.NfeChave)).OrderByDescending(x => x.DataEmissao).ToList();
            if (notas.IsNullOrEmpty())
            {
                SefazLogHelper.Debug(this.Cnpj, "Nenhuma nota para importar");
                return true;
            }

            foreach (var nota in notas)
            {
                try
                {
                    var sefazTecnoSpeedNota = await RetornaNota(nota);

                    var result = await ChecaMDEConhecimentoOuFazConhecimento(sefazTecnoSpeedNota, nota.NfeChave).ConfigureAwait(false);
                    sefazTecnoSpeedNota = result.SefazTecnoSpeedNota;
                    var nfe = result.Nfe;

                    nfe = await SincronizaEFazDownloadNfe(nfe, nota.NfeChave).ConfigureAwait(false);

                    CheckNfeSincronizada(nfe, sefazTecnoSpeedNota);

                    await SalvaSefazTecnoSpeedNota(sefazTecnoSpeedNota, this.StringConnection).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    SefazLogHelper.Error(this.Cnpj, $"SefazTecnoSpeedSincronizaNotasJob - {e.Message}", e);
                }
            }
            return true;
        }

        private async Task<SefazTecnoSpeedNotas> RetornaNota(NotasDataEmissaoEChaveDto nota)
        {
            var queryNota = $@"SELECT * FROM {SefazTecnoSpeedNotas.SefazTecnoSpeedNotasTable} WITH (NOLOCK) WHERE ChaveNfe = @NfeChave";
            SefazTecnoSpeedNotas sefazTecnoSpeedNota = null;
            using (var sqlConn = new SqlConnection(this.StringConnection))
            {
                sefazTecnoSpeedNota = await sqlConn.QueryFirstOrDefaultAsync<SefazTecnoSpeedNotas>(queryNota, new { nota.NfeChave }).ConfigureAwait(false);
                if (sefazTecnoSpeedNota == null)
                {
                    sefazTecnoSpeedNota = new SefazTecnoSpeedNotas(this.Cnpj, nota.NfeChave, nota.DataEmissao.ToLocalTime());
                }
            }

            return sefazTecnoSpeedNota;
        }

        protected class RetornoChecaMDEConhecimentoOuFazConhecimento
        {
            public SefazTecnoSpeedNotas SefazTecnoSpeedNota { get; set; }
            public RetornoNFeDto Nfe { get; set; }
        }

        private async Task<RetornoChecaMDEConhecimentoOuFazConhecimento> ChecaMDEConhecimentoOuFazConhecimento(SefazTecnoSpeedNotas sefazTecnoSpeedNota, string nfeChave)
        {
            RetornoNFeDto nfe = null;
            try
            {
                nfe = await DownloadNfePorChaveAsync(nfeChave).ConfigureAwait(false);
                if (nfe != null && nfe.NfeProc != null)
                {
                    sefazTecnoSpeedNota.IsMDEConhecimento = true;
                }
            }
            catch (Exception) { }

            if (!sefazTecnoSpeedNota.IsMDEConhecimento)
            {
                try
                {
                    LockChavesNfe.AguardaTimeoutSefaz(nfeChave);
                    await this.EnviarManifestacaoDoDestinatario(nfeChave).ConfigureAwait(false);
                    sefazTecnoSpeedNota.IsMDEConhecimento = true;
                }
                catch (SefazDuplicidadeEventoException)
                {
                    sefazTecnoSpeedNota.IsMDEConhecimento = true;
                }
            }

            if (!sefazTecnoSpeedNota.IsMDEConhecimento)
            {
                sefazTecnoSpeedNota.LastAttemptSefazTecnospeed = DateTime.Now;
            }

            return new RetornoChecaMDEConhecimentoOuFazConhecimento { Nfe = nfe, SefazTecnoSpeedNota = sefazTecnoSpeedNota };
        }

        private async Task<RetornoNFeDto> SincronizaEFazDownloadNfe(RetornoNFeDto nfe, string nfeChave)
        {
            if (nfe == null || nfe.NfeProc == null)
            {
                LockChavesNfe.AguardaTimeoutSefaz(nfeChave);
                if (await SincronizaSefazAsync(nfeChave).ConfigureAwait(false))
                {
                    nfe = await DownloadNfePorChaveAsync(nfeChave).ConfigureAwait(false);
                }
            }
            return nfe;
        }

        private static void CheckNfeSincronizada(RetornoNFeDto nfe, SefazTecnoSpeedNotas sefazTecnoSpeedNota)
        {
            if (nfe == null || nfe.NfeProc == null)
            {
                sefazTecnoSpeedNota.AttemptNotaSefazTecnospeedCount++;
            }
            else
            {
                sefazTecnoSpeedNota.DadosNfe(nfe.NfeProc);
                sefazTecnoSpeedNota.DateNotaSefazTecnospeed = DateTime.Now;
                sefazTecnoSpeedNota.IsNotaSefazTecnospeed = true;
            }
        }

        private async Task SalvaSefazTecnoSpeedNota(SefazTecnoSpeedNotas sefazTecnoSpeedNota, string connectionString)
        {
            using (var sqlConn = new SqlConnection(this.StringConnection))
            {
                await sqlConn.OpenAsync();
                using (var transaction = sqlConn.BeginTransaction())
                {
                    if (sefazTecnoSpeedNota.Id != 0)
                    {
                        await sqlConn.UpdateAsync(sefazTecnoSpeedNota, transaction).ConfigureAwait(false);
                    }
                    else
                    {
                        sefazTecnoSpeedNota.Id = await sqlConn.InsertAsync(sefazTecnoSpeedNota, transaction).ConfigureAwait(false);

                    }
                    transaction.Commit();
                }

                sqlConn.Close();
            }
        }
    }
}
