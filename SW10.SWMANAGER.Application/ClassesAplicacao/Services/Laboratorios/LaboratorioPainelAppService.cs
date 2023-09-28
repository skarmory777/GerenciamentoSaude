using Abp.Application.Services.Dto;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Enumeradores;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Internal;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class LaboratorioPainelAppService : SWMANAGERAppServiceBase, ILaboratorioPainelAppService
    {
        public async Task<PagedResultDto<LaboratorioPainelIndexOutput>> RetornaPainelData(LaboratorioPainelIndexInput input)
        {
	        await AjustaUnidadesOrganizacionais(input);
	        
	        switch (input.Tipo)
	        {
		        case "urgente":
		        case "rotina":
			        return await RetornaPainelDataUrgenteRotina(input);
		        case "pendente":
			        return await RetornaPainelDataPendentes(input);
			        break;
		        case "cultura":
			        return await RetornaPainelDataCulturas(input);
			        break;
	        }

	        return new PagedResultDto<LaboratorioPainelIndexOutput>();
        }
        
        public async Task<LaboratorioPainelIndexCounters> RetornaContadores(LaboratorioPainelIndexInput input)
        {
	        var result = new LaboratorioPainelIndexCounters();
	        try
	        {
		        
		        await AjustaUnidadesOrganizacionais(input);
		        await RetornaContadorUrgentes(result, input);
		        await RetornaContadorRotina(result, input);
		        await RetornaContadorPendentes(result, input);
		        await RetornaContadorCulturas(result, input);
		        return result;
	        }
	        catch (Exception e)
	        {
		        
	        }

	        return result;
        }

        public async Task<BuscarPorSolicitacaoDto> BuscarPorSolicitacao(string codigo)
        {
	        var result = new BuscarPorSolicitacaoDto();
	        long id;
	        if (!long.TryParse(codigo, out id))
	        {
		        return null;
	        }
	        
	        using (var resultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
	        using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
	        {
		        var resultado = (await resultadoRepository.Object.FirstOrDefaultAsync(x => x.Nic == id));

		        if (resultado == null || resultado.Id == 0 || !resultado.SolicitacaoExameId.HasValue)
		        {
			        return null;
		        }

		        var exameStatus = new List<long>()
		        {
			        ExameStatusDto.Inicial,
			        ExameStatusDto.EmColeta,
		        };
		        
		        result.HasBaixa = resultado.ResultadoStatusId == ResultadoStatusDto.Inicial || resultado.ResultadoStatusId == ResultadoStatusDto.EmAndamento;
		        result.ResultadoId = resultado.Id;
		        result.SolicitacaoId = resultado.SolicitacaoExameId ?? 0;
		        result.ResultadoExameIds = resultadoExameRepository.Object.GetAll()
			        .Where(x => x.ResultadoId == result.ResultadoId && x.ExameStatusId.HasValue && exameStatus.Contains(x.ExameStatusId.Value))
			        .Select(x => x.Id).ToList();
		        return result;

	        }
        }

        public async Task<PainelVerificaExamesDto> VerificaExamesBaixa(PainelVerificaExamesDto input)
        {
	        using (var resultadoExameRepository =
		        IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
	        {
		        if (!input.ResultadoId.HasValue)
		        {
			        return null;
		        }

		        var resultadoExames = resultadoExameRepository.Object.GetAll().AsNoTracking()
			        .Where(x => x.ResultadoId == input.ResultadoId)
			        .Where(x => x.ExameStatusId == ExameStatusDto.Inicial || x.ExameStatusId == ExameStatusDto.EmColeta)
			        .Where(x => !input.ResultadoExameIds.Contains(x.Id)).Select(x=> x.Id);

		        return new PainelVerificaExamesDto
		        {
			        ResultadoId = input.ResultadoId,
			        ResultadoExameIds = await resultadoExames.ToListAsync()
		        };
	        }
        }

        private async Task AjustaUnidadesOrganizacionais(LaboratorioPainelIndexInput input)
        {
	        using (var conn = new SqlConnection(this.GetConnection()))
	        {
		        var queryUnidadesOrganizacionais = @"
			        SELECT 
			               SisUnidadeOrganizacional.Id  AS Id
				    FROM
			        	SisUnidadeOrganizacional
				    INNER JOIN 
			            AbpOrganizationUnits ON SisUnidadeOrganizacional.SisOrganizationUnitId = AbpOrganizationUnits.Id
			        WHERE 
			            SisUnidadeOrganizacional.IsDeleted = @isDeleted AND AbpOrganizationUnits.IsDeleted =@isDeleted
			        	AND (SisUnidadeOrganizacional.Id IN @UnidadesOrganizacionais OR SisUnidadeOrganizacional.Id IN (
				    SELECT 
				        DISTINCT SisUnidadeOrganizacional.Id AS Id 
			        FROM
				        SisUnidadeOrganizacional
			        INNER JOIN
			        (
				        SELECT 
				        	AbpOrganizationUnits.ParentId 
				        FROM
			        		SisUnidadeOrganizacional
				        LEFT JOIN 
			        	    AbpOrganizationUnits ON SisUnidadeOrganizacional.SisOrganizationUnitId = AbpOrganizationUnits.Id
				        WHERE 
				              SisUnidadeOrganizacional.IsDeleted = @isDeleted AND AbpOrganizationUnits.IsDeleted =@isDeleted
				        	  AND SisUnidadeOrganizacional.Id IN @UnidadesOrganizacionais 
				    ) AS  UnitParents  ON SisUnidadeOrganizacional.SisOrganizationUnitId = UnitParents.ParentId))";

		        input.UnidadesOrganizacionais = (await conn.QueryAsync<long>(queryUnidadesOrganizacionais, new
		        {
			        UnidadesOrganizacionais = input.UnidadesOrganizacionais,
			        isDeleted = false
		        }).ConfigureAwait(false)).ToList();
	        }
        }
        
        private async Task<PagedResultDto<LaboratorioPainelIndexOutput>> RetornaPainelDataUrgenteRotina(LaboratorioPainelIndexInput input)
        {
            try
            {
                var dataTableQuery = this.CreateDataTable<LaboratorioPainelIndexOutput, LaboratorioPainelIndexInput>();

                return await dataTableQuery
                    .AddDefaultField("Solicitacao.Id")
                    .AddOrderByClause("Prioridade.Id ASC, Solicitacao.DataSolicitacao DESC")
                    .AddSelectClause(@"
                            Solicitacao.Id,
							Solicitacao.Codigo,
                            Solicitacao.CreationTime,
                            Solicitacao.CreatorUserId,
                            Solicitacao.DeletionTime,
                            Solicitacao.DeleterUserId,
                            Solicitacao.LastModificationTime,
                            Solicitacao.LastModifierUserId,
							Prioridade.Descricao Prioridade,
							Atendimento.Id AS AtendimentoId,
                            PacientePessoa.Id AS PacienteId,
							PacientePessoa.NomeCompleto AS NomePaciente,
                            ConvenioPessoa.Id AS ConvenioId,
							ConvenioPessoa.NomeFantasia AS ConvenioNome,
							Solicitacao.DataSolicitacao AS DataSolicitacao,
							Atendimento.DataRegistro AS DataAtendimento,
							Atendimento.DataAlta AS DataAlta,
							MedicoPessoa.NomeCompleto AS Solicitante,
                            MedicoPessoa.Id AS SolicitanteId,
							Atendimento.Codigo AS Codigo,
							Leito.Descricao AS LeitoNome,
                            Leito.Id AS LeitoId,
							TipoAcomodacao.Descricao AS TipoLeito,
							(CASE WHEN Atendimento.IsInternacao = 1 THEN
							'Internação'
							ELSE 
							'Ambulatório/Emergêcia'
							END) AS TipoAtendimento, 
							COALESCE(Exames.QtdExames,0) AS QtdExames,
							LabResultado.Nic AS CodigoColeta,
							CASE WHEN LabResultadoStatus.Id IS NULL THEN -1 ELSE LabResultadoStatus.Id END AS ResultadoStatusId,
							CASE WHEN LabResultadoStatus.CorFonte IS NULL THEN '#FFF' ELSE LabResultadoStatus.CorFonte END AS ResultadoStatusCorFonte,
							CASE WHEN LabResultadoStatus.CorFundo IS NULL THEN '#E26A6A' ELSE LabResultadoStatus.CorFundo END AS resultadoStatusCorFundo,
							CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS ResultadoStatusDescricao")
                    .AddFromClause($@"
                        AssSolicitacaoExame Solicitacao LEFT JOIN ( 
							SELECT 
								COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
							FROM AssSolicitacaoExameItem ExameItem
								LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
								LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
							WHERE
								ExameItem.IsDeleted = 0  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
							GROUP BY 
								ExameItem.AssSolicitacaoExameId) 
							AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
							LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId AND Atendimento.IsDeleted = @isDeleted
                            LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id  AND Paciente.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS PacientePessoa ON Paciente.SisPessoaId = PacientePessoa.Id AND PacientePessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisMedico AS Medico ON Solicitacao.SisMedicoSolicitanteId = Medico.Id AND Medico.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS MedicoPessoa ON Medico.SisPessoaId = MedicoPessoa.Id AND MedicoPessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisConvenio AS Convenio ON Atendimento.SisConveniolId = Convenio.Id AND Convenio.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS ConvenioPessoa ON Convenio.SisPessoaId = ConvenioPessoa.Id AND ConvenioPessoa.IsDeleted = @isDeleted
                            LEFT JOIN AteLeito AS Leito ON Atendimento.AteLeitoId = Leito.Id AND Leito.IsDeleted = @isDeleted
                            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON Leito.SisTipoAcomodacaoId = TipoAcomodacao.Id AND TipoAcomodacao.IsDeleted = @isDeleted
							LEFT JOIN AssSolicitacaoExamePrioridade As Prioridade ON Prioridade.Id = Solicitacao.Prioridade
							LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
							LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted")
                    .AddWhereMethod((filtro, dapperParameters) =>
                    {
                        dapperParameters.Add("isDeleted", false);
                        dapperParameters.Add("isTrue", true);
                        dapperParameters.Add("isSistema", false);
                        if (dapperParameters.ContainsKey("StartDate"))
                        {
	                        
                        }
                        // if (!filtro.UnidadesOrganizacionais.IsNullOrEmpty())
                        // {
	                       //  dapperParameters.Add("unidadesOrganizacionais", filtro.UnidadesOrganizacionais.ToArray());
                        // }

                        var where = new StringBuilder(
		                        "AND Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 ")
	                        .WhereIf(filtro.EmpresaId.HasValue, " AND Atendimento.SisEmpresaId = @EmpresaId")
	                        .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue,
		                        " AND Solicitacao.DataSolicitacao BETWEEN @StartDate AND @EndDate ")
	                        .WhereIf(filtro.MedicoId.HasValue, " AND Medico.Id = @MedicoId")
	                        .WhereIf(filtro.ConvenioId.HasValue, " AND Convenio.Id = @ConvenioId")
	                        .WhereIf(filtro.UnidadeId.HasValue,
		                        " AND Atendimento.SisUnidadeOrganizacionalId = @UnidadeId")
	                        .WhereIf(filtro.PacienteId.HasValue, " AND Paciente.Id = @PacienteId")
	                        .WhereIf(filtro.TipoAtendimento == "INT", " AND Atendimento.IsInternacao = 1")
	                        .WhereIf(filtro.TipoAtendimento == "AMB", " AND Atendimento.IsAmbulatorioEmergencia = 1")
	                        .WhereIf(filtro.Tipo == "urgente",
		                        $" AND Prioridade.Id = {SolicitacaoExamePrioridade.Urgente}")
	                        .WhereIf(filtro.Tipo == "rotina",
		                        $" AND Prioridade.Id = {SolicitacaoExamePrioridade.Rotina}")
	                        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
	                        .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
                        return where.ToString();
                    }).ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }
        
        private async Task<PagedResultDto<LaboratorioPainelIndexOutput>> RetornaPainelDataCulturas(LaboratorioPainelIndexInput input)
        {
            try
            {
                var dataTableQuery = this.CreateDataTable<LaboratorioPainelIndexOutput, LaboratorioPainelIndexInput>();

                return await dataTableQuery
                    .AddDefaultField("Solicitacao.Id")
                    .AddOrderByClause("Prioridade.Id ASC, Solicitacao.DataSolicitacao DESC")
                    .AddSelectClause(@"
                            Solicitacao.Id,
							Solicitacao.Codigo,
                            Solicitacao.CreationTime,
                            Solicitacao.CreatorUserId,
                            Solicitacao.DeletionTime,
                            Solicitacao.DeleterUserId,
                            Solicitacao.LastModificationTime,
                            Solicitacao.LastModifierUserId,
							Prioridade.Descricao Prioridade,
							Atendimento.Id AS AtendimentoId,
                            PacientePessoa.Id AS PacienteId,
							PacientePessoa.NomeCompleto AS NomePaciente,
                            ConvenioPessoa.Id AS ConvenioId,
							ConvenioPessoa.NomeFantasia AS ConvenioNome,
							Solicitacao.DataSolicitacao AS DataSolicitacao,
							Atendimento.DataRegistro AS DataAtendimento,
							Atendimento.DataAlta AS DataAlta,
							MedicoPessoa.NomeCompleto AS Solicitante,
                            MedicoPessoa.Id AS SolicitanteId,
							Atendimento.Codigo AS Codigo,
							Leito.Descricao AS LeitoNome,
                            Leito.Id AS LeitoId,
							TipoAcomodacao.Descricao AS TipoLeito,
							(CASE WHEN Atendimento.IsInternacao = 1 THEN
							'Internação'
							ELSE 
							'Ambulatório/Emergêcia'
							END) AS TipoAtendimento, 
							COALESCE(Exames.QtdExames,0) AS QtdExames,
							LabResultado.Nic AS CodigoColeta,
							CASE WHEN LabResultadoStatus.Id IS NULL THEN -1 ELSE LabResultadoStatus.Id END AS ResultadoStatusId,
							CASE WHEN LabResultadoStatus.CorFonte IS NULL THEN '#FFF' ELSE LabResultadoStatus.CorFonte END AS ResultadoStatusCorFonte,
							CASE WHEN LabResultadoStatus.CorFundo IS NULL THEN '#E26A6A' ELSE LabResultadoStatus.CorFundo END AS resultadoStatusCorFundo,
							CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS ResultadoStatusDescricao")
                    .AddFromClause($@"
						AssSolicitacaoExame Solicitacao LEFT JOIN ( 
							SELECT 
								COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
							FROM AssSolicitacaoExameItem ExameItem
								LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
								LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
							WHERE
								ExameItem.IsDeleted = 0  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue)
							GROUP BY 
								ExameItem.AssSolicitacaoExameId) 
							AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id INNER JOIN ( 
							SELECT 
								COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
							FROM AssSolicitacaoExameItem ExameItem
								LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
								LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
							WHERE
								ExameItem.IsDeleted = 0  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) AND FatItem.IsCultura = @isTrue
							GROUP BY 
								ExameItem.AssSolicitacaoExameId) 
							AS ExamesCultura ON ExamesCultura.AssSolicitacaoExameId = Solicitacao.Id
							LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId AND Atendimento.IsDeleted = @isDeleted
                            LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id  AND Paciente.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS PacientePessoa ON Paciente.SisPessoaId = PacientePessoa.Id AND PacientePessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisMedico AS Medico ON Solicitacao.SisMedicoSolicitanteId = Medico.Id AND Medico.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS MedicoPessoa ON Medico.SisPessoaId = MedicoPessoa.Id AND MedicoPessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisConvenio AS Convenio ON Atendimento.SisConveniolId = Convenio.Id AND Convenio.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS ConvenioPessoa ON Convenio.SisPessoaId = ConvenioPessoa.Id AND ConvenioPessoa.IsDeleted = @isDeleted
                            LEFT JOIN AteLeito AS Leito ON Atendimento.AteLeitoId = Leito.Id AND Leito.IsDeleted = @isDeleted
                            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON Leito.SisTipoAcomodacaoId = TipoAcomodacao.Id AND TipoAcomodacao.IsDeleted = @isDeleted
							LEFT JOIN AssSolicitacaoExamePrioridade As Prioridade ON Prioridade.Id = Solicitacao.Prioridade
							LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
							LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted")
                    .AddWhereMethod((filtro, dapperParameters) =>
                    {
                        dapperParameters.Add("isDeleted", false);
                        dapperParameters.Add("isTrue", true);
                        dapperParameters.Add("isSistema", false);
                        // if (!filtro.UnidadesOrganizacionais.IsNullOrEmpty())
                        // {
	                       //  dapperParameters.Add("unidadesOrganizacionais", filtro.UnidadesOrganizacionais.ToArray());
                        // }

                        var where = new StringBuilder("AND Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 ")
                            .WhereIf(filtro.EmpresaId.HasValue, " AND Atendimento.SisEmpresaId = @EmpresaId")
                            .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @StartDate AND @EndDate ")
                            .WhereIf(filtro.MedicoId.HasValue, " AND Medico.Id = @MedicoId")
                            .WhereIf(filtro.ConvenioId.HasValue, " AND Convenio.Id = @ConvenioId")
                            .WhereIf(filtro.UnidadeId.HasValue, " AND Atendimento.SisUnidadeOrganizacionalId = @UnidadeId")
                            .WhereIf(filtro.PacienteId.HasValue, " AND Paciente.Id = @PacienteId")
                            .WhereIf(filtro.TipoAtendimento == "INT", " AND Atendimento.IsInternacao = 1")
                            .WhereIf(filtro.TipoAtendimento == "AMB", " AND Atendimento.IsAmbulatorioEmergencia = 1")
	                        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
                            .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
                        return where.ToString();
                    }).ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }
        
        private async Task<PagedResultDto<LaboratorioPainelIndexOutput>> RetornaPainelDataPendentes(LaboratorioPainelIndexInput input)
        {
            try
            {
                var dataTableQuery = this.CreateDataTable<LaboratorioPainelIndexOutput, LaboratorioPainelIndexInput>();

                return await dataTableQuery
                    .AddDefaultField("Solicitacao.Id")
                    .AddOrderByClause("Prioridade.Id ASC, Solicitacao.DataSolicitacao DESC")
                    .AddSelectClause(@"
                            Solicitacao.Id,
							Solicitacao.Codigo,
                            Solicitacao.CreationTime,
                            Solicitacao.CreatorUserId,
                            Solicitacao.DeletionTime,
                            Solicitacao.DeleterUserId,
                            Solicitacao.LastModificationTime,
                            Solicitacao.LastModifierUserId,
							Prioridade.Descricao Prioridade,
							Atendimento.Id AS AtendimentoId,
                            PacientePessoa.Id AS PacienteId,
							PacientePessoa.NomeCompleto AS NomePaciente,
                            ConvenioPessoa.Id AS ConvenioId,
							ConvenioPessoa.NomeFantasia AS ConvenioNome,
							Solicitacao.DataSolicitacao AS DataSolicitacao,
							Atendimento.DataRegistro AS DataAtendimento,
							Atendimento.DataAlta AS DataAlta,
							MedicoPessoa.NomeCompleto AS Solicitante,
                            MedicoPessoa.Id AS SolicitanteId,
							Atendimento.Codigo AS Codigo,
							Leito.Descricao AS LeitoNome,
                            Leito.Id AS LeitoId,
							TipoAcomodacao.Descricao AS TipoLeito,
							(CASE WHEN Atendimento.IsInternacao = 1 THEN
							'Internação'
							ELSE 
							'Ambulatório/Emergêcia'
							END) AS TipoAtendimento, 
							COALESCE(Exames.QtdExames,0) AS QtdExames,
							LabResultado.Nic AS CodigoColeta,
							CASE WHEN LabResultadoStatus.Id IS NULL THEN -1 ELSE LabResultadoStatus.Id END AS ResultadoStatusId,
							CASE WHEN LabResultadoStatus.CorFonte IS NULL THEN '#FFF' ELSE LabResultadoStatus.CorFonte END AS ResultadoStatusCorFonte,
							CASE WHEN LabResultadoStatus.CorFundo IS NULL THEN '#E26A6A' ELSE LabResultadoStatus.CorFundo END AS resultadoStatusCorFundo,
							CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS ResultadoStatusDescricao")
                    .AddFromClause($@"
                        AssSolicitacaoExame Solicitacao LEFT JOIN ( 
							SELECT 
								COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
							FROM AssSolicitacaoExameItem ExameItem
								LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
								LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
							WHERE
								ExameItem.IsDeleted = 0  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
							GROUP BY 
								ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
							INNER JOIN (
								SELECT COUNT(LabResultadoExame.Id) AS TotalPendencia, LabResultado.SolicitacaoExameId 
								FROM LabResultado INNER JOIN LabResultadoExame 
								ON LabResultadoExame.LabResultadoId = LabResultado.Id
								WHERE LabResultadoExame.IsDeleted = @isDeleted AND LabResultado.IsDeleted = @isDeleted AND LabResultadoExame.IsPendencia = @isTrue
								GROUP BY LabResultado.SolicitacaoExameId
							) AS TotalPendencia ON SolicitacaoExameId = Solicitacao.Id
							LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId AND Atendimento.IsDeleted = @isDeleted
                            LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id  AND Paciente.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS PacientePessoa ON Paciente.SisPessoaId = PacientePessoa.Id AND PacientePessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisMedico AS Medico ON Solicitacao.SisMedicoSolicitanteId = Medico.Id AND Medico.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS MedicoPessoa ON Medico.SisPessoaId = MedicoPessoa.Id AND MedicoPessoa.IsDeleted = @isDeleted
                            LEFT JOIN SisConvenio AS Convenio ON Atendimento.SisConveniolId = Convenio.Id AND Convenio.IsDeleted = @isDeleted
                            LEFT JOIN SisPessoa AS ConvenioPessoa ON Convenio.SisPessoaId = ConvenioPessoa.Id AND ConvenioPessoa.IsDeleted = @isDeleted
                            LEFT JOIN AteLeito AS Leito ON Atendimento.AteLeitoId = Leito.Id AND Leito.IsDeleted = @isDeleted
                            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON Leito.SisTipoAcomodacaoId = TipoAcomodacao.Id AND TipoAcomodacao.IsDeleted = @isDeleted
							LEFT JOIN AssSolicitacaoExamePrioridade As Prioridade ON Prioridade.Id = Solicitacao.Prioridade
							LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
							LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted")
                    .AddWhereMethod((filtro, dapperParameters) =>
                    {
                        dapperParameters.Add("isDeleted", false);
                        dapperParameters.Add("isTrue", true);
                        dapperParameters.Add("isSistema", false);
                        // if (!filtro.UnidadesOrganizacionais.IsNullOrEmpty())
                        // {
	                       //  dapperParameters.Add("unidadesOrganizacionais", filtro.UnidadesOrganizacionais.ToArray());
                        // }

                        var where = new StringBuilder("AND Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 ")
                            .WhereIf(filtro.EmpresaId.HasValue, " AND Atendimento.SisEmpresaId = @EmpresaId")
                            .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @StartDate AND @EndDate ")
                            .WhereIf(filtro.MedicoId.HasValue, " AND Medico.Id = @MedicoId")
                            .WhereIf(filtro.ConvenioId.HasValue, " AND Convenio.Id = @ConvenioId")
                            .WhereIf(filtro.UnidadeId.HasValue, " AND Atendimento.SisUnidadeOrganizacionalId = @UnidadeId")
                            .WhereIf(filtro.PacienteId.HasValue, " AND Paciente.Id = @PacienteId")
                            .WhereIf(filtro.TipoAtendimento == "INT", " AND Atendimento.IsInternacao = 1")
                            .WhereIf(filtro.TipoAtendimento == "AMB", " AND Atendimento.IsAmbulatorioEmergencia = 1")
	                        .WhereIf(filtro.Tipo == "urgente", $" AND Prioridade.Id = {SolicitacaoExamePrioridade.Urgente}")
	                        .WhereIf(filtro.Tipo == "rotina", $" AND Prioridade.Id = {SolicitacaoExamePrioridade.Rotina}")
	                        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
                            .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
                        return where.ToString();
                    }).ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        private async Task RetornaContadorUrgentes(LaboratorioPainelIndexCounters result, LaboratorioPainelIndexInput filtro)
        {
	        using (var conn = new SqlConnection(this.GetConnection()))
	        {
		        var where = new StringBuilder($"Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 AND Solicitacao.Prioridade = @prioridade ")
			        .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @startDate AND @endDate ")
			        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
			        .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
		        var query = @$"
					SELECT 
						CASE 
							WHEN Atendimento.IsAmbulatorioEmergencia = 0 AND Leito.SisUnidadeOrganizacionalId is NOT NULL THEN 
								Leito.SisUnidadeOrganizacionalId
							WHEN Atendimento.IsAmbulatorioEmergencia = 0 AND AtendimentoLeito.SisUnidadeOrganizacionalId IS NOT NULL THEN
								AtendimentoLeito.SisUnidadeOrganizacionalId
							ELSE 
							Atendimento.SisUnidadeOrganizacionalId
							END as UnidadeOrganizacionalId, 
						count(Solicitacao.id) AS Valor
			        FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
				        SELECT 
					        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
				        FROM AssSolicitacaoExameItem ExameItem 
							LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
					        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
				        WHERE
							ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
				        GROUP BY 
				        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
						LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
						LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
						LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
						LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
						LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted
			        WHERE 
						{where.ToString()}
			        GROUP BY 
						CASE 
							WHEN Atendimento.IsAmbulatorioEmergencia = 0 AND Leito.SisUnidadeOrganizacionalId is NOT NULL THEN 
								Leito.SisUnidadeOrganizacionalId
							WHEN Atendimento.IsAmbulatorioEmergencia = 0 AND AtendimentoLeito.SisUnidadeOrganizacionalId IS NOT NULL THEN
								AtendimentoLeito.SisUnidadeOrganizacionalId
							ELSE 
							Atendimento.SisUnidadeOrganizacionalId
							END";
		        var param = new
		        {
			        isDeleted = false,
			        isTrue = true,
			        prioridade = SolicitacaoExamePrioridade.Urgente,
			        startDate = filtro.StartDate,
			        endDate = filtro.EndDate,
			        unidadesOrganizacionais = filtro.UnidadesOrganizacionais,
			        labResultadoStatus = filtro.LabResultadoStatus,
		        };
		        result.UnidadesUrgente = await conn.QueryAsync<LaboratorioPainelIndexCounterUnidade>(query, param);
		        if (!result.UnidadesUrgente.IsNullOrEmpty())
		        {
			        result.UrgenteValor = result.UnidadesUrgente.Sum(x => x.Valor);
		        }

		        var queryStatus = @$"SELECT 
					CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS Status,
					COUNT(CASE WHEN LabResultado.Id IS NULL THEN 1 ELSE LabResultado.Id END) AS Valor
				FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
					SELECT 
						COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
					FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = @isDeleted
						LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = @isDeleted
					WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
					GROUP BY 
					ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultado.LabResultadoStatusId = LabResultadoStatus.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
				WHERE 
					{where.ToString()}
				GROUP BY LabResultadoStatus.Descricao";
		        result.StatusUrgente = await conn.QueryAsync<LaboratorioPainelIndexCounterStatus>(queryStatus, param);
	        }
        }
        private async Task RetornaContadorRotina(LaboratorioPainelIndexCounters result, LaboratorioPainelIndexInput filtro)
        {
	        using (var conn = new SqlConnection(this.GetConnection()))
	        {
		        var where = new StringBuilder($"Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 AND Solicitacao.Prioridade = @prioridade ")
			        .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @startDate AND @endDate ")
			        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
			        .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
		        var query = @$"SELECT Solicitacao.SisUnidadeOrganizacionalId as UnidadeOrganizacionalId, count(Solicitacao.id) AS Valor
		        FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
			        SELECT 
				        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
			        FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
				        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
			        WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
			        GROUP BY 
			        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted
		        WHERE {where.ToString()}
		        
		        GROUP BY Solicitacao.SisUnidadeOrganizacionalId";
		        var param = new
		        {
			        isDeleted = false,
			        isTrue = true,
			        prioridade = SolicitacaoExamePrioridade.Rotina,
			        startDate = filtro.StartDate,
			        endDate = filtro.EndDate,
			        unidadesOrganizacionais = filtro.UnidadesOrganizacionais,
                    labResultadoStatus = filtro.LabResultadoStatus,
		        };
		        result.UnidadesRotina = await conn.QueryAsync<LaboratorioPainelIndexCounterUnidade>(query, param);
		        if (!result.UnidadesRotina.IsNullOrEmpty())
		        {
			        result.RotinaValor = result.UnidadesRotina.Sum(x => x.Valor);
		        }
		        
		        var queryStatus = @$"SELECT 
					CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS Status,
					COUNT(CASE WHEN LabResultado.Id IS NULL THEN 1 ELSE LabResultado.Id END) AS Valor
				FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
					SELECT 
						COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
					FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = @isDeleted
						LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = @isDeleted
					WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
					GROUP BY 
					ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultado.LabResultadoStatusId = LabResultadoStatus.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
				WHERE 
					{where.ToString()}
				GROUP BY LabResultadoStatus.Descricao";
		        result.StatusRotina = await conn.QueryAsync<LaboratorioPainelIndexCounterStatus>(queryStatus, param);
	        }
        } 
        private async Task RetornaContadorPendentes(LaboratorioPainelIndexCounters result, LaboratorioPainelIndexInput filtro)
        {
	        using (var conn = new SqlConnection(this.GetConnection()))
	        {
		        var where = new StringBuilder($"Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0 ")
			        .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @startDate AND @endDate ")
			        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
			        .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
		        var query = @$"SELECT Solicitacao.SisUnidadeOrganizacionalId as UnidadeOrganizacionalId, count(Solicitacao.id) AS Valor
		        FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
			        SELECT 
				        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
			        FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
				        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
			        WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
			        GROUP BY 
			        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					INNER JOIN (SELECT LabResultado.SolicitacaoExameId 
					FROM LabResultado INNER JOIN LabResultadoExame 
					ON LabResultadoExame.LabResultadoId = LabResultado.Id 
					WHERE LabResultadoExame.IsPendencia = @isTrue AND LabResultado.IsDeleted = @isDeleted AND LabResultadoExame.IsDeleted = @isDeleted
					GROUP BY LabResultado.SolicitacaoExameId) AS LabResultadoPendencias ON LabResultadoPendencias.SolicitacaoExameId = Solicitacao.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted
		        WHERE {where.ToString()}
		        GROUP BY Solicitacao.SisUnidadeOrganizacionalId";
		        var param = new
		        {
			        isDeleted = false,
			        isTrue = true,
			        prioridade = SolicitacaoExamePrioridade.Rotina,
			        startDate = filtro.StartDate,
			        endDate = filtro.EndDate,
			        unidadesOrganizacionais = filtro.UnidadesOrganizacionais,
			        labResultadoStatus = filtro.LabResultadoStatus
		        };
		        result.UnidadesPendente = await conn.QueryAsync<LaboratorioPainelIndexCounterUnidade>(query, param);
		        if (!result.UnidadesPendente.IsNullOrEmpty())
		        {
			        result.PendenteValor = result.UnidadesPendente.Sum(x => x.Valor);
		        }
		        
		        var queryStatus = @$"
				SELECT 
					CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS Status,
					COUNT(CASE WHEN LabResultado.Id IS NULL THEN 1 ELSE LabResultado.Id END) AS Valor
		        FROM AssSolicitacaoExame  AS Solicitacao LEFT JOIN ( 
			        SELECT 
				        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
			        FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
				        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
			        WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) 
			        GROUP BY 
			        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					INNER JOIN (SELECT LabResultado.SolicitacaoExameId 
					FROM LabResultado INNER JOIN LabResultadoExame 
					ON LabResultadoExame.LabResultadoId = LabResultado.Id 
					WHERE LabResultadoExame.IsPendencia = @isTrue AND LabResultado.IsDeleted = @isDeleted AND LabResultadoExame.IsDeleted = @isDeleted
					GROUP BY LabResultado.SolicitacaoExameId) AS LabResultadoPendencias ON LabResultadoPendencias.SolicitacaoExameId = Solicitacao.Id
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultado.LabResultadoStatusId = LabResultadoStatus.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
		        WHERE {where.ToString()}
		        GROUP BY LabResultadoStatus.Descricao";
		        result.StatusPendente = await conn.QueryAsync<LaboratorioPainelIndexCounterStatus>(queryStatus, param);
	        }
        }
        
        private async Task RetornaContadorCulturas(LaboratorioPainelIndexCounters result, LaboratorioPainelIndexInput filtro)
        {
	        using (var conn = new SqlConnection(this.GetConnection()))
	        {
		        var where = new StringBuilder($"Solicitacao.IsDeleted = @isDeleted AND COALESCE(Exames.QtdExames,0) > 0")
			        .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND Solicitacao.DataSolicitacao BETWEEN @startDate AND @endDate ")
			        .WhereIf(!filtro.UnidadesOrganizacionais.IsNullOrEmpty(), @" 
								AND (Leito.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais OR Atendimento.SisUnidadeOrganizacionalId IN @unidadesOrganizacionais)")
			        .WhereIf(!filtro.LabResultadoStatus.IsNullOrEmpty(), @"
							AND CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END IN @labResultadoStatus");
		        var query = @$"SELECT Solicitacao.SisUnidadeOrganizacionalId as UnidadeOrganizacionalId, count(Solicitacao.id) AS Valor
		        FROM AssSolicitacaoExame  AS Solicitacao INNER JOIN ( 
			        SELECT 
				        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
			        FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
				        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
			        WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) AND FatItem.IsCultura = @isTrue
			        GROUP BY 
			        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultadoStatus.Id = LabResultado.LabResultadoStatusId AND LabResultadoStatus.IsDeleted = @isDeleted
		        WHERE {where.ToString()}
		        
		        GROUP BY Solicitacao.SisUnidadeOrganizacionalId";
		        var param = new
		        {
			        isDeleted = false,
			        isTrue = true,
			        startDate = filtro.StartDate,
			        endDate = filtro.EndDate,
			        unidadesOrganizacionais = filtro.UnidadesOrganizacionais,
			        labResultadoStatus = filtro.LabResultadoStatus
		        };
		        result.UnidadesCultura = await conn.QueryAsync<LaboratorioPainelIndexCounterUnidade>(query, param);
		        if (!result.UnidadesCultura.IsNullOrEmpty())
		        {
			        result.CulturaValor = result.UnidadesCultura.Sum(x => x.Valor);
		        }
		        
		        var queryStatus = @$"SELECT CASE WHEN LabResultadoStatus.Descricao IS NULL THEN 'Sem Coleta' ELSE LabResultadoStatus.Descricao END AS Status,
					COUNT(CASE WHEN LabResultado.Id IS NULL THEN 1 ELSE LabResultado.Id END) AS Valor
		        FROM AssSolicitacaoExame  AS Solicitacao INNER JOIN ( 
			        SELECT 
				        COUNT(ExameItem.AssSolicitacaoExameId) AS QtdExames, AssSolicitacaoExameId 
			        FROM AssSolicitacaoExameItem ExameItem 
						LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = 0
				        LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = 0
			        WHERE
						ExameItem.IsDeleted = @isDeleted  AND (FatItem.IsLaboratorio = @isTrue OR FatGrupo.IsLaboratorio = @isTrue) AND FatItem.IsCultura = @isTrue
			        GROUP BY 
			        ExameItem.AssSolicitacaoExameId) AS Exames ON Exames.AssSolicitacaoExameId = Solicitacao.Id
					LEFT JOIN LabResultado ON LabResultado.SolicitacaoExameId = Solicitacao.Id AND LabResultado.IsDeleted = @isDeleted
					LEFT JOIN LabResultadoStatus ON LabResultado.LabResultadoStatusId = LabResultadoStatus.Id
					LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Solicitacao.AtendimentoId
					LEFT JOIN AteLeito AS AtendimentoLeito ON AtendimentoLeito.Id = Atendimento.AteLeitoId
					LEFT JOIN AteLeito AS Leito ON Leito.Id = Solicitacao.SisLeitoId
		        WHERE {where.ToString()}
		        GROUP BY LabResultadoStatus.Descricao";
		        result.StatusCultura = await conn.QueryAsync<LaboratorioPainelIndexCounterStatus>(queryStatus, param);
	        }
        }


    }
}
