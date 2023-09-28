using Abp.AutoMapper;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    [AutoMap(typeof(NFe.Classes.Servicos.DistribuicaoDFe.Schemas.eventoInfEvento), typeof(NFe.Classes.Servicos.DistribuicaoDFe.Schemas.retInfEvento))]
    public class ProcEventoNFeOutput
    {
        public decimal versao { get; set; }
        public byte cOrgao { get; set; }

        public byte tpAmb { get; set; }

        public string CNPJ { get; set; }

        public string chNFe { get; set; }

        public DateTime dhEvento { get; set; }

        public uint tpEvento { get; set; }

        public byte nSeqEvento { get; set; }

        public decimal verEvento { get; set; }

        public string Id { get; set; }

        public string descEvento { get; set; }

        public string nProt { get; set; }

        public string xJust { get; set; }

        public string verAplic { get; set; }

        public byte cStat { get; set; }

        public string xMotivo { get; set; }

        public string xEvento { get; set; }

        public string CNPJDest { get; set; }

        public string emailDest { get; set; }

        public DateTime dhRegEvento { get; set; }

    }
}
