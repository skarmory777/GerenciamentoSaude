using Abp.AutoMapper;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    [AutoMap(typeof(NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resEvento))]
    public class ResEventoOutput
    {
        public decimal versao { get; set; }

        public string cOrgao { get; set; }

        public ulong CNPJ { get; set; }

        public ulong CPF { get; set; }

        public string chNFe { get; set; }

        public DateTime dhEvento { get; set; }

        public string ProxydhEvento { get; set; }

        public string tpEvento { get; set; }

        public string nSeqEvento { get; set; }

        public string xEvento { get; set; }

        public DateTime dhRecbto { get; set; }

        public ulong nProt { get; set; }
    }
}
