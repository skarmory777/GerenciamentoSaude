using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Enumeradores;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos
{
    public class ResultadoLaudoValidacaoService
    {
        private readonly IRepository<ItemResultado, long> _itemResultadoRepository;
        private readonly IRepository<Resultado, long> _resultadoRepository;

        public ResultadoLaudoValidacaoService(
            IRepository<ItemResultado, long> itemResultadoRepository,
            IRepository<Resultado, long> resultadoRepository)
        {
            _itemResultadoRepository = itemResultadoRepository;
            _resultadoRepository = resultadoRepository;
        }


        List<ErroDto> Lista = new List<ErroDto>();
        public List<ErroDto> Validar(List<FormatacaoItemIndexDto> resultadosDto, long coletaId)
        {
            this.ValidarSoma100(resultadosDto.Where(m => m.ExameStatusId != 4).ToList());

            var resultado = this._resultadoRepository.GetAll()
                                                .Where(w => w.Id == coletaId)
                                                .Include(i => i.Atendimento.Paciente)
                                                .Include(i => i.Atendimento.Paciente.SisPessoa)
                                                .FirstOrDefault();




            foreach (var item in resultadosDto.Where(m => m.ExameStatusId != 4).ToList())
            {
                if (resultado.Atendimento.Paciente.SexoId != null)
                {
                    this.ValidarValoresConfigurados(item, (long)resultado.Atendimento.Paciente.SexoId);
                }
            }
            return this.Lista;
        }

        void ValidarSoma100(List<FormatacaoItemIndexDto> resultadosDto)
        {
            decimal soma = 0;
            var existeIsSoma = false;
            foreach (var item in resultadosDto)
            {
                var itemResultado = this._itemResultadoRepository
                                                           .GetAll()
                                                           .FirstOrDefault(
                                                               w => w.Id == item.ItemId
                                                                    && w.IsSoma100);
                if (itemResultado != null)
                {
                    existeIsSoma = true;
                    var valor = 0m;
                    if (decimal.TryParse(item.Resultado, out valor))
                    {
                        soma += valor;
                    }
                }
            }

            if (existeIsSoma && soma != 100)
            {
                Lista.Add(new ErroDto { CodigoErro = "EXAM003", Parametros = new List<object> { soma } });
            }
        }

        void ValidarValoresConfigurados(FormatacaoItemIndexDto resultado, long sexoId)
        {
            var itemResultado = this._itemResultadoRepository.GetAll().AsNoTracking().FirstOrDefault(w => w.Id == resultado.ItemId);

            if (itemResultado != null && itemResultado.TipoResultadoId == (long)EnumTipoResultado.Numerico)
            {
                decimal valorResultado;

                if (decimal.TryParse(resultado.Resultado, out valorResultado))
                {
                    if (sexoId == (long)EnumSexo.Masculino)
                    {
                        //if (itemResultado.MinimoAceitavelMasculino.HasValue)
                        //{
                        //    if (valorResultado < itemResultado.MinimoAceitavelMasculino)
                        //    {
                        //        Lista.Add(new ErroDto { CodigoErro = "EXAM001", Parametros = new List<object> { itemResultado.Descricao } });
                        //    }
                        //    else if (valorResultado > itemResultado.MaximoAceitavelMasculino)
                        //    {
                        //        Lista.Add(new ErroDto { CodigoErro = "EXAM002", Parametros = new List<object> { itemResultado.Descricao } });
                        //    }
                        //}
                    }
                    else
                    {
                        //if (itemResultado.MinimoAceitavelFeminino.HasValue)
                        //{
                        //    if (valorResultado < itemResultado.MinimoAceitavelFeminino)
                        //    {
                        //        Lista.Add(new ErroDto { CodigoErro = "EXAM001", Parametros = new List<object> { itemResultado.Descricao } });
                        //    }
                        //    else if (valorResultado > itemResultado.MaximoAceitavelFeminino)
                        //    {
                        //        Lista.Add(new ErroDto { CodigoErro = "EXAM002", Parametros = new List<object> { itemResultado.Descricao } });
                        //    }
                        //}
                    }
                }
            }
        }

    }
}
