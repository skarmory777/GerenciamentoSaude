using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Ghostscript.NET.Rasterizer;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias
{
    public class GuiaAppService : SWMANAGERAppServiceBase, IGuiaAppService
    {
        #region Metodos padrao

        public async Task<CriarOuEditarGuia> CriarOuEditar(CriarOuEditarGuia input)
        {
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                {
                    var guia = CriarOuEditarGuia.Mapear(input);

                    // Gerar PNG
                    int xDpi = 100;
                    int yDpi = 100;
                    int pagina = 1;


                    using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                    {
                        Stream pdfStream = new MemoryStream(guia.ModeloPDF);

                        rasterizer.Open(pdfStream);
                        var imagemPDF = rasterizer.GetPage(xDpi, yDpi, pagina);

                        using (var memoryStream = new MemoryStream())
                        {
                            imagemPDF.Save(memoryStream, ImageFormat.Png);
                            var imagemPNG = memoryStream.ToArray();
                            guia.ModeloPNG = imagemPNG;
                            guia.ModeloPNGMimeType = "image/png";
                        }
                    }

                    if (input.Id.Equals(0))
                    {
                        guia.Id = await _guiaRepository.Object.InsertAndGetIdAsync(guia);
                    }
                    else
                    {
                        await _guiaRepository.Object.UpdateAsync(guia);
                    }

                    return CriarOuEditarGuia.Mapear(guia);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task<CriarOuEditarGuia> AtualizarCoordenadas(CriarOuEditarGuia guia, string camposAlterados)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer(new SimpleTypeResolver());
                    GuiaCampoDto[] camposAlteradosDto = jsonSerializer.Deserialize<GuiaCampoDto[]>(camposAlterados);
                    GuiaCampoDto[] camposAntigos = jsonSerializer.Deserialize<GuiaCampoDto[]>(guia.CamposJson);

                    foreach (var campo in camposAntigos)
                    {
                        var campoAlterado = camposAlteradosDto.Where(ca => ca.Descricao == campo.Descricao).FirstOrDefault();

                        if (campoAlterado != null)
                        {
                            campo.CoordenadaX = campoAlterado.CoordenadaX;
                            campo.CoordenadaY = campoAlterado.CoordenadaY;
                        }

                        foreach (var subCampo in campo.SubConjuntos)
                        {
                            var subCampoAlterado = camposAlteradosDto.Where(sca => sca.Descricao == subCampo.Descricao).FirstOrDefault();

                            if (subCampoAlterado != null)
                            {
                                subCampo.CoordenadaX = subCampoAlterado.CoordenadaX;
                                subCampo.CoordenadaY = subCampoAlterado.CoordenadaY;
                            }
                        }
                    }


                    guia.CamposJson = jsonSerializer.Serialize(camposAntigos);
                    var guiaCore = CriarOuEditarGuia.Mapear(guia);
                    await _guiaRepository.Object.UpdateAsync(guiaCore);
                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                    return guia;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarGuia input)
        {
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                    await _guiaRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<GuiaDto>> Listar(ListarGuiasInput input)
        {
            var contarGuias = 0;
            List<Guia> guias;
            List<GuiaDto> guiasDtos = new List<GuiaDto>();
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                {
                    var query = _guiaRepository.Object
                    .GetAll()
                    .Include(m => m.Originaria)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Descricao.Contains(input.Filtro)
                    );

                    contarGuias = await query
                        .CountAsync();

                    guias = await query
                        //.AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    guiasDtos = GuiaDto.Mapear(guias);

                    return new PagedResultDto<GuiaDto>(
                    contarGuias,
                    guiasDtos
                    );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GuiaDto>> ListarTodos()
        {
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                {
                    var guias = await _guiaRepository.Object
                    .GetAll()
                    .Include(m => m.Originaria)
                    .AsNoTracking()
                    .ToListAsync();

                    var guiasDtos = GuiaDto.Mapear(guias);

                    return new PagedResultDto<GuiaDto> { Items = guiasDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<CriarOuEditarGuia> Obter(long id)
        {
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                {
                    var query = _guiaRepository.Object
                   .GetAll()
                   .Include(m => m.Originaria);

                    Guia g = await query.FirstOrDefaultAsync(x => x.Id == id);
                    var guia = CriarOuEditarGuia.Mapear(g);

                    return guia;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<bool> SalvarCampos(string camposJson, long guiaId)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer(new SimpleTypeResolver());
            GuiaCampoDto[] campos = new GuiaCampoDto[] { };

            try
            {
                campos = jsonSerializer.Deserialize<GuiaCampoDto[]>(camposJson);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            using (var _guiaCampoAppService = IocManager.Instance.ResolveAsDisposable<IGuiaCampoAppService>())
            foreach (var campo in campos)
            {
                if (campo.IsConjunto)
                {
                    CriarOuEditarGuiaCampo guiaCampo = new CriarOuEditarGuiaCampo();
                    try
                    {
                        var subs = campo.SubConjuntos;
                        var temp = GuiaCampoDto.Mapear(campo);
                        guiaCampo = CriarOuEditarGuiaCampo.Mapear(temp);
                        guiaCampo.SubConjuntos = subs;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    var campoSalvo = await _guiaCampoAppService.Object.CriarOuEditar(guiaCampo);

                    if (campoSalvo.Value == true)
                    {
                        CriarOuEditarRelacaoGuiaCampo relacao = new CriarOuEditarRelacaoGuiaCampo();
                        var campoSalvo2 = campoSalvo.Key;

                        relacao.GuiaId = guiaId;
                        relacao.GuiaCampoId = campoSalvo2.Id;
                        relacao.CoordenadaX = campoSalvo2.CoordenadaX;
                        relacao.CoordenadaY = campoSalvo2.CoordenadaY;

                        using (var _relacaoGuiaCampoAppService = IocManager.Instance.ResolveAsDisposable<IRelacaoGuiaCampoAppService>())
                            await _relacaoGuiaCampoAppService.Object.CriarOuEditar(relacao);
                    }

                    if (guiaCampo.SubConjuntos != null)
                    {
                        foreach (var sub in guiaCampo.SubConjuntos)
                        {
                            var subs = sub.SubConjuntos;//talvez desnecessario
                            var temp = GuiaCampoDto.Mapear(sub);
                            var subMapeado = CriarOuEditarGuiaCampo.Mapear(temp);
                            subMapeado.ConjuntoId = campoSalvo.Key.Id;
                            subMapeado.SubConjuntos = subs;//talvez desnecessario
                            await _guiaCampoAppService.Object.CriarOuEditar(subMapeado);
                        }
                    }
                }
                else
                {
                    CriarOuEditarGuiaCampo guiaCampo = new CriarOuEditarGuiaCampo();
                    try
                    {
                        // teoricamentre nao precisa de subConjuntos
                        var temp = GuiaCampoDto.Mapear(campo);
                        guiaCampo = CriarOuEditarGuiaCampo.Mapear(temp);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    var campoSalvo = await _guiaCampoAppService.Object.CriarOuEditar(guiaCampo);
                }
            }

            return true;
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var _guiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Guia, long>>())
                {
                    //get com filtro
                    var query = from p in _guiaRepository.Object.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                        m.Descricao.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                        .Replace("à", "a").Replace("è", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u")
                        .Replace("â", "a").Replace("ê", "e").Replace("î", "i").Replace("ô", "o").Replace("û", "u")
                        .Replace("ã", "a").Replace("õ", "o")
                        .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U")
                        .Replace("À", "A").Replace("È", "E").Replace("Ì", "I").Replace("Ô", "O").Replace("Ù", "U")
                        .Replace("Â", "A").Replace("Ê", "E").Replace("Î", "I").Replace("Õ", "O").Replace("Û", "U")
                        .Replace("Ã", "A").Replace("Õ", "O")
                        .Contains(dropdownInput.search.ToLower())
                        )
                                orderby p.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                    //paginação 
                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
    #endregion
}

