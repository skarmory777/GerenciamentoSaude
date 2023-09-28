using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos
{
    public class ModeloTextoAppService : SWMANAGERAppServiceBase, IModeloTextoAppService
    {        
        public async Task<TextoModeloDto> Obter(long id)
        {
            using (var _modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())
            {
                var item = await _modeloTextoRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);

                var obj = new TextoModeloDto()
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    Descricao = item.Descricao,
                    Texto = item.Texto,
                    IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia,
                    IsInternacao = item.IsInternacao,
                    IsMostraAtendimento = item.IsMostraAtendimento,
                    TipoModeloId = item.TipoModeloId,
                    TamanhoModeloId = item.TamanhoModeloId,
                    TamanhoModelo = item.TamanhoModelo,
                    TipoModelo = item.TipoModelo
                };

                return obj;
            }
        }

        /// <inheritdoc />
        public async Task<PagedResultDto<TextoModeloDto>> Listar(ListarModeloTextoInput input)
        {
            using (var _modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())
            using (var _textoModeloEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloEmpresa, long>>())
            using (var _textoModeloGuiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloGuia, long>>())
            using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoGuia, long>>())
            using (var _empresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Empresa, long>>())
            {
                var lst = new List<TextoModeloDto>();
                var modeloTexto = _modeloTextoRepository.Object.GetAll().AsNoTracking()
                .Where(w => string.IsNullOrEmpty(input.Filtro) || (w.Codigo.Contains(input.Filtro) || w.Descricao.Contains(input.Filtro)));

                var count = await modeloTexto.CountAsync().ConfigureAwait(false);

                var collection = await modeloTexto
                                     .OrderBy(input.Sorting)
                                     .PageBy(input)
                                     .ToListAsync().ConfigureAwait(false);

                foreach (var item in collection)
                {
                    var entityTextoModeloEmpresa = await _textoModeloEmpresaRepository.Object.GetAll().AsNoTracking()
                                                       .Where(w => w.TextoId == item.Id)
                                                       .Select(x => new { x.EmpresaId })
                                                       .ToListAsync().ConfigureAwait(false);

                    var entityTextoModeloGuia = await _textoModeloGuiaRepository.Object.GetAll().AsNoTracking()
                                                    .Where(w => w.TextoId == item.Id)
                                                    .Select(x => new { x.FatGuiaId })
                                                    .ToListAsync().ConfigureAwait(false);

                    var lstTextoModeloEmpresaId = new List<GridEmpresaDto>();

                    foreach (var itemTextoModeloEmpresa in entityTextoModeloEmpresa)
                    {
                        var entityEmpresa = await _empresaRepository.Object.GetAll().AsNoTracking()
                                                .Where(w => w.Id == itemTextoModeloEmpresa.EmpresaId)
                                                .Select(x => new { x.Codigo, x.NomeFantasia })
                                                .SingleOrDefaultAsync().ConfigureAwait(false);

                        lstTextoModeloEmpresaId.Add(
                            new GridEmpresaDto
                            {
                                EspecialidadeDescricao = entityEmpresa?.Codigo + " - " + entityEmpresa?.NomeFantasia,
                                EspecialidadeId = itemTextoModeloEmpresa.EmpresaId
                            });
                    }

                    var lstTextoModeloGuiaId = new List<GridGuiaDto>();

                    foreach (var itemTextoModeloGuia in entityTextoModeloGuia)
                    {
                        var entityGuia = await _guiaRepository.Object.GetAll().AsNoTracking()
                                             .Where(w => w.Id == itemTextoModeloGuia.FatGuiaId)
                                             .Select(x => new { x.Codigo, x.Descricao }).SingleOrDefaultAsync()
                                             .ConfigureAwait(false);

                        lstTextoModeloGuiaId.Add(new GridGuiaDto
                        {
                            TipoGuiaDescricao = entityGuia?.Codigo + " - " + entityGuia?.Descricao,
                            TipoGuiaId = itemTextoModeloGuia.FatGuiaId
                        });
                    }

                    var obj = new TextoModeloDto
                    {
                        Id = item.Id,
                        Codigo = item.Codigo,
                        Descricao = item.Descricao,
                        Texto = item.Texto,
                        IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia,
                        Empresas = lstTextoModeloEmpresaId,
                        Guias = lstTextoModeloGuiaId,
                        IsInternacao = item.IsInternacao,
                        IsMostraAtendimento = item.IsMostraAtendimento
                    };
                    lst.Add(obj);
                }

                return new PagedResultDto<TextoModeloDto>(count, lst);
            }
        }

        [UnitOfWork]
        public async Task<DefaultReturn<TextoModeloDto>> CriarOuEditar(TextoModeloDto input)
        {
            using (var _modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())
            using (var _textoModeloEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloEmpresa, long>>())
            using (var _textoModeloGuiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloGuia, long>>())
            using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                var retornoPadrao = new DefaultReturn<TextoModeloDto> { Warnings = new List<ErroDto>(), Errors = new List<ErroDto>() };
                input.Id = input.Id ?? 0;

                try
                {
                    var textoModelo = new TextoModelo()
                    {
                        Codigo = input.Codigo,
                        Descricao = input.Descricao,
                        IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia,
                        IsInternacao = input.IsInternacao,
                        IsMostraAtendimento = input.IsMostraAtendimento,
                        Texto = input.Texto,
                        TipoModeloId = input.TipoModeloId,
                        TamanhoModeloId = input.TamanhoModeloId
                    };

                    if (input.Id.Equals((long)0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _modeloTextoRepository.Object.InsertAndGetIdAsync(textoModelo).ConfigureAwait(false);

                            if (input.LstEmpresaId.Count > 0)
                            {
                                foreach (var item in input.LstEmpresaId)
                                {
                                    var obj = new TextoModeloEmpresa()
                                    {
                                        TextoId = input.Id,
                                        EmpresaId = item
                                    };
                                    var idEmp = await _textoModeloEmpresaRepository.Object.InsertAndGetIdAsync(obj).ConfigureAwait(false);
                                }
                            }

                            if (input.LstFatGuiaId.Count > 0)
                            {
                                foreach (var item in input.LstFatGuiaId)
                                {
                                    var obj = new TextoModeloGuia()
                                    {
                                        TextoId = input.Id,
                                        FatGuiaId = item
                                    };
                                    var idGuia = await _textoModeloGuiaRepository.Object.InsertAndGetIdAsync(obj).ConfigureAwait(false);
                                }
                            }

                            await unitOfWork.CompleteAsync().ConfigureAwait(false);
                            await _unitOfWorkManager.Object.Current.SaveChangesAsync().ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            var entity = _modeloTextoRepository.Object.GetAll().FirstOrDefault(w => w.Id == input.Id);

                            var entityTextoModeloEmpresa = await _textoModeloEmpresaRepository.Object.GetAll()
                                                               .Where(w => w.TextoId == input.Id).ToListAsync()
                                                               .ConfigureAwait(false);

                            var entityTextoModeloGuia = await _textoModeloGuiaRepository.Object.GetAll()
                                                            .Where(w => w.TextoId == input.Id).ToListAsync()
                                                            .ConfigureAwait(false);

                            if (entity != null)
                            {
                                entity.Codigo = input.Codigo;
                                entity.Descricao = input.Descricao;
                                entity.IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia;
                                entity.IsInternacao = input.IsInternacao;
                                entity.IsMostraAtendimento = input.IsMostraAtendimento;
                                entity.Texto = input.Texto;
                                entity.TipoModeloId = input.TipoModeloId;
                                entity.TamanhoModeloId = input.TamanhoModeloId;
                            }

                            var obj = await _modeloTextoRepository.Object.UpdateAsync(entity).ConfigureAwait(false);

                            foreach (var item in entityTextoModeloEmpresa)
                            {
                                await _textoModeloEmpresaRepository.Object.DeleteAsync(item).ConfigureAwait(false);
                            }

                            foreach (var itemInputEmpresa in input.LstEmpresaId)
                            {
                                var objTextoModeloEmpresa = new TextoModeloEmpresa()
                                {
                                    TextoId = input.Id,
                                    EmpresaId = itemInputEmpresa
                                };
                                var idEmp = await _textoModeloEmpresaRepository.Object
                                                .InsertAndGetIdAsync(objTextoModeloEmpresa).ConfigureAwait(false);
                            }

                           
                            foreach (var item in entityTextoModeloGuia)
                            {
                                await _textoModeloGuiaRepository.Object.DeleteAsync(item).ConfigureAwait(false);
                            }

                            foreach (var itemInputGuia in input.LstFatGuiaId)
                            {
                                var objTextoModeloGuia = new TextoModeloGuia()
                                {
                                    TextoId = input.Id,
                                    FatGuiaId = itemInputGuia
                                };
                                var idGuia = await _textoModeloGuiaRepository.Object.InsertAndGetIdAsync(objTextoModeloGuia)
                                                 .ConfigureAwait(false);
                            }

                            // }
                            foreach (var itemInputEmpresa in input.LstEmpresaId)
                            {
                                var entityTxtModeloEmpresa = await _textoModeloEmpresaRepository.Object.GetAll()
                                                                 .Where(
                                                                     w => w.TextoId == input.Id
                                                                          && w.EmpresaId == itemInputEmpresa).ToListAsync()
                                                                 .ConfigureAwait(false);

                                if (entityTxtModeloEmpresa.Count() > 1)
                                {
                                    for (var i = 1; i < entityTxtModeloEmpresa.Count(); i++)
                                    {
                                        await _textoModeloEmpresaRepository.Object.DeleteAsync(entityTxtModeloEmpresa[i])
                                            .ConfigureAwait(false);
                                    }
                                }
                            }

                            await unitOfWork.CompleteAsync().ConfigureAwait(false);
                            await _unitOfWorkManager.Object.Current.SaveChangesAsync().ConfigureAwait(false);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        var inner = ex.InnerException;
                        retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                    }
                    else
                    {
                        retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                    }
                }

                return retornoPadrao;
            }
        }

        [UnitOfWork]
        public async Task<bool> Excluir(long id)
        {
            try
            {
                using (var _modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())
                using (var _textoModeloEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloEmpresa, long>>())
                using (var _textoModeloGuiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloGuia, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    var entityTextoModeloEmpresa = await _textoModeloEmpresaRepository.Object.GetAll()
                                                       .Where(w => w.TextoId == id).ToListAsync().ConfigureAwait(false);

                    var entityTextoModeloGuia = await _textoModeloGuiaRepository.Object.GetAll()
                                                    .Where(w => w.TextoId == id).ToListAsync().ConfigureAwait(false);

                    foreach (var item in entityTextoModeloEmpresa)
                    {
                        await _textoModeloEmpresaRepository.Object.DeleteAsync(item).ConfigureAwait(false);
                    }

                    foreach (var item in entityTextoModeloGuia)
                    {
                        await _textoModeloGuiaRepository.Object.DeleteAsync(item).ConfigureAwait(false);
                    }

                    await _modeloTextoRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                    await unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await _unitOfWorkManager.Object.Current.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The is guide.
        /// </summary>
        /// <param name="empresaId">
        /// The empresa id.
        /// </param>
        /// <param name="guideId">
        /// The guide id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> IsGuide(long empresaId, long guideId)
        {
            using (var _textoModeloEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloEmpresa, long>>())
            using (var _textoModeloGuiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloGuia, long>>())
            {

                var entityTextoModeloEmpresa = await _textoModeloEmpresaRepository.Object.GetAll()
                                               .Where(w => w.EmpresaId == empresaId)
                                               .ToListAsync().ConfigureAwait(false);
                if (entityTextoModeloEmpresa.Any())
                {
                    foreach (var item in entityTextoModeloEmpresa)
                    {
                        var entityGuia = await _textoModeloGuiaRepository.Object.GetAll()
                                             .Where(w => w.TextoId == item.TextoId && w.FatGuiaId == guideId).AsNoTracking().AnyAsync().ConfigureAwait(false);

                        if (entityGuia)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }


        /// <summary>
        /// The obter por tipo async.
        /// </summary>
        /// <param name="tipoModeloId">
        /// The tipo modelo id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<TextoModelo> ObterPorTipoAsync(long tipoModeloId)
        {
            using (var _modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())            
                return _modeloTextoRepository.Object.GetAll().Include(c => c.TipoModelo.Variaveis).AsNoTracking().OrderBy(c => c.Id)
                .FirstOrDefaultAsync(c => c.TipoModeloId == tipoModeloId);
        }
    }
}
