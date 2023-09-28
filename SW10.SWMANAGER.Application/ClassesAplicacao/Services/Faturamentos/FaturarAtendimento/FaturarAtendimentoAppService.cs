using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Extensions;
using Abp.UI;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento
{
    public class FaturarAtendimentoAppService: SWMANAGERAppServiceBase, IFaturarAtendimentoAppService
    {
        public async Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamento(FaturarAtendimentoInputDto input)
        {
            const string DefaultField = "AteAtendimento.Id";
            
            const string SelectClause = @" 
                Distinct
                AteAtendimento.Id,
                AteAtendimentoStatus.Descricao AS AteStatus,
                AteAtendimentoStatus.CorFundo AS AteCorFundo,
                AteAtendimentoStatus.CorTexto AS AteCorTexto,
                FatAtendimentoStatus.Descricao AS FatStatus,
                FatAtendimentoStatus.Cor AS FatCor,
                AteAtendimento.Codigo AS CodigoAtendimento,
                AteAtendimento.DataRegistro,
                AteAtendimento.DataAlta,
                SisPaciente.Codigo AS CodigoPaciente,
                SisPessoa.NomeCompleto,
                SisConvenio.NomeFantasia AS Convenio,
                FatContaAtendimento.Id AS FatContaAtendimentoId,
                FatContaAtendimento.DataInicio AS DataInicio,
                CASE WHEN FatContaAtendimento.FatContaStatusId = 1 THEN 1
                ELSE 0
                END AS IsParcial";
            
            const string FromClause = @"AteAtendimento 
                LEFT JOIN AteAtendimentoStatus ON AteAtendimentoStatus.Id = AteAtendimento.AteAtendimentoStatusId AND AteAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN FatAtendimentoStatus ON FatAtendimentoStatus.Id = AteAtendimento.FatAtendimentoStatusId AND FatAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN SisPaciente ON SisPaciente.Id = AteAtendimento.SisPacienteId AND SisPaciente.IsDeleted = @deleted
                LEFT JOIN SisPessoa ON SisPessoa.Id = SisPaciente.SisPessoaId AND SisPessoa.IsDeleted = @deleted
                LEFT JOIN SisConvenio ON SisConvenio.Id = AteAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @deleted
                LEFT JOIN (
                    SELECT FatConta.Id, FatContaAtendimentoMax.DataInicio, FatConta.SisAtendimentoId, FatContaStatusId FROM FatConta INNER JOIN (
                        SELECT MAX(DataInicio) AS DataInicio, SisAtendimentoId FROM FatConta 
                        WHERE FatConta.IsDeleted = @deleted AND FatConta.IsAtivo = 1
                        GROUP BY SisAtendimentoId
                    ) FatContaAtendimentoMax ON FatContaAtendimentoMax.DataInicio = FatConta.DataInicio AND FatConta.SisAtendimentoId = FatContaAtendimentoMax.SisAtendimentoId AND FatConta.IsAtivo = 1
                ) AS FatContaAtendimento ON FatContaAtendimento.SisAtendimentoId = AteAtendimento.Id";
            const string WhereClause = "(FatAtendimentoStatusId != @statusFaturado OR FatAtendimentoStatusId IS NULL) AND AteAtendimento.IsDeleted = @deleted ";
            
            
            return await this.CreateDataTable<FaturarAtendimentoIndexDto, FaturarAtendimentoInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod(ExecutaFiltroFaturamento)
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamentoAuditoriaInterna(FaturarAtendimentoInputDto input)
        {
            const string DefaultField = "AteAtendimento.Id";

            const string SelectClause = @" 
                Distinct
                AteAtendimento.Id,
                AteAtendimentoStatus.Descricao AS AteStatus,
                AteAtendimentoStatus.CorFundo AS AteCorFundo,
                AteAtendimentoStatus.CorTexto AS AteCorTexto,
                FatAtendimentoStatus.Descricao AS FatStatus,
                FatAtendimentoStatus.Cor AS FatCor,
                AteAtendimento.Codigo AS CodigoAtendimento,
                AteAtendimento.DataRegistro,
                AteAtendimento.DataAlta,
                SisPaciente.Codigo AS CodigoPaciente,
                SisPessoa.NomeCompleto,
                SisConvenio.NomeFantasia AS Convenio,
                FatContaAtendimento.Id AS FatContaAtendimentoId,
                FatContaAtendimento.DataInicio AS DataInicio,
                CASE WHEN FatContaAtendimento.FatContaStatusId = 1 THEN 1
                ELSE 0
                END AS IsParcial";

            const string FromClause = @"AteAtendimento 
                LEFT JOIN AteAtendimentoStatus ON AteAtendimentoStatus.Id = AteAtendimento.AteAtendimentoStatusId AND AteAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN FatAtendimentoStatus ON FatAtendimentoStatus.Id = AteAtendimento.FatAtendimentoStatusId AND FatAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN SisPaciente ON SisPaciente.Id = AteAtendimento.SisPacienteId AND SisPaciente.IsDeleted = @deleted
                LEFT JOIN SisPessoa ON SisPessoa.Id = SisPaciente.SisPessoaId AND SisPessoa.IsDeleted = @deleted
                LEFT JOIN SisConvenio ON SisConvenio.Id = AteAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @deleted
                LEFT JOIN (
                    SELECT FatConta.Id,FatContaAtendimentoMax.DataInicio, FatConta.SisAtendimentoId, FatContaStatusId FROM FatConta INNER JOIN (
                        SELECT MAX(DataInicio) AS DataInicio, SisAtendimentoId FROM FatConta 
                        WHERE FatConta.IsDeleted = @deleted AND FatConta.IsAtivo = 1
                        GROUP BY SisAtendimentoId
                    ) FatContaAtendimentoMax ON FatContaAtendimentoMax.DataInicio = FatConta.DataInicio AND FatConta.SisAtendimentoId = FatContaAtendimentoMax.SisAtendimentoId AND FatConta.IsAtivo = 1
                ) AS FatContaAtendimento ON FatContaAtendimento.SisAtendimentoId = AteAtendimento.Id";
            const string WhereClause = "(FatAtendimentoStatusId != @statusFaturado OR FatAtendimentoStatusId IS NULL) AND AteAtendimento.IsDeleted = @deleted AND FatContaAtendimento.FatContaStatusId = @fatContaStatusId ";


            return await this.CreateDataTable<FaturarAtendimentoIndexDto, FaturarAtendimentoInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod(ExecutaFiltroFaturamentoAuditoriaInterna)
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamentoAuditoriaExterna(FaturarAtendimentoInputDto input)
        {
            const string DefaultField = "AteAtendimento.Id";

            const string SelectClause = @" 
                Distinct
                AteAtendimento.Id,
                AteAtendimentoStatus.Descricao AS AteStatus,
                AteAtendimentoStatus.CorFundo AS AteCorFundo,
                AteAtendimentoStatus.CorTexto AS AteCorTexto,
                FatAtendimentoStatus.Descricao AS FatStatus,
                FatAtendimentoStatus.Cor AS FatCor,
                AteAtendimento.Codigo AS CodigoAtendimento,
                AteAtendimento.DataRegistro,
                AteAtendimento.DataAlta,
                SisPaciente.Codigo AS CodigoPaciente,
                SisPessoa.NomeCompleto,
                SisConvenio.NomeFantasia AS Convenio,
                FatContaAtendimento.DataInicio AS DataInicio,
                FatContaAtendimento.Id AS FatContaAtendimentoId,
                CASE WHEN FatContaAtendimento.FatContaStatusId = 1 THEN 1
                ELSE 0
                END AS IsParcial";

            const string FromClause = @"AteAtendimento 
                LEFT JOIN AteAtendimentoStatus ON AteAtendimentoStatus.Id = AteAtendimento.AteAtendimentoStatusId AND AteAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN FatAtendimentoStatus ON FatAtendimentoStatus.Id = AteAtendimento.FatAtendimentoStatusId AND FatAtendimentoStatus.IsDeleted = @deleted
                LEFT JOIN SisPaciente ON SisPaciente.Id = AteAtendimento.SisPacienteId AND SisPaciente.IsDeleted = @deleted
                LEFT JOIN SisPessoa ON SisPessoa.Id = SisPaciente.SisPessoaId AND SisPessoa.IsDeleted = @deleted
                LEFT JOIN SisConvenio ON SisConvenio.Id = AteAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @deleted
                LEFT JOIN (
                    SELECT FatConta.Id, FatContaAtendimentoMax.DataInicio, FatConta.SisAtendimentoId, FatContaStatusId FROM FatConta INNER JOIN (
                        SELECT MAX(DataInicio) AS DataInicio, SisAtendimentoId FROM FatConta 
                        WHERE FatConta.IsDeleted = @deleted AND FatConta.IsAtivo = 1
                        GROUP BY SisAtendimentoId
                    ) FatContaAtendimentoMax ON FatContaAtendimentoMax.DataInicio = FatConta.DataInicio AND FatConta.SisAtendimentoId = FatContaAtendimentoMax.SisAtendimentoId AND FatConta.IsAtivo = 1
                ) AS FatContaAtendimento ON FatContaAtendimento.SisAtendimentoId = AteAtendimento.Id";
            const string WhereClause = "(FatAtendimentoStatusId != @statusFaturado OR FatAtendimentoStatusId IS NULL) AND AteAtendimento.IsDeleted = @deleted AND FatContaAtendimento.FatContaStatusId = @fatContaStatusId ";


            return await this.CreateDataTable<FaturarAtendimentoIndexDto, FaturarAtendimentoInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod(ExecutaFiltroFaturamentoAuditoriaExterna)
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }

        private static string ExecutaFiltroFaturamentoAuditoriaInterna(FaturarAtendimentoInputDto dto, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("fatContaStatusId", FaturamentoContaStatusDto.AuditoriaInterna);

            return ExecutaFiltroFaturamento(dto, dapperParameters);
        }

        private static string ExecutaFiltroFaturamentoAuditoriaExterna(FaturarAtendimentoInputDto dto, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("fatContaStatusId", FaturamentoContaStatusDto.AuditoriaExterna);

            return ExecutaFiltroFaturamento(dto, dapperParameters);
        }

        private static string ExecutaFiltroFaturamento(FaturarAtendimentoInputDto dto, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("statusFaturado", FaturamentoAtendimentoStatus.Finalizado);
            dapperParameters.Add("deleted", false);
            var where = new StringBuilder();

            where.WhereIf(dto.ConvenioId.HasValue, " AND SisConvenio.Id = @ConvenioId ");
            where.WhereIf(dto.EmpresaId.HasValue, " AND AteAtendimento.SisEmpresaId = @EmpresaId ");
            where.WhereIf(dto.PacienteId.HasValue, " AND SisPaciente.Id = @PacienteId ");
            where.WhereIf(dto.TipoInternacao.HasValue, " AND AteAtendimento.IsAmbulatorioEmergencia = @TipoInternacao ");
            var today = DateTime.Today.AddDays(1).AddSeconds(-1);
            dapperParameters.Add("today",today);
            switch (dto.Periodo)
            {
                case "1mes":
                {
                    var inicial = today.AddMonths(-1).Date;
                    dapperParameters.Add("inicial",inicial);
                    where.Append(" AND AteAtendimento.DataRegistro BETWEEN @inicial AND @today ");
                    break;
                }
                case "1semana":
                {
                    var inicial = today.AddDays(-7).Date;
                    dapperParameters.Add("inicial",inicial);
                    where.Append(" AND AteAtendimento.DataRegistro BETWEEN @inicial AND @today ");
                    break;
                }
                case "24hrs":
                {
                    var inicial = today.AddHours(-24).Date;
                    dapperParameters.Add("inicial",inicial);
                    where.Append(" AND AteAtendimento.DataRegistro BETWEEN @inicial AND @today ");
                    break;
                }
                case "customizado":
                {
                    where.WhereIf(dto.StartDate.HasValue && dto.EndDate.HasValue, " AND AteAtendimento.DataRegistro BETWEEN @StartDate AND @EndDate ");
                    break;
                }
                default:
                {
                    break;
                }
            }
            
            return where.ToString();
        }


        public async Task<PagedResultDto<FaturarAtendimentoContaMedicaIndexDto>> ListarContaMedica(ListarContaMedicaInputDto input)
        {
            const string DefaultField = "FatConta.Id";

            const string SelectClause = @"
                FatConta.Id,
                FatConta.Codigo,
                FatContaStatus.Descricao AS FatContaStatus,
                FatContaStatus.Cor AS FatContaStatusCor,
                FatConta.MotivoPendencia,
                FatConta.Observacao,
                FatConta.DataInicio AS DataInicio,
                FatConta.DataFim,
                SisConvenio.NomeFantasia AS Convenio,
                SisPlano.Descricao AS Plano,
                DATEDIFF(day, FatConta.DataInicio, FatConta.DataFim) AS DiasFaturados";
            const string FromClause = @"FatConta
            INNER JOIN FatContaStatus ON FatConta.FatContaStatusId = FatContaStatus.Id
            LEFT JOIN SisConvenio ON FatConta.SisConvenioId = SisConvenio.Id
            LEFT JOIN SisPlano ON FatConta.PlanoId = SisPlano.Id";

            const string WhereClause = @"FatConta.IsDeleted = @isDeleted AND  FatConta.SisAtendimentoId = @atendimentoId AND FatConta.IsAtivo = @isAtivo";
            
            return await this.CreateDataTable<FaturarAtendimentoContaMedicaIndexDto, ListarContaMedicaInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod((inputWhere, dapperParamaters) =>
                {
                    dapperParamaters.Add("isDeleted", false);
                    dapperParamaters.Add("isAtivo", true);
                    return "";
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }
        
        public async Task<PagedResultDto<FaturarItemAtendimentoIndexDto>> ListarFaturamentoItem(ListarFaturamentoItemInputDto input)
        {
            const string DefaultField = "FatItemAtendimento.Id";
            
            const string SelectClause = @" 
                FatItemAtendimento.Id,
                FatItemAtendimento.Data,
                Medico.NomeCompleto AS MedicoNomeCompleto,
                FatItemAtendimento.Quantidade,
                FatItemAtendimento.Entidade,
                FatItemAtendimento.EntidadeId";
            
            const string FromClause = @"FatItemAtendimento
                LEFT JOIN SisMedico ON FatItemAtendimento.SisMedicoId = SisMedico.Id
                LEFT JOIN SisPessoa AS Medico ON SisMedico.SisPessoaId = Medico.Id";
            const string WhereClause = "FatItemAtendimento.AteAtendimentoId = @atendimentoId AND FatItemAtendimento.IsDeleted = @isDeleted";
            
            var table = await this.CreateDataTable<FaturarItemAtendimentoIndexDto, ListarFaturamentoItemInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod((inputWhere, daperParamters) =>
                {
                    daperParamters.Add("isDeleted", false);
                    return "";
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);

            if (table.Items.All(x => x.Entidade.IsNullOrEmpty()))
            {
                return table;
            }
            var prescricaoItemResposta = table.Items.Where(x => x.Entidade == "AssPrescricaoItemResposta" && x.EntidadeId.HasValue);
            if (!prescricaoItemResposta.Any())
            {
                return table;
            }
            
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                const string query = @"SELECT DISTINCT 
                        AssPrescricaoItem.Descricao AS AssPrescricaoItemDescricao, 
                        AssPrescricaoItem.FatItemId, 
                        FatItem.Descricao AS FatItemDescricao,
                        AssPrescricaoItemResposta.Id AS AssPrescricaoItemRespostaId
                    FROM AssPrescricaoItemResposta
                    LEFT JOIN AssPrescricaoItem ON AssPrescricaoItemResposta.AssPrescricaoItemId = AssPrescricaoItem.Id
                    LEFT JOIN FatItem ON FatItem.Id = AssPrescricaoItem.FatItemId
                    WHERE AssPrescricaoItemResposta.Id IN @AssPrescricaoItemRespostaIds";
                var itemRepostas = await conn.QueryAsync<FaturarItemAtendimentoAssPrescricaoItemResposta>(query, new
                {
                    AssPrescricaoItemRespostaIds = prescricaoItemResposta.Select(x => x.EntidadeId).ToList()
                });

                foreach (var itemReposta in itemRepostas)
                {
                    var item = prescricaoItemResposta.FirstOrDefault(x => x.EntidadeId == itemReposta.AssPrescricaoItemRespostaId);
                    if (item == null) continue;
                    item.Descricao = !itemReposta.FatItemDescricao.IsNullOrEmpty() ? itemReposta.FatItemDescricao : itemReposta.AssPrescricaoItemDescricao;
                    item.Tipo = "Prescrição";
                    item.FatItemId = itemReposta.FatItemId;
                }
            }

            return table;
        }


        
    }
}