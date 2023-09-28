using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos.Fichas;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    [AutoMapFrom(typeof(AtendimentoDto))]
    public class CriarOuEditarAtendimentoModalViewModel : AtendimentoDto
    {
        public FichaAmbulatorioInput FichaAmbulatorioInput { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public int AbaId { get; set; }

        public Empresa UserEmpresa { get; set; }

        public SelectList Pacientes { get; set; }

        public SelectList Medicos { get; set; }

        public SelectList Leitos { get; set; }

        public SelectList Especialidades { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList Origens { get; set; }

        public SelectList LocaisProcedencia { get; set; }

        public SelectList Convenios { get; set; }

        public SelectList Nacionalidades { get; set; }

        public SelectList Planos { get; set; }

        public SelectList AtendimentoTipos { get; set; }

        public SelectList UnidadesOrganizacionais { get; set; }

        public SelectList GuiaTipos { get; set; }// renomear para Guias

        public SelectList ServicosMedicosPrestados { get; set; }

        public List<AtendimentoDto> Atendimentos { get; set; }

        public bool Internacao { get; set; }

        public bool PreAtendimento { get; set; }

        public bool isAmbulatorioEmergencia { get; set; }

        public bool FiltroDataAtendimento { get; set; }

        public string MenuItemName { get; set; }

        public DateTime Data { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarAtendimentoModalViewModel(AtendimentoDto output)
        {
            output.MapTo(this);
        }

        public string Filtro { get; set; }

        public bool IsParticular { get; set; }

    }
}