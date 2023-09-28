#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas
{
    public class FaturamentoEntregaContaAppService : SWMANAGERAppServiceBase, IFaturamentoEntregaContaAppService
    {
        #region Dependencias
        private readonly IRepository<FaturamentoEntregaConta, long> _entregaRepository;
        private readonly IRepository<FaturamentoConta, long> _contaRepository;
        private readonly IRepository<FaturamentoContaItem, long> _contaItemRepository;
        private readonly IListarFaturamentoEntregaContasExcelExporter _listarEntregaContasExcelExporter;
        private readonly IFaturamentoGrupoAppService _faturamentoGrupoAppService;
        private readonly IFaturamentoItemAppService _faturamentoItemAppService;

        public FaturamentoEntregaContaAppService(
            IRepository<FaturamentoEntregaConta, long> entregaRepository,
            IRepository<FaturamentoConta, long> contaRepository,
            IRepository<FaturamentoContaItem, long> contaItemRepository,
            IListarFaturamentoEntregaContasExcelExporter listarEntregaContasExcelExporter,
            IFaturamentoGrupoAppService faturamentoGrupoAppService,
            IFaturamentoItemAppService faturamentoItemAppService
            )
        {
            _entregaRepository = entregaRepository;
            _contaRepository = contaRepository;
            _contaItemRepository = contaItemRepository;
            _listarEntregaContasExcelExporter = listarEntregaContasExcelExporter;
            _faturamentoGrupoAppService = faturamentoGrupoAppService;
            _faturamentoItemAppService = faturamentoItemAppService;
        }
        #endregion dependencias.

        public async Task<PagedResultDto<FaturamentoEntregaContaDto>> Listar(ListarEntregasInput input)
        {
            var itemrEntregaContas = 0;
            List<FaturamentoEntregaConta> itens;
            List<FaturamentoEntregaContaDto> itensDtos = new List<FaturamentoEntregaContaDto>();
            try
            {
                var query = _entregaRepository.GetAll();
                itemrEntregaContas = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    // .OrderBy(input.Sorting)
                    // .PageBy(input)
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoEntregaContaDto>>();

                return new PagedResultDto<FaturamentoEntregaContaDto>(
                    itemrEntregaContas,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarConferidas(ListarEntregasInput input)
        {
            var itemrEntregaContas = 0;
            List<FaturamentoEntregaConta> itens;
            List<FaturamentoEntregaContaDto> itensDtos = new List<FaturamentoEntregaContaDto>();
            try
            {
                var query = _entregaRepository.GetAll()
                    .Include(c => c.ContaMedica)
                    .Include(s => s.ContaMedica.Status)

                    ;
                itemrEntregaContas = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    // .OrderBy(input.Sorting)
                    // .PageBy(input)
                    .Where(x => x.ContaMedica.Status.Codigo == "2")
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoEntregaContaDto>>();

                return new PagedResultDto<FaturamentoEntregaContaDto>(
                    itemrEntregaContas,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ContasQuitacaoPacienteDto>> ListarContasQuitacaoPaciente(ContasQuitacaoPacienteInput input)
        {
            var qtdEntregaContas = 0;
            var itensDto = new List<ContasQuitacaoPacienteDto>();
            try
            {
                var query = _entregaRepository.GetAll()
                    .Include(x => x.ContaMedica)
                    .Include(x => x.ContaMedica.Convenio)
                    .Include(x => x.ContaMedica.Convenio.SisPessoa)
                    .Include(x => x.ContaMedica.Paciente)
                    .Include(x => x.ContaMedica.Paciente.SisPessoa)
                    .Include(x => x.ContaMedica.Guia)
                    .Include(x => x.ContaMedica.Atendimento)

                    .WhereIf((input.ConvenioId.HasValue), e => e.ContaMedica.ConvenioId == input.ConvenioId)
                    .WhereIf((input.PacienteId.HasValue), e => e.ContaMedica.PacienteId == input.PacienteId)
                    .WhereIf((input.ModalidadeAtendimento == "i"), e => e.ContaMedica.Atendimento.IsInternacao)
                    .WhereIf((input.ModalidadeAtendimento == "e"), e => e.ContaMedica.Atendimento.IsAmbulatorioEmergencia)
                    .WhereIf((input.Situacao == "g"), e => e.IsGlosa)
                    .WhereIf((input.Situacao == "q"), e => e.ValorRecebido == e.ValorConta)
                    .WhereIf((input.Situacao == "a"), e => e.ValorRecebido != e.ValorConta)
                    .WhereIf(input.StartDateEntrega.HasValue && input.EndDateEntrega.HasValue,
                        e => input.StartDateEntrega.Value <= e.DataEntrega && e.DataEntrega <= input.EndDateEntrega.Value);
                qtdEntregaContas = await query.CountAsync();

                var itens = await query
                    .AsNoTracking()
                       .OrderBy(x => x.Id)
                       .PageBy(input)
                    .ToListAsync();

                itensDto = itens.Select(x => new ContasQuitacaoPacienteDto()
                {
                    Id = x.Id,
                    EntregaLoteId = x.EntregaLoteId,
                    DataEntrega = x.DataEntrega,
                    DataInicio = x.ContaMedica.DataInicio,
                    DataFim = x.ContaMedica.DataFim,
                    Matricula = x.ContaMedica.Matricula,
                    TipoGuia = x.ContaMedica.Guia.Descricao,
                    NumeroGuia = x.ContaMedica.NumeroGuia,
                    PacienteNomeCompleto = x.ContaMedica.Paciente.NomeCompleto,
                    ConvenioNomeFantasia = x.ContaMedica.Convenio.NomeFantasia,
                    ValorRecebidoAnterior = x.ValorRecebido,
                    ValorRecebidoAtual = x.ValorRecebidoTemp
                }).ToList();

                return new PagedResultDto<ContasQuitacaoPacienteDto>(
                    qtdEntregaContas,
                    itensDto
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<bool> AlterarValorRecebidoAtual(long id, float newValue)
        {
            try
            {
                var entregaConta = await _entregaRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if(entregaConta != null)
                {
                    entregaConta.ValorRecebidoTemp = newValue;
                    await _entregaRepository.UpdateAsync(entregaConta);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AlterarValorGlosaRecuperavelTemp(long id, float newValue)
        {
            try
            {
                var entregaConta = await _entregaRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (entregaConta != null)
                {
                    entregaConta.ValorGlosaRecuperavelTemp = newValue;
                    await _entregaRepository.UpdateAsync(entregaConta);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AlterarValorGlosaIrrecuperavelTemp(long id, float newValue)
        {
            try
            {
                var entregaConta = await _entregaRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (entregaConta != null)
                {
                    entregaConta.ValorGlosaIrrecuperavelTemp = newValue;
                    await _entregaRepository.UpdateAsync(entregaConta);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarEntregues(ListarEntregasInput input)
        {
            var itemrEntregaContas = 0;
            List<FaturamentoEntregaConta> itens;
            List<FaturamentoEntregaContaDto> itensDtos = new List<FaturamentoEntregaContaDto>();
            try
            {
                var query = _entregaRepository.GetAll()
                    .Include(c => c.ContaMedica)
                    .Include(c => c.ContaMedica.Atendimento)
                    .Include(c => c.ContaMedica.Atendimento.Paciente)
                    .Include(c => c.ContaMedica.Atendimento.Paciente.SisPessoa)
                    .Include(s => s.ContaMedica.Status)
                    .Include(s => s.ContaMedica.Paciente)
                    .Include(s => s.ContaMedica.Paciente.SisPessoa)
                    .Include(s => s.ContaMedica.Convenio)
                    .Include(s => s.ContaMedica.Convenio.SisPessoa)
                    .Include(s => s.ContaMedica.Status)

                    .WhereIf(input.IsEmergencia, e => e.ContaMedica.Atendimento.IsAmbulatorioEmergencia)
                    .WhereIf(input.IsInternacao, e => e.ContaMedica.Atendimento.IsInternacao)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId), e => e.ContaMedica.EmpresaId.ToString() == input.EmpresaId)
                    .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ContaMedica.ConvenioId.ToString() == input.ConvenioId)
                    .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.ContaMedica.Atendimento.PacienteId.ToString() == input.PacienteId)
                    .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.ContaMedica.MedicoId.ToString() == input.MedicoId)
                    .WhereIf(input.TipoGuiaId != null, e => e.ContaMedica.Atendimento.FatGuiaId == input.TipoGuiaId)
                    ;

                itemrEntregaContas = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    // .OrderBy(input.Sorting)
                    // .PageBy(input)
                    .Where(x => x.ContaMedica.Status.Codigo == "3")
                    .ToListAsync();


                foreach (var item in itens)
                {
                    itensDtos.Add(FaturamentoEntregaContaDto.Mapear(item));
                }


                // itensDtos = itens.MapTo<List<FaturamentoEntregaContaDto>>();

                return new PagedResultDto<FaturamentoEntregaContaDto>(
                    itemrEntregaContas,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarParaLotesGerados(ListarEntregasInput input)
        {
            var itemrEntregaContas = 0;
            List<FaturamentoEntregaConta> itens;
            List<FaturamentoEntregaContaDto> itensDtos = new List<FaturamentoEntregaContaDto>();
            try
            {
                var query = _entregaRepository.GetAll()
                    .Include(c => c.ContaMedica)
                    .Include(c => c.ContaMedica.Atendimento)
                    .Include(c => c.ContaMedica.Atendimento.Paciente)
                    .Include(c => c.ContaMedica.Atendimento.Paciente.SisPessoa)
                    .Include(s => s.ContaMedica.Status)
                    .Include(s => s.ContaMedica.Paciente)
                    .Include(s => s.ContaMedica.Paciente.SisPessoa)
                    .Include(s => s.ContaMedica.Convenio)
                    .Include(s => s.ContaMedica.Convenio.SisPessoa)

                    ;
                itemrEntregaContas = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                    // .OrderBy(input.Sorting)
                    // .PageBy(input)
                    .Where(x => x.EntregaLoteId == input.LoteId)
                //    .Where(x => x.ContaMedica.Status.Codigo == "3")
                    .ToListAsync();

                // itensDtos = itens.MapTo<List<FaturamentoEntregaContaDto>>();

                foreach (var item in itens)
                {
                    itensDtos.Add(FaturamentoEntregaContaDto.Mapear(item));
                }


                return new PagedResultDto<FaturamentoEntregaContaDto>(
                    itemrEntregaContas,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoEntregaContaDto input)
        {
            try
            {
                var EntregaConta = input.MapTo<FaturamentoEntregaConta>();

                if (input.Id.Equals(0))
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task CriarVarias(CrudEntregaContaInput input)
        {
            try
            {
                foreach (var contaId in input.ContasIds)
                {
                    var conta = _contaRepository.Get(contaId);
                    conta.StatusId = 3;
                    await _contaRepository.UpdateAsync(conta);

                    var entregaConta = new FaturamentoEntregaConta();
                    entregaConta.ContaMedicaId = contaId;

                    // Usuario que entregou a conta
                    var usuario = GetCurrentUser();
                    entregaConta.UsuarioEntregaId = usuario.Id;

                    entregaConta.DataEntrega = DateTime.Now;

                    // Atualizando Status de entrega dos itens das contas
                    var itens = _contaItemRepository.GetAll().Where(x => x.FaturamentoContaId == contaId).ToList();
                    foreach (var i in itens)
                    {
                        i.StatusId = 4;
                        _contaItemRepository.Update(i);
                    }

                    await _entregaRepository.InsertAsync(entregaConta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task CancelarEntregas(CrudEntregaContaInput input)
        {
            try
            {
                if (input.ContasIds.Count() == 0)
                    return;

                var contas = new List<FaturamentoConta>();

                foreach (var contaId in input.ContasIds)
                {
                    var entrega = _entregaRepository.Get(contaId);
                    var conta = _contaRepository.Get((long)entrega.ContaMedicaId);
                    conta.StatusId = 2;
                    entrega.EntregaLoteId = null;
                    await _contaRepository.UpdateAsync(conta);
                    await _entregaRepository.DeleteAsync(entrega.Id);
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroCancelarEntrega"), ex);
            }
        }

        public async Task RemoverDoLote(long id)
        {
            try
            {
                var entrega = _entregaRepository.Get(id);
                var conta = _contaRepository.Get((long)entrega.ContaMedicaId);
                conta.StatusId = 3;
                entrega.EntregaLoteId = null;
                await _contaRepository.UpdateAsync(conta);
                await _entregaRepository.UpdateAsync(entrega);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroRemoverEntregaDoLote"), ex);
            }
        }


        public async Task Excluir(FaturamentoEntregaContaDto input)
        {
            try
            {
                await _entregaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoEntregaContaDto> Obter(long id)
        {
            try
            {
                var query = await _entregaRepository.GetAsync(id);
                //.GetAll()
                //.Where(m => m.Id == id)
                //.FirstOrDefaultAsync();

                var item = query.MapTo<FaturamentoEntregaContaDto>();
                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEntregasInput input)
        {
            try
            {
                var result = await Listar(input);
                var itens = result.Items;
                return _listarEntregaContasExcelExporter.ExportToFile(itens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ListResultDto<FaturamentoEntregaContaDto>> ListarTodos()
        {
            try
            {
                var query = _entregaRepository.GetAll();
                var faturamentoEntregaContasDto = await query.ToListAsync();
                return new ListResultDto<FaturamentoEntregaContaDto> { Items = faturamentoEntregaContasDto.MapTo<List<FaturamentoEntregaContaDto>>() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<ListResultDto<FaturamentoItemDto>> ListarPrevio (teste itens)
        //{
        //    try
        //    {
        //        List<FaturamentoItemDto> listItens = new List<FaturamentoItemDto>();

        //        foreach (var id in itens.ids.Distinct())
        //        {
        //            //var novoItem = _faturamentoItemAppService.Obter(id);

        //            var novoItem = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter(id));
        //            listItens.Add(novoItem);
        //        }

        //        return new ListResultDto<FaturamentoItemDto> { Items = listItens.MapTo<List<FaturamentoItemDto>>() };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoItemDto> faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _entregaRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();
                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }




    }
}
