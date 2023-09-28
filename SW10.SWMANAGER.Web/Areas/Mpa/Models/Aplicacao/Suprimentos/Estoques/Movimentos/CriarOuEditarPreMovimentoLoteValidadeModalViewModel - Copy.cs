using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Dto.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    [AutoMap(typeof(EstoquePreMovimentoLoteValidadeDto))]
    public class CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel2 : EstoquePreMovimentoLoteValidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Laboratorios { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel2(EstoquePreMovimentoLoteValidadeDto output)
        {
            output.MapTo(this);
        }
    }
}