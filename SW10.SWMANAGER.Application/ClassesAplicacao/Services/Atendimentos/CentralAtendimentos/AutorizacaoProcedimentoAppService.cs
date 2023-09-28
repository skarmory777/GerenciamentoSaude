using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    using Abp.Auditing;

    public class AutorizacaoProcedimentoAppService : SWMANAGERAppServiceBase, IAutorizacaoProcedimentoAppService
    {
        private readonly IRepository<AutorizacaoProcedimento, long> _autorizacaoProcedimentoRepository;
        private readonly IRepository<Medico, long> _medicoRepository;
        private readonly IRepository<MedicoEspecialidade, long> _medicoEspecialidadeRepository;
        private readonly IRepository<FaturamentoItem, long> _faturamentoItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<AutorizacaoProcedimentoItem, long> _autorizacaoProcedimentoItemRepository;
        private readonly IRepository<StatusSolicitacaoProcedimento, long> _statusSolicitacaoProcedimentoRepository;
        private readonly IRepository<ComentarioAutorizacaoProcedimento, long> _comentarioAutorizacaoProcedimentoRepository;

        public AutorizacaoProcedimentoAppService(IRepository<AutorizacaoProcedimento, long> autorizacaoProcedimentoRepository
                                                , IRepository<MedicoEspecialidade, long> medicoEspecialidadeRepository
                                                , IRepository<FaturamentoItem, long> faturamentoItemRepository
                                                , IUnitOfWorkManager unitOfWorkManager
                                                , IRepository<AutorizacaoProcedimentoItem, long> autorizacaoProcedimentoItemRepository
                                                , IRepository<StatusSolicitacaoProcedimento, long> statusSolicitacaoProcedimentoRepository
                                                , IRepository<ComentarioAutorizacaoProcedimento, long> comentarioAutorizacaoProcedimentoRepository)
        {
            _autorizacaoProcedimentoRepository = autorizacaoProcedimentoRepository;
            _medicoEspecialidadeRepository = medicoEspecialidadeRepository;
            _faturamentoItemRepository = faturamentoItemRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _autorizacaoProcedimentoItemRepository = autorizacaoProcedimentoItemRepository;
            _statusSolicitacaoProcedimentoRepository = statusSolicitacaoProcedimentoRepository;
            _comentarioAutorizacaoProcedimentoRepository = comentarioAutorizacaoProcedimentoRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<AutorizacaoProcedimentoIndexDto>> ListarAutorizacao(ListarAutorizacaoProcedimentoInput input)
        {
            var query = from autorizacao in _autorizacaoProcedimentoRepository.GetAll()
                        join item in _autorizacaoProcedimentoItemRepository.GetAll()
                        on autorizacao.Id equals item.AutorizacaoProcedimentoId

                        where (input.ConvenioId == null || autorizacao.ConvenioId == input.ConvenioId)
                          && (input.PeridoDe == null || (autorizacao.DataSolicitacao >= input.PeridoDe && autorizacao.DataSolicitacao <= input.PeridoAte))

                          && autorizacao.IsProrrogacao == false

                        select new AutorizacaoProcedimentoIndexDto
                        {
                            Id = autorizacao.Id,
                            CodigoAutorizacao = item.Senha,
                            Convenio = autorizacao.Convenio.NomeFantasia,
                            Medico = autorizacao.Solicitante.NomeCompleto,
                            Paciente = autorizacao.Paciente.NomeCompleto,
                            MedicoId = autorizacao.SolicitanteId,
                            FaturamentoItemId = item.FaturamentoItemId,
                            FaturamentoItem = item.FaturamentoItem.Descricao,
                            DataAutorizacao = item.DataAutorizacao,
                            ItemId = item.Id,
                            Status = item.StatusSolicitacaoProcedimento.Descricao,
                            IsOrtese = item.IsOrtese,
                            NumeroGuia = autorizacao.NumeroGuia
                        };


            var contarAutorizacoes = await query.CountAsync().ConfigureAwait(false);

            var autorizacoes = await query
                                   .AsNoTracking()
                                   .OrderBy(input.Sorting)
                                   .PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);


            foreach (var item in autorizacoes)
            {
                if (item.Medico != null && item.MedicoId != 0)
                {
                    var medicoEspecialidades = _medicoEspecialidadeRepository.GetAll()
                                                                             .Where(w => w.MedicoId == item.MedicoId)
                                                                             .Include(i => i.Especialidade)
                                                                             .ToList();

                    if (medicoEspecialidades != null && medicoEspecialidades.Any())
                    {
                        item.Especialidade = medicoEspecialidades.First().Especialidade.Nome;
                    }
                }
            }


            return new PagedResultDto<AutorizacaoProcedimentoIndexDto>(
                   contarAutorizacoes,
                   autorizacoes
                   );
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public PagedResultDto<AutorizacaoProcedimentoItemDto> ListarItensJson(string json)
        {
            List<AutorizacaoProcedimentoItemDto> autorizacaoItens = JsonConvert.DeserializeObject<List<AutorizacaoProcedimentoItemDto>>(json);
            var contarPreMovimentos = autorizacaoItens.Count;
            try
            {


                foreach (var item in autorizacaoItens)
                {
                    var faturamentoItem = _faturamentoItemRepository
                                                                    .GetAll()
                                                                    .FirstOrDefault(w => w.Id == (item.FaturamentoItemId ?? 0));
                    if (faturamentoItem != null)
                    {
                        item.FaturamentoItem = new FaturamentoItemDto { Id = faturamentoItem.Id, Codigo = faturamentoItem.Codigo, Descricao = faturamentoItem.Descricao };
                    }

                    if (item.StatusId != null)
                    {
                        var status = _statusSolicitacaoProcedimentoRepository.Get((long)item.StatusId);
                        item.StatusDescricao = status.Descricao;
                    }
                }


                return new PagedResultDto<AutorizacaoProcedimentoItemDto>(
                    contarPreMovimentos,
                    autorizacaoItens
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<AutorizacaoProcedimentoDto> CriarOuEditarAutorizacaoProcedimento(AutorizacaoProcedimentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<AutorizacaoProcedimentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                //TODO: Criar Validações



                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var itens = JsonConvert.DeserializeObject<List<AutorizacaoProcedimentoItemDto>>(input.Itens, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    if (input.Id.Equals(0))
                    {

                        AutorizacaoProcedimento autorizacaoProcedimento = new AutorizacaoProcedimento();

                        autorizacaoProcedimento.Id = input.Id;
                        autorizacaoProcedimento.ConvenioId = input.ConvenioId;
                        autorizacaoProcedimento.DataSolicitacao = (input.DataSolicitacao == null || input.DataSolicitacao == DateTime.MinValue) ? DateTime.Now : input.DataSolicitacao;
                        autorizacaoProcedimento.Observacao = input.Observacao;
                        autorizacaoProcedimento.PacienteId = input.PacienteId;
                        autorizacaoProcedimento.SolicitanteId = input.SolicitanteId;
                        autorizacaoProcedimento.NumeroGuia = input.NumeroGuia;

                        input.Id = _autorizacaoProcedimentoRepository.InsertAndGetId(autorizacaoProcedimento);

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();

                        foreach (var item in itens)
                        {
                            AutorizacaoProcedimentoItem autorizacaoProcedimentoItem = new AutorizacaoProcedimentoItem();

                            autorizacaoProcedimentoItem.AutorizacaoProcedimentoId = input.Id;
                            autorizacaoProcedimentoItem.DataAutorizacao = item.DataAutorizacao;
                            autorizacaoProcedimentoItem.FaturamentoItemId = item.FaturamentoItemId;
                            autorizacaoProcedimentoItem.Id = item.Id;
                            autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                            autorizacaoProcedimentoItem.QuantidadeAutorizada = item.QuantidadeAutorizada;
                            autorizacaoProcedimentoItem.Senha = item.Senha;
                            autorizacaoProcedimentoItem.StatusId = item.StatusId;
                            autorizacaoProcedimentoItem.QuantidadeSolicitada = item.QuantidadeSolicitada;
                            autorizacaoProcedimentoItem.Observacao = item.Observacao;
                            autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                            autorizacaoProcedimentoItem.AutorizadoPor = item.AutorizadoPor;

                            _autorizacaoProcedimentoItemRepository.Insert(autorizacaoProcedimentoItem);
                        }

                        if (input.Comentarios != null && input.Comentarios.Count > 0)
                        {

                            foreach (var comentarioAutorizacaoProcedimentoDto in input.Comentarios.OrderBy(o => o.DataRegistro))
                            {
                                var comentarioAutorizacaoProcedimento = ComentarioAutorizacaoProcedimentoDto.Mapear(comentarioAutorizacaoProcedimentoDto);//.MapTo<ComentarioAutorizacaoProcedimento>();

                                comentarioAutorizacaoProcedimento.UsuarioId = AbpSession.GetUserId();
                                comentarioAutorizacaoProcedimento.AutorizacaoProcedimentoId = input.Id;
                                _comentarioAutorizacaoProcedimentoRepository.Insert(comentarioAutorizacaoProcedimento);
                            }
                        }
                    }
                    else
                    {
                        var autorizacao = _autorizacaoProcedimentoRepository.GetAll()
                                                                            .Where(w => w.Id == input.Id)
                                                                            .FirstOrDefault();

                        if (autorizacao != null)
                        {
                            autorizacao.ConvenioId = input.ConvenioId;
                            autorizacao.Observacao = input.Observacao;
                            autorizacao.PacienteId = input.PacienteId;
                            autorizacao.SolicitanteId = input.SolicitanteId;
                            autorizacao.NumeroGuia = input.NumeroGuia;

                            _autorizacaoProcedimentoRepository.Update(autorizacao);

                            foreach (var item in itens)
                            {
                                AutorizacaoProcedimentoItem autorizacaoProcedimentoItem = new AutorizacaoProcedimentoItem();

                                autorizacaoProcedimentoItem.AutorizacaoProcedimentoId = input.Id;
                                autorizacaoProcedimentoItem.DataAutorizacao = item.DataAutorizacao;
                                autorizacaoProcedimentoItem.FaturamentoItemId = item.FaturamentoItemId;
                                autorizacaoProcedimentoItem.Id = item.Id;
                                autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                                autorizacaoProcedimentoItem.QuantidadeAutorizada = item.QuantidadeAutorizada;
                                autorizacaoProcedimentoItem.Senha = item.Senha;
                                autorizacaoProcedimentoItem.StatusId = item.StatusId;
                                autorizacaoProcedimentoItem.QuantidadeSolicitada = item.QuantidadeSolicitada;
                                autorizacaoProcedimentoItem.Observacao = item.Observacao;
                                autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                                autorizacaoProcedimentoItem.AutorizadoPor = item.AutorizadoPor;

                                _autorizacaoProcedimentoItemRepository.InsertOrUpdate(autorizacaoProcedimentoItem);
                            }

                            var itensPrecistidos = _autorizacaoProcedimentoItemRepository.GetAll()
                                                                              .Where(w => w.AutorizacaoProcedimentoId == autorizacao.Id)
                                                                              .ToList();

                            var itensMantidos = itens.Where(w => w.Id != 0).ToList();
                            var itensExcluidos = itensPrecistidos.Where(w => !itensMantidos.Any(a => a.Id == w.Id));//    Except(itensMantidos);

                            foreach (var item in itensExcluidos)
                            {
                                _autorizacaoProcedimentoItemRepository.DeleteAsync(item);
                            }

                        }
                    }

                    _retornoPadrao.ReturnObject = input;

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
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

        public async Task<AutorizacaoProcedimentoDto> Obter(long id)
        {
            try
            {
                var result = await _autorizacaoProcedimentoRepository
                    .GetAll()
                     .Include(i => i.Atendimento)
                     .Include(i => i.Atendimento.Paciente)
                     .Include(i => i.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Paciente)
                    .Include(i => i.Paciente.SisPessoa)
                    .Include(i => i.Solicitante)
                    .Include(i => i.Solicitante.SisPessoa)
                    .Include(i => i.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(i => i.Convenio.FormaAutorizacao)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var autorizacaoProcedimentoDto = AutorizacaoProcedimentoDto.Mapear(result);

                return autorizacaoProcedimentoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<List<AutorizacaoProcedimentoItemDto>> ObterItens(long id)
        {
            try
            {
                List<AutorizacaoProcedimentoItemDto> itensDto = new List<AutorizacaoProcedimentoItemDto>();

                var itens = await _autorizacaoProcedimentoItemRepository.GetAll()
                                                                  .Where(w => w.AutorizacaoProcedimentoId == id)
                                                                  .Include(i => i.FaturamentoItem)
                                                                  .Include(i => i.StatusSolicitacaoProcedimento)
                                                                  .ToListAsync();

                int idGrid = 0;
                foreach (var item in itens)
                {
                    var itemDto = AutorizacaoProcedimentoItemDto.Mapear(item);//.MapTo<AutorizacaoProcedimentoItemDto>();
                    itemDto.IdGrid = idGrid++;

                    itemDto.StatusDescricao = item.StatusSolicitacaoProcedimento.Descricao;
                    itemDto.FaturamentoItemDescricao = item.FaturamentoItem?.Descricao;
                    itensDto.Add(itemDto);
                }


                return itensDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<AutorizacaoProcedimentoDto> autorizacaoProcedimentosDto = new List<AutorizacaoProcedimentoDto>();
            try
            {
                //get com filtro
                var query = from p in _autorizacaoProcedimentoRepository
                            .GetAll()
                            .Include(m => m.Paciente)
                            .Include(m => m.Paciente.SisPessoa)
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Paciente.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Paciente.NomeCompleto ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Paciente.NomeCompleto) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<AutorizacaoProcedimentoDto> CriarOuEditarProrrogacaoInternacao(AutorizacaoProcedimentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<AutorizacaoProcedimentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                //TODO: Criar Validações

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var itens = JsonConvert.DeserializeObject<List<AutorizacaoProcedimentoItemDto>>(input.Itens, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    if (input.Id.Equals(0))
                    {

                        AutorizacaoProcedimento autorizacaoProcedimento = new AutorizacaoProcedimento();

                        autorizacaoProcedimento.Id = input.Id;
                        autorizacaoProcedimento.ConvenioId = input.ConvenioId;
                        autorizacaoProcedimento.DataSolicitacao = (input.DataSolicitacao == null || input.DataSolicitacao == DateTime.MinValue) ? DateTime.Now : input.DataSolicitacao;
                        autorizacaoProcedimento.Observacao = input.Observacao;
                        //autorizacaoProcedimento.PacienteId = input.PacienteId;
                        autorizacaoProcedimento.AtendimentoId = input.AtendimentoId;
                        autorizacaoProcedimento.SolicitanteId = input.SolicitanteId;
                        autorizacaoProcedimento.NumeroGuia = input.NumeroGuia;
                        autorizacaoProcedimento.IsProrrogacao = true;
                        input.Id = _autorizacaoProcedimentoRepository.InsertAndGetId(autorizacaoProcedimento);

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();

                        foreach (var item in itens)
                        {
                            AutorizacaoProcedimentoItem autorizacaoProcedimentoItem = new AutorizacaoProcedimentoItem();

                            autorizacaoProcedimentoItem.AutorizacaoProcedimentoId = input.Id;
                            autorizacaoProcedimentoItem.DataAutorizacao = item.DataAutorizacao;
                            autorizacaoProcedimentoItem.Id = item.Id;
                            autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                            autorizacaoProcedimentoItem.QuantidadeAutorizada = item.QuantidadeAutorizada;
                            autorizacaoProcedimentoItem.Senha = item.Senha;
                            autorizacaoProcedimentoItem.StatusId = item.StatusId;
                            autorizacaoProcedimentoItem.QuantidadeSolicitada = item.QuantidadeSolicitada;
                            autorizacaoProcedimentoItem.Observacao = item.Observacao;
                            autorizacaoProcedimentoItem.AutorizadoPor = item.AutorizadoPor;

                            _autorizacaoProcedimentoItemRepository.Insert(autorizacaoProcedimentoItem);
                        }

                        if (input.Comentarios != null && input.Comentarios.Count > 0)
                        {

                            foreach (var comentarioAutorizacaoProcedimentoDto in input.Comentarios.OrderBy(o => o.DataRegistro))
                            {
                                var comentarioAutorizacaoProcedimento = ComentarioAutorizacaoProcedimentoDto.Mapear(comentarioAutorizacaoProcedimentoDto);//.MapTo<ComentarioAutorizacaoProcedimento>();

                                comentarioAutorizacaoProcedimento.UsuarioId = AbpSession.GetUserId();
                                comentarioAutorizacaoProcedimento.AutorizacaoProcedimentoId = input.Id;
                                _comentarioAutorizacaoProcedimentoRepository.Insert(comentarioAutorizacaoProcedimento);
                            }
                        }
                    }
                    else
                    {
                        var autorizacao = _autorizacaoProcedimentoRepository.GetAll()
                                                                            .Where(w => w.Id == input.Id)
                                                                            .FirstOrDefault();

                        if (autorizacao != null)
                        {
                            autorizacao.ConvenioId = input.ConvenioId;
                            autorizacao.Observacao = input.Observacao;
                            autorizacao.AtendimentoId = input.AtendimentoId;
                            autorizacao.SolicitanteId = input.SolicitanteId;
                            autorizacao.NumeroGuia = input.NumeroGuia;
                            autorizacao.IsProrrogacao = true;

                            _autorizacaoProcedimentoRepository.Update(autorizacao);

                            foreach (var item in itens)
                            {
                                AutorizacaoProcedimentoItem autorizacaoProcedimentoItem = new AutorizacaoProcedimentoItem();

                                autorizacaoProcedimentoItem.AutorizacaoProcedimentoId = input.Id;
                                autorizacaoProcedimentoItem.DataAutorizacao = item.DataAutorizacao;
                                autorizacaoProcedimentoItem.Id = item.Id;
                                autorizacaoProcedimentoItem.IsOrtese = item.IsOrtese;
                                autorizacaoProcedimentoItem.QuantidadeAutorizada = item.QuantidadeAutorizada;
                                autorizacaoProcedimentoItem.Senha = item.Senha;
                                autorizacaoProcedimentoItem.StatusId = item.StatusId;
                                autorizacaoProcedimentoItem.QuantidadeSolicitada = item.QuantidadeSolicitada;
                                autorizacaoProcedimentoItem.Observacao = item.Observacao;
                                autorizacaoProcedimentoItem.AutorizadoPor = item.AutorizadoPor;

                                _autorizacaoProcedimentoItemRepository.InsertOrUpdate(autorizacaoProcedimentoItem);
                            }

                            var itensPrecistidos = _autorizacaoProcedimentoItemRepository.GetAll()
                                                                              .Where(w => w.AutorizacaoProcedimentoId == autorizacao.Id)
                                                                              .ToList();

                            var itensMantidos = itens.Where(w => w.Id != 0).ToList();
                            var itensExcluidos = itensPrecistidos.Where(w => !itensMantidos.Any(a => a.Id == w.Id));//    Except(itensMantidos);

                            foreach (var item in itensExcluidos)
                            {
                                _autorizacaoProcedimentoItemRepository.DeleteAsync(item);
                            }

                        }
                    }

                    _retornoPadrao.ReturnObject = input;

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
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

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<AutorizacaoProcedimentoIndexDto>> ListarProrrogacaoInternacao(ListarAutorizacaoProcedimentoInput input)
        {
            input.PeridoDe = ((DateTime)input.PeridoDe).Date;
            input.PeridoAte = ((DateTime)input.PeridoAte).Date.AddDays(1).AddMilliseconds(-1);


            var query = from autorizacao in _autorizacaoProcedimentoRepository.GetAll()
                        join item in _autorizacaoProcedimentoItemRepository.GetAll()
                        on autorizacao.Id equals item.AutorizacaoProcedimentoId

                        where (input.ConvenioId == null || autorizacao.ConvenioId == input.ConvenioId)
                          && (input.PeridoDe == null || (autorizacao.DataSolicitacao >= input.PeridoDe && autorizacao.DataSolicitacao <= input.PeridoAte))

                          && autorizacao.IsProrrogacao == true

                        select new AutorizacaoProcedimentoIndexDto
                        {
                            Id = autorizacao.Id,
                            CodigoAutorizacao = item.Senha,
                            Convenio = autorizacao.Convenio.NomeFantasia,
                            Medico = autorizacao.Solicitante.NomeCompleto,
                            Paciente = autorizacao.Atendimento.Paciente.NomeCompleto,
                            MedicoId = autorizacao.SolicitanteId,
                            DataAutorizacao = item.DataAutorizacao,
                            ItemId = item.Id,
                            Status = item.StatusSolicitacaoProcedimento.Descricao,
                            NumeroGuia = autorizacao.NumeroGuia,
                            QuantidadeSolicitada = item.QuantidadeSolicitada,
                            QuantidadeAutorizada = item.QuantidadeAutorizada
                        };


            var contarAutorizacoes = await query.CountAsync();

            var autorizacoes = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();


            foreach (var item in autorizacoes)
            {
                if (item.Medico != null && item.MedicoId != 0)
                {
                    var medicoEspecialidades = _medicoEspecialidadeRepository.GetAll()
                                                                             .Where(w => w.MedicoId == item.MedicoId)
                                                                             .Include(i => i.Especialidade)
                                                                             .ToList();

                    if (medicoEspecialidades != null && medicoEspecialidades.Count() > 0)
                    {
                        item.Especialidade = medicoEspecialidades.First().Especialidade.Nome;
                    }
                }
            }


            return new PagedResultDto<AutorizacaoProcedimentoIndexDto>(
                   contarAutorizacoes,
                   autorizacoes
                   );
        }

        public async Task<AutorizacaoProcedimentoDto> ObterProrrogacaoPorAtendimento(long atendimentoId)
        {
            try
            {
                var result = await _autorizacaoProcedimentoRepository
                    .GetAll()
                     .Include(i => i.Atendimento)
                     .Include(i => i.Atendimento.Paciente)
                     .Include(i => i.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Paciente)
                    .Include(i => i.Paciente.SisPessoa)
                    .Include(i => i.Solicitante)
                    .Include(i => i.Solicitante.SisPessoa)
                    .Include(i => i.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(i => i.Convenio.FormaAutorizacao)
                    .Where(m => m.AtendimentoId == atendimentoId
                               && m.IsProrrogacao)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    var autorizacaoProcedimentoDto = AutorizacaoProcedimentoDto.Mapear(result);
                    return autorizacaoProcedimentoDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
