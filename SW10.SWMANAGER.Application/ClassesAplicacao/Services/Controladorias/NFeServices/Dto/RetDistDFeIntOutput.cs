using Abp.AutoMapper;
using System;
using System.Collections.Generic;
namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto
{
    [AutoMap(typeof(NFe.Classes.Servicos.DistribuicaoDFe.retDistDFeInt))]
    public class RetDistDFeIntOutput
    {
        public RetDistDFeIntOutput()
        {
            this.procEventoNFe = new List<ProcEventoNFeOutput>();
            this.resNFe = new List<ResNFeOutput>();
            this.resEvento = new List<ResEventoOutput>();
            this.nfeProc = new List<NfeProcOutput>();
        }

        public decimal versao { get; set; }

        public byte tpAmb { get; set; }

        public string verAplic { get; set; }

        public ushort cStat { get; set; }

        public string xMotivo { get; set; }

        public DateTime dhResp { get; set; }

        public ushort ultNSU { get; set; }

        public ushort maxNSU { get; set; }

        public int qtdeReg { get; set; }

        public List<ProcEventoNFeOutput> procEventoNFe { get; set; }
        public List<ResNFeOutput> resNFe { get; set; }
        public List<ResEventoOutput> resEvento { get; set; }
        public List<NfeProcOutput> nfeProc { get; set; }
    }
}
