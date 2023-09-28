using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{

    [AutoMap(typeof(EstoquePreMovimentoDto))]
    public class CriarOuEditarPreMovimentoModalViewModel : EstoquePreMovimentoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Empresas { get; set; }
        public SelectList Fornecedores { get; set; }
        public SelectList Estoques { get; set; }
        public SelectList TipoMovimentos { get; set; }
        public SelectList Ordens { get; set; }
        public SelectList CFOPs { get; set; }
        public SelectList TipoFretes { get; set; }
        public SelectList CentroCustos { get; set; }
        public SelectList Pacientes { get; set; }
        public SelectList UnidadesOrganizacionais { get; set; }
        public SelectList Medicos { get; set; }
        public SelectList Atendimentos { get; set; }
        public SelectList SaidaPor { get; set; }
        public SelectList MotivosPerda { get; set; }
        public SelectList TipoOperacaoes { get; set; }
        public SelectList Unidades { get; set; }
        public SelectList TiposAtendimentos { get; set; }

        public bool PossuiNota { get; set; }
        public bool PossuiVales { get; set; }
        public bool PossuiItensConsignados { get; set; }
        public long EstoquePreMovimentoId { get; set; }

        public CriarOuEditarPreMovimentoModalViewModel(EstoquePreMovimentoDto output)
        {
            output.MapTo(this);
        }
    }

}