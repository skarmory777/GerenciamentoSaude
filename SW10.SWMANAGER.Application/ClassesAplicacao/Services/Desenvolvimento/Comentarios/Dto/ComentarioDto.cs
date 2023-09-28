using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [AutoMap(typeof(Comentario))]
    public class ComentarioDto : CamposPadraoCRUDDto
    {
        public long? UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public long TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }

        public DateTime? DataRegistro { get; set; }

        public string Conteudo { get; set; }

        public string GetNomeUsuario()
        {
            return NomeUsuario;
        }

        public string GetDataRegistro()
        {
            return DataRegistro.ToString();
        }
    }
}
