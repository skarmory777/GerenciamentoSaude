using Abp.AutoMapper;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    [AutoMap(typeof(NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resNFe))]
    public class ResNFeOutput
    {
        public decimal versao { get; set; }

        public string chNFe { get; set; }

        public ulong CNPJ { get; set; }

        public ulong CPF { get; set; }

        public string xNome { get; set; }

        public string IE { get; set; }

        public DateTime dhEmi { get; set; }

        public string ProxyDhEmi { get; set; }

        public byte tpNF { get; set; }

        public decimal vNF { get; set; }

        public string digVal { get; set; }

        public DateTime dhRecbto { get; set; }

        public ulong nProt { get; set; }

        public byte cSitNFe { get; set; }
    }
}
