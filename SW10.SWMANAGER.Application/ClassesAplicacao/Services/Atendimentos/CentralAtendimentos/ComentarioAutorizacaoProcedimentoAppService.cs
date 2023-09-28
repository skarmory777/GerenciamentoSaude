using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    using Abp.Auditing;

    public class ComentarioAutorizacaoProcedimentoAppService : SWMANAGERAppServiceBase, IComentarioAutorizacaoProcedimentoAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ComentarioAutorizacaoProcedimento, long> _comentarioAutorizacaoProcedimentoRepository;



        public ComentarioAutorizacaoProcedimentoAppService(IRepository<ComentarioAutorizacaoProcedimento, long> comentarioAutorizacaoProcedimentoRepository
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _comentarioAutorizacaoProcedimentoRepository = comentarioAutorizacaoProcedimentoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        public DefaultReturn<ComentarioAutorizacaoProcedimentoDto> Criar(ComentarioAutorizacaoProcedimentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<ComentarioAutorizacaoProcedimentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var comentarioAutorizacaoProcedimento = ComentarioAutorizacaoProcedimentoDto.Mapear(input);//.MapTo<ComentarioAutorizacaoProcedimento>();

                    //comentarioAutorizacaoProcedimento.DataRegistro = DateTime.Now;
                    comentarioAutorizacaoProcedimento.UsuarioId = AbpSession.GetUserId();

                    _comentarioAutorizacaoProcedimentoRepository.Insert(comentarioAutorizacaoProcedimento);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
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

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ComentarioAutorizacaoProcedimentoDto>> ListarPorAutorizacao(long autorizacaoProcedimentoId)
        {
            try
            {
                var comentarios = await _comentarioAutorizacaoProcedimentoRepository
                    .GetAll()
                    .Where(t => t.AutorizacaoProcedimentoId.Equals(autorizacaoProcedimentoId))
                    .AsNoTracking()
                    .ToListAsync();

                List<ComentarioAutorizacaoProcedimentoDto> comentariosDtos = new List<ComentarioAutorizacaoProcedimentoDto>();//comentarios.MapTo<List<ComentarioDto>>();

                foreach (var comentario in comentarios)
                {
                    var novoComentario = ComentarioAutorizacaoProcedimentoDto.Mapear(comentario);//.MapTo<ComentarioAutorizacaoProcedimentoDto>();


                    var user = UserManager.Users.Where(w => w.Id == comentario.UsuarioId).FirstOrDefault();
                    if (user != null)
                    {
                        novoComentario.NomeUsuario = user.Name;
                    }
                    comentariosDtos.Add(novoComentario);
                }

                var comentariosOrdenados = comentariosDtos.OrderBy(e => e.DataRegistro).ToList();

                return new PagedResultDto<ComentarioAutorizacaoProcedimentoDto> { Items = comentariosOrdenados };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
