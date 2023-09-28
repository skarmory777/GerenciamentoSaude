using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.Projetos
{
    public class ProjetosViewModel
    {
        public string Filtro { get; set; }
        public ProjetoDto Projeto { get; set; }
        public TarefaDto Tarefa { get; set; }
        public List<ComentarioDto> Comentarios { get; set; }

        public KeyValuePair<long, string> UsuarioLogado { get; set; }

        public ProjetosViewModel()
        {
            Projeto = new ProjetoDto();
            Tarefa = new TarefaDto();
            Comentarios = new List<ComentarioDto>();
            UsuarioLogado = new KeyValuePair<long, string>();
        }

        public void AdicionarComentario(ComentarioDto novoComentario)
        {
            Comentarios.Add(novoComentario);
        }
    }
}