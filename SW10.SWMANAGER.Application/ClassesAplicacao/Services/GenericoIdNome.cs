namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public class GenericoIdNome
    {
        public GenericoIdNome()
        {

        }

        public GenericoIdNome(long id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public long Id { get; set; }
        public string Nome { get; set; }
        public long IdGrid { get; set; }
    }
}
