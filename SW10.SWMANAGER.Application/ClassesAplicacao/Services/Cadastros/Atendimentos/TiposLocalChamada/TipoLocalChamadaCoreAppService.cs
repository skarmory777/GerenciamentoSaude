using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposLocalChamada.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposLocalChamada
{
    public class TipoLocalChamadaCoreAppService : SWMANAGERAppServiceBase, ITipoLocalChamadaCoreAppService
    {
        public async Task<DefaultReturn<TipoLocalChamadaDto>> CriarOuEditar(TipoLocalChamadaDto input)
        {
            using (var _TipoLocalChamadaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoLocalChamada, long>>())
            {
                var _retornoPadrao = new DefaultReturn<TipoLocalChamadaDto>();
                _retornoPadrao.Warnings = new List<ErroDto>();
                _retornoPadrao.Errors = new List<ErroDto>();

                try
                {
                    var tipoLocalChamada = TipoLocalChamadaDto.Mapear(input, true);
                    if (input.Id.Equals(0))
                    {
                        await _TipoLocalChamadaRepositorio.Object.InsertOrUpdateAsync(tipoLocalChamada);
                        _retornoPadrao.ReturnObject = input;
                    }
                    else
                    {
                        var get = _TipoLocalChamadaRepositorio.Object.GetAll().Where(t => t.Id == input.Id).Include(l => l.LocalChamadas).FirstOrDefault();
                        get.Codigo = input.Codigo;
                        get.Descricao = input.Descricao;
                        get.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                        var lst = new List<LocalChamada>();
                        foreach (var item in input.LocalChamadas)
                        {
                            if (item.Id.Equals(0))
                            {
                                var newItem = new LocalChamada();
                                newItem.Codigo = item.Codigo;
                                newItem.Descricao = item.Descricao;
                                newItem.TipoLocalChamadaId = input.Id;
                                lst.Add(newItem);
                            }
                            else
                            {
                                foreach (var itemGet in get.LocalChamadas)
                                {
                                    if (item.Id == itemGet.Id)
                                    {
                                        itemGet.Codigo = item.Codigo;
                                        itemGet.Descricao = item.Descricao;
                                        itemGet.TipoLocalChamadaId = item.TipoLocalChamadaId;
                                        lst.Add(itemGet);
                                    }
                                }
                            }
                        }
                        get.LocalChamadas = lst;
                        await _TipoLocalChamadaRepositorio.Object.UpdateAsync(get);
                        //_retornoPadrao.ReturnObject = ConverteObj<TipoLocalChamada>(get);
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
        }

        public async Task<DefaultReturn<TipoLocalChamadaDto>> Excluir(TipoLocalChamadaDto input)
        {
            using (var _TipoLocalChamadaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoLocalChamada, long>>())
            {
                var _retornoPadrao = new DefaultReturn<TipoLocalChamadaDto>();
                _retornoPadrao.Warnings = new List<ErroDto>();
                _retornoPadrao.Errors = new List<ErroDto>();

                try
                {
                    var get = _TipoLocalChamadaRepositorio.Object.GetAll().Where(t => t.Id == input.Id).Include(l => l.LocalChamadas).FirstOrDefault();
                    await _TipoLocalChamadaRepositorio.Object.DeleteAsync(get);
                    _retornoPadrao.ReturnObject = TipoLocalChamadaDto.Mapear(get, true);
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
        }

        public async Task<PagedResultDto<TipoLocalChamadaDto>> Listar(ListarLocalChamadaCoreInput input)
        {
            using (var _TipoLocalChamadaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoLocalChamada, long>>())
            {
                var contarTipoLocalChamada = 0;
                List<TipoLocalChamada> TipoLocalChamada;
                List<TipoLocalChamadaDto> TipoLocalChamadaDtos = new List<TipoLocalChamadaDto>();
                try
                {
                    var query = _TipoLocalChamadaRepositorio.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(input.Filtro)
                        );

                    contarTipoLocalChamada = await query
                        .CountAsync();

                    TipoLocalChamada = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoLocalChamadaDtos = TipoLocalChamadaDto.Mapear(TipoLocalChamada);

                    return new PagedResultDto<TipoLocalChamadaDto>(
                        contarTipoLocalChamada,
                        TipoLocalChamadaDtos
                        );
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        public async Task<TipoLocalChamadaDto> Obter(long id)
        {
            using (var _TipoLocalChamadaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoLocalChamada, long>>())
            {
                try
                {
                    var query = _TipoLocalChamadaRepositorio.Object
                        .GetAll()
                        .Where(m => m.Id == id)
                        .Include(u => u.UnidadeOrganizacional)
                        .Include(i => i.LocalChamadas)
                        .FirstOrDefault();

                    return TipoLocalChamadaDto.Mapear(query, true);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }
    }
}