namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown
{
    public class DropdownInput
    {
        public string search { get; set; }
        public string page { get; set; }
        public string totalPorPagina { get; set; }
        public string filtro { get; set; }
        public string[] filtros { get; set; }
        public string id { get; set; }
        public string tabelaDominioTiss { get; set; }
        /// <summary>
        /// Valores que devem ser ignorados no resultset
        /// Ex.: Todos os produtos, exceto os com ids já listados em um grid, por exemplo
        /// </summary>
        public string[] excecoes { get; set; }

    }
}
