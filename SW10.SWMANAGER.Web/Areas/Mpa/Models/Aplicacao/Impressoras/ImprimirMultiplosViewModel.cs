namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Impressoras
{
    public class ImprimirMultiplosViewModel
    {
        public ImprimirMultiplosViewModel()
        {

        }

        public ImprimirMultiplosViewModel(string targetAction, string name)
        {
            this.TargetAction = targetAction;
            this.Name = name;
        }

        public string TargetAction { get; set; }

        public string Name { get; set; }
    }
}