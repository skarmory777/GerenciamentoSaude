using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias
{
    public class RelacaoGuiaCampoAppService : SWMANAGERAppServiceBase, IRelacaoGuiaCampoAppService
    {
        public async Task<CriarOuEditarRelacaoGuiaCampo> CriarOuEditar(CriarOuEditarRelacaoGuiaCampo input)
        {
            try
            {
                using (var _relacaoGuiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RelacaoGuiaCampo, long>>())
                {
                    var relacaoGuiaCampo = _relacaoGuiaCampoRepository.Object.GetAllList().FirstOrDefault(
                    c => c.GuiaId == input.GuiaId &&
                         c.GuiaCampoId == input.GuiaCampoId
                    );

                    if (relacaoGuiaCampo == null)
                    {
                        relacaoGuiaCampo = RelacaoGuiaCampoDto.Mapear(input);

                        relacaoGuiaCampo.Id = await _relacaoGuiaCampoRepository.Object.InsertAndGetIdAsync(RelacaoGuiaCampoDto.Mapear(input));
                    }
                    else
                    {
                        relacaoGuiaCampo.CoordenadaX = input.CoordenadaX;
                        relacaoGuiaCampo.CoordenadaY = input.CoordenadaY;
                        await _relacaoGuiaCampoRepository.Object.UpdateAsync(relacaoGuiaCampo);
                    }

                    return RelacaoGuiaCampoDto.Mapear(relacaoGuiaCampo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarRelacaoGuiaCampo input)
        {
            try
            {
                using (var _relacaoGuiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RelacaoGuiaCampo, long>>())
                    await _relacaoGuiaCampoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<CriarOuEditarRelacaoGuiaCampo> Obter(long id)
        {
            try
            {
                using (var _relacaoGuiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RelacaoGuiaCampo, long>>())
                {
                    var query = await _relacaoGuiaCampoRepository.Object
                    .GetAll()
                    .Include(m => m.Guia)
                    .Include(m => m.GuiaCampo)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var relacaoGuiaCampo = RelacaoGuiaCampoDto.Mapear(query);


                    return relacaoGuiaCampo;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
                
        public PagedResultDto<GuiaCampoDto> ListarParaConjunto(long conjuntoId, long guiaId)
        {
            try
            {
                using (var _guiaCampoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<GuiaCampo, long>>())
                {
                    var subCampos = _guiaCampoRepository.Object
                    .GetAll()
                    .Where(s => s.ConjuntoId == conjuntoId && s.IsSubItem == true)
                    ;

                    var subCamposDto = new List<GuiaCampoDto>();

                    foreach (var sub in subCampos)
                    {
                        subCamposDto.Add(GuiaCampoDto.Mapear(sub));
                    }

                    return new PagedResultDto<GuiaCampoDto>(0, subCamposDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
