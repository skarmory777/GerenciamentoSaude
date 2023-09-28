using Abp.Domain.Uow;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Dto;
    using System;
    using System.Threading.Tasks;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Receituarios;
    using SW10.SWMANAGER.Authorization.Users;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
    using RestSharp;
    using System.Configuration;
    using Newtonsoft.Json;
    using System.Data.Entity;
    using Abp.Application.Services.Dto;
    using System.Collections.Generic;
    using Abp.AutoMapper;
    using Abp.UI;
    using System.Linq;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
    using System.Text;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;

    public class ReceituarioMedicoAppService : SWMANAGERAppServiceBase, IReceituarioMedicoAppService
    {
        #region Services
        private readonly IUserAppService _userAppService;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        #endregion

        #region Repositorios
        private readonly IRepository<Medico, long> _medicoRepository;
        private readonly IRepository<ReceituarioMedico, long> _receituarioMedicoRepository;
        #endregion

        #region Construtor

        public ReceituarioMedicoAppService(
        #region Services
            IUserAppService userAppService,
            IUltimoIdAppService ultimoIdAppService,
        #endregion

        #region Repositorios
            IRepository<Medico, long> medicoRepository,
            IRepository<ReceituarioMedico, long> receituarioMedicoRepository
        #endregion
        )
        {
            #region → Services
            _userAppService = userAppService;
            _ultimoIdAppService = ultimoIdAppService;
            #endregion

            #region → Repositorios
            _receituarioMedicoRepository = receituarioMedicoRepository;
            _medicoRepository = medicoRepository;
            #endregion
        }

        #endregion

        #region Métodos Públicos
        public async Task<ListResultDto<ReceituarioMedicoDto>> ListarTodos(ListarReceituarioMedicoInput input)
        {
            List<ReceituarioMedicoDto> dtos = new List<ReceituarioMedicoDto>();
            try
            {
                var receituariosMedicos = await _receituarioMedicoRepository
                    .GetAll()
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Where(w => w.AtendimentoId == input.AtendimentoId)
                    .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                    .OrderByDescending(m => m.DataReceituario)
                    .AsNoTracking()
                    .ToListAsync();

                dtos = receituariosMedicos
                    .MapTo<List<ReceituarioMedicoDto>>();

                return new ListResultDto<ReceituarioMedicoDto> { Items = dtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<ReceituarioMedicoDto> GerarNovoReceituarioMedico(long atendimentoId)
        {
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var receituarioMedicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ReceituarioMedico, long>>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var user = await userAppService.Object.GetUser().ConfigureAwait(false);

                if (user.MedicoId.HasValue)
                {
                    var medico = await medicoRepository.Object.GetAll().AsNoTracking()
                                     .Include(m => m.Sexo)
                                     .Include(m => m.Estado)
                                     .Include(m => m.Conselho)
                                     .Include(m => m.SisPessoa)
                                     .Include(m => m.SisPessoa.Enderecos.Select(s => s.Estado))
                                     .Where(m => m.Id == user.MedicoId).FirstOrDefaultAsync();

                    var mensagemCamposObrigatorios = this.validaCamposObrigatoriosCriarPrescritorMemed(medico);

                    if (string.IsNullOrEmpty(mensagemCamposObrigatorios))
                    {
                        var model = new ReceituarioMedico
                        {
                            AtendimentoId = atendimentoId,
                            DataReceituario = DateTime.Now,
                            MedicoId = (long)user.MedicoId
                        };

                        model.Id = await receituarioMedicoRepository.Object.InsertOrUpdateAndGetIdAsync(model).ConfigureAwait(false);
                        await SalvarArquivoReceituarioMedicoAsync(model.Id, model.AtendimentoId).ConfigureAwait(false);

                        var memedToken = this.ObtenhaMedicoPrescritorMemedToken(medico);
                        medico = await medicoRepository.Object.GetAll().Where(m => m.Id == user.MedicoId).FirstOrDefaultAsync();
                        medico.PrescritorMemedToken = memedToken;
                        await medicoRepository.Object.UpdateAsync(medico);

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current?.SaveChanges();

                        unitOfWork.Dispose();

                        return ReceituarioMedicoDto.Mapear(model);
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat(L("IsRequerido"), ": \n", mensagemCamposObrigatorios));
                    }
                }
                else
                {
                    throw new UserFriendlyException(L("MedicoNaoVinculadoAoUsuario"));
                }
            }
        }

        public async Task<string> ObterLinkMemedPDFPrescricao(long receituarioId, long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var receituarioMedicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ReceituarioMedico, long>>())
                using (var registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var receituarioMedico = await receituarioMedicoRepository.Object.GetAll().Include(x => x.Medico).FirstOrDefaultAsync(x => x.Id == receituarioId).ConfigureAwait(false);
                    var registroArquivo = await registroArquivoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.RegistroId == receituarioMedico.Id && x.AtendimentoId == atendimentoId && x.RegistroTabelaId == (long)EnumArquivoTabela.ReceituarioMedico).ConfigureAwait(false);

                    return RecuperaLinkPDFPrescrição(receituarioMedico.PrescricaoMemedId, receituarioMedico.Medico.PrescritorMemedToken);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroIncluir"), ex);
            }
        }

        public async Task<ReceituarioMedicoRecuperaLinkPrescricaoMemedDto.Attributes> ObterLinkMemedReceitaDigitalPacientePrescricao(long receituarioId, long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var receituarioMedicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ReceituarioMedico, long>>())
                using (var registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    var receituarioMedico = await receituarioMedicoRepository.Object.GetAll().Include(x => x.Medico).FirstOrDefaultAsync(x => x.Id == receituarioId).ConfigureAwait(false);
                    var registroArquivo = await registroArquivoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.RegistroId == receituarioMedico.Id && x.AtendimentoId == atendimentoId && x.RegistroTabelaId == (long)EnumArquivoTabela.ReceituarioMedico).ConfigureAwait(false);

                    return RecuperaLinkReceitaDigitalPacientePrescrição(receituarioMedico.PrescricaoMemedId, receituarioMedico.Medico.PrescritorMemedToken);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroIncluir"), ex);
            }
        }

        public async Task SalvarDadosDaReceitaMemed(RetornoReceitaMemedInput input)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var receituarioMedicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ReceituarioMedico, long>>())
                using (var registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                {
                    // Atualiza o Receituário Médico
                    var receituarioMedico = await receituarioMedicoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.Id == input.ReceituarioId && x.AtendimentoId == input.AtendimentoId).ConfigureAwait(false);
                    receituarioMedico.PrescricaoMemedId = input.PrescricaoMemedId;
                    await receituarioMedicoRepository.Object.UpdateAsync(receituarioMedico).ConfigureAwait(false);
                    // Atualiza o Documento Gerado (Receita)
                    var registroArquivo = await registroArquivoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.RegistroId == receituarioMedico.Id && x.AtendimentoId == input.AtendimentoId && x.RegistroTabelaId == (long)EnumArquivoTabela.ReceituarioMedico).ConfigureAwait(false);
                    registroArquivo.ArquivoNome = input.ReceitaDocumentoCompletoId;
                    registroArquivo.ArquivoTipo = "memed";
                    await registroArquivoRepository.Object.UpdateAsync(registroArquivo).ConfigureAwait(false);
                    // Atualiza o Paciente com seu Memed Token
                    var atendimentoDto = await atendimentoAppService.Object.Obter(input.AtendimentoId).ConfigureAwait(false);
                    var paciente = await pacienteRepository.Object.GetAsync((long) atendimentoDto.PacienteId).ConfigureAwait(false);
                    paciente.MemedId = input.PacienteMemedId;
                    await pacienteRepository.Object.UpdateAsync(paciente).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroIncluir"), ex);
            }
        }
        #endregion

        #region Métodos Privados
        private string validaCamposObrigatoriosCriarPrescritorMemed(Medico medico)
        {
            var mensagemCamposObrigatorios = new StringBuilder();
            if (!medico.SisPessoa.Nascimento.HasValue)
            {
                mensagemCamposObrigatorios.AppendLine(L("DataNascimentoMedicoObrigatorio"));
            }

            if (string.IsNullOrEmpty(medico.SisPessoa.Cpf))
            {
                mensagemCamposObrigatorios.AppendLine(L("CpfMedicoObrigatorio"));
            }

            if (medico.Estado == null)
            {
                mensagemCamposObrigatorios.AppendLine(L("EstadoMedicoObrigatorio"));
            }

            if (medico.SexoId == null)
            {
                mensagemCamposObrigatorios.AppendLine(L("SexoObrigatorio"));
            }

            return mensagemCamposObrigatorios.ToString();
        }

        private string ObtenhaMedicoPrescritorMemedToken(Medico medico)
        {
            try
            {
                // ----------------------------------------------------------------------------------
                // Consulta o Médico Prescritor para verificarmos se ele já possui o Token de usuário
                // ----------------------------------------------------------------------------------
                var client = new RestClient(string.Concat(ConfigurationManager.AppSettings.Get("MemedBaseUrl"),
                                                          "/v1/sinapse-prescricao/usuarios/",
                                                          medico.Id,
                                                          "?api-key=" + ConfigurationManager.AppSettings.Get("MemedApiKey") +
                                                          "&secret-key=" + ConfigurationManager.AppSettings.Get("MemedSecretKey")));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/vnd.api+json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);

                var receituarioMedicoRetornoMedicoMemedDto = JsonConvert.DeserializeObject<ReceituarioMedicoRetornoMedicoMemedDto>(response.Content);

                if (receituarioMedicoRetornoMedicoMemedDto.data == null)
                {
                    // ---------------------------------------------------------------------------------------------------------------
                    // O Médico Prescritor ainda não foi informado ao Memed, iremos solicitar a criação e recuperar o token de usuário
                    // ---------------------------------------------------------------------------------------------------------------
                    client = new RestClient(string.Concat(ConfigurationManager.AppSettings.Get("MemedBaseUrl"),
                                                          "/v1/sinapse-prescricao/usuarios",
                                                          "?api-key=" + ConfigurationManager.AppSettings.Get("MemedApiKey") +
                                                          "&secret-key=" + ConfigurationManager.AppSettings.Get("MemedSecretKey")));
                    client.Timeout = -1;
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/vnd.api+json");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Content-Type", "application/json");

                    var receituarioMedicoEntradaMedicoMemedDto = new ReceituarioMedicoEntradaMedicoMemedDto();
                    receituarioMedicoEntradaMedicoMemedDto.data = new ReceituarioMedicoEntradaMedicoMemedDto.Data();
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes = new ReceituarioMedicoEntradaMedicoMemedDto.Attributes();
                    receituarioMedicoEntradaMedicoMemedDto.data.type = "usuarios";
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.external_id = medico.Id.ToString();
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.nome = medico.SisPessoa.NomeCompleto.Substring(0, medico.SisPessoa.NomeCompleto.IndexOf(" "));
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.sobrenome = medico.SisPessoa.NomeCompleto.Substring(medico.SisPessoa.NomeCompleto.IndexOf(" "), medico.SisPessoa.NomeCompleto.Length - medico.SisPessoa.NomeCompleto.IndexOf(" "));
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.data_nascimento = medico.SisPessoa.Nascimento?.ToString("dd/MM/yyyy");
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.cpf = medico.SisPessoa.Cpf;
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.uf = medico.Estado.Uf;
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.sexo = medico.SexoId == 1 ? "M" : "F";
                    receituarioMedicoEntradaMedicoMemedDto.data.attributes.crm = medico.NumeroConselho.ToString();

                    request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(receituarioMedicoEntradaMedicoMemedDto), ParameterType.RequestBody);
                    response = client.Execute(request);

                    receituarioMedicoRetornoMedicoMemedDto = JsonConvert.DeserializeObject<ReceituarioMedicoRetornoMedicoMemedDto>(response.Content);

                    return receituarioMedicoRetornoMedicoMemedDto.data.attributes.token;
                }
                else
                {
                    return receituarioMedicoRetornoMedicoMemedDto.data.attributes.token;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro na integração com o Memed. Entre em contato com o suporte.", e);
            }
        }

        private ReceituarioMedicoRecuperaLinkPrescricaoMemedDto.Attributes RecuperaLinkReceitaDigitalPacientePrescrição(string receituarioMemedId, string medicoMemedToken)
        {
            try
            {
                // ------------------------------------------------
                // Recuperar o link da Receita Digital do Paciente
                // ------------------------------------------------
                var client = new RestClient(string.Concat(ConfigurationManager.AppSettings.Get("MemedBaseUrl"),
                                                          "/v1/prescricoes/",
                                                          receituarioMemedId,
                                                          "/get-digital-prescription-link?token=",
                                                          medicoMemedToken));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/vnd.api+json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);

                var receituarioMedicoRecuperaLinkPrescricaoMemedDto = JsonConvert.DeserializeObject<ReceituarioMedicoRecuperaLinkPrescricaoMemedDto>(response.Content);

                return receituarioMedicoRecuperaLinkPrescricaoMemedDto.data[0].attributes;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro na integração com o Memed. Entre em contato com o suporte.", e);
            }
        }

        private string RecuperaLinkPDFPrescrição(string receituarioMemedId, string medicoMemedToken)
        {
            try
            {
                // --------------------------------------
                // Recuperar o link do PDF da prescrição
                // --------------------------------------
                var client = new RestClient(string.Concat(ConfigurationManager.AppSettings.Get("MemedBaseUrl"),
                                                          "/v1/prescricoes/",
                                                          receituarioMemedId,
                                                          "/url-document/full?token=",
                                                          medicoMemedToken));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/vnd.api+json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);

                var receituarioMedicoRecuperaLinkPrescricaoMemedDto = JsonConvert.DeserializeObject<ReceituarioMedicoRecuperaLinkPrescricaoMemedDto>(response.Content);

                return receituarioMedicoRecuperaLinkPrescricaoMemedDto.data[0].attributes.link;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro na integração com o Memed. Entre em contato com o suporte.", e);
            }
        }

        private static async Task SalvarArquivoReceituarioMedicoAsync(long id, long atendimentoId)
        {
            using (var RegistroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
            {
                var registroArquivo = new RegistroArquivo
                {
                    RegistroTabelaId = (long)EnumArquivoTabela.ReceituarioMedico,
                    RegistroId = id,
                    AtendimentoId = atendimentoId
                };

                await RegistroArquivoRepository.Object.InsertOrUpdateAndGetIdAsync(registroArquivo).ConfigureAwait(false);
            }
        }
        #endregion
    }
}