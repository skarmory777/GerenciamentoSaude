using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho
{
    public class CabecalhoAppService : SWMANAGERAppServiceBase, ICabecalhoAppService
    {
        private readonly IRepository<Parametro, long> _parametroRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CabecalhoAppService(IRepository<Parametro, long> parametroRepository
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _parametroRepository = parametroRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public CabecalhoDto Obter()
        {
            CabecalhoDto cabecalhoDto = new CabecalhoDto();

            var parametroCabecalho = _parametroRepository.GetAll()
                                 .Where(w => w.Codigo == "LABCABECEX")
                                 .FirstOrDefault();

            if (parametroCabecalho != null)
            {
                cabecalhoDto.CodigoParamentroCabecalho = parametroCabecalho.Codigo;
                cabecalhoDto.DescricaoCabecalho = parametroCabecalho.Descricao;
            }

            var parametroTextoCabecalho = _parametroRepository.GetAll()
                                .Where(w => w.Codigo == "LABTEXTCAB")
                                .FirstOrDefault();

            if (parametroTextoCabecalho != null)
            {
                cabecalhoDto.CodigoParamentroTextoCabecalho = parametroTextoCabecalho.Codigo;
                cabecalhoDto.TextoCabecalho = parametroTextoCabecalho.Descricao;
            }

            return cabecalhoDto;
        }


        public DefaultReturn<CabecalhoDto> Editar(CabecalhoDto cabecalhoDto)
        {
            DefaultReturn<CabecalhoDto> _retorno = new DefaultReturn<CabecalhoDto>();
            _retorno.Errors = new List<ErroDto>();

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {


                var parametroCabecalho = _parametroRepository.GetAll()
                              .Where(w => w.Codigo == "LABCABECEX")
                              .FirstOrDefault();

                if (parametroCabecalho != null)
                {
                    parametroCabecalho.Descricao = cabecalhoDto.DescricaoCabecalho;
                }
                else
                {
                    parametroCabecalho = new Parametro();

                    parametroCabecalho.Codigo = "LABCABECEX";
                    parametroCabecalho.Descricao = cabecalhoDto.DescricaoCabecalho;
                    _parametroRepository.Insert(parametroCabecalho);
                }

                var parametroTextoCabecalho = _parametroRepository.GetAll()
                                  .Where(w => w.Codigo == "LABTEXTCAB")
                                  .FirstOrDefault();

                if (parametroTextoCabecalho != null)
                {
                    parametroTextoCabecalho.Descricao = cabecalhoDto.TextoCabecalho;
                }
                else
                {
                    parametroTextoCabecalho = new Parametro();

                    parametroTextoCabecalho.Codigo = "LABTEXTCAB";
                    parametroTextoCabecalho.Descricao = cabecalhoDto.TextoCabecalho;
                    _parametroRepository.Insert(parametroTextoCabecalho);
                }

                unitOfWork.Complete();
                unitOfWork.Dispose();
                _unitOfWorkManager.Current.SaveChanges();
            }
            return _retorno;
        }
    }
}
