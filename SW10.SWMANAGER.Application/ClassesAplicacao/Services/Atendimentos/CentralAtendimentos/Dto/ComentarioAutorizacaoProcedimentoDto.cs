using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    [AutoMap(typeof(ComentarioAutorizacaoProcedimento))]
    public class ComentarioAutorizacaoProcedimentoDto : CamposPadraoCRUDDto
    {
        public long AutorizacaoProcedimentoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public string Conteudo { get; set; }
        public long? UsuarioId { get; set; }
        public string NomeUsuario { get; set; }

        public AutorizacaoProcedimentoDto AutorizacaoProcedimento { get; set; }

        public static ComentarioAutorizacaoProcedimentoDto Mapear(ComentarioAutorizacaoProcedimento comentarioAutorizacaoProcedimento)
        {
            if (comentarioAutorizacaoProcedimento == null)
            {
                return null;
            }

            var comentarioAutorizacaoProcedimentoDto = MapearBase<ComentarioAutorizacaoProcedimentoDto>(comentarioAutorizacaoProcedimento);

            comentarioAutorizacaoProcedimentoDto.AutorizacaoProcedimentoId = comentarioAutorizacaoProcedimento.AutorizacaoProcedimentoId;
            comentarioAutorizacaoProcedimentoDto.DataRegistro = comentarioAutorizacaoProcedimento.DataRegistro;
            comentarioAutorizacaoProcedimentoDto.Conteudo = comentarioAutorizacaoProcedimento.Conteudo;
            comentarioAutorizacaoProcedimentoDto.UsuarioId = comentarioAutorizacaoProcedimento.UsuarioId;

            if (comentarioAutorizacaoProcedimento.AutorizacaoProcedimento != null)
            {
                comentarioAutorizacaoProcedimentoDto.AutorizacaoProcedimento = AutorizacaoProcedimentoDto.Mapear(comentarioAutorizacaoProcedimento.AutorizacaoProcedimento);
            }

            return comentarioAutorizacaoProcedimentoDto;
        }



        public static ComentarioAutorizacaoProcedimento Mapear(ComentarioAutorizacaoProcedimentoDto comentarioAutorizacaoProcedimentoDto)
        {
            if (comentarioAutorizacaoProcedimentoDto == null)
            {
                return null;
            }

            var comentarioAutorizacaoProcedimento = MapearBase<ComentarioAutorizacaoProcedimento>(comentarioAutorizacaoProcedimentoDto);

            comentarioAutorizacaoProcedimento.AutorizacaoProcedimentoId = comentarioAutorizacaoProcedimentoDto.AutorizacaoProcedimentoId;
            comentarioAutorizacaoProcedimento.DataRegistro = comentarioAutorizacaoProcedimentoDto.DataRegistro;
            comentarioAutorizacaoProcedimento.Conteudo = comentarioAutorizacaoProcedimentoDto.Conteudo;
            comentarioAutorizacaoProcedimento.UsuarioId = comentarioAutorizacaoProcedimentoDto.UsuarioId;

            if (comentarioAutorizacaoProcedimento.AutorizacaoProcedimento != null)
            {
                comentarioAutorizacaoProcedimento.AutorizacaoProcedimento = AutorizacaoProcedimentoDto.Mapear(comentarioAutorizacaoProcedimentoDto.AutorizacaoProcedimento);
            }

            return comentarioAutorizacaoProcedimento;
        }



    }
}
