namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos
{
    using Abp.Auditing;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;
    using System.Data.Entity;
    using System.Linq;

    public class ModeloTextoGuiaAppService : SWMANAGERAppServiceBase, IModeloTextoGuiaAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public TextoModeloDto Obter(long? guiaId, long? emprasId)
        {
            using (var modeloTextoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModelo, long>>())
            using (var textoModeloEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloEmpresa, long>>())
            using (var textoModeloGuiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TextoModeloGuia, long>>())
            {
                var textoModeloEmpresa = textoModeloEmpresaRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.EmpresaId == emprasId).Select(x => x.TextoId).ToList();
                if (textoModeloEmpresa.Any())
                {
                    var textoModeloGuias = textoModeloGuiaRepository.Object.GetAll().AsNoTracking()
                        .Where(w => w.FatGuiaId == guiaId && textoModeloEmpresa.Contains(w.TextoId)).ToList();

                    foreach (var itemEmpresaTextoId in textoModeloEmpresa)
                    {
                        var textoModeloGuia = textoModeloGuias.SingleOrDefault(w => w.TextoId == itemEmpresaTextoId);

                        if (textoModeloGuia != null && textoModeloGuia.FatGuiaId == guiaId)
                        {
                            return modeloTextoRepository.Object.GetAll().AsNoTracking().Select(
                                item => new TextoModeloDto
                                {
                                    Id = item.Id,
                                    Codigo = item.Codigo,
                                    Descricao = item.Descricao,
                                    Texto = item.Texto,
                                    IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia,
                                    IsInternacao = item.IsInternacao,
                                    IsMostraAtendimento = item.IsMostraAtendimento
                                }).SingleOrDefault(o => o.Id == textoModeloGuia.TextoId);
                        }
                    }
                }

                return new TextoModeloDto();
            }
        }

        //public async Task<PagedResultDto<TextoModeloDto>> Listar(ListarModeloTextoInput input)
        //{

        //    var lst = new List<TextoModeloDto>();
        //    var query = _modeloTextoRepository.GetAll()
        //    .Where(w => string.IsNullOrEmpty(input.Filtro) || (w.Codigo.ToUpper().Contains(input.Filtro) || (w.Descricao.ToUpper().Contains(input.Filtro))));

        //    var count = await query.CountAsync();

        //    var collection = await query
        //            .AsNoTracking()
        //            .OrderBy(input.Sorting)
        //            .PageBy(input)
        //            .ToListAsync();

        //    foreach (var item in collection)
        //    {
        //        var entityTextoModeloEmpresa = _textoModeloEmpresaRepository.GetAll()
        //                                               .Where(w => w.TextoId == item.Id)
        //                                               .ToList();
        //        var entityTextoModeloGuia = _textoModeloGuiaRepository.GetAll()
        //                                               .Where(w => w.TextoId == item.Id)
        //                                               .ToList();

        //        var lstTextoModeloEmpresaId = new List<GridEmpresaDto>();

        //        foreach (var itemTextoModeloEmpresa in entityTextoModeloEmpresa)
        //        {

        //            var entityEmpresa = _empresaRepository.GetAll()
        //                                           .Where(w => w.Id == itemTextoModeloEmpresa.EmpresaId)
        //                                           .SingleOrDefault();

        //            lstTextoModeloEmpresaId.Add(new GridEmpresaDto()
        //            {

        //                EspecialidadeDescricao = entityEmpresa.Codigo + " - " + entityEmpresa.NomeFantasia,
        //                EspecialidadeId = itemTextoModeloEmpresa.EmpresaId

        //            });
        //        }

        //        var lstTextoModeloGuiaId = new List<GridGuiaDto>();

        //        foreach (var itemTextoModeloGuia in entityTextoModeloGuia)
        //        {
        //            var entityGuia = _guiaRepository.GetAll()
        //                                           .Where(w => w.Id == itemTextoModeloGuia.FatGuiaId)
        //                                           .SingleOrDefault();

        //            lstTextoModeloGuiaId.Add(new GridGuiaDto()
        //            {

        //                TipoGuiaDescricao = entityGuia.Codigo + " - " + entityGuia.Descricao,
        //                TipoGuiaId = itemTextoModeloGuia.FatGuiaId

        //            });
        //        }

        //        var obj = new TextoModeloDto()
        //        {
        //            Id = item.Id,
        //            Codigo = item.Codigo,
        //            Descricao = item.Descricao,
        //            Texto = item.Texto,
        //            IsAmbulatorioEmergencia = item.IsAmbulatorioEmergencia,
        //            Empresas = lstTextoModeloEmpresaId,
        //            Guias = lstTextoModeloGuiaId,
        //            IsInternacao = item.IsInternacao,
        //            IsMostraAtendimento = item.IsMostraAtendimento
        //        };
        //        lst.Add(obj);
        //    }


        //    return new PagedResultDto<TextoModeloDto>(
        //           count,
        //           lst
        //           );
        //}

        //[UnitOfWork]
        //public async Task<DefaultReturn<TextoModeloDto>> CriarOuEditar(TextoModeloDto input)
        //{

        //    var _retornoPadrao = new DefaultReturn<TextoModeloDto>();
        //    _retornoPadrao.Warnings = new List<ErroDto>();
        //    _retornoPadrao.Errors = new List<ErroDto>();
        //    input.Id = input.Id == null? 0 : input.Id;

        //    try
        //    {
        //        var textoModelo = new TextoModelo()
        //        {
        //            Codigo = input.Codigo,
        //            Descricao = input.Descricao,
        //            IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia,
        //            IsInternacao = input.IsInternacao,
        //            IsMostraAtendimento = input.IsMostraAtendimento,
        //            Texto = input.Texto
        //        };

        //        if (input.Id.Equals(0))
        //        {
        //            using (var unitOfWork = _unitOfWorkManager.Begin())
        //            {
        //                input.Id = await _modeloTextoRepository.InsertAndGetIdAsync(textoModelo);

        //                if (input.LstEmpresaId.Count > 0)
        //                {
        //                    foreach (var item in input.LstEmpresaId)
        //                    {
        //                        var obj = new TextoModeloEmpresa()
        //                        {
        //                            TextoId = input.Id,
        //                            EmpresaId = item
        //                        };
        //                        var idEmp = await _textoModeloEmpresaRepository.InsertAndGetIdAsync(obj);
        //                    }
        //                }

        //                if (input.LstFatGuiaId.Count > 0)
        //                {
        //                    foreach (var item in input.LstFatGuiaId)
        //                    {
        //                        var obj = new TextoModeloGuia()
        //                        {
        //                            TextoId = input.Id,
        //                            FatGuiaId = item
        //                        };
        //                        var idGuia = await _textoModeloGuiaRepository.InsertAndGetIdAsync(obj);
        //                    }
        //                }

        //                unitOfWork.Complete();
        //                _unitOfWorkManager.Current.SaveChanges();
        //            }
        //        }
        //        else
        //        {
        //            using (var unitOfWork = _unitOfWorkManager.Begin())
        //            {

        //                var entity = _modeloTextoRepository.GetAll()
        //                                               .Where(w => w.Id == input.Id)
        //                                               .FirstOrDefault();

        //                var entityTextoModeloEmpresa = _textoModeloEmpresaRepository.GetAll()
        //                                               .Where(w => w.TextoId == input.Id)
        //                                               .ToList();

        //                var entityTextoModeloGuia = _textoModeloGuiaRepository.GetAll()
        //                                                       .Where(w => w.TextoId == input.Id)
        //                                                       .ToList();

        //                if (entity != null)
        //                {
        //                    entity.Codigo = input.Codigo;
        //                    entity.Descricao = input.Descricao;
        //                    entity.IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia;
        //                    entity.IsInternacao = input.IsInternacao;
        //                    entity.IsMostraAtendimento = input.IsMostraAtendimento;
        //                    entity.Texto = input.Texto;
        //                }

        //                var obj = await _modeloTextoRepository.UpdateAsync(entity);

        //                //if (entityTextoModeloEmpresa.Count > 0)
        //                //{

        //                foreach (var item in entityTextoModeloEmpresa)
        //                {
        //                    await _textoModeloEmpresaRepository.DeleteAsync(item);
        //                }

        //                foreach (var itemInputEmpresa in input.LstEmpresaId)
        //                {
        //                    var objTextoModeloEmpresa = new TextoModeloEmpresa()
        //                    {
        //                        TextoId = input.Id,
        //                        EmpresaId = itemInputEmpresa
        //                    };
        //                    var idEmp = await _textoModeloEmpresaRepository.InsertAndGetIdAsync(objTextoModeloEmpresa);
        //                }
        //                //}

        //                //if (input.LstFatGuiaId.Count > 0)
        //                //{

        //                foreach (var item in entityTextoModeloGuia)
        //                {
        //                    await _textoModeloGuiaRepository.DeleteAsync(item);
        //                }

        //                foreach (var itemInputGuia in input.LstFatGuiaId)
        //                {
        //                    var objTextoModeloGuia = new TextoModeloGuia()
        //                    {
        //                        TextoId = input.Id,
        //                        FatGuiaId = itemInputGuia
        //                    };
        //                    var idGuia = await _textoModeloGuiaRepository.InsertAndGetIdAsync(objTextoModeloGuia);
        //                }
        //                //}

        //                unitOfWork.Complete();
        //                _unitOfWorkManager.Current.SaveChanges();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var erro = new ErroDto();

        //        if (ex.InnerException != null)
        //        {
        //            var inner = ex.InnerException;
        //            erro = new ErroDto();
        //            erro.CodigoErro = inner.HResult.ToString();
        //            erro.Descricao = inner.Message.ToString();
        //            _retornoPadrao.Errors.Add(erro);
        //        }
        //        else
        //        {
        //            erro.CodigoErro = ex.HResult.ToString();
        //            erro.Descricao = ex.Message.ToString();
        //            _retornoPadrao.Errors.Add(erro);
        //        }
        //    }

        //    return _retornoPadrao;
        //}

        //[UnitOfWork]
        //public async Task<bool> Excluir(long id)
        //{
        //    try
        //    {
        //        using (var unitOfWork = _unitOfWorkManager.Begin())
        //        {
        //            var entityTextoModeloEmpresa = _textoModeloEmpresaRepository.GetAll()
        //                                              .Where(w => w.TextoId == id)
        //                                              .ToList();

        //            var entityTextoModeloGuia = _textoModeloGuiaRepository.GetAll()
        //                                                   .Where(w => w.TextoId == id)
        //                                                   .ToList();

        //            foreach (var item in entityTextoModeloEmpresa)
        //            {
        //                await _textoModeloEmpresaRepository.DeleteAsync(item);
        //            }

        //            foreach (var item in entityTextoModeloGuia)
        //            {
        //                await _textoModeloGuiaRepository.DeleteAsync(item);
        //            }

        //            await _modeloTextoRepository.DeleteAsync(id);
        //            unitOfWork.Complete();
        //            _unitOfWorkManager.Current.SaveChanges();
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }

        //    return true;
        //}
    }
}
