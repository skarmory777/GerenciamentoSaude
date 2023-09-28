﻿using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.InstituicoesTransferencia;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Dto
{
    [AutoMap(typeof(InstituicaoTransferencia))]
    public class InstituicaoTransferenciaDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

    }
}
