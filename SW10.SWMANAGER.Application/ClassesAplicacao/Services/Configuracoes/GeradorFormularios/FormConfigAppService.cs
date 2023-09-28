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
    using Abp.Collections.Extensions;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.EntityFramework.Repositories;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais;
    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    public class FormConfigAppService : SWMANAGERAppServiceBase, IFormConfigAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(FormConfigDto input)
        {
            try
            {
                using (var formConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    // O codigo em FormularioApp.js verifica campos reservados atraves do Name
                    // da ColConfig, sempre usando 'toUpperCase()', portanto, sempre salvar ColConfig.Name
                    // usando 'ToUpper()'
                    foreach (var linha in input.Linhas)
                    {
                        foreach (var col in linha.ColConfigs)
                        {
                            col.Name = col.Name.ToUpper();
                        }
                    }

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            var entity = FormConfigDto.Mapear(input);
                            await formConfigRepository.Object.InsertAsync(entity).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await this.Editar(input).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }


        public async Task Editar(FormConfigDto formConfig)
        {
            try
            {
                using (var formConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var formConfigExistente = formConfigRepository.Object.GetAllIncluding(f => f.Linhas).FirstOrDefault(j => j.Id == formConfig.Id);
                        formConfigExistente.Nome = formConfig.Nome;
                        formConfigExistente.FontSize = formConfig.FontSize;
                        var itensDeletados = new List<RowConfigDto>();

                        itensDeletados.AddRange(
                            formConfig.Linhas.Where(f => !formConfigExistente.Linhas.Select(e => e.Id).Contains(f.Id)));

                        var itensAdicionados = new List<RowConfig>();

                        foreach (var it in formConfig.Linhas)
                        {
                            var linha = formConfigExistente.Linhas.FirstOrDefault(ite => ite.Id == it.Id);
                            if (linha == null)
                            {
                                itensAdicionados.Add(RowConfigDto.MapearEntidade(it));
                            }
                            else
                            {
                                foreach (var colConfig in it.ColConfigs)
                                {
                                    var colConfigEntity = ColConfigDto.MapearEntidade(colConfig);
                                    formConfigRepository.Object.GetDbContext().Entry(colConfigEntity).State = colConfigEntity.IsTransient() ? EntityState.Added : EntityState.Modified;
                                }

                                formConfigRepository.Object.GetDbContext().Entry(linha).State = linha.IsTransient() ? EntityState.Added : EntityState.Modified;
                            }
                        }

                        itensDeletados.ForEach(x => formConfigExistente.Linhas.Remove(RowConfigDto.MapearEntidade(x)));

                        foreach (var c in itensAdicionados)
                        {
                            formConfigRepository.Object.GetDbContext().Entry(c).State = EntityState.Added;
                            formConfigExistente.Linhas.Add(c);
                        }

                        formConfigRepository.Object.GetDbContext().Entry(formConfigExistente).State = EntityState.Modified;

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }
        }

        public async Task Excluir(FormConfigDto input)
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    await formConfigRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<FormConfigDto>> Listar(ListarInput input)
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    var query = formConfigRepository.Object.GetAll().AsNoTracking().Where(f => f.Descricao != "RESERVADO")
                        .Include(i => i.Linhas.Select(ss => ss.ColConfigs.Select(sss => sss.MultiOption))).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Nome.Contains(input.Filtro) || m.DataAlteracao.ToString().Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input)
                                                       .ToListAsync().ConfigureAwait(false);
                    var formsConfigResult = new List<FormConfig>();

                    // Verificando se o formulário é editável (somente se não tiver nenhuma resposta)
                    foreach (var formConfig in formsConfig)
                    {
                        var conf = await formRespostaRepository.Object.GetAll().AsNoTracking().Where(m => m.FormConfigId == formConfig.Id).CountAsync().ConfigureAwait(false);
                        formsConfigResult.Add(formConfig);
                    }

                    return new PagedResultDto<FormConfigDto>(
                        contarGeradorFormularios,
                        formsConfigResult?.Select(FormConfigDto.Mapear).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigDto>> ListarTodos()
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    var query = await formConfigRepository.Object.GetAll().AsNoTracking().ToListAsync().ConfigureAwait(false);
                    var formsConfigResult = new List<FormConfig>();

                    // Verificando se o formulário é editável (somente se não tiver nenhuma resposta)
                    foreach (var formConfig in query)
                    {
                        var conf = await formRespostaRepository.Object.GetAll().AsNoTracking().Where(m => m.FormConfigId == formConfig.Id).CountAsync().ConfigureAwait(false);
                        formConfig.IsProducao = conf > 0;
                        formsConfigResult.Add(formConfig);
                    }

                    return new ListResultDto<FormConfigDto>
                    {
                        Items = formsConfigResult?.Select(FormConfigDto.Mapear).ToList()
                    };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigDto> Obter(long id)
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                using (var formRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormResposta, long>>())
                {
                    var formConfig = await formConfigRepository.Object.GetAll().AsNoTracking().Include(i => i.Linhas)
                                         .Include(
                                             i => i.Linhas.Select(ss => ss.ColConfigs.Select(sss => sss.MultiOption)))
                                         .Where(m => m.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

                    formConfig.IsProducao = await formRespostaRepository.Object.GetAll().AsNoTracking()
                                                .Where(m => m.FormConfigId == formConfig.Id).AnyAsync().ConfigureAwait(false);

                    return FormConfigDto.Mapear(formConfig);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigDto> ObterDapper(long id)
        {
            try
            {
                var query = @"
                    SELECT 
                        SisFormConfig.*,SisFormRowConfig.*,SisFormColConfig.*,SisFormColMultiOption.*
                    FROM 
                    SisFormConfig 
                    LEFT JOIN SisFormRowConfig 
                        ON SisFormRowConfig.FormConfig_Id = SisFormConfig.Id
                    LEFT JOIN SisFormColConfig
                        ON SisFormColConfig.RowConfig_Id = SisFormRowConfig.Id
                    LEFT JOIN SisFormColMultiOption 
                        ON SisFormColMultiOption.ColConfig_Id = SisFormColConfig.Id
                    WHERE SisFormConfig.IsDeleted = @deleted
                        AND SisFormConfig.Id = @formConfigId;";

                var queryCount = @"SELECT COUNT(id) FROM SisFormResposta WHERE SisFormResposta.FormConfigId = @formConfigId;";

                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var dicFormConfig = new Dictionary<long, FormConfigDto>();

                    var result = (await connection.QueryAsync<FormConfigDto, RowConfigDto, ColConfigDto, ColMultiOptionDto, FormConfigDto>(query,
                        (formConfigDto, rowConfigDto, colConfigDto, colMultiOptionDto) =>
                     {
                         if (formConfigDto == null)
                         {
                             return null;
                         }

                         if (dicFormConfig.ContainsKey(formConfigDto.Id))
                         {
                             formConfigDto = dicFormConfig[formConfigDto.Id];
                         }

                         if (formConfigDto.Linhas.IsNullOrEmpty())
                         {
                             formConfigDto.Linhas = new List<RowConfigDto>();
                         }

                         if (formConfigDto.Linhas.Exists(x => x.Id == rowConfigDto.Id))
                         {
                             rowConfigDto = formConfigDto.Linhas.FirstOrDefault(x => x.Id == rowConfigDto.Id);
                         }

                         if (rowConfigDto.ColConfigs.IsNullOrEmpty())
                         {
                             rowConfigDto.ColConfigs = new List<ColConfigDto>();
                         }

                         if (rowConfigDto.ColConfigs.Exists(x => x.Id == colConfigDto.Id))
                         {
                             colConfigDto = rowConfigDto.ColConfigs.FirstOrDefault(x => x.Id == colConfigDto.Id);
                         }

                         if (colMultiOptionDto != null)
                         {
                             if (colConfigDto.MultiOption.IsNullOrEmpty())
                             {
                                 colConfigDto.MultiOption = new List<ColMultiOptionDto>();
                             }

                             if (!colConfigDto.MultiOption.Exists(x => x.Id == colMultiOptionDto.Id))
                             {
                                 colConfigDto.MultiOption.Add(colMultiOptionDto);
                             }
                         }

                         if (!rowConfigDto.ColConfigs.Exists(x => x.Id == colConfigDto.Id))
                         {
                             rowConfigDto.ColConfigs.Add(colConfigDto);
                         }


                         if (!formConfigDto.Linhas.Exists(x => x.Id == rowConfigDto.Id))
                         {
                             formConfigDto.Linhas.Add(rowConfigDto);
                         }

                         if (dicFormConfig.ContainsKey(formConfigDto.Id))
                         {
                             dicFormConfig[formConfigDto.Id] = formConfigDto;
                         }
                         else
                         {
                             dicFormConfig.Add(formConfigDto.Id, formConfigDto);
                         }


                         return formConfigDto;

                     }, new { formConfigId = id, deleted = false }, commandTimeout: 0).ConfigureAwait(false)).FirstOrDefault();

                    if (result == null)
                    {
                        return null;
                    }

                    result.IsProducao = (await connection.QueryFirstAsync<long>(queryCount, new { formConfigId = id, deleted = false }, commandTimeout: 0).ConfigureAwait(false)) > 0;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigDto> Clonar(long id)
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    // var formConfig = await Obter(id);
                    var query = formConfigRepository.Object.GetAll().AsNoTracking().Where(m => m.Id == id).Include(
                        i => i.Linhas.Select(ss => ss.ColConfigs.Select(sss => sss.MultiOption)));

                    // .FirstOrDefaultAsync();
                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    var result = new FormConfig();
                    if (formConfig == null)
                    {
                        return FormConfigDto.Mapear(result);
                    }

                    var linhas = new List<RowConfig>();
                    result = new FormConfig
                    {
                        DataAlteracao = DateTime.Now,
                        Nome = "Clone de " + formConfig.Nome
                    };
                    formConfig.Linhas.ForEach(
                        f =>
                            {
                                var linha = new RowConfig { Ordem = f.Ordem };
                                var cols = new List<ColConfig>();
                                //f.Id = 0;
                                f.ColConfigs.ForEach(
                                    ff =>
                                        {
                                            var col = new ColConfig
                                            {
                                                Colspan = ff.Colspan,
                                                Label = ff.Label,
                                                Name = ff.Name,
                                                Ordem = ff.Ordem,
                                                Placeholder = ff.Placeholder,
                                                Readonly = ff.Readonly,
                                                Type = ff.Type,
                                                Valores = new List<FormData>(),
                                                Value = string.Empty
                                            };
                                            var multiOption = new List<ColMultiOption>();
                                            ff.MultiOption.ForEach(
                                                fff =>
                                                    {
                                                        multiOption.Add(
                                                            new ColMultiOption
                                                            {
                                                                Opcao = fff.Opcao,
                                                                Selecionado = fff.Selecionado
                                                            });
                                                    });
                                            col.MultiOption = multiOption;
                                            cols.Add(col);
                                        });
                                linha.ColConfigs = cols;
                                linhas.Add(linha);
                            });
                    result.Linhas = linhas;

                    return FormConfigDto.Mapear(result);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException("ErroPesquisar");
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    var query = from p in formConfigRepository.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.Codigo.Contains(dropdownInput.search)
                                         || m.Nome.Contains(dropdownInput.search))
                                orderby p.Nome ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Nome) };

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

        public async Task<ResultDropdownList> ListarRelacionadosDropdown(DropdownInput dropdownInput)
        {
            try
            {
                long unidadeOrganizacionalId = 0;

                long operacaoId = 0;

                long.TryParse(dropdownInput.filtro, out unidadeOrganizacionalId);

                long.TryParse(dropdownInput.id, out operacaoId);

                var filtroEspecialidade = dropdownInput.filtros != null && !string.IsNullOrWhiteSpace(dropdownInput.filtros[0]) ? dropdownInput.filtros[0].ToString() : string.Empty;

                var filtroAtendimento = dropdownInput.filtros != null && dropdownInput.filtros.Count() >= 2 && !string.IsNullOrWhiteSpace(dropdownInput.filtros[1]) ? dropdownInput.filtros[1] : string.Empty;

                long? especialidadeId = null;

                if (!filtroEspecialidade.IsNullOrWhiteSpace())
                {
                    especialidadeId = Convert.ToInt64(filtroEspecialidade);
                }

                long atendimentoId = 0;

                if (!filtroAtendimento.IsNullOrWhiteSpace())
                {
                    atendimentoId = Convert.ToInt64(filtroAtendimento);
                }

                //var result3 = long.TryParse(dropdownInput.filtros[0], out especialidadeId);

                var formsConfigQ = await this.ListarRelacionados(operacaoId, unidadeOrganizacionalId, especialidadeId, atendimentoId).ConfigureAwait(false);

                //var formsDist = formsConfigQ; //.ToList();
                var pageInt = int.Parse(dropdownInput.page) - 1;
                var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
                var query = from p in formsConfigQ.AsQueryable().WhereIf(
                                !dropdownInput.search.IsNullOrEmpty(),
                                m => m.Codigo.Contains(dropdownInput.search) || m.Nome.Contains(dropdownInput.search))
                            orderby p.Nome ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Nome) };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var total = query.Count();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<FormConfigDto>> ListarRelacionados(long operacaoId, long unidadeOrganizacionalId, long? especialidadeId, long atendimentoId)
        {
            var queryOperacao = "SELECT IsEspecialidade FROM SisOperacao WHERE @deleted = @deleted and id = @operacaoId;";

            var queryAtendimento = @"
                SELECT AteAtendimento.SisUnidadeOrganizacionalId AS AteAtendimentoUnidadeOrganizacionalId, 
                       AteAtendimento.IsInternacao, AteLeito.SisUnidadeOrganizacionalId AS AteLeitoUnidadeOrganizacionalId 
                FROM AteAtendimento LEFT JOIN AteLeito ON AteAtendimento.AteLeitoId = AteLeito.Id AND AteLeito.IsDeleted = @deleted WHERE AteAtendimento.IsDeleted = @deleted  AND AteAtendimento.Id = @atendimentoId";
            var query = $@"
                {queryOperacao};
                {queryAtendimento};
            ";
            try
            {
                using (var formConfigEspecialidadeAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigEspecialidadeAppService>())
                using (var formConfigUnidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigUnidadeOrganizacionalAppService>())
                using (var formConfigOperacaoAppService = IocManager.Instance.ResolveAsDisposable<FormConfigOperacaoAppService>())
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    using (var queryMultiple = await connection.QueryMultipleAsync(query, new { deleted = false, atendimentoId, operacaoId }, commandTimeout: 0).ConfigureAwait(false))
                    {
                        var operacaoIsEspecialidade = await queryMultiple.ReadFirstOrDefaultAsync<bool?>();
                        var atendimento = await queryMultiple.ReadFirstOrDefaultAsync<(long? AteAtendimentoUnidadeOrganizacionalId, bool IsInternacao, long? AteLeitoUnidadeOrganizacionalId)>();

                        var _unidadeOrganizacionalId = atendimento.IsInternacao ? (atendimento.AteLeitoUnidadeOrganizacionalId ?? 0) : (atendimento.AteAtendimentoUnidadeOrganizacionalId ?? 0);

                        var queryUnidadeOrganizacional = await formConfigUnidadeOrganizacionalAppService.Object.ListarTodosPorUnidadeDapper(_unidadeOrganizacionalId)
                                                        .ConfigureAwait(false);

                        var listaOperacao = await formConfigOperacaoAppService.Object.ListarTodosPorOperacaoDapper(operacaoId).ConfigureAwait(false);

                        listaOperacao = listaOperacao.Where(c => queryUnidadeOrganizacional.Select(x => x.Id).Contains(c.Id)).ToList();

                        var especialidades = new List<FormConfigDto>();

                        if (operacaoIsEspecialidade.HasValue && operacaoIsEspecialidade.Value)
                        {
                            if (especialidadeId.HasValue)
                            {
                                var queryEspecialidade =
                                    await formConfigEspecialidadeAppService.Object.ListarTodos().ConfigureAwait(false);

                                var relacaoEspecialidade = queryEspecialidade.Items
                                    .Where(m => m.EspecialidadeId == especialidadeId)
                                    .Where(ro => listaOperacao.Select(rlo => rlo.Id).Contains(ro.Id))
                                    .Select(m => m.FormConfig).ToList();

                                especialidades = relacaoEspecialidade;
                            }
                        }

                        var formsConfigQ = listaOperacao.ToList();
                        if (operacaoIsEspecialidade.HasValue && operacaoIsEspecialidade.Value)
                        {
                            formsConfigQ.AddRange(especialidades);
                        }

                        // removendo duplicidades, já que o form pode estar vinculado à unidade e à operação simultaneamente
                        return formsConfigQ.Distinct();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Campos reservados
        public async Task<long> CriarReservadoAndGetId()
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    var novoForm = new FormConfig
                    {
                        Linhas = new List<RowConfig>(),
                        Nome = "Campos Reservados",
                        IsProducao = false,
                        Codigo = "COD-RESERVADO",
                        Descricao = "DESC-RESERVADO",
                        Id = 0,
                        DataAlteracao = DateTime.Now,
                        IsSistema = false,
                        IsDeleted = false
                    };
                    return await formConfigRepository.Object.InsertAndGetIdAsync(novoForm).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<long> ObterReservadoId()
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    var query = formConfigRepository.Object.GetAll().AsNoTracking()
                        .Where(m => m.Descricao.Contains("RESERVADO"));

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    if (formConfig == null)
                    {
                        return 0;
                    }

                    return formConfig.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigDto> ListarReservados()
        {
            try
            {
                using (var formConfigRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfig, long>>())
                {
                    var query = formConfigRepository.Object.GetAll().AsNoTracking()
                        .Where(m => m.Descricao.Contains("RESERVADO"));

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    return FormConfigDto.Mapear(formConfig);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public string ObterValorUltimoLancamento(string colConfigName, long? atendimentoId)
        {
            try
            {
                using (var prontuarioRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Prontuario, long>>())
                {
                    var prontuarios = prontuarioRepository.Object.GetAll().Include(f => f.FormResposta.FormConfig)
                        .Include(l => l.FormResposta.FormConfig.Linhas.Select(c => c.ColConfigs))
                        .Where(a => a.AtendimentoId == atendimentoId);

                    var cols = new List<ColConfig>();

                    foreach (var prontuario in prontuarios)
                    {
                        var form = prontuario.FormResposta.FormConfig;

                        foreach (var linha in form.Linhas)
                        {
                            if (linha.ColConfigs == null)
                                continue;

                            foreach (var col in linha.ColConfigs)
                            {
                                if (col.Name == colConfigName)
                                {
                                    cols.Add(col);
                                }
                            }
                        }
                    }

                    var ultimoAlterado = cols.OrderByDescending(item => item.LastModificationTime).FirstOrDefault();

                    return ultimoAlterado.Value;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
