using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas
{
    public class ParametroAppService : SWMANAGERAppServiceBase, IParametroAppService
    {
        private readonly IRepository<Parametro, long> _parametroRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ParametroAppService(IRepository<Parametro, long> parametroRepository
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _parametroRepository = parametroRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        public ParametroDto ObterPorCodigoEmpresa(long empresaId, string codigo)
        {
            var parametro = _parametroRepository.GetAll()
                                                .Where(w => w.Codigo == codigo
                                                        && w.EmpresaId == empresaId)
                                                .FirstOrDefault();

            if (parametro != null)
            {
                return ParametroDto.Mapear(parametro);
            }
            return null;
        }

        public ParametroDto ObterPorCodigo(string codigo)
        {
            var parametro = _parametroRepository.GetAll()
                                                .Where(w => w.Codigo == codigo)
                                                .FirstOrDefault();

            if (parametro != null)
            {
                return ParametroDto.Mapear(parametro);
            }
            return null;
        }


        public async Task CriarOuEditar(ParametroDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var parametro = ParametroDto.Mapear(input); //input.MapTo<Resultado>();

                    if (input.Id.Equals(0))
                    {
                        await _parametroRepository.InsertAsync(parametro);
                    }
                    else
                    {
                        var _parametro = _parametroRepository.GetAll()
                                                             .Where(w => w.Id == input.Id)
                                                             .FirstOrDefault();

                        if (_parametro != null)
                        {
                            _parametro.Codigo = input.Codigo;
                            _parametro.Descricao = input.Descricao;
                            _parametro.EmpresaId = input.EmpresaId;
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    _unitOfWorkManager.Current.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
