﻿using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposVinculosEmpregaticios
{
    [AutoMap(typeof(TipoVinculoEmpregaticioDto))]
    public class CriarOuEditarTipoVinculoEmpregaticioModalViewModel : TipoVinculoEmpregaticioDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarTipoVinculoEmpregaticioModalViewModel(TipoVinculoEmpregaticioDto output)
        {
            output.MapTo(this);
        }
    }
}