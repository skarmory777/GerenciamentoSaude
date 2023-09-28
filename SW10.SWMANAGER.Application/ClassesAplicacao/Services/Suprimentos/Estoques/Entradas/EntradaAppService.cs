using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas
{
    public class EntradaAppService : SWMANAGERAppServiceBase, IEntradaAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(CriarOuEditarEntrada input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    var objEntrada = input.MapTo<Entrada>();
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await entradaRepository.Object.InsertAsync(objEntrada);

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await entradaRepository.Object.UpdateAsync(objEntrada);

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

        public async Task Excluir(CriarOuEditarEntrada input)
        {
            try
            {
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    await entradaRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<EntradaDto>> Listar(ListarEntradasInput input)
        {
            var contarEntradas = 0;
            List<Entrada> Entradas;
            List<EntradaDto> EntradasDtos = new List<EntradaDto>();
            try
            {
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    var query = entradaRepository.Object.GetAll().WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.NumeroDocumento.ToString().Contains(input.Filtro));

                    contarEntradas = await query.CountAsync();
                    Entradas = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    EntradasDtos = Entradas.MapTo<List<EntradaDto>>();

                    return new PagedResultDto<EntradaDto>(contarEntradas, EntradasDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<EntradaDto>> ListarTodos()
        {
            var contarEntradas = 0;
            List<Entrada> Entradas;
            List<EntradaDto> EntradasDtos = new List<EntradaDto>();
            try
            {
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    var query = entradaRepository.Object.GetAll().AsNoTracking();

                    contarEntradas = await query.CountAsync();

                    Entradas = await query.ToListAsync();

                    EntradasDtos = Entradas.MapTo<List<EntradaDto>>();

                    return new ListResultDto<EntradaDto> { Items = EntradasDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    var query = await entradaRepository.Object.GetAll().AsNoTracking()
                        .WhereIf(!input.IsNullOrEmpty(), m => m.NumeroDocumento.ToString().Contains(input))
                        .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.NumeroDocumento.ToString() })
                        .ToListAsync();

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEntradasInput input)
        {
            try
            {
                using (var listarEntradasExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarEntradasExcelExporter>())
                {
                    var result = await this.Listar(input);
                    var entradas = result.Items;
                    return listarEntradasExcelExporter.Object.ExportToFile(entradas.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarEntrada> Obter(long id)
        {
            try
            {
                using (var entradaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Entrada, long>>())
                {
                    var query = entradaRepository.Object.GetAll().AsNoTracking().Where(m => m.Id == id);

                    var result = await query.FirstOrDefaultAsync();
                    var entrada = result.MapTo<CriarOuEditarEntrada>();

                    return entrada;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<CriarOuEditarEntradaItem>> ListarItens(ListarEntradasInput input)
        {
            var contarEntradas = 0;
            List<CriarOuEditarEntradaItem> iEntradasItem = null;
            //List<CriarOuEditarEntradaItem> EntradasItemDtos = new List<CriarOuEditarEntradaItem>();
            try
            {
                if (input.Filtro != "0")
                {
                    var query = await Obter(Convert.ToInt64(input.Filtro));
                    //var entrada = query.MapTo<Entrada>();
                    contarEntradas = query.EntradaItem.Count();//entrada.EntradaItem.Count();


                    iEntradasItem = query.EntradaItem.ToList();
                    //.AsNoTracking()
                    //.ToListAsync();

                    //EntradasItemDtos = iEntradasItem
                    //    .MapTo<List<CriarOuEditarEntradaItem>>();
                }

                return new PagedResultDto<CriarOuEditarEntradaItem>(contarEntradas, iEntradasItem);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
