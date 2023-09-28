using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;

using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class ComentarioAppService : SWMANAGERAppServiceBase, IComentarioAppService
    {
        private readonly IRepository<Comentario, long> _comentarioRepository;

        public ComentarioAppService(
            IRepository<Comentario, long> comentarioRepository
            )
        {
            _comentarioRepository = comentarioRepository;
        }

        public async Task CriarOuEditar(ComentarioDto input)
        {
            try
            {
                var comentario = input.MapTo<Comentario>();

                if (input.Id.Equals(0))
                {
                    await _comentarioRepository.InsertAsync(comentario);
                }
                else
                {
                    await _comentarioRepository.UpdateAsync(comentario);
                }

                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();
                var user = users.FirstOrDefault(q => q.id == (long)input.UsuarioId);
                input.NomeUsuario = user.text;

                SMWEMensageria.SMWEHubApp.ApenderNovoComentario(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ComentarioDto input)
        {
            try
            {
                await _comentarioRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ComentarioDto> Obter(long id)
        {
            try
            {
                var query = await _comentarioRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var comentario = query.MapTo<ComentarioDto>();

                return comentario;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ComentarioDto>> ListarTodos()
        {
            try
            {
                var projetos = await _comentarioRepository
                    .GetAll()

                    .AsNoTracking()
                    .ToListAsync();

                var comentariosDtos = projetos
                    .MapTo<List<ComentarioDto>>();

                return new PagedResultDto<ComentarioDto> { Items = comentariosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ComentarioDto>> ListarPorTarefa(long tarefaId)
        {
            try
            {
                var comentarios = await _comentarioRepository
                    .GetAll()
                    .Where(t => t.TarefaId.Equals(tarefaId))
                    .AsNoTracking()
                    .ToListAsync();

                List<ComentarioDto> comentariosDtos = new List<ComentarioDto>();//comentarios.MapTo<List<ComentarioDto>>();
                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();
                foreach (var c in comentarios)
                {
                    var novoComentario = c.MapTo<ComentarioDto>();


                    var user = users.FirstOrDefault(q => q.id == (long)c.UsuarioId);
                    novoComentario.NomeUsuario = user.text;

                    comentariosDtos.Add(novoComentario);
                }

                var comentariosOrdenados = comentariosDtos.OrderByDescending(e => e.DataRegistro).ToList();

                return new PagedResultDto<ComentarioDto> { Items = comentariosOrdenados };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> GetUsuarioLogadoAsync()
        {
            //var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            //if (user == null)
            //{
            //    throw new ApplicationException("There is no current user!");
            //}
            var usuarioId = AbpSession.GetUserId();
            return usuarioId;
            //  return user;
        }

    }
}
