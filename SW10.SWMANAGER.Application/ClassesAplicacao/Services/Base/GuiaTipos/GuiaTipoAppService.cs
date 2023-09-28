using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.GuiaTipos
{
    public class GuiaTipoAppService : SWMANAGERAppServiceBase, IGuiaTipoAppService
    {
        private readonly IRepository<GuiaTipo, long> _guiaTipoRepository;

        public GuiaTipoAppService(IRepository<GuiaTipo, long> guiaTipoRepository)
        {
            _guiaTipoRepository = guiaTipoRepository;
        }

        public async Task CriarOuEditar(GuiaTipoDto input)
        {
            try
            {
                var guiaTipo = input.MapTo<GuiaTipo>();
                if (input.Id.Equals(0))
                {
                    await _guiaTipoRepository.InsertAsync(guiaTipo);
                }
                else
                {
                    await _guiaTipoRepository.UpdateAsync(guiaTipo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(GuiaTipoDto input)
        {
            try
            {
                await _guiaTipoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<GuiaTipoDto>> ListarTodos()
        {
            List<GuiaTipo> guiaTipos;
            List<GuiaTipoDto> guiaTiposDtos = new List<GuiaTipoDto>();
            try
            {
                guiaTipos = await _guiaTipoRepository
                    .GetAll()
                    .ToListAsync();

                guiaTiposDtos = guiaTipos
                    .MapTo<List<GuiaTipoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<GuiaTipoDto> { Items = guiaTiposDtos };
        }

        public async Task<GuiaTipoDto> Obter(long id)
        {
            try
            {
                var result = await _guiaTipoRepository.GetAsync(id);
                var guiaTipo = result.MapTo<GuiaTipoDto>();
                return guiaTipo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
