using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto
{
    [AutoMap(typeof(LaudoGrupo))]
    public class LaudoGrupoDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        public ModalidadeDto Modalidade { get; set; }
        public long? ModalidadeId { get; set; }

        public List<FaturamentoItemDto> ExamesDto { get; set; }
    }
}
