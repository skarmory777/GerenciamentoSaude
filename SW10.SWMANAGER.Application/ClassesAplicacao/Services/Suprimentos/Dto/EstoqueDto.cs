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
    [AutoMap(typeof(Estoque))]
    public class EstoqueDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Descrição Estoque
        /// </summary>
        public string Descricao { get; set; }
    }
}
