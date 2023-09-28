namespace SW10.SWMANAGER.ClassesAplicacao.Services.Tarefas.Dto
{
    public class ListarTarefasInput : ListarInput
    {

        public string StatusId { get; set; }

        public bool FiltrarData { get; set; }

        public string PrioridadeId { get; set; }

        public string ModuloId { get; set; }

        public string ResponsavelId { get; set; }

        public string CriadorId { get; set; }

        public string TipoData { get; set; }

        public string TarefaId { get; set; }

        //public bool IsMostrarGlobal { get; set; }
    }
}
