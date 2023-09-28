using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public class GuiaCampoAppService : SWMANAGERAppServiceBase, IGuiaCampoAppService
    {
        public async Task<KeyValuePair<CriarOuEditarGuiaCampo, bool>> CriarOuEditar(CriarOuEditarGuiaCampo input)
        {
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    bool novo = true;
                    var guiaCampo = _guiaCampoRepository.Object.GetAllList().FirstOrDefault(
                        c => c.IsConjunto == input.IsConjunto &&
                        c.Descricao == input.Descricao &&
                        c.MaximoElementos == input.MaximoElementos &&
                        c.IsSubItem == input.IsSubItem
                        );

                    if (guiaCampo == null)
                    {
                        guiaCampo = GuiaCampoDto.Mapear(input);
                        guiaCampo.Id = await _guiaCampoRepository.Object.InsertAndGetIdAsync(guiaCampo);
                    }
                    else
                    {

                        guiaCampo.ConjuntoId = input.ConjuntoId;
                        guiaCampo.CoordenadaX = input.CoordenadaX;
                        guiaCampo.CoordenadaY = input.CoordenadaY;
                        guiaCampo.Descricao = input.Descricao;
                        guiaCampo.IsConjunto = input.IsConjunto;
                        guiaCampo.IsSubItem = input.IsSubItem;
                        //    guiaCampo.MaximoElementos = input.MaximoElementos;

                        guiaCampo = await _guiaCampoRepository.Object.UpdateAsync(guiaCampo);
                        novo = false;
                    }

                    var gc = CriarOuEditarGuiaCampo.Mapear(guiaCampo);

                    return new KeyValuePair<CriarOuEditarGuiaCampo, bool>(gc, novo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarGuiaCampo input)
        {
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                    await _guiaCampoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<GuiaCampoDto>> Listar(ListarGuiaCamposInput input)
        {
            var contarGuiaCampos = 0;
            List<GuiaCampo> motivosAlta;
            List<GuiaCampoDto> motivosAltaDtos = new List<GuiaCampoDto>();
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    var query = _guiaCampoRepository.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                    contarGuiaCampos = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = GuiaCampoDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GuiaCampoDto>(
                contarGuiaCampos,
                motivosAltaDtos
                );
        }

        public PagedResultDto<GuiaCampoDto> ListarParaConjunto(long conjuntoId)
        {
            var contarGuiaCampos = 0;
            List<GuiaCampo> motivosAlta;
            List<GuiaCampoDto> motivosAltaDtos = new List<GuiaCampoDto>();
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    var query = _guiaCampoRepository.Object
                    .GetAll()
                    .Where(
                        c => c.ConjuntoId == conjuntoId
                    );

                    contarGuiaCampos = query
                        .Count();

                    motivosAlta = query
                        .ToList();

                    motivosAltaDtos = GuiaCampoDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GuiaCampoDto>(
                contarGuiaCampos,
                motivosAltaDtos
                );
        }

        public async Task<CriarOuEditarGuiaCampo> Obter(long id)
        {
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    var query = await _guiaCampoRepository.Object.GetAsync(id);
                    var motivoAlta = CriarOuEditarGuiaCampo.Mapear(query);

                    return motivoAlta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public GuiaCampoDto ObterDto(long id)
        {
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    var query = _guiaCampoRepository.Object.Get(id);

                    var motivoAlta = GuiaCampoDto.Mapear(query);

                    return motivoAlta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
