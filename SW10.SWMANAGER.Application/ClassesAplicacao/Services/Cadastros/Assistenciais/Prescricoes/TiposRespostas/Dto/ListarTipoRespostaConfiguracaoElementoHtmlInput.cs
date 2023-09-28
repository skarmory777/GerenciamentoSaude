﻿using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto
{
    public class ListarTipoRespostaConfiguracaoElementoHtmlInput : ListarInput
    {
        public long TipoRespostaConfiguracaoId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Codigo";
            }
        }
    }
}
