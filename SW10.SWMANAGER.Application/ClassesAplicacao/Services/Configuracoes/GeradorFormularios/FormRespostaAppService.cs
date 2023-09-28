namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using Castle.Core.Internal;
    using Dapper;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
    using SW10.SWMANAGER.Helpers;

    public class FormRespostaAppService : SWMANAGERAppServiceBase, IFormRespostaAppService
    {
        [UnitOfWork]
        public async Task<long> CriarOuEditar(FormConfigDto formConfig, long idDadosResposta, string nomeClasse, string registroClasseId)
        {
            try
            {
                if (formConfig == null || formConfig.Id == 0 || formConfig.Linhas == null)
                {
                    throw new Exception();
                }

                using (var formDataAppService = IocManager.Instance.ResolveAsDisposable<IFormDataAppService>())
                using (var colConfigAppService = IocManager.Instance.ResolveAsDisposable<IColConfigAppService>())
                using (var formRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                using (var formDataRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormData, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var dados = new List<FormData>();
                    var colunasId = formConfig.Linhas.SelectMany(s => s.ColConfigs).Select(s => s.Id);
                    colunasId = colunasId.Distinct();

                    var idConfig = formConfig.Id;

                    var colunasBaseDto = (await colConfigAppService.Object.ListarTodos(colunasId).ConfigureAwait(false))
                        .Items;

                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var result = idDadosResposta != 0
                                     ? idDadosResposta
                                     : await formRespostaRepository.Object.InsertAndGetIdAsync(new FormResposta
                                     {
                                         DataResposta = DateTime.Now,
                                         NomeClasse = nomeClasse,
                                         RegistroClasseId = registroClasseId,
                                         FormConfigId = idConfig
                                     }).ConfigureAwait(false);

                        foreach (var linha in formConfig.Linhas)
                        {
                            foreach (var coluna in linha.ColConfigs.Where(coluna => coluna != null))
                            {
                                ProcessarValor(
                                    dados,
                                    coluna,
                                    colunasBaseDto.Where(w => w.Id == coluna.Id).FirstOrDefault(),
                                    coluna.MultiOption?.Where(s => s.Selecionado),
                                    result);
                            }
                        }

                        var respostaBase = await this.ObterNoLazy(result).ConfigureAwait(false);
                        respostaBase.DataResposta = DateTime.Now;

                        if (!respostaBase.IsPreenchido)
                        {
                            respostaBase.IsPreenchido = dados.Count != 0;
                        }

                        formRespostaRepository.Object.InsertOrUpdate(respostaBase);

                        formDataAppService.Object.Excluir(respostaBase.ColRespostas.Select(m => m.Id));
                        
                        foreach (var item in dados)
                        {
                            item.Id = formDataRepository.Object.InsertOrUpdateAndGetId(item);
                        }
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                        
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FormResposta input)
        {
            try
            {
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    await formRespostaRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<FormResposta>> Listar(ListarFormRespostaInput input)
        {
            try
            {
                using (var formRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    var query = formRespostaRepository.Object.GetAll().Where(m => m.FormConfigId == input.FormId)
                        .WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.DataResposta.ToString().Contains(input.Filtro))

                        // .Include(i => i.FormConfig)
                        // .Include(i => i.FormConfig.Linhas.Select(ss => ss.Col1.MultiOption))
                        // .Include(i => i.FormConfig.Linhas.Select(ss => ss.Col2.MultiOption))
                        // .Include(i => i.FormConfig.Linhas.Select(ss => ss.ColConfigs.Select(sss => sss.MultiOption)));
                        .Include(i => i.ColRespostas.Select(ss => ss.Coluna.MultiOption));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsResposta = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var formsRespostaResult = formsResposta.Select(m => new FormResposta
                    {
                        Id = m.Id,
                        DataResposta = m.DataResposta,
                        LastModificationTime = m.LastModificationTime,
                        CreationTime = m.CreationTime,
                        ColRespostas = formsResposta.SelectMany(f => f.ColRespostas)
                                         .Where(f => f.FormRespostaId == m.Id).Select(
                                             c => new FormData
                                             {
                                                 ColConfigId = c.ColConfigId,
                                                 Coluna = new ColConfig
                                                 {
                                                     Name = c.Coluna.Name,
                                                     Value = c.Coluna.Value,
                                                     CreationTime = c.Coluna.CreationTime,
                                                     LastModificationTime =
                                                                               c.Coluna.LastModificationTime
                                                 },
                                                 Id = c.Id,
                                                 FormRespostaId = c.FormRespostaId,
                                                 Valor = c.Valor,
                                                 CreationTime = c.CreationTime,
                                                 LastModificationTime = c.LastModificationTime
                                             }).Distinct().ToList()
                    }).Distinct().ToList();

                    return new PagedResultDto<FormResposta>(contarGeradorFormularios, formsRespostaResult);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormResposta>> ListarTodos()
        {
            try
            {
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    // var query = await _formRespostaRepository
                    // .GetAllListAsync();
                    var query = formRespostaRepository.Object.GetAll().Include(m => m.ColRespostas)
                        .Include(m => m.FormConfig).AsNoTracking();

                    // .Include(m => m.FormConfig.Linhas);
                    var forms = await query.ToListAsync().ConfigureAwait(false);

                    // var formsResposta = forms.MapTo<List<FormRespostaDto>>();
                    return new ListResultDto<FormResposta> { Items = forms };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormRespostaDto> Obter(long id)
        {
            try
            {
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    var query = formRespostaRepository.Object.GetAll()
                        .AsNoTracking()
                        .Where(m => m.Id == id)
                        .Include(i => i.FormConfig)
                        .Include(i => i.FormConfig.Linhas.Select(ss => ss.ColConfigs.Select(sss => sss.MultiOption)))
                        .Include(i => i.ColRespostas).Include(i => i.ColRespostas.Select(x => x.Coluna));

                    var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    // var formConfig = result
                    // .MapTo<FormRespostaDto>();
                    return FormRespostaDto.Mapear(result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormResposta> ObterNoLazy(long id)
        {
            try
            {
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    return await formRespostaRepository.Object.GetAllIncluding(m => m.ColRespostas)
                               .Where(m => m.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormRespostaDto> ObterUltimoLancamentoPorFormConfig(long formConfigId, long formRespostaId)
        {

            var queryFirstItem = @"
                ";

            var queryFormResposta = @$"
                    WITH CTE_FormResposta(SisFormRespostaId) AS (
                    SELECT 
                            TOP 1
	                        P.SisFormRespostaId 
                        FROM 
                            AssProntuario  P
                            INNER JOIN  SisFormResposta SFR (NOLOCK) ON  P.SisFormRespostaId = SFR.Id AND SFR.IsDeleted = @IsDeleted
                            INNER JOIN AteAtendimento (NOLOCK)  Ate ON  P.AteAtendimentoId = Ate.Id AND Ate.IsDeleted = @IsDeleted
                        WHERE 
                            P.IsDeleted = @IsDeleted
                            AND Ate.SisPacienteId = (
                                SELECT TOP 1
                                    Ate.SisPacienteId
                                FROM 
                                    AssProntuario  P
                                    INNER JOIN AteAtendimento (NOLOCK)  Ate ON  P.AteAtendimentoId =Ate.Id AND Ate.IsDeleted = @IsDeleted
                                WHERE 
                                    SisFormRespostaId = @formRespostaId AND P.IsDeleted = @IsDeleted
                            ) 
                        AND SFR.IsPreenchido = @IsTrue
                        AND SFR.Id != @formRespostaId
                        ORDER BY p.ID DESC
                    )
                    SELECT
                        {QueryHelper.CreateQueryFields<FormResposta>().TableAlias("SFR").GetFields()},
                        {QueryHelper.CreateQueryFields<FormData>().TableAlias("SFD").GetFields()},
                        {QueryHelper.CreateQueryFields<ColConfig>().TableAlias("SFCC").GetFields()},
                        {QueryHelper.CreateQueryFields<FormConfig>().TableAlias("SFC").GetFields()}
                    FROM 
                        SisFormResposta SFR
                        INNER JOIN CTE_FormResposta ON SFR.Id = CTE_FormResposta.SisFormRespostaId
                        LEFT JOIN SisFormData SFD (NOLOCK) ON  SFD.FormRespostaId = SFR.Id AND SFD.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormColConfig SFCC (NOLOCK) ON SFD.ColConfigId = SFCC.Id AND SFCC.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormConfig SFC (NOLOCK) ON  SFR.FormConfigId = SFC.Id ANd SFC.IsDeleted = @IsDeleted
                    WHERE
                        SFR.IsDeleted = @IsDeleted
                ";

            using (var connection = new SqlConnection(this.GetConnection()))
            {

                var lookup = new Dictionary<long, FormRespostaDto>();
                await connection.QueryAsync<FormRespostaDto, FormDataDto, ColConfigDto, FormConfigDto, FormRespostaDto>(queryFormResposta,
                    (fr, formData, colunaConfig, formConfig) =>
                    {
                        FormRespostaDto formResposta;
                        if (!lookup.TryGetValue(fr.Id, out formResposta))
                        {
                            lookup.Add(fr.Id, formResposta = fr);
                        }

                        if (formResposta.ColRespostas == null)
                        {
                            formResposta.ColRespostas = new List<FormDataDto>();
                        }

                        formResposta.FormConfig = formConfig ?? new FormConfigDto();
                        formData = formData ?? new FormDataDto();
                        formData.Coluna = colunaConfig;

                        formResposta.ColRespostas.Add(formData);

                        return formResposta;
                    }, new { formRespostaId, IsDeleted = false, IsTrue = true })
                    .ConfigureAwait(false);
                return lookup.Values.FirstOrDefault();

            }
        }
        
        public async Task<List<FormRespostaDto>> ObterUltimoLancamentosPorFormConfig(long formConfigId, long formRespostaId, long atendimentoId)
        {
            var queryFormResposta = @$"
                    WITH CT_FormResposta(SisFormRespostaId) 
                    AS
                    (
                        SELECT 
	                        P.SisFormRespostaId 
                        FROM 
                            AssProntuario  P
                            INNER JOIN  SisFormResposta SFR (NOLOCK) ON  P.SisFormRespostaId = SFR.Id AND SFR.IsDeleted = @IsDeleted
                            INNER JOIN AteAtendimento Ate ON  P.AteAtendimentoId = Ate.Id AND Ate.IsDeleted = @IsDeleted
                        WHERE 
                            P.IsDeleted = 0
                            AND Ate.SisPacienteId = (
                                SELECT 
			                        TOP 1
                                    Ate.SisPacienteId
                                FROM 
                                    AssProntuario  P
                                    INNER JOIN AteAtendimento Ate ON  P.AteAtendimentoId =Ate.Id AND Ate.IsDeleted = @IsDeleted
                                WHERE 
                                    SisFormRespostaId = @formRespostaId AND P.IsDeleted = @IsDeleted
                            )
                        AND SFR.IsPreenchido = @IsTrue
                        AND SFR.Id != @formRespostaId
                    )
                    SELECT
                        {QueryHelper.CreateQueryFields<FormResposta>().TableAlias("SFR").GetFields()},
                        {QueryHelper.CreateQueryFields<FormData>().TableAlias("SFD").GetFields()},
                        {QueryHelper.CreateQueryFields<ColConfig>().TableAlias("SFCC").GetFields()},
                        {QueryHelper.CreateQueryFields<FormConfig>().TableAlias("SFC").GetFields()}
                    FROM 
                        SisFormResposta SFR
                        LEFT JOIN SisFormData SFD (NOLOCK) ON  SFD.FormRespostaId = SFR.Id AND SFD.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormColConfig SFCC (NOLOCK) ON SFD.ColConfigId = SFCC.Id AND SFCC.IsDeleted = @IsDeleted
                        LEFT JOIN SisFormConfig SFC (NOLOCK) ON  SFR.FormConfigId = SFC.Id ANd SFC.IsDeleted = @IsDeleted
                        INNER JOIN CT_FormResposta ON SFR.Id = CT_FormResposta.SisFormRespostaId
                    WHERE
                        SFR.IsDeleted = @IsDeleted;
                ";

            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var lookup = new Dictionary<long, FormRespostaDto>();
                await connection.QueryAsync<FormRespostaDto, FormDataDto, ColConfigDto, FormConfigDto, FormRespostaDto>(queryFormResposta,
                    (fr, formData, colunaConfig, formConfig) =>
                    {
                        FormRespostaDto formResposta;
                        if (!lookup.TryGetValue(fr.Id, out formResposta))
                        {
                            lookup.Add(fr.Id, formResposta = fr);
                        }

                        if (formResposta.ColRespostas == null)
                        {
                            formResposta.ColRespostas = new List<FormDataDto>();
                        }

                        formResposta.FormConfig = formConfig ?? new FormConfigDto();
                        formData = formData ?? new FormDataDto();
                        formData.Coluna = colunaConfig;

                        formResposta.ColRespostas.Add(formData);

                        return formResposta;
                    }, new { formRespostaId, IsDeleted = false, IsTrue = true })
                    .ConfigureAwait(false);
                return lookup.Values.ToList();

            }
        }

        private static void ProcessarValor(
            List<FormData> dados,
            ColConfigDto col,
            ColConfigDto colunaBase,
            IEnumerable<ColMultiOptionDto> multiSelected,
            long formRespostaId)
        {
            var valor = string.Empty;
            if (col.Type == "checkbox" && multiSelected != null)
            {
                dados.AddRange(multiSelected.Select(option => new FormData
                {
                    Valor = option.Opcao,
                    ColConfigId = colunaBase.Id,
                    //Coluna = ColConfigDto.MapearEntidade(colunaBase),
                    FormRespostaId = formRespostaId,
                }));
                return;
            }

            valor = col.Value;
            dados.Add(new FormData
            {
                Valor = valor,
                ColConfigId = colunaBase.Id,
                FormRespostaId = formRespostaId
            });
        }

        async Task AlterarMedicoAtendimento(long respostaId)
        {
            using (var prontuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                var prontuario = prontuarioRepository.Object.GetAll()
                    .FirstOrDefault(w => w.FormRespostaId == respostaId);

                if (prontuario != null)
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        //await atendimentoAppService.Object.AlterarMedicoAtendimento(prontuario.AtendimentoId)
                        //    .ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
        }
    }
}
