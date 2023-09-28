using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Input;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados
{
    public class ResultadoAppService : SWMANAGERAppServiceBase, IResultadoAppService
    {

        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task<ResultadoDto> CriarOuEditar(ResultadoDto input)
        {
            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var faturamentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
                using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var solicitacaoExameItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var resultado = ResultadoDto.Mapear(input); // input.MapTo<Resultado>();

                    var exames = JsonConvert.DeserializeObject<List<ResultadoExameIndexCrudDto>>(input.ResultadosExamesList, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    resultado.FaturamentoConta = null;

                    if (input.Id.Equals(0))
                    {
                        resultado.Nic = Convert.ToInt64(await ultimoIdAppService.Object.ObterProximoCodigo("ColetaLaboratorio").ConfigureAwait(false));

                        resultado.Id = await resultadoRepositorio.Object.InsertAndGetIdAsync(resultado).ConfigureAwait(false);

                        unitOfWork.Complete();

                        // unitOfWork.Dispose();
                        unitOfWorkManager.Object.Current.SaveChanges();
                    }
                    else
                    {
                        resultado = await resultadoRepositorio.Object.GetAsync(input.Id).ConfigureAwait(false);
                        resultado.Altura = input.Altura;
                        resultado.AtendimentoId = input.AtendimentoId;
                        resultado.AutorizacaoProcedimentoId = input.AutorizacaoProcedimentoId;
                        resultado.CentroCustoId = input.CentroCustoId;
                        resultado.Codigo = input.Codigo;
                        resultado.ConvenioId = input.ConvenioId;
                        resultado.CRMSolicitante = input.CRMSolicitante;
                        resultado.DataColeta = input.DataColeta;
                        resultado.DataConferido = input.DataConferido;
                        resultado.DataDigitado = input.DataDigitado;
                        resultado.DataEntregaExame = input.DataEntregaExame;
                        resultado.DataEnvioEmail = input.DataEnvioEmail;
                        resultado.DataPrevEntregaExame = input.DataPrevEntregaExame;
                        resultado.DataTecnico = input.DataTecnico;
                        resultado.DataUsuarioCiente = input.DataUsuarioCiente;
                        resultado.Descricao = input.Descricao;
                        resultado.FaturamentoContaId = input.FaturamentoContaId;
                        resultado.Gemelar = input.Gemelar;
                        resultado.IsAvisoLab = input.IsAvisoLab;
                        resultado.IsAvisoMed = input.IsAvisoMed;
                        resultado.IsCiente = input.IsCiente;
                        resultado.IsEmail = input.IsEmail;
                        resultado.IsEmergencia = input.IsEmergencia;
                        resultado.IsRn = input.IsRn;
                        resultado.IsRotina = input.IsRotina;
                        resultado.IsSistema = input.IsSistema;
                        resultado.IsTransferencia = input.IsTransferencia;
                        resultado.IsUrgente = input.IsUrgente;
                        resultado.IsVisualiza = input.IsVisualiza;
                        resultado.LeitoAtualId = input.LeitoAtualId;
                        resultado.LocalAtualId = input.LocalAtualId;
                        resultado.LocalUtilizacaoId = input.LocalUtilizacaoId;
                        resultado.MedicoSolicitanteId = input.MedicoSolicitanteId;
                        resultado.Nic = input.Nic;
                        resultado.NomeMedicoSolicitante = input.NomeMedicoSolicitante;
                        resultado.Numero = input.Numero;
                        resultado.ObsEntrega = input.ObsEntrega;
                        resultado.Peso = input.Peso;
                        resultado.PessoaEntrega = input.PessoaEntrega;
                        resultado.Remedio = input.Remedio;
                        resultado.RequisicaoMovId = input.RequisicaoMovId;
                        resultado.ResponsavelId = input.ResponsavelId;
                        resultado.ResultadoStatusId = input.ResultadoStatusId;
                        resultado.RotinaId = input.RotinaId;
                        resultado.SexoRnId = input.SexoRnId;
                        resultado.TecnicoColetaId = input.TecnicoColetaId;
                        resultado.TecnicoId = input.TecnicoId;
                        resultado.TerceirizadoId = input.TerceirizadoId;
                        resultado.TipoAcomodacaoId = input.TipoAcomodacaoId;
                        resultado.TurnoId = input.TurnoId;
                        resultado.UsuarioCienteId = input.UsuarioCienteId;
                        resultado.UsuarioConferidoId = input.UsuarioConferidoId;
                        resultado.UsuarioDigitadoId = input.UsuarioDigitadoId;
                        resultado.UsuarioEntregaId = input.UsuarioEntregaId;

                        resultadoRepositorio.Object.Update(resultado);
                    }


                    foreach (var _item in exames)
                    {
                        if (_item.Id == 0 && !_item.ExameStatusId.HasValue)
                        {
                            _item.ExameStatusId = 1;
                        }

                        if (_item.ExameStatusId.HasValue && _item.ExameStatusId != 4)
                        {
                            var faturamentoItem = faturamentoItemRepositorio.Object.FirstOrDefault(_item.FaturamentoItemId.Value);

                            //var _resultado = await this.Obter(input.Id).ConfigureAwait(false);

                            var exame = new ResultadoExameDto
                            {
                                Codigo = _item.Codigo,
                                CreationTime = _item.CreationTime,
                                CreatorUserId = _item.CreatorUserId,
                                DataConferidoExame = _item.DataConferido,
                                DataDigitadoExame = _item.DataDigitado,
                                DataPendenteExame = _item.DataPendente,
                                FaturamentoItemId = _item.FaturamentoItemId,
                                FaturamentoContaItemId = _item.FaturamentoContaItemId,
                                Id = _item.Id,
                                IsDeleted = _item.IsDeleted,
                                IsSigiloso = _item.IsSigiloso,
                                IsSistema = _item.IsSistema,
                                LastModificationTime = _item.LastModificationTime,
                                LastModifierUserId = _item.LastModifierUserId,
                                MaterialId = _item.MaterialId,
                                MotivoPendenteExame = _item.MotivoPendenteExame,
                                Observacao = _item.ObservacaoExame,
                                Quantidade = _item.Quantidade,
                                UsuarioConferidoExameId = _item.UsuarioConferidoId,
                                UsuarioDigitadoExameId = _item.UsuarioDigitadoId,
                                UsuarioPendenteExameId = _item.UsuarioPendenteId,
                                ResultadoId = resultado.Id,
                                FormataId = faturamentoItem?.FormataId,
                                ExameStatusId = (long)EnumStatusExame.Inicial,
                                SolicitacaoExameId = _item.SolicitacaoItemId
                            };

                            // using (var unitOfWork = _unitOfWorkManager.Begin())
                            // {
                            if (exame.IsDeleted)
                            {
                                await resultadoExameAppService.Object.Excluir(exame).ConfigureAwait(false);
                            }
                            else
                            {
                                await resultadoExameAppService.Object.CriarOuEditar(exame).ConfigureAwait(false);

                                // CarregarFaturamentoContaItem(ResultadoExameDto.Mapear(exame));
                                var _exame = ResultadoExameDto.Mapear(exame);
                                _exame.FaturamentoItem = faturamentoItem;
                                CarregarFaturamentoContaItem(_exame, resultado);

                                // salvar os itens de fatconta
                                var faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto
                                {
                                    AtendimentoId = resultado.AtendimentoId ?? 0,
                                    Data = resultado.DataColeta,
                                    ItensFaturamento = new List<FaturamentoContaItemDto>()
                                };

                                var item = exame.FaturamentoContaItem ?? new FaturamentoContaItemDto(); // FaturamentoContaItemDto.MapearFromCore(_exame.FaturamentoContaItem);
                                item.Id = exame.FaturamentoItemId.Value;
                                faturamentoContaItemInsertDto.ItensFaturamento.Add(item);

                                await faturamentoContaItemAppService.Object.InserirItensContaFaturamento(faturamentoContaItemInsertDto).ConfigureAwait(false);
                            }

                            var solicitacaoItem = await solicitacaoExameItemRepository.Object.GetAll().FirstOrDefaultAsync(w => w.Id == _item.SolicitacaoItemId).ConfigureAwait(false);

                            if (solicitacaoItem != null)
                            {
                                solicitacaoItem.StatusSolicitacaoExameItemId = (long)EnumSolicitacaoExameItem.Registrado;
                                solicitacaoExameItemRepository.Object.Update(solicitacaoItem);
                            }


                            // unitOfWork.Complete();
                            // _unitOfWorkManager.Current.SaveChanges();
                            // unitOfWork.Dispose();
                            // }
                            this.GerarResultadoExameLaudo(exame);
                        }
                    }

                    // await this._visualAsaAppService.MigrarProRegExame(input.Id).ConfigureAwait(false);

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    return await this.Obter(resultado.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }

        }

        void GerarResultadoExameLaudo(ResultadoExameDto exame)
        {
            try
            {
                if (exame.FormataId == null || exame.FormataId == 0)
                {
                    return;
                }

                using (var formataItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormataItem, long>>())
                using (var resultadoLaudoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoLaudo, long>>())
                using (var itemResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ItemResultado, long>>())
                {
                    var formataItens = formataItemRepositorio.Object.GetAll().Where(w => w.FormataId == exame.FormataId).Include(i => i.ItemResultado).ToList();
                    var resultadoIds = formataItens.Select(x => x.ItemResultadoId).ToList();
                    var resultadoLaudos = resultadoLaudoRepository.Object.GetAll().Where(w => w.ResultadoExameId == exame.Id && resultadoIds.Contains(w.ItemResultadoId)).ToList();

                    foreach (var item in formataItens)
                    {
                        var resultadoLaudo = resultadoLaudos.FirstOrDefault(w => w.ItemResultadoId == item.ItemResultadoId);

                        if (resultadoLaudo == null)
                        {
                            resultadoLaudo = new ResultadoLaudo();
                        }

                        if (item.ItemResultado == null && item.ItemResultadoId.HasValue)
                        {
                            var _itemResultado = itemResultadoRepository.Object.Get(item.ItemResultadoId.Value);
                            item.ItemResultado = _itemResultado;
                        }

                        if (item.ItemResultado != null)
                        {
                            var itemResultado = item.ItemResultado;

                            resultadoLaudo.IsInterface = itemResultado.IsInterface;
                            resultadoLaudo.ItemResultadoId = itemResultado.Id;
                            resultadoLaudo.UnidadeId = itemResultado.LaboratorioUnidadeId;
                            resultadoLaudo.Referencia = itemResultado.Referencia;
                            resultadoLaudo.TipoResultadoId = itemResultado.TipoResultadoId;
                            resultadoLaudo.CasaDecimal = itemResultado.CasaDecimal;
                            resultadoLaudo.ResultadoExameId = exame.Id;
                            resultadoLaudo.Formula = item.Formula;

                            resultadoLaudo.MaximoAceitavelFeminino = item.ItemResultado.MaximoAceitavelFeminino;
                            resultadoLaudo.MinimoAceitavelFeminino = item.ItemResultado.MinimoAceitavelFeminino;
                            resultadoLaudo.MaximoFeminino = item.ItemResultado.MaximoFeminino;
                            resultadoLaudo.MinimoFeminino = item.ItemResultado.MinimoFeminino;
                            resultadoLaudo.NormalFeminino = item.ItemResultado.NormalFeminino;

                            resultadoLaudo.MaximoAceitavelMasculino = item.ItemResultado.MaximoAceitavelMasculino;
                            resultadoLaudo.MinimoAceitavelMasculino = item.ItemResultado.MinimoAceitavelMasculino;
                            resultadoLaudo.MaximoMasculino = item.ItemResultado.MaximoMasculino;
                            resultadoLaudo.MinimoMasculino = item.ItemResultado.MinimoMasculino;
                            resultadoLaudo.NormalMasculino = item.ItemResultado.NormalMasculino;
                        }

                        resultadoLaudo.Ordem = item.Ordem;

                        if (resultadoLaudo.Id == 0)
                        {
                            resultadoLaudoRepository.Object.Insert(resultadoLaudo);
                        }
                        else
                        {
                            resultadoLaudoRepository.Object.Update(resultadoLaudo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroInserir", ex);
            }
        }

        public async Task Excluir(ResultadoDto input)
        {
            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {
                    await resultadoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        public async Task<ListResultDto<ResultadoDto>> ListarTodos()
        {
            try
            {
                using (var _resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {

                    var query = await _resultadoRepositorio.Object.GetAllListAsync().ConfigureAwait(false);

                    var resultadosDto = ResultadoDto.Mapear(query); // .MapTo<List<Resultado>>();

                    return new ListResultDto<ResultadoDto> { Items = resultadosDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ResultadoIndexDto>> Listar(ListarResultadosInput input)
        {
            var resultadosDtos = new List<ResultadoIndexDto>();
            var dataFinal = ((DateTime)input.EndDate).Date.AddDays(1).AddMilliseconds(-1);

            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {
                    var query = resultadoRepositorio.Object.GetAll().Include(i => i.Tecnico)
                        .Include(i => i.Atendimento.Paciente.SisPessoa).Include(i => i.Atendimento.Empresa)
                        .Where(
                            m => (input.EmpresaId == null || m.Atendimento.EmpresaId == input.EmpresaId)
                                 && (input.TipoAtendimento == string.Empty || input.TipoAtendimento == null
                                                                           || input.TipoAtendimento == string.Empty
                                                                           || (input.TipoAtendimento == "AMB"
                                                                               && m.Atendimento.IsAmbulatorioEmergencia)
                                                                           || (input.TipoAtendimento == "INT"
                                                                               && m.Atendimento.IsInternacao)
                                                                           || input.TipoAtendimento == "ALL")
                                 && (input.StartDate == null || m.DataColeta >= ((DateTime)input.StartDate).Date)
                                 && (input.EndDate == null || m.DataColeta <= dataFinal))
                        .Where(
                            m => (input.PacienteId == null || m.Atendimento.PacienteId == input.PacienteId)
                                 && (input.MedicoId == null || m.MedicoSolicitanteId == input.MedicoId)
                                 && (input.ConvenioId == null || m.ConvenioId == input.ConvenioId)
                                 && (input.UnidadeId == null
                                     || m.Atendimento.UnidadeOrganizacionalId == input.UnidadeId)).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Codigo.Contains(input.Filtro)
                                 || m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                                 || m.Numero.Contains(input.Filtro)
                                 || m.Atendimento.Paciente.SisPessoa.NomeCompleto.ToUpper()
                                     .Contains(input.Filtro.ToUpper()) || m.MedicoSolicitante.SisPessoa.NomeCompleto
                                     .ToUpper().Contains(input.Filtro.ToUpper()));

                    var contar = await query.CountAsync().ConfigureAwait(false);

                    var resultados = await query.AsNoTracking()
                                         .OrderByDescending(o => o.DataColeta).PageBy(input)
                                         .Select(x => new ResultadoIndexDto()
                                         {
                                             isRn = x.IsRn,
                                             DataColeta = x.DataColeta,
                                             DataEntrega = x.DataEntregaExame,
                                             DataTecnico = x.DataTecnico,
                                             EntreguePor = x.UsuarioEntrega != null ? x.UsuarioEntrega.Name : string.Empty,
                                             Numero = x.Numero,
                                             Nic = x.Nic,
                                             Tecnico = x.Tecnico != null ? x.Tecnico.Descricao : string.Empty,
                                             Id = x.Id,
                                             Paciente = (x.Atendimento != null && x.Atendimento.Paciente != null) ? x.Atendimento.Paciente.NomeCompleto : string.Empty,
                                             MedicoSolicitante = (x.Atendimento != null && x.Atendimento.Medico != null) ? x.Atendimento.Medico.NomeCompleto : string.Empty,
                                             EmpresaId = x.Atendimento != null ? x.Atendimento.EmpresaId : 0,
                                             Empresa = (x.Atendimento != null && x.Atendimento.Empresa != null) ? x.Atendimento.Empresa.NomeFantasia : string.Empty
                                         }).ToListAsync();

                    return new PagedResultDto<ResultadoIndexDto>(contar, resultados);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultadoDto> Obter(long id)
        {
            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {
                    var query = resultadoRepositorio.Object.GetAll().AsNoTracking().Include(m => m.FaturamentoConta)
                        .Include(m => m.LeitoAtual).Include(m => m.MedicoSolicitante)
                        .Include(m => m.MedicoSolicitante.SisPessoa).Include(m => m.LocalAtual)
                        .Include(m => m.LocalUtilizacao).Include(m => m.Responsavel).Include(m => m.Tecnico)
                        .Include(m => m.TecnicoColeta).Include(m => m.UsuarioCiente).Include(m => m.UsuarioConferido)
                        .Include(m => m.UsuarioDigitado).Include(m => m.UsuarioEntrega).Include(m => m.Atendimento)
                        .Include(m => m.Atendimento.Paciente).Include(m => m.Atendimento.Paciente.SisPessoa)
                        .Include(m => m.Atendimento.Medico).Include(m => m.Atendimento.Medico.SisPessoa)
                        .Include(m => m.Atendimento.Leito)
                        .Include(m => m.Convenio).Include(m => m.Convenio.SisPessoa).Include(m => m.TipoAcomodacao)
                        .Include(m => m.Turno).Include(m => m.CentroCusto).Include(m => m.LeitoAtual)
                        .Include(m => m.ResultadoStatus)
                        .Where(m => m.Id == id);

                    var resultado = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var resultadoDto = ResultadoDto.Mapear(resultado); // .MapTo<ResultadoDto>();

                    return resultadoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {
                    var query = await resultadoRepositorio.Object.GetAll().AsNoTracking()
                                    .WhereIf(
                                        !input.IsNullOrEmpty(),
                                        m => m.Descricao.ToUpper().Contains(input.ToUpper()))
                                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao }).ToListAsync()
                                    .ConfigureAwait(false);
                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarResultadosInput input)
        {
            try
            {
                using (var listarResultadosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarResultadosExcelExporter>())
                {
                    var result = await this.Listar(input).ConfigureAwait(false);
                    var Resultados = result.Items;
                    return listarResultadosExcelExporter.Object.ExportToFile(Resultados.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            try
            {
                using (var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
                {
                    var pageInt = int.Parse(dropdownInput.page) - 1;
                    var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);

                    // get com filtro
                    var query = from p in resultadoRepositorio.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    // paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        //TODO: Isso precisar sair daqui e ir para um service
        public static void CarregarFaturamentoContaItem(ResultadoExame resultadoExame, Resultado resultado)
        {
            try
            {
                using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var faturamentoContas = faturamentoContaRepository.Object.GetAll()
                                                                      .Where(w => w.AtendimentoId == resultado.AtendimentoId)
                                                                      .ToList();

                    var atendimento = atendimentoRepository.Object
                                                            .GetAll()
                                                            .FirstOrDefault(w => w.Id == resultado.AtendimentoId);

                    var faturamentoConta = atendimento.IsAmbulatorioEmergencia ? faturamentoContas.FirstOrDefault() : faturamentoContas.LastOrDefault();

                    if (faturamentoConta != null)
                    {
                        // foreach (var item in resultado.   .LaudoMovimentoItens)
                        // {
                        if (resultadoExame.FaturamentoContaItemId == null)
                        {
                            var faturamentoContaItem = new FaturamentoContaItem
                            {
                                FaturamentoItemId = resultadoExame.FaturamentoItemId,
                                FaturamentoItem = resultadoExame.FaturamentoItem,
                                Data = DateTime.Now,
                                FaturamentoContaId = faturamentoConta.Id,
                                MedicoId = atendimento?.MedicoId,
                                Observacao = resultadoExame.Observacao,
                                Qtde = 1
                            };


                            // faturamentoContaItem.CentroCustoId = resultadoExame. .CentroCustoId;

                            // faturamentoContaItem.TipoLeitoId = laudoMovimento.TipoAcomodacaoId;
                            // faturamentoContaItem.TurnoId = laudoMovimento.TurnoId;
                            // faturamentoContaItem.UnidadeOrganizacionalId = laudoMovimento.UnidadeOrganizacionalId;
                            var contaCalculoItem = new ContaCalculoItem
                            {
                                EmpresaId = (long)atendimento.EmpresaId,
                                ConvenioId = (long)atendimento.ConvenioId,
                                PlanoId = (long)atendimento.PlanoId
                            };


                            var calculoContaItemInput = new CalculoContaItemInput
                            {
                                conta = contaCalculoItem,
                                FatContaItemDto = FaturamentoContaItemDto.MapearFromCore(faturamentoContaItem)
                            };

                            faturamentoContaItem.ValorItem = AsyncHelper.RunSync(() => faturamentoContaItemAppService.Object.CalcularValorUnitarioContaItem(calculoContaItemInput));

                            resultadoExame.FaturamentoContaItem = faturamentoContaItem;

                            // }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<PagedResultDto<ResultadoColetaIndexDto>> ListarExamesPorColeta(ResultadoColetaInput input)
        {
            try
            {
                using (var resultadoExameRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
                {
                    var resultadosDtos = new List<ResultadoColetaIndexDto>();
                    var query = resultadoExameRepositorio.Object.GetAll().Include(i => i.FaturamentoItem)
                        .Where(w => w.ResultadoId == input.ColetaId);

                    var contar = await query.CountAsync().ConfigureAwait(false);

                    var resultados = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                         .ConfigureAwait(false);

                    foreach (var item in resultados)
                    {
                        resultadosDtos.Add(
                            new ResultadoColetaIndexDto
                            {
                                ResultadoExameId = item.Id,
                                Exame = item.FaturamentoItem.Descricao,
                            });
                    }

                    return new PagedResultDto<ResultadoColetaIndexDto>(contar, resultadosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FormatacaoItemIndexDto>> ListarItensFormatacaoExame(LaudoResultadoInput input)
        {
            var contar = 0;
            List<FormataItem> resultados;
            List<FormatacaoItemIndexDto> resultadosDtos = new List<FormatacaoItemIndexDto>();
            try
            {
                using (var formataItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormataItem, long>>())
                using (var faturamentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                {
                    var exame = faturamentoItemRepositorio.Object.GetAll().FirstOrDefault(w => w.Id == input.ExameId);

                    if (exame != null)
                    {
                        var query = formataItemRepositorio.Object.GetAll().Include(i => i.ItemResultado)
                            .Include(i => i.ItemResultado.LaboratorioUnidade)
                            .Where(w => w.FormataId == exame.FormataId);



                        contar = await query.CountAsync().ConfigureAwait(false);

                        resultados = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                         .ConfigureAwait(false);

                        foreach (var item in resultados)
                        {
                            resultadosDtos.Add(
                                new FormatacaoItemIndexDto
                                {
                                    ItemId = item.Id,
                                    CodigoItem = item.ItemResultado.Codigo,
                                    DescricaoItem = item.ItemResultado.Descricao,
                                    Referencia = item.ItemResultado.Referencia,
                                    Unidade = item.ItemResultado.LaboratorioUnidade?.Descricao
                                });
                        }

                        // resultadosDtos = resultadosDtos
                        // .AsQueryable()
                        // .AsNoTracking()
                        // .OrderBy(input.Sorting)
                        // .PageBy(input)
                        // .ToList();
                    }

                    return new PagedResultDto<FormatacaoItemIndexDto>(contar, resultadosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultadoExameDto> ObterResultadoExame(long id)
        {
            try
            {
                using (var resultadoExameRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
                {
                    var query = resultadoExameRepositorio.Object.GetAll().AsNoTracking().Include(i => i.Resultado)
                        .Include(i => i.Resultado.Atendimento).Include(i => i.FaturamentoItem)
                        .Include(i => i.Resultado.MedicoSolicitante).Include(i => i.Resultado.Atendimento.Paciente)
                        .Include(i => i.Resultado.Atendimento.Paciente.SisPessoa).Where(m => m.Id == id);

                    var resultado = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var resultadoDto = ResultadoExameDto.Mapear(resultado); // .MapTo<ResultadoDto>();

                    return resultadoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<FormatacaoItemIndexDto>> ListarItensFormatacaoPorExame(LaudoResultadoInput input)
        {
            try
            {
                var contar = 0;
                var resultadosDtos = new List<FormatacaoItemIndexDto>();
                using (var faturamentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var resultadoLaudoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoLaudo, long>>())
                using (var formataItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormataItem, long>>())
                {
                    var exame = await faturamentoItemRepositorio.Object.GetAll()
                                    .FirstOrDefaultAsync(w => w.Id == input.ExameId).ConfigureAwait(false);

                    if (exame != null)
                    {


                        var query = formataItemRepositorio.Object.GetAll().AsNoTracking().Include(i => i.ItemResultado)
                            .Include(i => i.ItemResultado.LaboratorioUnidade)
                            .Where(w => w.FormataId == exame.FormataId);
                        var resultados = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                             .ConfigureAwait(false);

                        foreach (var item in resultados)
                        {

                            var resultadoLaudo = await resultadoLaudoRepository.Object.GetAll().AsNoTracking()
                                                     .FirstOrDefaultAsync(
                                                         w => w.ItemResultadoId == item.ItemResultadoId
                                                              && w.ResultadoExameId == input.ResultadoExameId)
                                                     .ConfigureAwait(false);


                            resultadosDtos.Add(
                                new FormatacaoItemIndexDto
                                {
                                    // Id = item.Id,
                                    ItemId = item.ItemResultadoId ?? 0,
                                    CodigoItem = item.ItemResultado.Codigo,
                                    DescricaoItem = item.ItemResultado.Descricao,
                                    Referencia = item.ItemResultado.Referencia,
                                    Unidade = item.ItemResultado.LaboratorioUnidade?.Descricao,
                                    UnidadeId = item.ItemResultado.LaboratorioUnidadeId,
                                    Resultado = resultadoLaudo?.Resultado,
                                    LaudoResultadoId = resultadoLaudo?.Id
                                });
                        }
                    }

                    return resultadosDtos;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<FormatacaoItemIndexDto>> ListarItensFormatacaoPorColeta(LaudoResultadoInput input)
        {
            var resultadosDtos = new List<FormatacaoItemIndexDto>();
            try
            {
                using (var _resultadoExameRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
                using (var _resultadoLaudoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoLaudo, long>>())
                {

                    var exames = await _resultadoExameRepositorio.Object.GetAll().AsNoTracking().Include(i => i.FaturamentoItem)
                                     .Where(w => w.ResultadoId == input.ColetaId).ToListAsync().ConfigureAwait(false);

                    long id = 0;

                    foreach (var exame in exames)
                    {
                        if (exame != null)
                        {
                            var laudos = _resultadoLaudoRepository.Object.GetAll().AsNoTracking()
                                .Where(
                                    w => w.ResultadoExameId == exame.Id
                                         && (w.ItemResultado.TipoResultadoId == 1
                                             || w.ItemResultado.TipoResultadoId == 2
                                             || w.ItemResultado.TipoResultadoId == 4)).Include(i => i.ItemResultado)
                                .Include(i => i.LaboratorioUnidade).Include(i => i.ResultadoExame).OrderBy(i => i.Ordem)
                                .ToList();

                            foreach (var item in laudos)
                            {
                                var resultado = new FormatacaoItemIndexDto
                                {
                                    ItemId = item.ItemResultadoId ?? 0,
                                    CodigoItem = item.ItemResultado.Codigo,
                                    DescricaoItem = item.ItemResultado.Descricao,
                                    Referencia = item.Referencia,
                                    Unidade = item.LaboratorioUnidade?.Descricao,
                                    UnidadeId = item.UnidadeId,
                                    Resultado = item.Resultado,
                                    LaudoResultadoId = item.Id,
                                    Exame = exame.FaturamentoItem.Descricao,
                                    ResultadoExameId = exame.Id,
                                    TipoResultadoId = item.TipoResultadoId ?? 0,
                                    GridId = ++id,
                                    CasaDecimal = item.CasaDecimal,
                                    TabelaId = item.ItemResultado.TabelaId,
                                    ExameStatusId = item.ResultadoExame.ExameStatusId
                                };

                                // Id = ++id,
                                this.AtualizarResultadoVisualizacao(resultado);

                                resultadosDtos.Add(resultado);
                            }
                        }
                    }

                    return resultadosDtos;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        void AtualizarResultadoVisualizacao(FormatacaoItemIndexDto formatacaoItemIndexDto)
        {

            if (formatacaoItemIndexDto.TipoResultadoId != 4)
            {
                formatacaoItemIndexDto.ResultadoVisualizacao = formatacaoItemIndexDto.Resultado;
            }
            else
            {

                long resultado;

                long.TryParse(formatacaoItemIndexDto.Resultado, out resultado);
                using (var tabelaResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TabelaResultado, long>>())
                {
                    var tabelaResultado = tabelaResultadoRepository.Object.GetAll().FirstOrDefault(
                        w => w.TabelaId == formatacaoItemIndexDto.TabelaId && w.Id == resultado);
                    if (tabelaResultado != null)
                    {
                        formatacaoItemIndexDto.ResultadoVisualizacao = tabelaResultado.Descricao;
                    }
                }
            }
        }

        public async Task<PagedResultDto<ResultadoIndexDto>> ListarNaoConferido(ListarResultadosInput input)
        {
            try
            {
                using var resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>();

                var query = resultadoRepositorio.Object.GetAll()
                    .Include(i => i.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Atendimento.Medico.SisPessoa)
                    .Where(m => (input.AtendimentoId == null || m.AtendimentoId == input.AtendimentoId))

                    .WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.Contains(input.Filtro)
                             || m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()));

                var contar = await query.CountAsync().ConfigureAwait(false);

                var resultados = await query.AsNoTracking()
                                 .OrderByDescending(o => o.DataColeta).PageBy(input)
                                 .Select(x => new ResultadoIndexDto()
                                 {
                                     isRn = x.IsRn,
                                     DataColeta = x.DataColeta,
                                     DataEntrega = x.DataEntregaExame,
                                     DataTecnico = x.DataTecnico,
                                     EntreguePor = x.UsuarioEntrega != null ? x.UsuarioEntrega.Name : string.Empty,
                                     MedicoSolicitante = (x.Atendimento != null && x.Atendimento.Medico != null) ? x.Atendimento.Medico.NomeCompleto : string.Empty,
                                     NomeMedicoSolicitante = (x.Atendimento != null && x.Atendimento.Medico != null) ? x.Atendimento.Medico.NomeCompleto : string.Empty,
                                     Numero = x.Numero,
                                     Nic = x.Nic,
                                     Id = x.Id,
                                     Paciente = (x.Atendimento != null && x.Atendimento.Paciente != null) ? x.Atendimento.Paciente.NomeCompleto : string.Empty,
                                     PacienteId = x.Atendimento != null ? x.Atendimento.PacienteId : 0,
                                     Codigo = x.Nic.ToString()
                                 })
                                 .ToListAsync();

                return new PagedResultDto<ResultadoIndexDto>(contar, resultados);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }


        public RegistroArquivoDto ObterArquivoExameColeta(long coletaId)
        {


            using (var _parametroRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametro, long>>())
            using (var _registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
            using (var _resultadoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
            using (var _resultadoExameRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var resultadoLaudoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoLaudoAppService>())
            using (var ms = new MemoryStream())
            {
                var examesIds = _resultadoExameRepositorio.Object.GetAll().AsNoTracking().Where(w => w.ResultadoId == coletaId).Select(s => s.Id).ToList();
                var registroArquivos = _registroArquivoRepository.Object.GetAll().AsNoTracking().Where(w => examesIds.Contains(w.RegistroId) && w.RegistroTabelaId == (long)EnumArquivoTabela.LaboratorioExame)
                        .OrderByDescending(o => o.Id)
                        .GroupBy(x => new { x.RegistroId, x.RegistroTabela })
                        .ToList().Select(x => x.ToList().OrderByDescending(ra => ra.Id).FirstOrDefault());

                if (!registroArquivos.Any(x => x?.Arquivo != null))
                {
                    resultadoLaudoAppService.Object.GerarArquivo(coletaId);
                    registroArquivos = _registroArquivoRepository.Object.GetAll().AsNoTracking().Where(w => examesIds.Contains(w.RegistroId) && w.RegistroTabelaId == (long)EnumArquivoTabela.LaboratorioExame).OrderByDescending(o => o.Id).ToList();
                }
                try
                {
                    using (var pdf = new Document(PageSize.A4, 10, 10, 20, 10))
                    using (var writer = new PdfCopy(pdf, ms))
                    {
                        if (writer == null)
                        {
                            return null;
                        }

                        pdf.Open();
                        foreach (var registroArquivo in registroArquivos.Where(x => x != null && x.Arquivo != null))
                        {
                            using (var reader = new PdfReader(registroArquivo.Arquivo))
                            {
                                writer.AddDocument(reader);
                            }
                        }
                    }
                    var registroArquivoDto = new RegistroArquivoDto();
                    registroArquivoDto.Arquivo = ms.ToArray();

                    return registroArquivoDto;
                }
                catch (Exception e)
                {
                    this.Logger.Error($" [ObterArquivoExameColeta] - Erro ao gerar o Exame de coleta para o Id {coletaId}", e);
                    return null;
                }
            }
        }


        public async Task<ResultadoDto> ObterPorSolicitacaoExameId(long solicitacaoExameId)
        {
            using (var resultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
            {
                var resultado = await resultadoRepository.Object.GetAll().AsNoTracking()
                    .Include(x => x.Responsavel)
                    .Include(x => x.ResultadoStatus)
                    .FirstOrDefaultAsync(
                        x => x.SolicitacaoExameId == solicitacaoExameId);
                return resultado == null ? null : ResultadoDto.Mapear(resultado);
            }
        }

        public async Task<PagedResultDto<ResultadoColetaDetalhamentoIndexDto>> ObterResultadoExamesPorResultadoId(
            ResultadoColetaDetalhamentoIndexFilterDto input)
        {
            const string idField = "ResultadoExame.Id";
            const string selectStatement = @"ResultadoExame.Id,
                ResultadoExame.Codigo,
                ResultadoExame.CreationTime AS DataColeta,
                LabExameStatus.Id AS StatusId,
                LabExameStatus.Descricao AS StatusDescricao,
                LabExameStatus.Cor AS StatusCor,
                ResultadoExame.DataColetaBaixa,
                CONCAT(ColetaBaixaUser.Name,' ', ColetaBaixaUser.Surname) AS UsuarioColetaBaixa,
                ResultadoExame.DataConferidoExame,
                CONCAT(ConferidoUser.Name,' ', ConferidoUser.Surname) AS UsuarioConferido,
                Resultado.DataDigitado,
                CONCAT(DigitadoUser.Name,' ', DigitadoUser.Surname) AS UsuarioDigitado,
                Item.Codigo AS Codigo,
                Item.Descricao AS Exame,
                Item.DescricaoTuss AS ExameDescricao,
                Item.Mneumonico AS ExameMneumonico,
                Material.Descricao AS DescricaoMaterial,
                Material.Codigo AS CodigoMaterial,
                ResultadoExame.MotivoPendencia,
                ResultadoExame.IsPendencia,
                CONVERT(varchar(10),ResultadoExame.PendenciaDateTime, 103) AS PendenciaDateTime,
                CONCAT(PendenciaUser.Name,' ', PendenciaUser.Surname) AS UsuarioPendencia,
                ResultadoExame.Observacao";
            
            const string fromStatement = @"LabResultado Resultado
                LEFT JOIN LabResultadoExame ResultadoExame ON  ResultadoExame.LabResultadoId = Resultado.Id
                LEFT JOIN FatItem Item ON Item.Id = ResultadoExame.LabFaturamentoItemId
                LEFT JOIN LabMaterial Material ON Material.Id = ResultadoExame.LabMaterialId
                LEFT JOIN AbpUsers AS PendenciaUser ON PendenciaUser.Id = ResultadoExame.PendenciaUserId
                LEFT JOIN LabExameStatus ON LabExameStatus.Id = ResultadoExame.ExameStatusId
                LEFT JOIN AbpUsers AS ColetaBaixaUser ON ColetaBaixaUser.Id = UsuarioColetaBaixaId
                LEFT JOIN AbpUsers AS ConferidoUser ON ConferidoUser.Id =  ResultadoExame.SisUsuarioConferidoId
                LEFT JOIN AbpUsers AS DigitadoUser ON DigitadoUser.Id =  ResultadoExame.SisUsuarioDigitadoid";

            const string whereStatement = @"Resultado.IsDeleted = @IsDeleted AND ResultadoExame.IsDeleted = @IsDeleted";
            return await this
                .CreateDataTable<ResultadoColetaDetalhamentoIndexDto, ResultadoColetaDetalhamentoIndexFilterDto>()
                .AddDefaultField(idField)
                .AddSelectClause(selectStatement)
                .AddFromClause(fromStatement)
                .AddWhereClause(whereStatement)
                .AddWhereMethod((dto, dapperParameters) =>
                {
                    dapperParameters.Add("IsDeleted", false);
                    return "AND Resultado.Id = @id";
                }).ExecuteAsync(input);
        }

        public async Task AtualizaStatus(long resultadoId)
        {
            using (var resultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
            using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var resultadoStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            {
                var resultadoStatus = await resultadoStatusRepository.Object.GetAll().AsNoTracking().ToListAsync();
                var resultado = await resultadoRepository.Object.FirstOrDefaultAsync(resultadoId);
                if (resultado == null)
                {
                    return;
                }
                var exames = await resultadoExameRepository.Object.GetAll().AsNoTracking().Where(x => x.ResultadoId == resultadoId).ToListAsync();
                var coletaStatusId = resultado.ResultadoStatusId;
                var novoStatusId = GetStatus(exames);
                resultado.ResultadoStatusId = novoStatusId;
                await resultadoRepository.Object.UpdateAsync(resultado);
                await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                    ResultadoDto.OcorrenciaVoltarStatus(resultado, 
                        resultadoStatus.FirstOrDefault(x=> x.Id == coletaStatusId)?.Descricao, resultadoStatus.FirstOrDefault(x=> x.Id == novoStatusId)?.Descricao, (await this.GetCurrentUserAsync()).UserName, DateTime.Now ),
                    TipoOcorrencia.ResultadoExame,
                    null,
                    typeof(Resultado).FullName, resultado.Id));
            }

            long GetStatus(IReadOnlyCollection<ResultadoExame> exames)
            {
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.Inicial))
                {
                    return ResultadoStatusDto.Inicial;
                }
                
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.EmColeta))
                {
                    return ResultadoStatusDto.EmColeta;
                }
                
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.Coletado))
                {
                    return ResultadoStatusDto.Coletado;
                }

                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.Interfaceado))
                {
                    return ResultadoStatusDto.Interfaceado;
                }
                
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.EnviadoEquipamento))
                {
                    return ResultadoStatusDto.EnviadoEquipamento;
                }
                
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.Digitado))
                {
                    return ResultadoStatusDto.Digitado;
                }
                
                if (exames.Any(x => x.ExameStatusId == ExameStatusDto.Conferido))
                {
                    return ResultadoStatusDto.Realizado;
                }
                
                return ResultadoStatusDto.Inicial;
            }
        }
    }
}






public class ITextEvents : PdfPageEventHelper
{
    // This is the contentbyte object of the writer
    PdfContentByte cb;

    // we will put the final number of pages in a template
    PdfTemplate headerTemplate;

    PdfTemplate footerTemplate;

    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;

    // This keeps track of the creation time
    DateTime PrintTime = DateTime.Now;

    #region Fields
    private string _header;
    public Image ImagemCabecalho { get; set; }
    public string TextoCabecalho { get; set; }
    #endregion

    #region Properties
    public string Header
    {
        get { return this._header; }
        set
        {
            this._header = value;
        }
    }
    #endregion    

    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            this.PrintTime = DateTime.Now;
            this.bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            this.cb = writer.DirectContent;
            this.headerTemplate = this.cb.CreateTemplate(15, 15);
            this.footerTemplate = this.cb.CreateTemplate(10, 10);

            this.headerTemplate.Height = 200;

            // ImagemCabecalho.SetAbsolutePosition(20, 10);// (document.PageSize.Height - 1500));
            // ImagemCabecalho.Width = 50;
        }
        catch (DocumentException)
        {
        }
        catch (IOException)
        {
        }
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        try
        {
            base.OnEndPage(writer, document);
            Font baseFontNormal = new Font(Font.FontFamily.HELVETICA, 12f, Font.NORMAL, BaseColor.BLACK);
            Font baseFontBig = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
            HTMLWorker obj = new HTMLWorker(document);
            StringReader se = new StringReader(this._header);
            obj.Parse(se);


            PdfPTable tbHeader = new PdfPTable(3);
            PdfPCell logocell = new PdfPCell(this.ImagemCabecalho, false) { Border = 0 };
            tbHeader.AddCell(logocell);

            PdfPCell cell2 = new PdfPCell(new Phrase("Laboratório de Análise Clínicas")) { Border = 0 };
            PdfPCell cell3 =
                new PdfPCell(new Phrase(this.TextoCabecalho, new Font(null, 9)))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };


            tbHeader.AddCell(cell2);
            tbHeader.AddCell(cell3);

            tbHeader.TotalWidth = document.PageSize.Width - 50;


            tbHeader.WriteSelectedRows(0, -1, 2, document.PageSize.Height - 5, writer.DirectContent);

            tbHeader.HorizontalAlignment = Element.ALIGN_CENTER;





            // Phrase p1Header = new Phrase(_header, baseFontNormal);


            ////Create PdfTable object
            // PdfPTable pdfTab = new PdfPTable(3);

            ////We will have to create separate cells to include image logo and 2 separate strings
            ////Row 1
            // PdfPCell pdfCell1 = new PdfPCell(ImagemCabecalho);
            ////PdfPCell pdfCell2 = new PdfPCell();
            ////PdfPCell pdfCell3 = new PdfPCell();
            // String text = "Page " + writer.PageNumber + " of ";


            ////Add paging to header
            // {
            // cb.BeginText();
            // cb.SetFontAndSize(bf, 12);
            // cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
            // cb.SetTextMatrix(document.LeftMargin, document.PageSize.Height - document.TopMargin);
            // // cb.ShowText(text);
            // cb.EndText();
            // float len = bf.GetWidthPoint(text, 12);
            // //Adds "12" in Page 1 of 12
            // cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            // }
            ////Add paging to footer
            // {
            // cb.BeginText();
            // cb.SetFontAndSize(bf, 12);
            // cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
            // //cb.ShowText(text);
            // cb.EndText();
            // //  float len = bf.GetWidthPoint(text, 12);
            // //cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            // }

            ////Row 2
            ////PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));

            //////Row 3 
            ////PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            ////PdfPCell pdfCell6 = new PdfPCell();
            ////PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));

            ////set the alignment of all three cells and set border to 0
            // pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            ////pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            ////pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            ////pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            ////pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            ////pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            ////pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            ////pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            ////pdfCell4.Colspan = 3;

            // pdfCell1.Border = 0;
            ////pdfCell2.Border = 0;
            ////pdfCell3.Border = 0;
            ////pdfCell4.Border = 0;
            ////pdfCell5.Border = 0;
            ////pdfCell6.Border = 0;
            ////pdfCell7.Border = 0;

            //////add all three cells into PdfTable
            // pdfTab.AddCell(pdfCell1);
            ////pdfTab.AddCell(pdfCell2);
            ////pdfTab.AddCell(pdfCell3);
            ////pdfTab.AddCell(pdfCell4);
            ////pdfTab.AddCell(pdfCell5);
            ////pdfTab.AddCell(pdfCell6);
            ////pdfTab.AddCell(pdfCell7);

            // pdfTab.TotalWidth = document.PageSize.Width - 80f;
            // pdfTab.WidthPercentage = 70;
            // pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;

            ////call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            ////first param is start row. -1 indicates there is no end row and all the rows to be included to write
            ////Third and fourth param is x and y position to start writing
            // pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            ////set pdfContent value

            ////Move the pointer and draw line to separate header section from rest of page
            this.cb.MoveTo(10, document.PageSize.Height - 160);
            this.cb.LineTo(document.PageSize.Width - 10, document.PageSize.Height - 160);
            this.cb.Stroke();

            ////Move the pointer and draw line to separate footer section from rest of page
            // cb.MoveTo(40, document.PageSize.GetBottom(50));
            // cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            // cb.Stroke();
        }
        catch (Exception)
        {

        }
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

        this.headerTemplate.BeginText();
        this.headerTemplate.SetFontAndSize(this.bf, 12);
        this.headerTemplate.SetTextMatrix(0, 0);
        this.headerTemplate.ShowText((writer.PageNumber - 1).ToString());
        this.headerTemplate.EndText();

        this.footerTemplate.BeginText();
        this.footerTemplate.SetFontAndSize(this.bf, 12);
        this.footerTemplate.SetTextMatrix(0, 0);
        this.footerTemplate.ShowText((writer.PageNumber - 1).ToString());
        this.footerTemplate.EndText();
    }
}
