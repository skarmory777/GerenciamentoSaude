using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Dto
{
    [AutoMap(typeof(EstoqueLocalizacao))]
    public class EstoqueLocalizacaoDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Código Localização Estoque
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Descrição Localização Estoque
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Estoque
        /// </summary>
        public long EstoqueId { get; set; }
        public virtual Estoque Estoque { get; set; }

    }

}