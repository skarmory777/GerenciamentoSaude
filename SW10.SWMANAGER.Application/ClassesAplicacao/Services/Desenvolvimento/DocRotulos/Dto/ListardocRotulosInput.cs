namespace SW10.SWMANAGER.ClassesAplicacao.Services.DocRotulos.Dto
{
    public class ListardocRotulosInput : ListarInput
    {

        //public override string Codigo { get; set; }
        //public override string Descricao { get; set; }
        public string Titulo { get; set; }
        public float Ordem { get; set; }
        public bool IsCapitulo { get; set; }
        public bool IsSessao { get; set; }
        public bool IsAssunto { get; set; }
        public bool IsModulo { get; set; }
        public bool IsStatus { get; set; }

        public bool IsPrioridade { get; set; }
    }
}
