using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Orcamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class OrcamentoAppService : SWMANAGERAppServiceBase, IOrcamentoAppService
    {
        private readonly IRepository<Orcamento, long> _orcamentoRepository;
        //    private readonly IListarOrcamentosExcelExporter _listarOrcamentosExcelExporter;

        public OrcamentoAppService(
            IRepository<Orcamento, long> orcamentoRepository
            //,
            //     IListarOrcamentosExcelExporter listarOrcamentosExcelExporter
            )
        {
            _orcamentoRepository = orcamentoRepository;
            //    _listarOrcamentosExcelExporter = listarOrcamentosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarOrcamento input)
        {
            try
            {
                var orcamento = input.MapTo<Orcamento>();

                if (input.Id.Equals(0))
                {
                    await _orcamentoRepository.InsertAsync(orcamento);
                }
                else
                {
                    await _orcamentoRepository.UpdateAsync(orcamento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(long id)
        {
            try
            {
                await _orcamentoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<OrcamentoDto>> ListarTodos()
        {
            var contarLeitos = 0;
            List<Orcamento> leitos;
            List<OrcamentoDto> leitosDtos = new List<OrcamentoDto>();
            try
            {
                var query = _orcamentoRepository
                    .GetAll();

                contarLeitos = await query
                                   .CountAsync().ConfigureAwait(false);

                leitos = await query
                             .AsNoTracking()
                             .ToListAsync().ConfigureAwait(false);

                leitosDtos = leitos
                    .MapTo<List<OrcamentoDto>>();

                return new PagedResultDto<OrcamentoDto>(
                contarLeitos,
                leitosDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<PagedResultDto<OrcamentoDto>> Listar(ListarOrcamentosInput input)
        //{
        //    var contarOrcamentos = 0;
        //    List<Orcamento> orcamentos;
        //    List<OrcamentoDto> orcamentosDtos = new List<OrcamentoDto>();
        //    try
        //    {
        //        var query = _orcamentoRepository
        //            .GetAll()
        //            .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
        //                m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper())
        //            );

        //        contarOrcamentos = await query
        //            .CountAsync();

        //        orcamentos = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //        orcamentosDtos = orcamentos
        //            .MapTo<List<OrcamentoDto>>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //    return new PagedResultDto<OrcamentoDto>(
        //        contarOrcamentos,
        //        orcamentosDtos
        //        );
        //}

        //public async Task<FileDto> ListarParaExcel(ListarOrcamentosInput input)
        //{
        //    try
        //    {
        //        //var result = await Listar(input);
        //        var orcamentos = result.Items;
        //        return _listarOrcamentosExcelExporter.ExportToFile(orcamentos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        //public async Task<ICollection<Orcamento>> ListarPorMedico(long? medicoId, long? medicoEspecialidadeId, DateTime start, DateTime end)
        //{
        //    try
        //    {
        //        //var query = await _orcamentoRepository
        //        return await _orcamentoRepository
        //            .GetAll()
        //            .WhereIf(medicoId.HasValue && medicoId >= 0, m => m.MedicoId == medicoId)
        //            .WhereIf(medicoEspecialidadeId.HasValue && medicoEspecialidadeId >= 0, m => m.MedicoEspecialidade.EspecialidadeId == medicoEspecialidadeId)
        //            .Where(m => m.DataAgendamento >= start && m.DataAgendamento <= end) // start <= m.DataAgendamento && end >= m.DataAgendamento)
        //            .ToListAsync();
        //        //return query.MapTo<List<OrcamentoDto>>();
        //        //var orcamentos = await query
        //        ////.AsNoTracking()
        //        //.ToListAsync();
        //        //var orcamentosDtos = orcamentos.MapTo<List<OrcamentoDto>>();
        //        //return orcamentosDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        //public async Task<ICollection<Orcamento>> ListarPorData(DateTime start, DateTime end)
        //{
        //    return await _orcamentoRepository.GetAllListAsync(m => m.DataAgendamento >= start && m.DataAgendamento <= end);
        //}

        public async Task<CriarOuEditarOrcamento> Obter(long id)
        {
            try
            {
                var result = await _orcamentoRepository
                    .GetAsync(id);

                var orcamento = result
                    .MapTo<CriarOuEditarOrcamento>();

                return orcamento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<bool> ChecarDisponibilidade(long medicoDisponibilidadeId, DateTime hora, long id = 0)
        //{
        //    var query = await _orcamentoRepository.GetAllListAsync(m => m.OrcamentoMedicoDisponibilidadeId == medicoDisponibilidadeId && m.HoraAgendamento == hora);
        //    if (query.Count() > 0)
        //    {
        //        //no caso de edição, se o horário for o próprio do registro, ele deve ser incluído.
        //        if (id > 0)
        //        {
        //            var agendamento = query.FirstOrDefault().Id == id;
        //            if (agendamento)
        //            {
        //                return true;
        //            }
        //        }
        //        return false; //Não está disponível
        //    }
        //    else
        //    {
        //        return true; //Está disponível
        //    }
        //}

    }
}
