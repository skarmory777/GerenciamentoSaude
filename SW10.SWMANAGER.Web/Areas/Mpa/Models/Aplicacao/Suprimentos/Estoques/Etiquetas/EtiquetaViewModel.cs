using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Etiquetas
{
    public class EtiquetaViewModel
    {
        public long Id { get; set; }
        public long LoteValidadeId {get;set;}
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public string Laboratorio { get; set; }
        public decimal Quantidade { get; set; }
        public string Produto { get; set; }

        public string Modelo { get; set; }

    }
}