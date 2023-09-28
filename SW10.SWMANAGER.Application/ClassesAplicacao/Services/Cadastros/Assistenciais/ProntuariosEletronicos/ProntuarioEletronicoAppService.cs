using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Dapper;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos
{
    using Abp.Dependency;

    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.Helpers;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text;

    public class ProntuarioEletronicoAppService : SWMANAGERAppServiceBase, IProntuarioEletronicoAppService
    {
        [UnitOfWork]
        public async Task<ProntuarioEletronicoDto> CriarOuEditar(ProntuarioEletronicoDto input)
        {
            try
            {
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var prontuario = ProntuarioEletronicoDto.Mapear(input);

                    if (prontuario.FormRespostaId == null && input.FormConfigId.HasValue)
                    {
                        prontuario.FormResposta = new FormResposta
                        {
                            FormConfigId = input.FormConfigId.Value,
                            DataResposta = DateTime.Now,
                            NomeClasse = "ProntuarioEletronico"
                        };
                    }

                    if (input.Id.Equals(0))
                    {
                        var ultimosId = await ultimoIdAppService.Object.ListarTodos().ConfigureAwait(false);
                        var ultimoId = ultimosId.Items.FirstOrDefault(m => m.NomeTabela == "AdmissaoMedica");
                        prontuario.Codigo = ultimoId.Codigo;
                        input.Codigo = prontuario.Codigo;
                        var codigo = Convert.ToInt64(ultimoId.Codigo);
                        codigo++;
                        ultimoId.Codigo = codigo.ToString();
                        await ultimoIdAppService.Object.CriarOuEditar(ultimoId).ConfigureAwait(false);
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await prontuarioRepository.Object.InsertAndGetIdAsync(prontuario)
                                           .ConfigureAwait(false);
                            prontuario.FormResposta.RegistroClasseId = input.Id.ToString();
                            input.FormRespostaId = prontuario.FormRespostaId;
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await prontuarioRepository.Object.UpdateAsync(prontuario).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    return input;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task AtualizarFormId(long id, long respostaId)
        {
            try
            {
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var prontuario = await prontuarioRepository.Object.FirstOrDefaultAsync(id).ConfigureAwait(false);
                        prontuario.FormRespostaId = respostaId;
                        await prontuarioRepository.Object.UpdateAsync(prontuario).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        unitOfWorkManager.Object.Current.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        [UnitOfWork]
        public async Task AlterarLeito(long id, long? leitoId)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prontuario = await prontuarioRepository.Object.FirstOrDefaultAsync(id).ConfigureAwait(false);
                    prontuario.LeitoId = leitoId;
                    await prontuarioRepository.Object.UpdateAsync(prontuario).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        public async Task Excluir(long id, string justificativa)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prontuario = await prontuarioRepository.Object.FirstOrDefaultAsync(id).ConfigureAwait(false);
                    prontuario.EstaInativo = true;
                    prontuario.InativacaoData = DateTime.Now;
                    prontuario.InativacaoUserId = (await GetCurrentUserAsync()).Id;
                    prontuario.InativacaoJustificativa = justificativa;

                    await prontuarioRepository.Object.UpdateAsync(prontuario).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task Reativar(long id, string justificativa)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prontuario = await prontuarioRepository.Object.FirstOrDefaultAsync(id).ConfigureAwait(false);
                    prontuario.EstaInativo = false;
                    prontuario.AtivacaoData = DateTime.Now;
                    prontuario.AtivacaoUserId = (await GetCurrentUserAsync()).Id;
                    prontuario.AtivacaoJustificativa = justificativa;

                    await prontuarioRepository.Object.UpdateAsync(prontuario).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroReativar"), ex);
            }
        }


        [UnitOfWork(false)]
        public async Task<PagedResultDto<ProntuarioEletronicoIndexDto>> Listar(ListarInput input)
        {
            //List<Prontuario> prontuarios;
            long atendimentoId = 0;
            long.TryParse(input.PrincipalId, out atendimentoId);
            try
            {
                const string selectFields = @"
                        Id,
                        DataAdmissao,
                        CodigoAtendimento,
                        Formulario,
                        FormRespostaId,
                        Medico,
                        Paciente,
                        UnidadeOrganizacional,
                        CreationTime,
                        CreatorUserId,
                        Usuario";

                const string fromFields = @"
                (
                    SELECT
                        AssProntuario.Id,
                        AssProntuario.DataAdmissao,
                        AteAtendimento.Codigo AS CodigoAtendimento,
                        SisFormConfig.Nome AS Formulario,
                        AssProntuario.SisFormRespostaId AS FormRespostaId,
                        Medico.NomeCompleto AS Medico,
                        Paciente.NomeCompleto AS Paciente,
                        SisUnidadeOrganizacional.Descricao AS UnidadeOrganizacional,
                        AssProntuario.CreationTime AS CreationTime,
                        AssProntuario.CreatorUserId AS CreatorUserId,
                        AssProntuario.Codigo AS Codigo,
                        AssProntuario.Descricao AS Descricao,
                        AssProntuario.SisOperacaoId AS SisOperacaoId,
                        AssProntuario.AteAtendimentoId,
                        AssProntuario.IsDeleted,
                        AssProntuario.EstaInativo,
                        Concat(dbo.InitCap(AbpUsers.Name), ' ', dbo.InitCap(AbpUsers.UserName)) AS Usuario
                    FROM
                    AssProntuario
                    LEFT JOIN AteAtendimento
                        ON AteAtendimento.Id = AssProntuario.AteAtendimentoId
                    LEFT JOIN SisMedico
                        ON SisMedico.Id = AteAtendimento.SisMedicoId
                    LEFT JOIN SisPessoa AS Medico
                        ON Medico.Id = SisMedico.SisPessoaId
                    LEFT JOIN SisPaciente
                        ON SisPaciente.Id = AteAtendimento.SisPacienteId
                    LEFT JOIN SisPessoa AS Paciente
                        ON Paciente.Id = SisPaciente.SisPessoaId
                    LEFT JOIN SisFormResposta 
                        ON SisFormResposta.Id = AssProntuario.SisFormRespostaId
                    LEFT JOIN SisFormConfig
                        ON SisFormResposta.FormConfigId = SisFormConfig.Id
                    LEFT JOIN SisUnidadeOrganizacional
                        ON SisUnidadeOrganizacional.Id = AssProntuario.SisUnidadeOrganizacionalId
                    LEFT JOIN AbpUsers 
                        ON  AssProntuario.CreatorUserId = AbpUsers.Id
                ) AS Prontuario";

                var whereBuilder = new StringBuilder(@"Prontuario.IsDeleted = @deleted AND Prontuario.EstaInativo = @deleted");

                var parameters = new Dictionary<string, object> { { "deleted", false } };

                if (input.OperacaoId.HasValue)
                {
                    parameters.Add("operacaoId", input.OperacaoId);
                    whereBuilder.Append(" AND Prontuario.SisOperacaoId = @operacaoId");
                }

                if (atendimentoId > 0)
                {
                    parameters.Add("atendimentoId", atendimentoId);
                    whereBuilder.Append(" AND Prontuario.AteAtendimentoId = @atendimentoId");
                }

                if (!input.Filtro.IsNullOrEmpty())
                {
                    parameters.Add("filtro", input.Filtro);
                    whereBuilder.Append(" AND (Prontuario.Codigo LIKE '%'+@filtro+'%' OR Prontuario.Descricao LIKE '%'+@filtro+'%')");
                }

                parameters.Add("skip", input.SkipCount);
                parameters.Add("take", input.MaxResultCount);

                var user = GetCurrentUser();
                using (var operacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Operacao, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var atendimento = await atendimentoRepository.Object.GetAll().AsNoTracking()
                        .Select(x => new { x.Id, x.DataAlta })
                        .FirstOrDefaultAsync(x => x.Id == atendimentoId).ConfigureAwait(false);
                    var permissoesEdit = new List<string>();
                    var permissoesDelete = new List<string>();
                    if (input.OperacaoId.HasValue)
                    {
                        var operacao = await operacaoRepository.Object.FirstOrDefaultAsync(input.OperacaoId.Value).ConfigureAwait(false);

                        if (operacao != null)
                        {
                            var permissions = await userManager.Object.GetGrantedPermissionsAsync(user).ConfigureAwait(false);
                            permissoesEdit = permissions.Where(x => x.Name.Contains(AppPermissions.FormularioDinamico_Edit) && x.Name.Contains(operacao.Name)).Select(x => x.Name).ToList();
                            permissoesDelete = permissions.Where(x => x.Name.Contains(AppPermissions.FormularioDinamico_Delete) && x.Name.Contains(operacao.Name)).Select(x => x.Name).ToList();
                        }
                    }

                    using (var queryMultiple = await connection.QueryMultipleAsync(DataTableHelper.BuildQueryWithTakeSkip("Id", selectFields, fromFields, whereBuilder.ToString(), input.Sorting), parameters, commandTimeout: 0).ConfigureAwait(false))
                    {
                        var totalItens = await queryMultiple.ReadFirstAsync<int>().ConfigureAwait(false);
                        var itens = (await queryMultiple.ReadAsync<ProntuarioEletronicoIndexDto>().ConfigureAwait(false)).ToList();
                        foreach (var item in itens)
                        {
                            if (!permissoesEdit.IsNullOrEmpty() || !permissoesDelete.IsNullOrEmpty())
                            {
                                item.EnableEdit = CheckEditPermission(item, atendimento.DataAlta, permissoesEdit, user.Id);
                                item.EnableDelete = CheckDeletePermission(item, atendimento.DataAlta, permissoesDelete, user.Id);
                            }
                        }

                        return new PagedResultDto<ProntuarioEletronicoIndexDto>(totalItens, itens);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<ProntuarioEletronicoIndexDto>> ListarInativos(ListarInput input)
        {
            long atendimentoId = 0;
            long.TryParse(input.PrincipalId, out atendimentoId);
            try
            {
                const string selectFields = @"
                        Id,
                        DataAdmissao,
                        CodigoAtendimento,
                        Formulario,
                        FormRespostaId,
                        Medico,
                        Paciente,
                        UnidadeOrganizacional,
                        CreationTime,
                        CreatorUserId,
                        Usuario";

                const string fromFields = @"
                (
                    SELECT
                        AssProntuario.Id,
                        AssProntuario.DataAdmissao,
                        AteAtendimento.Codigo AS CodigoAtendimento,
                        SisFormConfig.Nome AS Formulario,
                        AssProntuario.SisFormRespostaId AS FormRespostaId,
                        Medico.NomeCompleto AS Medico,
                        Paciente.NomeCompleto AS Paciente,
                        SisUnidadeOrganizacional.Descricao AS UnidadeOrganizacional,
                        AssProntuario.CreationTime AS CreationTime,
                        AssProntuario.CreatorUserId AS CreatorUserId,
                        AssProntuario.Codigo AS Codigo,
                        AssProntuario.Descricao AS Descricao,
                        AssProntuario.SisOperacaoId AS SisOperacaoId,
                        AssProntuario.AteAtendimentoId,
                        AssProntuario.IsDeleted,
                        AssProntuario.EstaInativo,
                        Concat(dbo.InitCap(AbpUsers.Name), ' ', dbo.InitCap(AbpUsers.UserName)) AS Usuario
                    FROM
                    AssProntuario
                    LEFT JOIN AteAtendimento
                        ON AteAtendimento.Id = AssProntuario.AteAtendimentoId
                    LEFT JOIN SisMedico
                        ON SisMedico.Id = AteAtendimento.SisMedicoId
                    LEFT JOIN SisPessoa AS Medico
                        ON Medico.Id = SisMedico.SisPessoaId
                    LEFT JOIN SisPaciente
                        ON SisPaciente.Id = AteAtendimento.SisPacienteId
                    LEFT JOIN SisPessoa AS Paciente
                        ON Paciente.Id = SisPaciente.SisPessoaId
                    LEFT JOIN SisFormResposta 
                        ON SisFormResposta.Id = AssProntuario.SisFormRespostaId
                    LEFT JOIN SisFormConfig
                        ON SisFormResposta.FormConfigId = SisFormConfig.Id
                    LEFT JOIN SisUnidadeOrganizacional
                        ON SisUnidadeOrganizacional.Id = AssProntuario.SisUnidadeOrganizacionalId
                    LEFT JOIN AbpUsers 
                        ON  AssProntuario.CreatorUserId = AbpUsers.Id
                ) AS Prontuario";

                var whereBuilder = new StringBuilder(@"Prontuario.IsDeleted = @deleted AND Prontuario.EstaInativo = @estaInativo");

                var parameters = new Dictionary<string, object> { { "deleted", false }, { "estaInativo", true } };

                if (input.OperacaoId.HasValue)
                {
                    parameters.Add("operacaoId", input.OperacaoId);
                    whereBuilder.Append(" AND Prontuario.SisOperacaoId = @operacaoId");
                }

                if (atendimentoId > 0)
                {
                    parameters.Add("atendimentoId", atendimentoId);
                    whereBuilder.Append(" AND Prontuario.AteAtendimentoId = @atendimentoId");
                }

                if (!input.Filtro.IsNullOrEmpty())
                {
                    parameters.Add("filtro", input.Filtro);
                    whereBuilder.Append(" AND (Prontuario.Codigo LIKE '%'+@filtro+'%' OR Prontuario.Descricao LIKE '%'+@filtro+'%')");
                }

                parameters.Add("skip", input.SkipCount);
                parameters.Add("take", input.MaxResultCount);

                var user = GetCurrentUser();
                using (var operacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Operacao, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var atendimento = await atendimentoRepository.Object.GetAll().AsNoTracking()
                        .Select(x => new { x.Id, x.DataAlta })
                        .FirstOrDefaultAsync(x => x.Id == atendimentoId).ConfigureAwait(false);
                    var permissoesEdit = new List<string>();
                    var permissoesDelete = new List<string>();
                    if (input.OperacaoId.HasValue)
                    {
                        var operacao = await operacaoRepository.Object.FirstOrDefaultAsync(input.OperacaoId.Value).ConfigureAwait(false);

                        if (operacao != null)
                        {
                            var permissions = await userManager.Object.GetGrantedPermissionsAsync(user).ConfigureAwait(false);
                            permissoesEdit = permissions.Where(x => x.Name.Contains(AppPermissions.FormularioDinamico_Edit) && x.Name.Contains(operacao.Name)).Select(x => x.Name).ToList();
                            permissoesDelete = permissions.Where(x => x.Name.Contains(AppPermissions.FormularioDinamico_Delete) && x.Name.Contains(operacao.Name)).Select(x => x.Name).ToList();
                        }
                    }

                    using (var queryMultiple = await connection.QueryMultipleAsync(DataTableHelper.BuildQueryWithTakeSkip("Id", selectFields, fromFields, whereBuilder.ToString(), input.Sorting), parameters, commandTimeout: 0).ConfigureAwait(false))
                    {
                        var totalItens = await queryMultiple.ReadFirstAsync<int>().ConfigureAwait(false);
                        var itens = (await queryMultiple.ReadAsync<ProntuarioEletronicoIndexDto>().ConfigureAwait(false)).ToList();
                        //foreach (var item in itens)
                        //{
                        //    if (!permissoesEdit.IsNullOrEmpty() || !permissoesDelete.IsNullOrEmpty())
                        //    {
                        //        item.EnableEdit = CheckEditPermission(item, atendimento.DataAlta, permissoesEdit, user.Id);
                        //        item.EnableDelete = CheckDeletePermission(item, atendimento.DataAlta, permissoesDelete, user.Id);
                        //    }
                        //}

                        return new PagedResultDto<ProntuarioEletronicoIndexDto>(totalItens, itens);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private static bool CheckEditPermission(ProntuarioEletronicoIndexDto item, DateTime? dataAlta, IEnumerable<string> permissions, long currentUserId)
        {
            if (permissions.IsNullOrEmpty())
            {
                return false;
            }

            if (permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Edit, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            if (DateTime.Now.Subtract(item.CreationTime).TotalHours <= 24 && permissions.Any(x =>
                x.EndsWith(AppPermissions.FormularioDinamico_Edit_Proprio_24hrs, StringComparison.InvariantCultureIgnoreCase) &&
                item.CreatorUserId == currentUserId))
            {
                return true;
            }

            if (DateTime.Now.Subtract(item.CreationTime).TotalHours <= 24 &&
                permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Edit_24hrs, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            if (permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Edit_Internados_ate_alta_1semana,
                StringComparison.InvariantCultureIgnoreCase)) && !dataAlta.HasValue)
            {
                return true;
            }
            if (dataAlta.HasValue && DateTime.Now.Subtract(dataAlta.Value).TotalDays <= 7 &&
                permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Edit_Internados_ate_alta_1semana, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        private static bool CheckDeletePermission(ICreationAudited item, DateTime? dataAlta, IEnumerable<string> permissions, long currentUserId)
        {
            if (permissions.IsNullOrEmpty())
            {
                return false;
            }

            if (permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Delete)))
            {
                return true;
            }

            if (DateTime.Now.Subtract(item.CreationTime).TotalHours <= 24 && permissions.Any(x =>
                x.Contains(AppPermissions.FormularioDinamico_Delete_Proprio_24hrs) &&
                item.CreatorUserId == currentUserId))
            {
                return true;
            }

            if (DateTime.Now.Subtract(item.CreationTime).TotalHours <= 24 &&
                permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Delete_24hrs)))
            {
                return true;
            }

            if (permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Delete_Internados_ate_alta_1semana,
                StringComparison.InvariantCultureIgnoreCase)) && !dataAlta.HasValue)
            {
                return true;
            }
            if (dataAlta.HasValue && DateTime.Now.Subtract(dataAlta.Value).TotalDays <= 7 &&
                permissions.Any(x => x.EndsWith(AppPermissions.FormularioDinamico_Delete_Internados_ate_alta_1semana, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        public async Task<ProntuarioEletronicoDto> ObterUltimoProntuarioPorAtendimentoEFormulario(long atendimentoId, long formConfigId, long formRespostaId)
        {
            var queryFirst = @$"
                    SELECT
                        TOP 1 P.Id
                    FROM 
                        AssProntuario P
                        INNER JOIN  SisFormResposta SFR (NOLOCK) ON  P.SisFormRespostaId = SFR.Id AND SFR.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormData SFD (NOLOCK) ON  SFD.FormRespostaId = SFR.Id AND SFD.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormColConfig SFCC (NOLOCK) ON SFD.ColConfigId = SFCC.Id AND SFCC.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormConfig SFC (NOLOCK) ON  SFR.FormConfigId = SFC.Id ANd SFC.IsDeleted = @IsDeleted
                    WHERE
                        P.IsDeleted = @IsDeleted
                        AND P.AteAtendimentoId = @atendimentoId
                        AND SFR.FormConfigId = @formConfigId
                        AND SFR.IsPreenchido = @IsTrue
                        AND SFR.Id != @formRespostaId
                        ORDER BY P.Id DESC
                ";
            var queryItem = @$"
                    SELECT
                        {QueryHelper.CreateQueryFields<Prontuario>().TableAlias("P").GetFields()},
                        {QueryHelper.CreateQueryFields<FormResposta>().TableAlias("SFR").GetFields()},
                        {QueryHelper.CreateQueryFields<FormData>().TableAlias("SFD").GetFields()},
                        {QueryHelper.CreateQueryFields<ColConfig>().TableAlias("SFCC").GetFields()},
                        {QueryHelper.CreateQueryFields<FormConfig>().TableAlias("SFC").GetFields()}
                    FROM 
                        AssProntuario P
                        INNER JOIN  SisFormResposta SFR (NOLOCK) ON  P.SisFormRespostaId = SFR.Id AND SFR.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormData SFD (NOLOCK) ON  SFD.FormRespostaId = SFR.Id AND SFD.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormColConfig SFCC (NOLOCK) ON SFD.ColConfigId = SFCC.Id AND SFCC.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormConfig SFC (NOLOCK) ON  SFR.FormConfigId = SFC.Id ANd SFC.IsDeleted = @IsDeleted
                    WHERE
                        P.IsDeleted = @IsDeleted AND P.Id = @Id
                ";

            using (var connection = new SqlConnection(this.GetConnection()))
            {

                var resultId = await connection.QueryFirstOrDefaultAsync<long?>(queryFirst,
                    new
                    {
                        atendimentoId,
                        formRespostaId,
                        formConfigId,
                        IsTrue = true,
                        IsDeleted = false
                    })
                    .ConfigureAwait(false);
                if (resultId == null || resultId == 0)
                {
                    return null;
                }
                var lookup = new Dictionary<long, ProntuarioEletronicoDto>();
                await connection.QueryAsync<ProntuarioEletronicoDto, FormResposta, FormData, ColConfig, FormConfig, ProntuarioEletronicoDto>(queryItem,
                    (p, formResposta, formData, colunaConfig, formConfig) =>
                    {
                        ProntuarioEletronicoDto prontuario;
                        if (!lookup.TryGetValue(p.Id, out prontuario))
                        {
                            lookup.Add(p.Id, prontuario = p);
                        }
                        if(prontuario.FormResposta == null)
                        {
                            prontuario.FormResposta = formResposta;
                        }

                        if (prontuario.FormResposta.ColRespostas == null)
                        {
                            prontuario.FormResposta.ColRespostas = new List<FormData>();
                        }

                        prontuario.FormResposta.FormConfig = formConfig ?? new FormConfig();

                        formData = formData ?? new FormData();
                        formData.Coluna = colunaConfig;

                        prontuario.FormResposta.ColRespostas.Add(formData);

                        return prontuario;
                    }, new { Id = resultId.Value, IsDeleted = false })
                    .ConfigureAwait(false);
                return lookup.Values.FirstOrDefault();
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<ProntuarioEletronicoDto>> ListarTodos()
        {
            try
            {
                using (var prontuarioRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                {
                    var query = await prontuarioRepository.Object.GetAll().Select(
                                    m => new ProntuarioEletronicoDto
                                    {
                                        AtendimentoId = m.AtendimentoId,
                                        Codigo = m.Codigo,
                                        CreationTime = m.CreationTime,
                                        CreatorUserId = m.CreatorUserId,
                                        DataAdmissao = m.DataAdmissao,
                                        DeleterUserId = m.DeleterUserId,
                                        DeletionTime = m.DeletionTime,
                                        Descricao = m.Descricao,
                                        FormRespostaId = m.FormRespostaId,
                                        Id = m.Id,
                                        IsDeleted = m.IsDeleted,
                                        IsSistema = m.IsSistema,
                                        LastModificationTime = m.LastModificationTime,
                                        LastModifierUserId = m.LastModifierUserId,
                                        Observacao = m.Observacao,
                                        OperacaoId = m.OperacaoId,
                                        ProfissionalSaudeId = m.ProfissionalSaudeId,
                                        ProntuarioPrincipalId = m.ProntuarioPrincipalId,
                                        UnidadeOrganizacionalId = m.UnidadeOrganizacionalId
                                    }).ToListAsync().ConfigureAwait(false);

                    var prontuariosDto = query.ToList();

                    return new ListResultDto<ProntuarioEletronicoDto> { Items = prontuariosDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ProntuarioEletronicoDto> Obter(long id)
        {
            try
            {
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var operacaoAppService = IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                {
                    var query = prontuarioRepository.Object.GetAll()
                        .Include(x => x.FormResposta).Include(x => x.FormResposta.FormConfig).Where(m => m.Id == id)
                        .Select(
                        m => new ProntuarioEletronicoDto
                        {
                            AtendimentoId = m.AtendimentoId,
                            Codigo = m.Codigo,
                            CreationTime = m.CreationTime,
                            CreatorUserId = m.CreatorUserId,
                            DataAdmissao = m.DataAdmissao,
                            DeleterUserId = m.DeleterUserId,
                            DeletionTime = m.DeletionTime,
                            Descricao = m.Descricao,
                            FormRespostaId = m.FormRespostaId,
                            Id = m.Id,
                            IsDeleted = m.IsDeleted,
                            IsSistema = m.IsSistema,
                            LastModificationTime = m.LastModificationTime,
                            LastModifierUserId = m.LastModifierUserId,
                            Observacao = m.Observacao,
                            OperacaoId = m.OperacaoId,
                            ProfissionalSaudeId = m.ProfissionalSaudeId,
                            ProntuarioPrincipalId = m.ProntuarioPrincipalId,
                            UnidadeOrganizacionalId = m.UnidadeOrganizacionalId,
                            FormConfigId = m.FormResposta != null && m.FormResposta.FormConfig != null ? m.FormResposta.FormConfig.Id : 0,
                            FormConfigNome = m.FormResposta != null && m.FormResposta.FormConfig != null ? m.FormResposta.FormConfig.Nome : "",
                            EstaInativo = m.EstaInativo,
                            LeitoId = m.LeitoId
                        });

                    var prontuarioDto = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    prontuarioDto.Atendimento = AtendimentoDto.Mapear(await atendimentoRepository.Object.FirstOrDefaultAsync(prontuarioDto.AtendimentoId));

                    if (prontuarioDto.OperacaoId.HasValue)
                    {
                        prontuarioDto.Operacao = await operacaoAppService.Object.Obter(prontuarioDto.OperacaoId.Value).ConfigureAwait(false);
                    }

                    if (prontuarioDto.ProfissionalSaudeId.HasValue)
                    {

                    }

                    if (prontuarioDto.UnidadeOrganizacionalId > 0)
                    {
                        prontuarioDto.UnidadeOrganizacional =
                            await unidadeOrganizacionalAppService.Object.ObterPorId(prontuarioDto.UnidadeOrganizacionalId).ConfigureAwait(false);
                    }

                    return prontuarioDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                {
                    //get com filtro
                    var query = from p in prontuarioRepository.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                         || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };
                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
