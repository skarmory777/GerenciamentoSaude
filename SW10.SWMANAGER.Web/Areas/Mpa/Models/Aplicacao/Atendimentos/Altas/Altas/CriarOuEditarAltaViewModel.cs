//using Abp.AutoMapper;

//using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Altas.Dto;
//using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AltaMedica;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Altas.Altas
//{
//    [AutoMap(typeof(AltaDto))]
//    public class CriarOuEditarAltaViewModel : AltaDto
//    {
//        public bool IsEditMode { get { return Id > 0; } }

//        public CriarOuEditarAltaMedicaViewModel AltaMedicaViewModel { get; set; }


//        public CriarOuEditarAltaViewModel(AltaDto output)
//        {
//            output.MapTo(this);
//        }
//    }
//}
