using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto
{
    [AutoMap(typeof(ConsultorTabelaCampo))]
    public class CriarOuEditarConsultorTabelaCampo : ConsultorTabelaCampoDto
    {
        public virtual ICollection<ConsultorTabelaCampoDto> ConsultorTabelaCampos { get; set; }
    }
}
