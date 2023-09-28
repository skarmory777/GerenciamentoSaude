using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Favoritos
{
    public class FavoritoAppService : SWMANAGERAppServiceBase, IFavoritoAppService
    {
        private readonly IRepository<Favorito, long> _favoritoRepository;

        public FavoritoAppService(IRepository<Favorito, long> favoritoRepository)
        {
            _favoritoRepository = favoritoRepository;
        }


        public async Task CriarOuEditar(FavoritoDto input)
        {
            try
            {
                var favorito = input.MapTo<Favorito>();
                if (input.Id.Equals(0))
                {
                    await _favoritoRepository.InsertAsync(favorito);
                }
                else
                {
                    await _favoritoRepository.UpdateAsync(favorito);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FavoritoDto input)
        {
            var fav = _favoritoRepository.GetAll().Where(f => f.Name == input.Name).First();

            var favoritoId = fav.Id;

            try
            {
                await _favoritoRepository.DeleteAsync(favoritoId);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<FavoritoDto>> Listar(long userId)
        {
            List<Favorito> todos;
            List<FavoritoDto> favoritosDtos = new List<FavoritoDto>();
            try
            {
                todos = await _favoritoRepository
                    .GetAll()
                    .ToListAsync();

                var favoritos = todos.Where(f => f.UserId == userId);

                favoritosDtos = favoritos
                    .MapTo<List<FavoritoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<FavoritoDto> { Items = favoritosDtos };
        }

        public async Task<FavoritoDto> Obter(long id)
        {
            try
            {
                var result = await _favoritoRepository.GetAsync(id);
                var favorito = result.MapTo<FavoritoDto>();
                return favorito;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
