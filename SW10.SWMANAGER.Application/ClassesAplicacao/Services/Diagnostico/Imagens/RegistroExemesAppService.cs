using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Threading;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens
{
    public class RegistroExemesAppService : SWMANAGERAppServiceBase, IRegistroExemesAppService
    {
        private readonly IRepository<LaudoMovimento, long> _laudoMovimentoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IRepository<LaudoMovimentoItem, long> _laudoMovimentoItemRepository;
        private readonly IRepository<FaturamentoContaItem, long> _faturamentoContaItemRepository;
        private readonly IRepository<SolicitacaoExameItem, long> _solicitacaoExameItemRepository;
        private readonly IFaturamentoContaItemAppService _faturamentoContaItemAppService;

        public RegistroExemesAppService(IRepository<LaudoMovimento, long> laudoMovimentoRepository
                                      , IUnitOfWorkManager unitOfWorkManager
            , IUltimoIdAppService ultimoIdAppService
            , IRepository<LaudoMovimentoItem, long> laudoMovimentoItemRepository
            , IRepository<FaturamentoContaItem, long> faturamentoContaItemRepository
            , IRepository<SolicitacaoExameItem, long> solicitacaoExameItemRepository
            , IFaturamentoContaItemAppService faturamentoContaItemAppService)
        {
            _laudoMovimentoRepository = laudoMovimentoRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoIdAppService;
            _laudoMovimentoItemRepository = laudoMovimentoItemRepository;
            _faturamentoContaItemRepository = faturamentoContaItemRepository;
            _solicitacaoExameItemRepository = solicitacaoExameItemRepository;
            _faturamentoContaItemAppService = faturamentoContaItemAppService;
        }


        public async Task<PagedResultDto<RegistroExameIndex>> Listar(ListarLauMovimentoItensInput input)
        {

            var laudosMovimentos = _laudoMovimentoItemRepository.GetAll()
                .Include(i => i.LaudoMovimento)
                .Include(i => i.LaudoMovimento.Atendimento.Paciente)
                .Include(i => i.LaudoMovimento.Atendimento.Paciente.SisPessoa)
                .Include(i => i.LaudoMovimento.Convenio)
                .Include(i => i.LaudoMovimento.Convenio.SisPessoa)
                .Include(i => i.FaturamentoItem)
                .Include(i => i.LaudoMovimento.Atendimento.Leito)
                .Include(i => i.LaudoMovimento.Atendimento.UnidadeOrganizacional)
                .Where(w => (input.ConvenioId == null || w.LaudoMovimento.ConvenioId == input.ConvenioId)
                            && (input.PacienteId == null || w.LaudoMovimento.Atendimento.PacienteId == input.PacienteId)

                            && (input.EmissaoDe == null || w.LaudoMovimento.DataRegistro >= input.EmissaoDe)
                            && (input.EmissaoAte == null || w.LaudoMovimento.DataRegistro <= input.EmissaoAte)
                            && w.LaudoMovimentoId != null
                            && (w.FaturamentoItem.IsLaudo || w.FaturamentoItem.Grupo.IsLaudo)
                );

            try
            {

                var total = await laudosMovimentos.CountAsync();
                var result = await laudosMovimentos.SortBy(input.Sorting).PageBy(input)
                    .Select(item => new RegistroExameIndex
                    {
                        Id = item.LaudoMovimento.Id,
                        Codigo = item.LaudoMovimento.Codigo,
                        PacienteDescricao = item.LaudoMovimento.Atendimento.Paciente.NomeCompleto,
                        ConvenioDescricao = item.LaudoMovimento.Convenio != null ? item.LaudoMovimento.Convenio.NomeFantasia : string.Empty,
                        Exame = item.FaturamentoItem.Descricao,
                        Status = item.Status,
                        Medico = item.LaudoMovimento.MedicoSolicitante,
                        Leito = item.LaudoMovimento.Leito != null ? item.LaudoMovimento.Leito.Descricao : string.Empty,
                        UnidadeOrganizacional = item.LaudoMovimento.Atendimento != null ? item.LaudoMovimento.Atendimento.UnidadeOrganizacional.Descricao : string.Empty,
                        Data = item.LaudoMovimento.DataRegistro
                    }).ToListAsync();

                return new PagedResultDto<RegistroExameIndex>(total, result);
            }
            catch (Exception)
            {

            }
            return null;
        }

        public async Task<PagedResultDto<RegistroExameIndex>> ListarMovimentosItens(ListarLauMovimentoItensInput input)
        {
            var registros = new List<RegistroExameIndex>();

            var laudosMovimentos = _laudoMovimentoItemRepository.GetAll()
                                                            .Include(i => i.LaudoMovimento.Atendimento.Paciente)
                                                            .Include(i => i.LaudoMovimento.Atendimento.Paciente.SisPessoa)
                                                            .Include(i => i.LaudoMovimento.Convenio)
                                                            .Include(i => i.LaudoMovimento.Convenio.SisPessoa)
                                                            .Include(i => i.FaturamentoItem)
                                                            .Include(i => i.FaturamentoItem.LaudoGrupo)
                                                            .Where(w => (input.ConvenioId == null || w.LaudoMovimento.ConvenioId == input.ConvenioId)
                                                                    && (input.PacienteId == null || w.LaudoMovimento.Atendimento.PacienteId == input.PacienteId)
                                                                    && (input.EmissaoDe == null || w.LaudoMovimento.DataRegistro >= input.EmissaoDe)
                                                                       && (input.EmissaoAte == null || w.LaudoMovimento.DataRegistro <= input.EmissaoAte)
                                                                    && w.LaudoMovimentoId != null
                                                                    && (input.ModalidadeId == null || w.FaturamentoItem.LaudoGrupo.ModalidadeId == input.ModalidadeId)
                                                                    );

            var total = await laudosMovimentos.CountAsync();
            registros = await laudosMovimentos.SortBy(input.Sorting).PageBy(input).Select(x => new RegistroExameIndex {
                Id = x.Id,
                Codigo = x.LaudoMovimento.Codigo,
                PacienteDescricao = x.LaudoMovimento.Atendimento.Paciente.NomeCompleto,
                ConvenioDescricao = x.LaudoMovimento.Convenio.NomeFantasia,
                Exame = x.FaturamentoItem.Descricao,
                Status = x.Status,
                IsContraste = x.LaudoMovimento.IsContraste,
                LoteContraste = x.LaudoMovimento.LoteContraste,
                QtdContraste = x.LaudoMovimento.VolumeContrasteTotal,
            }).ToListAsync(); 
            
            return new PagedResultDto<RegistroExameIndex>(total, registros);
        }


        public async Task<LaudoMovimentoDto> Obter(long id)
        {
            var laudoMovimento = _laudoMovimentoRepository.GetAll()
                                                          .Where(w => w.Id == id)
                                                          .Include(i => i.Atendimento.Paciente)
                                                          .Include(i => i.Atendimento.Paciente.SisPessoa)
                                                          .Include(i => i.Convenio)
                                                          .Include(i => i.Convenio.SisPessoa)
                                                          .Include(i => i.LaudoMovimentoItens)
                                                          .Include(i => i.Leito)
                                                          .Include(i => i.Medico)
                                                          .Include(i => i.Tecnico)
                                                          .Include(i => i.TipoAcomodacao)
                                                          .Include(i => i.Turno)
                                                          .Include(i => i.CentroCusto)
                                                          .Include(i => i.LaudoMovimentoItens.Select(s => s.FaturamentoItem))
                                                          .FirstOrDefault();



            var laudoMovimentoDto = LaudoMovimentoDto.Mapear(laudoMovimento);//.MapTo<LaudoMovimentoDto>();
            laudoMovimentoDto.LaudoMovimentoItensDto = new List<LaudoMovimentoItemDto>();

            foreach (var item in laudoMovimento.LaudoMovimentoItens)
            {
                laudoMovimentoDto.LaudoMovimentoItensDto.Add(LaudoMovimentoItemDto.Mapear(item));//.MapTo<LaudoMovimentoItemDto>());
            }

            return laudoMovimentoDto;
        }

        public virtual DefaultReturn<LaudoMovimentoDto> CriarOuEditar(LaudoMovimentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<LaudoMovimentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {


                if (_retornoPadrao.Errors.Count() == 0)
                {


                    var examesDto = JsonConvert.DeserializeObject<List<RegistroExameDto>>(input.ExamesJson);


                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        if (input.Id == 0)
                        {
                            LaudoMovimento laudoMovimento = LaudoMovimentoDto.Mapear(input);//.MapTo<LaudoMovimento>();

                            AtualizaListaExames(laudoMovimento, examesDto);

                            foreach (var item in laudoMovimento.LaudoMovimentoItens)
                            {
                                item.Status = (int)EnumStatusLaudo.Registrado;
                            }

                            laudoMovimento.IsBombaInsufora = input.Aplicacao == 1;
                            laudoMovimento.IsIonico = input.Ionico == 1;

                            laudoMovimento.LaudoMovimentoStatusId = 1;
                            laudoMovimento.Codigo = _ultimoIdAppService.ObterProximoCodigo("RegistroExame").Result;
                            CarregarFaturamentoContaItem(laudoMovimento);
                            AtualizarStatusSolicitacaoExame(examesDto);
                            AsyncHelper.RunSync(() => _laudoMovimentoRepository.InsertAsync(laudoMovimento));
                            input.Id = laudoMovimento.Id;
                            _retornoPadrao.ReturnObject = input;//   laudoMovimento.MapTo<LaudoMovimentoDto>();
                        }
                        else
                        {
                            var laudoMovimento = _laudoMovimentoRepository.GetAll()
                                                              .Include(i => i.LaudoMovimentoItens)
                                                              .Where(w => w.Id == input.Id)
                                                              .FirstOrDefault();

                            if (laudoMovimento != null)
                            {
                                laudoMovimento.Codigo = input.Codigo;
                                laudoMovimento.AtendimentoId = input.AtendimentoId;
                                laudoMovimento.ConvenioId = input.ConvenioId;
                                laudoMovimento.IsContraste = input.IsContraste;
                                laudoMovimento.LaudoMovimentoStatusId = 1;
                                laudoMovimento.LeitoId = input.LeitoId;
                                laudoMovimento.Obs = input.Obs;
                                laudoMovimento.QtdeConstraste = input.QtdeConstraste;
                                laudoMovimento.TecnicoId = input.TecnicoId;
                                laudoMovimento.MedicoSolicitanteId = input.MedicoSolicitanteId;
                                laudoMovimento.Crm = input.Crm;
                                laudoMovimento.MedicoSolicitante = input.MedicoSolicitante;
                                laudoMovimento.DataRegistro = input.DataRegistro ?? DateTime.Now;

                                laudoMovimento.VolumeContrasteTotal = input.VolumeContrasteTotal;
                                laudoMovimento.VolumeContrasteVenoso = input.VolumeContrasteVenoso;
                                laudoMovimento.VolumeContrasteOral = input.VolumeContrasteOral;
                                laudoMovimento.VolumeContrasteRetal = input.VolumeContrasteRetal;
                                laudoMovimento.IsIonico = input.IsIonico;
                                laudoMovimento.IsBombaInsufora = input.IsBombaInsufora;
                                laudoMovimento.IsContrasteVenoso = input.IsContrasteVenoso;
                                laudoMovimento.IsContrasteOral = input.IsContrasteOral;
                                laudoMovimento.IsContrasteRetal = input.IsContrasteRetal;
                                laudoMovimento.TurnoId = input.TurnoId;
                                laudoMovimento.TipoAcomodacaoId = input.TipoAcomodacaoId;
                                laudoMovimento.LoteContraste = input.LoteContraste;
                                laudoMovimento.IsBombaInsufora = input.Aplicacao == 1;
                                laudoMovimento.IsIonico = input.Ionico == 1;

                                AtualizaListaExames(laudoMovimento, examesDto);

                                foreach (var item in laudoMovimento.LaudoMovimentoItens)
                                {
                                    item.Status = (int)EnumStatusLaudo.Registrado;
                                }


                                CarregarFaturamentoContaItem(laudoMovimento);
                                AtualizarStatusSolicitacaoExame(examesDto);
                                AsyncHelper.RunSync(() => _laudoMovimentoRepository.UpdateAsync(laudoMovimento));

                                _retornoPadrao.ReturnObject = input;// laudoMovimento.MapTo<LaudoMovimentoDto>();


                            }
                        }

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();

                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }
            return _retornoPadrao;
        }

        void AtualizarStatusSolicitacaoExame(List<RegistroExameDto> exames)
        {
            foreach (var item in exames)
            {
                var solicitacaoExame = _solicitacaoExameItemRepository.GetAll()
                                                                      .Where(w => w.Id == item.SolicitacaoExameId)
                                                                      .FirstOrDefault();

                if (solicitacaoExame != null)
                {
                    solicitacaoExame.StatusSolicitacaoExameItemId = (long)EnumSolicitacaoExameItem.Registrado;
                    _solicitacaoExameItemRepository.Update(solicitacaoExame);
                }
            }
        }

        void AtualizaListaExames(LaudoMovimento laudoMovimento, List<RegistroExameDto> registrosExamesDto)
        {
            if (laudoMovimento.LaudoMovimentoItens == null)
            {
                laudoMovimento.LaudoMovimentoItens = new List<LaudoMovimentoItem>();
            }
            else
            {
                laudoMovimento.LaudoMovimentoItens.RemoveAll(r => !registrosExamesDto.Any(a => a.Id == r.Id));
            }

            foreach (var registroExame in registrosExamesDto.Where(w => (w.Id == 0 || w.Id == null)))
            {
                var laudoMovimentoItem = new LaudoMovimentoItem();

                laudoMovimentoItem.FaturamentocontaItemId = registroExame.FaturamentoContaItemId;
                laudoMovimentoItem.FaturamentoItemId = registroExame.ExameId;
                laudoMovimento.LaudoMovimentoItens.Add(laudoMovimentoItem);
            }

        }

        public async Task<LaudoMovimentoItemDto> ObterMovimentoItem(long id)
        {
            var laudoMovimentoItem = _laudoMovimentoItemRepository.GetAll()
                                                          .Where(w => w.Id == id)
                                                          .Include(i => i.LaudoMovimento)
                                                          .Include(i => i.FaturamentoItem.LaudoGrupo)
                                                          .Include(i => i.FaturamentoItem.LaudoGrupo.Modalidade)
                                                          .Include(i => i.LaudoMovimento.Atendimento.Paciente)
                                                          .Include(i => i.LaudoMovimento.Atendimento.Paciente.SisPessoa)
                                                          .Include(i => i.FaturamentoItem)
                                                          .FirstOrDefault();

            var laudoMovimentoItemDto = LaudoMovimentoItemDto.Mapear(laudoMovimentoItem);//.MapTo<LaudoMovimentoItemDto>();

            if (laudoMovimentoItemDto.UsuarioParecerId != null)
            {
                var usuarioParecer = await UserManager.FindByIdAsync((long)laudoMovimentoItemDto.UsuarioParecerId);
                if (usuarioParecer != null)
                {
                    laudoMovimentoItemDto.UsuarioParecer = usuarioParecer.Name;
                }
            }

            if (laudoMovimentoItemDto.UsuarioLaudoId != null)
            {
                var usuarioLaudo = await UserManager.FindByIdAsync((long)laudoMovimentoItemDto.UsuarioLaudoId);
                if (usuarioLaudo != null)
                {
                    laudoMovimentoItemDto.UsuarioLaudo = usuarioLaudo.Name;
                }
            }

            if (laudoMovimentoItemDto.UsuarioRevisaoId != null)
            {
                var usuarioRevisao = await UserManager.FindByIdAsync((long)laudoMovimentoItemDto.UsuarioRevisaoId);
                if (usuarioRevisao != null)
                {
                    laudoMovimentoItemDto.UsuarioRevisao = usuarioRevisao.Name;
                }
            }


            return laudoMovimentoItemDto;
        }

        public DefaultReturn<LaudoMovimentoDto> RegistrarLaudo(LaudoMovimentoItemDto input)
        {
            var _retornoPadrao = new DefaultReturn<LaudoMovimentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                if (_retornoPadrao.Errors.Count() == 0)
                {

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {

                        var laudoMovimentoItem = _laudoMovimentoItemRepository.GetAll()
                                                                          .Include(i => i.FaturamentoItem.LaudoGrupo.Modalidade)
                                                                          .Where(w => w.Id == input.Id)
                                                                          .FirstOrDefault();



                        if (laudoMovimentoItem != null)
                        {

                            if (laudoMovimentoItem.Status == (int)EnumStatusLaudo.Registrado)//laudoMovimentoItem.FaturamentoItem.LaudoGrupo.Modalidade.IsParecer)
                            {
                                laudoMovimentoItem.Parecer = input.Parecer;
                                laudoMovimentoItem.ParecerData = DateTime.Now;
                                laudoMovimentoItem.Status = (int)EnumStatusLaudo.ComParecer;
                                laudoMovimentoItem.UsuarioParecerId = AbpSession.UserId;
                            }

                            else if ((laudoMovimentoItem.Status == (int)EnumStatusLaudo.Registrado && false)//!laudoMovimentoItem.FaturamentoItem.LaudoGrupo.Modalidade.IsParecer)
                                || (laudoMovimentoItem.Status == (int)EnumStatusLaudo.ComParecer)
                                || input.IsEditarLaudo)
                            {

                                laudoMovimentoItem.Laudo = input.Laudo;
                                laudoMovimentoItem.LaudoData = DateTime.Now;
                                laudoMovimentoItem.Status = (int)EnumStatusLaudo.ComLaudo;
                                laudoMovimentoItem.UsuarioLaudoId = AbpSession.UserId;
                                laudoMovimentoItem.IsIndicativo = input.IsIndicativo;
                                laudoMovimentoItem.IsSolicitacaoRevisao = input.IsSolicitacaoRevisao;
                                laudoMovimentoItem.ComentarioLaudo = input.ComentarioLaudo;
                                laudoMovimentoItem.JustificativaContraste = input.JustificativaContraste;
                                laudoMovimentoItem.MotivoDiscordancia = input.MotivoDiscordancia;

                            }
                            else if (laudoMovimentoItem.Status == (int)EnumStatusLaudo.ComLaudo && !input.IsEditarLaudo)
                            {
                                laudoMovimentoItem.Revisao = input.Revisao;
                                laudoMovimentoItem.RevisaoData = DateTime.Now;
                                laudoMovimentoItem.Status = (int)EnumStatusLaudo.LaudoRevisado;
                                laudoMovimentoItem.UsuarioRevisaoId = AbpSession.UserId;
                            }

                            AsyncHelper.RunSync(() => _laudoMovimentoItemRepository.UpdateAsync(laudoMovimentoItem));

                            unitOfWork.Complete();
                            _unitOfWorkManager.Current.SaveChanges();
                            unitOfWork.Dispose();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }
            return _retornoPadrao;
        }

        void CarregarFaturamentoContaItem(LaudoMovimento laudoMovimento)
        {

            FaturamentoContaItemInsertDto faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto();


            faturamentoContaItemInsertDto.AtendimentoId = laudoMovimento.AtendimentoId;
            faturamentoContaItemInsertDto.CentroCustoId = laudoMovimento.CentroCustoId;
            faturamentoContaItemInsertDto.Data = laudoMovimento.DataRegistro;
            //faturamentoContaItemInsertDto.MedicoId = laudoMovimento.me

            faturamentoContaItemInsertDto.Obs = laudoMovimento.Obs;
            faturamentoContaItemInsertDto.TurnoId = laudoMovimento.TurnoId;
            faturamentoContaItemInsertDto.UnidadeOrganizacionalId = laudoMovimento.UnidadeOrganizacionalId;



            faturamentoContaItemInsertDto.ItensFaturamento = new List<FaturamentoContaItemDto>();

            foreach (var item in laudoMovimento.LaudoMovimentoItens)
            {
                faturamentoContaItemInsertDto.ItensFaturamento.Add(new FaturamentoContaItemDto { Id = item.FaturamentoItemId, Qtde = 1 });
            }



            _faturamentoContaItemAppService.InserirItensContaFaturamento(faturamentoContaItemInsertDto);

            //try
            //{
            //    var faturamentoContas = _faturamentoContaRepository.GetAll()
            //                                                      .Where(w => w.AtendimentoId == laudoMovimento.AtendimentoId)
            //                                                      .ToList();

            //    var atendimento = _atendimentoRepository.GetAll()
            //                                            .Where(w => w.Id == laudoMovimento.AtendimentoId)
            //                                            .FirstOrDefault();

            //    var faturamentoConta = atendimento.IsAmbulatorioEmergencia ? faturamentoContas.FirstOrDefault() : faturamentoContas.LastOrDefault();

            //    if (faturamentoConta != null)
            //    {
            //        foreach (var item in laudoMovimento.LaudoMovimentoItens)
            //        {
            //            if (item.FaturamentocontaItemId == null)
            //            {
            //                var faturamentoContaItem = new FaturamentoContaItem();

            //                faturamentoContaItem.FaturamentoItemId = item.FaturamentoItemId;
            //                faturamentoContaItem.CentroCustoId = laudoMovimento.CentroCustoId;
            //                faturamentoContaItem.Data = DateTime.Now;
            //                faturamentoContaItem.FaturamentoContaId = faturamentoConta.Id;
            //                faturamentoContaItem.MedicoId = atendimento?.MedicoId;
            //                faturamentoContaItem.Observacao = laudoMovimento.Obs;
            //                faturamentoContaItem.Qtde = 1;
            //                // faturamentoContaItem.TipoLeitoId = laudoMovimento.TipoAcomodacaoId;
            //                faturamentoContaItem.TurnoId = laudoMovimento.TurnoId;
            //                faturamentoContaItem.UnidadeOrganizacionalId = laudoMovimento.UnidadeOrganizacionalId;


            //                var contaCalculoItem = new ContaCalculoItem();

            //                contaCalculoItem.EmpresaId = (long)atendimento.EmpresaId;
            //                contaCalculoItem.ConvenioId = (long)atendimento.ConvenioId;
            //                contaCalculoItem.PlanoId = (long)atendimento.PlanoId;

            //                CalculoContaItemInput calculoContaItemInput = new CalculoContaItemInput();

            //                calculoContaItemInput.conta = contaCalculoItem;
            //                calculoContaItemInput.FatContaItemDto = FaturamentoContaItemDto.MapearFromCore(faturamentoContaItem);

            //                faturamentoContaItem.ValorItem = AsyncHelper.RunSync(() => _faturamentoContaItemAppService.CalcularValorUnitarioContaItem(calculoContaItemInput));

            //                item.FaturamentoContaItem = faturamentoContaItem;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        public async Task<PagedResultDto<RegistroExameIndex>> ListarExamesFaturadosSemregistros(ListarLauMovimentoItensInput input)
        {
            var registros = new List<RegistroExameIndex>();
            int total = 0;

            try
            {
                var laudoMovimentoItemQuery = _laudoMovimentoItemRepository.GetAll();

                var faturamentoContaItemQuery =
                    _faturamentoContaItemRepository.GetAll()
                                                    .Include(i => i.FaturamentoConta)
                                                    .Include(i => i.FaturamentoConta.Atendimento)
                                                    .Include(i => i.FaturamentoConta.Atendimento.Paciente)
                                                    .Include(i => i.FaturamentoConta.Atendimento.Paciente.SisPessoa)
                                                    .Include(i => i.FaturamentoConta.Convenio)
                                                    .Include(i => i.FaturamentoConta.Convenio.SisPessoa)
                                                    .Include(i => i.FaturamentoItem)
                                                    .Include(i => i.FaturamentoItem.Grupo)
                                                    .Where(w =>
                                                                (w.FaturamentoItem.IsLaudo || w.FaturamentoItem.Grupo.IsLaudo) &&
                                                                (!laudoMovimentoItemQuery.Any(a => a.FaturamentocontaItemId == w.Id)) &&
                                                                (input.AtendimentoId == null || w.FaturamentoConta.AtendimentoId == input.AtendimentoId) &&
                                                                (input.ConvenioId == null || w.FaturamentoConta.ConvenioId == input.ConvenioId) &&
                                                                (input.PacienteId == null || w.FaturamentoConta.Atendimento.PacienteId == input.PacienteId) &&
                                                                ((input.EmissaoDe == null || input.EmissaoAte == null) || input.EmissaoDe <= w.Data && w.Data <= input.EmissaoAte)
                                                    );

                total = await faturamentoContaItemQuery.CountAsync();
                registros = await faturamentoContaItemQuery.SortBy(input.Sorting).PageBy(input)
                    .Select(x => new RegistroExameIndex
                    {
                        Id = x.Id,
                        PacienteDescricao = x.FaturamentoConta.Atendimento.Paciente.NomeCompleto,
                        ConvenioDescricao = x.FaturamentoConta.Convenio.NomeFantasia,
                        Exame = x.FaturamentoItem.Descricao,
                        AtendimentoId = x.FaturamentoConta.AtendimentoId
                    }).ToListAsync();

                return new PagedResultDto<RegistroExameIndex>(total, registros);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LaudoMovimentoDto> ObterExamesFaturadosSemregistros(List<long> ids)
        {
            // var registros = new List<LaudoMovimentoDto>();
            var registroExameIndex = new LaudoMovimentoDto();
            try
            {
                var laudosMovimentos = _faturamentoContaItemRepository.GetAll()
                                                                      .Include(i => i.FaturamentoConta)
                                                                      .Include(i => i.FaturamentoConta.Atendimento)
                                                                      .Include(i => i.FaturamentoConta.Atendimento.Medico)
                                                                      .Include(i => i.FaturamentoConta.Atendimento.Medico.SisPessoa)
                                                                      .Include(i => i.FaturamentoConta.Atendimento.Leito)
                                                                      .Include(i => i.FaturamentoConta.Atendimento.Paciente)
                                                                      .Include(i => i.FaturamentoConta.Atendimento.Paciente.SisPessoa)
                                                                      .Include(i => i.FaturamentoConta.Convenio)
                                                                      .Include(i => i.FaturamentoConta.Convenio.SisPessoa)
                                                                      .Include(i => i.FaturamentoItem)
                                                                      .Include(i => i.FaturamentoItem.Grupo)
                                                                      .Where(w => ids.Any(a => a == w.Id)
                                                                             && (w.FaturamentoItem.IsLaudo || w.FaturamentoItem.Grupo.IsLaudo)
                                                                            )
                                                                      .ToList();

                registroExameIndex.LaudoMovimentoItensDto = new List<LaudoMovimentoItemDto>();

                foreach (var item in laudosMovimentos)
                {
                    registroExameIndex.Id = item.Id;
                    registroExameIndex.Atendimento = item.FaturamentoConta?.Atendimento.MapTo<AtendimentoDto>();
                    //registroExameIndex.Convenio = item.FaturamentoConta?.Convenio.MapTo<ConvenioDto>();
                    //registroExameIndex.Leito = item.FaturamentoConta?.Atendimento?.Leito.MapTo<LeitoDto>();
                    //registroExameIndex.MedicoSolicitante = item.FaturamentoConta?.Atendimento?.Medico?.NomeCompleto;


                    registroExameIndex.LaudoMovimentoItensDto.Add(new LaudoMovimentoItemDto
                    {
                        FaturamentoItemId = (long)item.FaturamentoItemId
                                                                                             ,
                        FaturamentoItem = item.FaturamentoItem.MapTo<FaturamentoItemDto>(),

                        FaturamentocontaItemId = item.Id

                    });
                }
            }
            catch (Exception)
            {

            }
            return registroExameIndex;
        }


    }
}