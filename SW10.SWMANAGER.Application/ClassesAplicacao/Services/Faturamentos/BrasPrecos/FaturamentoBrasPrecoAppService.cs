#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasPrecos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos
{
    public class FaturamentoBrasPrecoAppService : SWMANAGERAppServiceBase, IFaturamentoBrasPrecoAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoBrasPreco, long> _brasPrecoRepository;
        private readonly IListarBrasPrecosExcelExporter _listarBrasPrecosExcelExporter;

        public FaturamentoBrasPrecoAppService(IRepository<FaturamentoBrasPreco, long> brasPrecoRepository, IListarBrasPrecosExcelExporter listarBrasPrecosExcelExporter)
        {
            _brasPrecoRepository = brasPrecoRepository;
            _listarBrasPrecosExcelExporter = listarBrasPrecosExcelExporter;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoBrasPrecoDto>> Listar(ListarBrasPrecosInput input)
        {
            var brasPrecosCount = 0;
            List<FaturamentoBrasPreco> brasPrecos;
            List<FaturamentoBrasPrecoDto> brasPrecosDtos = new List<FaturamentoBrasPrecoDto>();
            try
            {
                var query = _brasPrecoRepository
                    .GetAll()
                    .Include(m => m.BrasApresentacao)
                    .Include(m => m.BrasLaboratorio)
                    .Include(m => m.BrasItem)
                    ;

                brasPrecosCount = await query
                    .CountAsync();

                brasPrecos = await query
                    .AsNoTracking()
                     .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                brasPrecosDtos = brasPrecos.MapTo<List<FaturamentoBrasPrecoDto>>();

                return new PagedResultDto<FaturamentoBrasPrecoDto>(brasPrecosCount, brasPrecosDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoBrasPrecoDto input)
        {
            try
            {
                var BrasPreco = input.MapTo<FaturamentoBrasPreco>();
                if (input.Id.Equals(0))
                {
                    await _brasPrecoRepository.InsertAsync(BrasPreco);
                }
                else
                {
                    await _brasPrecoRepository.UpdateAsync(BrasPreco);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoBrasPrecoDto input)
        {
            try
            {
                await _brasPrecoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoBrasPrecoDto> Obter(long id)
        {
            try
            {
                var query = await _brasPrecoRepository
                    .GetAll()
                    .Include(i => i.BrasApresentacao)
                    .Include(i => i.BrasLaboratorio)
                    .Include(i => i.BrasItem)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var brasPreco = query
                    .MapTo<FaturamentoBrasPrecoDto>();

                return brasPreco;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FaturamentoBrasPrecoDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _brasPrecoRepository
                    .GetAll()
                    ;

                var result = await query.FirstOrDefaultAsync();

                var brasPreco = result.MapTo<FaturamentoBrasPrecoDto>();

                return brasPreco;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarBrasPrecosInput input)
        {
            try
            {
                var result = await Listar(input);
                var brasPrecos = result.Items;
                return _listarBrasPrecosExcelExporter.ExportToFile(brasPrecos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ICollection<FaturamentoBrasApresentacao>> ListarBrasApresentacaoPorBrasItem(long brasItemId)
        {
            List<FaturamentoBrasPreco> brasPrecos;
            List<FaturamentoBrasPrecoDto> brasPrecosDtos = new List<FaturamentoBrasPrecoDto>();
            try
            {
                var query = from m in _brasPrecoRepository.GetAll()
                            where m.BrasItemId == brasItemId
                            select m;

                brasPrecos = await query
                    .Include(m => m.BrasApresentacao)
                    .AsNoTracking()
                    .ToListAsync();

                brasPrecosDtos = brasPrecos
                    .MapTo<List<FaturamentoBrasPrecoDto>>();

                ICollection<FaturamentoBrasApresentacao> brasApresentacoes = new List<FaturamentoBrasApresentacao>();

                foreach (var brasPreco in brasPrecosDtos)
                {
                    brasApresentacoes.Add(brasPreco.BrasApresentacao);
                }

                return brasApresentacoes;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<FaturamentoBrasLaboratorio>> ListarBrasLaboratorioPorBrasItem(long brasItemId)
        {
            List<FaturamentoBrasPreco> brasPrecos;
            List<FaturamentoBrasPrecoDto> brasPrecosDtos = new List<FaturamentoBrasPrecoDto>();
            try
            {
                var query = from m in _brasPrecoRepository.GetAll()
                            where m.BrasItemId == brasItemId
                            select m;

                brasPrecos = await query
                    .Include(m => m.BrasLaboratorio)
                    .AsNoTracking()
                    .ToListAsync();

                brasPrecosDtos = brasPrecos
                    .MapTo<List<FaturamentoBrasPrecoDto>>();

                ICollection<FaturamentoBrasLaboratorio> brasLaboratorios = new List<FaturamentoBrasLaboratorio>();

                foreach (var brasPreco in brasPrecosDtos)
                {
                    brasLaboratorios.Add(brasPreco.BrasLaboratorio);
                }

                return brasLaboratorios;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
