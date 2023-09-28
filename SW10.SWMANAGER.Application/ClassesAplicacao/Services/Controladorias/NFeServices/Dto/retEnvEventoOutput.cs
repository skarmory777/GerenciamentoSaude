using Abp.AutoMapper;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    [AutoMap(typeof(NFe.Classes.Servicos.Evento.retEnvEvento))]
    public class RetEnvEventoOutput
    {
        public RetEnvEventoOutput()
        {
            //this.retEvento = new List<retEvento>();
        }
        public string versao { get; set; }

        public int idLote { get; set; }

        public TipoAmbiente tpAmb { get; set; }

        public string verAplic { get; set; }

        public Estado cOrgao { get; set; }

        public int cStat { get; set; }

        public string xMotivo { get; set; }

        //public List<NFe.Classes.Servicos.Evento.retEvento> retEvento { get; set; }
    }
}