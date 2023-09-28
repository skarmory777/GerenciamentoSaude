using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;

using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Input;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    using Abp.Dependency;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Dto;
    using System.Data.SqlClient;
    using System.Text;

    public class EvolucaoResultadosAppService : SWMANAGERAppServiceBase, IEvolucaoResultadosAppService
    {

        public async Task<PagedResultDto<IndexPacientesOutputDto>> ListarNaoConferido(ListarPacientesInput input)
        {
            if (input.Sorting == "Codigo ASC")
                input.Sorting = "CodigoPaciente ASC";
            else if (input.Sorting == "Codigo DESC")
                input.Sorting = "CodigoPaciente DESC";
            if (input.Sorting == "Paciente ASC")
                input.Sorting = "NomeCompleto ASC";
            else if (input.Sorting == "Paciente DESC")
                input.Sorting = "NomeCompleto DESC";

            try
            {
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var query = (from p in pacienteRepository.Object.GetAll()
                                 join a in atendimentoRepository.Object.GetAll() on p.Id equals a.PacienteId
                                 select new
                                 {
                                     p.Id,
                                     atendimentoId = a.Id,
                                     p.CodigoPaciente,
                                     p.NomeCompleto,
                                     a.PacienteId
                                 })
                                 .WhereIf(
                                    !input.Filtro.IsNullOrEmpty(),
                                    m => m.NomeCompleto.Contains(input.Filtro)
                                 ).GroupBy(g => g.Id).Select(s => s.FirstOrDefault());


                    var contar = await query.CountAsync();

                    var resultados = await query
                                            .OrderBy(input.Sorting)
                                            .PageBy(input)
                                            .Select(x => new IndexPacientesOutputDto() {
                                                Id = x.Id,
                                                CodigoPaciente = x.CodigoPaciente,
                                                NomeCompleto = x.NomeCompleto,
                                                atendId = x.atendimentoId,
                                                PacienteId = x.PacienteId
                                            })
                                            .ToListAsync();
                    
                    return new PagedResultDto<IndexPacientesOutputDto>(contar, resultados);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ResultadoLaudoDto>> Obter(EvolucaoResultadoInput input)
        {
            try
            {
                using (var resultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
                using (var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var resultadoLaudoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoLaudo, long>>())
                using (var itemResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ItemResultado, long>>())
                using (var tipoResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResultado, long>>())
                {
                    var atendimentoIds = atendimentoRepository.Object.GetAll().Where(a => a.PacienteId == input.Id).Select(s => s.Id).ToList<long>();



                    if (!input.DateStart.HasValue || !input.DateEnd.HasValue)
                    {
                        return null;
                    }

                    var contar = 0;
                    List<ResultadoLaudo> lst = new List<ResultadoLaudo>();
                    List<ResultadoLaudoDto> lstDto = new List<ResultadoLaudoDto>();

                    //var queryResultado = _resultadoRepositorio.GetAll();
                    //var queryResultadoExame = _resultadoExameRepositorio.GetAll();
                    //var queryTabela = _tabelaRepository.GetAll();
                    //var queryResultadoLaudo = _resultadoLaudoRepository.GetAll();
                    //var queryTabelaResultado = _tabelaResultadoRepository.GetAll();
                    //var queryTipoResultado = _tipoResultadoRepository.GetAll();

                    if (atendimentoIds.Count > 0)
                    {
                        var query = (from resultado in resultadoRepository.Object.GetAll()
                                     join atendimento in atendimentoRepository.Object.GetAll() on resultado.AtendimentoId equals atendimento.Id
                                     join resultadoExame in resultadoExameRepository.Object.GetAll() on resultado.Id equals resultadoExame.ResultadoId
                                     join itemFaturamento in fatItemRepository.Object.GetAll() on resultadoExame.FaturamentoItemId equals itemFaturamento.Id
                                     //join tabela in _tabelaRepository.GetAll() on resultadoExame.TabelaId equals tabela.Id
                                     join resultadoLaudo in resultadoLaudoRepository.Object.GetAll() on resultadoExame.Id equals resultadoLaudo.ResultadoExameId
                                     join itemResultado in itemResultadoRepository.Object.GetAll() on resultadoLaudo.ItemResultadoId equals itemResultado.Id
                                     //join tabelaResultado in queryTabelaResultado on resultadoLaudo.TabelaResultadoId equals tabelaResultado.Id
                                     join tipoResultado in tipoResultadoRepository.Object.GetAll() on resultadoLaudo.TipoResultadoId equals tipoResultado.Id
                                     where input.IsDesseAtendimento ? resultado.AtendimentoId == input.AtendimentoId : atendimentoIds.Contains(atendimento.Id) &&
                                     (resultadoLaudo.Resultado != null && resultadoLaudo.Resultado != "")
                                     //||
                                     //atendimento.IsAmbulatorioEmergencia.Equals(input.IsAmbulatorioEmergencia) ||
                                     //atendimento.IsInternacao.Equals(input.IsInternacao) 
                                     select new
                                     {
                                         resultadoLaudo.Resultado,
                                         itemResultado.Referencia,
                                         resultadoLaudo.Ordem,
                                         ItemDescricao = itemFaturamento.Descricao,
                                         ItemInfo = itemResultado.Descricao,
                                         itemResultado.TipoResultadoId,
                                         resultado.DataColeta,
                                         HasAmbulatorioEmergencia = atendimento.IsAmbulatorioEmergencia,
                                         HasInternacao = atendimento.IsInternacao
                                     }).WhereIf(
                                          !input.Filtro.IsNullOrEmpty(),
                                          m => m.ItemInfo.Contains(input.Filtro) ||
                                          m.Referencia.Contains(input.Filtro) || m.ItemDescricao
                                              .Contains(input.Filtro)).Where(
                            m => (m.DataColeta >= input.DateStart && m.DataColeta <= input.DateEnd) && (m.HasAmbulatorioEmergencia.Equals(input.IsAmbulatorioEmergencia) || m
                                                         .HasInternacao.Equals(input.IsInternacao)) && m.Resultado != null).AsNoTracking();

                        contar = await query.CountAsync().ConfigureAwait(false);

                        var items = await query
                                             .OrderBy(input.Sorting)
                                             .PageBy(input)
                                             .Select(item => new ResultadoLaudoDto
                                             {
                                                 Resultado = item.Resultado,
                                                 Referencia = item.Referencia,
                                                 Ordem = item.Ordem,
                                                 ItemDescricao = item.ItemDescricao,
                                                 ItemInfo = item.ItemInfo,
                                                 TipoResultadoId = item.TipoResultadoId,
                                                 DataColeta = item.DataColeta,
                                                 IsAmbulatorioEmergencia = item.HasAmbulatorioEmergencia,
                                                 IsInternacao = item.HasInternacao
                                             })
                                             .ToListAsync().ConfigureAwait(false);

                    }

                    return new PagedResultDto<ResultadoLaudoDto>(contar, lstDto);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroInserir", ex);
            }
        }

        public async Task<ListaEvolucaoResultado> ListaEvolucaoResultado(EvolucaoResultadoInput input)
        {
            
            try
            {
                return new ListaEvolucaoResultado
                {
                    Coletas = await ListaEvolucaoResultadoPorTipo(input),
                    Culturas = await ListaEvolucaoResultadoPorTipo(input, true)
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroPesquisar", ex);
            }
        }

        private async Task<IEnumerable<EvolucaoResultadoDto>> ListaEvolucaoResultadoPorTipo(EvolucaoResultadoInput input, bool isCultura = false)
        {
            if (!input.DateStart.HasValue || !input.DateEnd.HasValue)
            {
                return null;
            }

            var query = @"SELECT 
                    DISTINCT
                    LabResultado.Id AS itemResultadoId,
                    FatItem.OrdemMapaResultado,
                    LabFormataItem.OrdemRegistro AS FormataOrdem,
                    FatItem.Descricao AS ItemDescricao,
                    LabItemResultado.Descricao AS ItemInfo,
                    LabItemResultado.MinimoAceitavelMasculino,
                    LabItemResultado.MinimoMasculino,
                    LabItemResultado.MinimoAceitavelFeminino,
                    LabItemResultado.MinimoFeminino,
                    LabItemResultado.MaximoAceitavelMasculino,
                    LabItemResultado.MaximoMasculino,
                    LabItemResultado.MaximoAceitavelFeminino,
                    LabItemResultado.MaximoFeminino,
                    LabResultadoLaudo.Resultado,
                    LabResultadoLaudo.Referencia,
                    LabResultadoLaudo.Ordem,
                    LabItemResultado.TipoResultadoId,
                    LabResultado.DataColeta,
                    AteAtendimento.IsAmbulatorioEmergencia AS HasAmbulatorioEmergencia,
                    AteAtendimento.IsInternacao AS HasInternacao,
                    Paciente.SexoId AS SexoId
                    FROM
                    LabResultado 
                    INNER JOIN AteAtendimento ON LabResultado.AteAtendimentoId = AteAtendimento.Id AND AteAtendimento.IsDeleted = @IsDeleted
                    INNER JOIN SisPaciente AS Paciente ON AteAtendimento.SisPacienteId = Paciente.Id AND Paciente.IsDeleted = @IsDeleted
                    LEFT JOIN LabResultadoExame ON LabResultadoExame.LabResultadoId = LabResultado.Id  AND LabResultadoExame.IsDeleted = @IsDeleted
                    LEFT JOIN LabResultadoLaudo ON LabResultadoLaudo.ResultadoExameId = LabResultadoExame.Id  AND LabResultadoLaudo.IsDeleted = @IsDeleted
                    INNER JOIN FatItem ON LabResultadoExame.LabFaturamentoItemId = FatItem.Id  AND LabResultadoExame.IsDeleted = @IsDeleted
                    LEFT JOIN LabItemResultado ON LabResultadoLaudo.ItemResultadoId = LabItemResultado.Id  AND LabResultadoLaudo.IsDeleted = @IsDeleted
                    LEFT JOIN LabFormataItem ON LabFormataItem.ItemResultadoId = LabResultadoLaudo.ItemResultadoId AND LabFormataItem.FormataId = FatItem.FormataId  AND LabFormataItem.IsDeleted = @IsDeleted";
            
            var queryWhere = new StringBuilder("LabResultadoLaudo.Resultado IS NOT NULL AND LabResultado.IsDeleted = @IsDeleted AND FatItem.IsCultura = @IsCultura");

            if (input.IsDesseAtendimento)
            {
                queryWhere.Append(" AND AteAtendimento.Id = @AtendimentoId");
            }
            else
            {
                queryWhere.Append(" AND AteAtendimento.SisPacienteId = @PacienteId");
            }

            if (!input.Filtro.IsNullOrEmpty())
            {
                queryWhere.Append(@" AND  (LabItemResultado.Descricao like '%'+@Filtro+'%' OR LabResultadoLaudo.Referencia like '%'+@Filtro+'%' OR FatItem.Descricao like '%'+@Filtro+'%')");
            }
            

            queryWhere.Append(@" AND ( AteAtendimento.IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia OR AteAtendimento.IsInternacao = @IsInternacao)  AND  LabResultado.DataColeta BETWEEN @DateStart AND @DateEnd ");

            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var items = await connection.QueryAsync<ResultadoLaudoDto>($"{query} WHERE {queryWhere} ORDER BY FatItem.OrdemMapaResultado, LabFormataItem.OrdemRegistro",
                    new
                    {
                        input.AtendimentoId,
                        input.Filtro,
                        input.IsAmbulatorioEmergencia,
                        input.IsInternacao,
                        input.PacienteId,
                        input.DateStart,
                        input.DateEnd,
                        IsDeleted = false,
                        IsCultura = isCultura
                    },
                    null,
                    0).ConfigureAwait(false);


                if (items != null && !items.Any())
                {
                    return new List<EvolucaoResultadoDto>();
                }

                var lstDto = new List<EvolucaoResultadoDto>();
                foreach (var grupo in items.GroupBy(x => new { x.ItemDescricao }))
                {
                    foreach (var item in grupo.GroupBy(x => new { x.ItemInfo }))
                    {
                        var firstItem = item.FirstOrDefault();
                        if (firstItem == null)
                        {
                            continue;
                        }

                        var itemDto = new EvolucaoResultadoDto
                        {
                            OrdemMapaResultado = firstItem.OrdemMapaResultado,
                            Referencia = firstItem.Referencia,
                            FormataOrdem = firstItem.FormataOrdem,
                            Ordem = firstItem.Ordem,
                            ItemDescricao = firstItem.ItemDescricao,
                            ItemInfo = firstItem.ItemInfo,
                            TipoResultadoId = firstItem.TipoResultadoId,
                            Resultados = new List<EolucaoResultadoComparativoDto>()
                        };
                        var itemsOrderer = item.OrderBy(x => x.DataColeta ?? DateTime.MinValue).ToList();
                        itemDto.Resultados = itemsOrderer.Select((x, index) =>
                            {
                                var dto = new EolucaoResultadoComparativoDto
                                {
                                    DataColeta = x.DataColeta,
                                    Resultado = x.Resultado
                                };
                                if (!x.TipoResultadoId.HasValue)
                                {
                                    return dto;
                                }

                                itemDto.Numerico = dto.Numerico = x.TipoResultadoId.Value == (long)EnumTipoResultado.Numerico || x.TipoResultadoId.Value == (long)EnumTipoResultado.Calculado;

                                decimal resultadoValorAtual = 0;


                                var resultAux = new StringBuilder();
                                for (int i = 0; i < x.Resultado.Length; i++)
                                {
                                    var c = x.Resultado[i];
                                    var letter = c;
                                    if (c.Equals(','))
                                    {
                                        c = ',';
                                    }
                                    else if (c.Equals('.'))
                                    {
                                        c = ',';
                                    }

                                    resultAux.Append(c);
                                }

                                x.Resultado = resultAux.ToString();


                                if (dto.Numerico && !decimal.TryParse(x.Resultado, out resultadoValorAtual))
                                {
                                    return dto;
                                }

                                dto.Valor = resultadoValorAtual;

                                CalculaMargem(dto, x);

                                /*
                                 * if valor maior ou igual que minimo aceitavel e menor ou igual ao maximo 
                                 * verde
                                 * if menor que minimo aceitavel e menor ou igual ao minimo fica amarelo
                                 * if maior que maximo aceitavel e menor ou igual que maximo fica amarelo
                                 * if maior que maximo ou menor que minimo fica vermelho
                                 */

                                if (itemsOrderer.ElementAtOrDefault(index - 1) == null)
                                {
                                    return dto;
                                }

                                //var resultadoAnteriror = itemsOrderer.ElementAtOrDefault(index - 1).Resultado;
                                //var resultadoValorAnterior = 0d;
                                //if (double.TryParse(resultadoAnteriror, out resultadoValorAnterior))
                                //{
                                //    dto.ComparativoVsAnterior = Math.Round(resultadoValorAtual - resultadoValorAnterior, 2);
                                //}

                                return dto;
                            }).OrderByDescending(x => x.DataColeta).ToList();

                        lstDto.Add(itemDto);
                    }
                }

                return lstDto.AsQueryable().OrderBy(x => x.OrdemMapaResultado).ThenBy(x => x.FormataOrdem ?? 0)
                    .ToList();
            }
        }

        public async Task<PagedResultDto<EvolucaoResultadoDto>> ListaEvolucaoResultadoPorColeta(EvolucaoResultadoInput input)
        {
            try
            {
                if (!input.DateStart.HasValue || !input.DateEnd.HasValue)
                {
                    return null;
                }

                var query = @"SELECT 
                        DISTINCT
                            LabResultado.Id AS itemResultadoId,
                            FatItem.OrdemMapaResultado,
                            LabFormataItem.OrdemRegistro AS FormataOrdem,
                            FatItem.Descricao AS ItemDescricao,
                            LabItemResultado.Descricao AS ItemInfo,
                            LabItemResultado.MinimoAceitavelMasculino,
                            LabItemResultado.MinimoMasculino,
                            LabItemResultado.MinimoAceitavelFeminino,
                            LabItemResultado.MinimoFeminino,
                            LabItemResultado.MaximoAceitavelMasculino,
                            LabItemResultado.MaximoMasculino,
                            LabItemResultado.MaximoAceitavelFeminino,
                            LabItemResultado.MaximoFeminino,
                            LabResultadoLaudo.Resultado,
                            LabResultadoLaudo.Referencia,
                            LabResultadoLaudo.Ordem,
                            LabItemResultado.TipoResultadoId,
                            LabResultado.DataColeta,
                            AteAtendimento.IsAmbulatorioEmergencia AS HasAmbulatorioEmergencia,
                            AteAtendimento.IsInternacao AS HasInternacao,
                            Paciente.SexoId AS SexoId
                        FROM
                        LabResultado 
                        INNER JOIN AteAtendimento ON LabResultado.AteAtendimentoId = AteAtendimento.Id AND AteAtendimento.IsDeleted = @IsDeleted
                        INNER JOIN SisPaciente AS Paciente ON AteAtendimento.SisPacienteId = Paciente.Id AND Paciente.IsDeleted = @IsDeleted
                        LEFT JOIN LabResultadoExame ON LabResultadoExame.LabResultadoId = LabResultado.Id  AND LabResultadoExame.IsDeleted = @IsDeleted
                        LEFT JOIN LabResultadoLaudo ON LabResultadoLaudo.ResultadoExameId = LabResultadoExame.Id  AND LabResultadoLaudo.IsDeleted = @IsDeleted
                        LEFT JOIN FatItem ON LabResultadoExame.LabFaturamentoItemId = FatItem.Id  AND LabResultadoExame.IsDeleted = @IsDeleted
                        LEFT JOIN LabItemResultado ON LabResultadoLaudo.ItemResultadoId = LabItemResultado.Id  AND LabResultadoLaudo.IsDeleted = @IsDeleted
                        LEFT JOIN LabFormataItem ON LabFormataItem.ItemResultadoId = LabResultadoLaudo.ItemResultadoId  AND LabFormataItem.IsDeleted = @IsDeleted";


                var queryWhere = new StringBuilder("LabResultadoLaudo.Resultado IS NOT NULL AND LabResultado.IsDeleted = @IsDeleted");

                if (input.IsDesseAtendimento)
                {
                    queryWhere.Append(" AND AteAtendimento.Id = @AtendimentoId");
                }
                else
                {
                    queryWhere.Append(" AND AteAtendimento.SisPacienteId = @PacienteId");
                }

                if (!input.Filtro.IsNullOrEmpty())
                {
                    queryWhere.Append(@" AND  (LabItemResultado.Descricao like '%'+@Filtro+'%' OR LabResultadoLaudo.Referencia like '%'+@Filtro+'%' OR FatItem.Descricao like '%'+@Filtro+'%')");
                }

                queryWhere.Append(@" AND ( AteAtendimento.IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia OR AteAtendimento.IsInternacao = @IsInternacao)  AND  LabResultado.DataColeta BETWEEN @DateStart AND @DateEnd ");

                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var items = await connection.QueryAsync<ResultadoLaudoDto>($"{query} WHERE {queryWhere.ToString()}",
                        new
                        {
                            input.AtendimentoId,
                            input.Filtro,
                            input.IsAmbulatorioEmergencia,
                            input.IsInternacao,
                            input.PacienteId,
                            input.DateStart,
                            input.DateEnd,
                            IsDeleted = false
                        },
                        null,
                        0).ConfigureAwait(false);


                    if (items.Count() <= 0)
                    {
                        return new PagedResultDto<EvolucaoResultadoDto>();
                    }

                    var lstDto = new List<EvolucaoResultadoDto>();
                    foreach (var grupo in items.GroupBy(x => new { x.ItemDescricao }))
                    {
                        foreach (var item in grupo.GroupBy(x => new { x.ItemInfo }))
                        {
                            var firstItem = item.FirstOrDefault();
                            if (firstItem == null)
                            {
                                continue;
                            }

                            var itemDto = new EvolucaoResultadoDto
                            {
                                OrdemMapaResultado = firstItem.OrdemMapaResultado,
                                Referencia = firstItem.Referencia,
                                FormataOrdem = firstItem.FormataOrdem,
                                Ordem = firstItem.Ordem,
                                ItemDescricao = firstItem.ItemDescricao,
                                ItemInfo = firstItem.ItemInfo,
                                TipoResultadoId = firstItem.TipoResultadoId,
                                Resultados = new List<EolucaoResultadoComparativoDto>()
                            };
                            var itemsOrderer = item.OrderBy(x => x.DataColeta ?? DateTime.MinValue).ToList();
                            itemDto.Resultados = itemsOrderer.Select((x, index) =>
                            {
                                var dto = new EolucaoResultadoComparativoDto
                                {
                                    DataColeta = x.DataColeta,
                                    Resultado = x.Resultado
                                };
                                if (!x.TipoResultadoId.HasValue)
                                {
                                    return dto;
                                }

                                itemDto.Numerico = dto.Numerico = x.TipoResultadoId.Value == (long)EnumTipoResultado.Numerico || x.TipoResultadoId.Value == (long)EnumTipoResultado.Calculado;

                                decimal resultadoValorAtual = 0;


                                var resultAux = new StringBuilder();
                                for (int i = 0; i < x.Resultado.Length; i++)
                                {
                                    var c = x.Resultado[i];
                                    var letter = c;
                                    if (c.Equals(','))
                                    {
                                        c = ',';
                                    }
                                    else if (c.Equals('.'))
                                    {
                                        c = ',';
                                    }

                                    resultAux.Append(c);
                                }

                                x.Resultado = resultAux.ToString();


                                if (dto.Numerico && !decimal.TryParse(x.Resultado, out resultadoValorAtual))
                                {
                                    return dto;
                                }

                                dto.Valor = resultadoValorAtual;

                                CalculaMargem(dto, x);

                                /*
                                 * if valor maior ou igual que minimo aceitavel e menor ou igual ao maximo 
                                 * verde
                                 * if menor que minimo aceitavel e menor ou igual ao minimo fica amarelo
                                 * if maior que maximo aceitavel e menor ou igual que maximo fica amarelo
                                 * if maior que maximo ou menor que minimo fica vermelho
                                 */

                                if (itemsOrderer.ElementAtOrDefault(index - 1) == null)
                                {
                                    return dto;
                                }

                                //var resultadoAnteriror = itemsOrderer.ElementAtOrDefault(index - 1).Resultado;
                                //var resultadoValorAnterior = 0d;
                                //if (double.TryParse(resultadoAnteriror, out resultadoValorAnterior))
                                //{
                                //    dto.ComparativoVsAnterior = Math.Round(resultadoValorAtual - resultadoValorAnterior, 2);
                                //}

                                return dto;
                            }).OrderByDescending(x => x.DataColeta).ToList();

                            lstDto.Add(itemDto);
                        }
                    }

                    return new PagedResultDto<EvolucaoResultadoDto>(lstDto.Count, lstDto.AsQueryable().OrderBy(x => x.OrdemMapaResultado).ThenBy(x => x.ItemDescricao).ThenBy(x => x.FormataOrdem ?? 0).ThenBy(x => x.ItemInfo).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroPesquisar", ex);
            }
        }

        public static void CalculaMargem(EolucaoResultadoComparativoDto dto, ResultadoLaudoDto item)
        {
            if (!item.SexoId.HasValue)
            {
                return;
            }

            if (item.SexoId == (long)EnumSexo.Masculino)
            {
                CalculaMargemMasculino(dto, item);
            }
            else if (item.SexoId == (long)EnumSexo.Feminino)
            {
                CalculaMargemFeminino(dto, item);
            }
        }

        private static string Verde = "#64dd17";
        private static string Branco = "#fafafa";
        private static string Amarelo = "#ffd600";
        private static string Vermelho = "#d50000";

        public static void CalculaMargemMasculino(EolucaoResultadoComparativoDto dto, ResultadoLaudoDto item)
        {
            // valor maior ou igual ao minimo aceitavel e valor menor ou igual ao maximo aceitavel
            if (item.MinimoAceitavelMasculino.HasValue && item.MaximoAceitavelMasculino.HasValue && dto.Valor >= item.MinimoAceitavelMasculino && dto.Valor <= item.MaximoAceitavelMasculino)
            {
                dto.CorFundo = Branco;
                //dto.CorTexto = Verde;
            }

            // valor menor que minimo aceitavel e maior que minimo
            else if ((item.MinimoAceitavelMasculino.HasValue && item.MinimoMasculino.HasValue && dto.Valor < item.MinimoAceitavelMasculino && dto.Valor >= item.MinimoMasculino)
                    || (item.MinimoAceitavelMasculino.HasValue && !item.MinimoMasculino.HasValue && dto.Valor < item.MinimoAceitavelMasculino))
            {
                dto.CorFundo = Amarelo;
                //dto.CorTexto = Branco;
            }

            // valor maior que maximo aceitavel e menor que maximo
            else if ((item.MaximoAceitavelMasculino.HasValue && item.MaximoMasculino.HasValue && dto.Valor > item.MaximoAceitavelMasculino && dto.Valor <= item.MaximoMasculino)
                    || (item.MaximoAceitavelMasculino.HasValue && !item.MaximoMasculino.HasValue && dto.Valor > item.MaximoAceitavelMasculino))
            {
                dto.CorFundo = Amarelo;
                //dto.CorTexto = Branco;
            }

            //valor menor que minino ou valor maior que maximo
            else if ((item.MinimoMasculino.HasValue && dto.Valor < item.MinimoMasculino) || (item.MaximoMasculino.HasValue && dto.Valor > item.MaximoMasculino))
            {
                dto.CorFundo = Vermelho;
                //dto.CorTexto = Branco;
            }

            dto.TooltipValor = $@"
                <div class='col-12'>
                   <div class='row' style='padding-top:10px;'>
                      <div class='col-md-6 text-center'>
                         <h4>Referência Mínimo </h4> 
                         <h3 style='font-weight:600;'>{FormataValor(item.MinimoAceitavelMasculino)}</h3> 
                      </div>
                      <div class='col-md-6 text-center'> 
                         <h4>Referência Máximo </h4> 
                         <h3 style='font-weight:600;'>{FormataValor(item.MaximoAceitavelMasculino)}</h3> 
                      </div>
                   </div>
                   <div class='row' style='padding-top:15px;'>
                      <div class='col-md-6 text-center'>
                         <h4>Crítico Mínimo </h4> 
	                     <h3 style='font-weight:600;'>{FormataValor(item.MinimoMasculino)}</h3> 
                      </div>
                      <div class='col-md-6 text-center'>
                         <h4>Crítico Máximo </h4> 
                         <h3 style='font-weight:600;'> {FormataValor(item.MaximoMasculino)}</h3>
                      </div>
                   </div>
                </div>";
        }

        public static string FormataValor(decimal? valor)
        {
            if (valor.HasValue)
            {
                return valor.ToString();
            }

            return string.Empty;
        }

        public static void CalculaMargemFeminino(EolucaoResultadoComparativoDto dto, ResultadoLaudoDto item)
        {
            // valor maior ou igual ao minimo aceitavel e valor menor ou igual ao maximo aceitavel
            if (item.MinimoAceitavelFeminino.HasValue && item.MaximoAceitavelFeminino.HasValue && dto.Valor >= item.MinimoAceitavelFeminino && dto.Valor <= item.MaximoAceitavelFeminino)
            {
                dto.CorFundo = Branco;
                //dto.CorTexto = Verde;
            }

            // valor menor que minimo aceitavel e maior que minimo
            else if ((item.MinimoAceitavelFeminino.HasValue && item.MinimoFeminino.HasValue && dto.Valor < item.MinimoAceitavelFeminino && dto.Valor >= item.MinimoFeminino)
                    || (item.MinimoAceitavelFeminino.HasValue && !item.MinimoFeminino.HasValue && dto.Valor < item.MinimoAceitavelFeminino))
            {
                dto.CorFundo = Amarelo;
                //dto.CorTexto = Branco;
            }

            // valor maior que maximo aceitavel e menor que maximo
            else if ((item.MaximoAceitavelFeminino.HasValue && item.MaximoFeminino.HasValue && dto.Valor > item.MaximoAceitavelFeminino && dto.Valor <= item.MaximoFeminino)
                    || (item.MaximoAceitavelFeminino.HasValue && !item.MaximoFeminino.HasValue && dto.Valor > item.MaximoAceitavelFeminino))
            {
                dto.CorFundo = Amarelo;
                //dto.CorTexto = Branco;
            }

            //valor menor que minino ou valor maior que maximo
            else if ((item.MinimoFeminino.HasValue && dto.Valor < item.MinimoFeminino) || (item.MaximoFeminino.HasValue && dto.Valor > item.MaximoFeminino))
            {
                dto.CorFundo = Vermelho;
                //dto.CorTexto = Branco;
            }

            dto.TooltipValor = $@"
                <div class='col-12'>
                   <div class='row' style='padding-top:10px;'>
                      <div class='col-md-6 text-center'>
                         <h4>Referência Mínimo  </h4> 
                         <h3 style='font-weight:600;'>{FormataValor(item.MinimoAceitavelFeminino)}</h3> 
                      </div>
                      <div class='col-md-6 text-center'> 
                         <h4> Referência Máximo  </h4> 
                         <h3 style='font-weight:600;'>{FormataValor(item.MaximoAceitavelFeminino)}</h3> 
                      </div>
                   </div>
                   <div class='row' style='padding-top:15px;'>
                      <div class='col-md-6 text-center'>
                         <h4> Crítico Mínimo </h4> 
	                     <h3 style='font-weight:600;'>{FormataValor(item.MinimoFeminino)}</h3> 
                      </div>
                      <div class='col-md-6 text-center'>
                         <h4> Crítico Máximo </h4> 
                         <h3 style='font-weight:600;'> {FormataValor(item.MaximoFeminino)}</h3>
                      </div>
                   </div>
                </div>";
        }
    }
}