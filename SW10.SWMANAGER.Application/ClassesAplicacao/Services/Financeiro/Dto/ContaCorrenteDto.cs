using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(ContaCorrente))]
    public class ContaCorrenteDto : CamposPadraoCRUDDto
    {
        public long TipoContaCorrenteId { get; set; }
        public TipoContaCorrenteDto TipoContaCorrente { get; set; }
        public long AgenciaId { get; set; }
        public AgenciaDto Agencia { get; set; }
        public long BancoId { get; set; }
        public BancoDto Banco { get; set; }
        public long EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }
        public DateTime DataAbertura { get; set; }
        public string NomeGerente { get; set; }
        public decimal? LimiteCredito { get; set; }
        public string Observacao { get; set; }
        public bool IsContaNaoOperacional { get; set; }

        public static ContaCorrenteDto Mapear(ContaCorrente tipoConta, Banco banco = null)
        {
            ContaCorrenteDto contaCorrenteDto = new ContaCorrenteDto();

            contaCorrenteDto.Id = tipoConta.Id;
            contaCorrenteDto.Codigo = tipoConta.Codigo;
            contaCorrenteDto.Descricao = tipoConta.Descricao;
            contaCorrenteDto.DataAbertura = tipoConta.DataAbertura;
            contaCorrenteDto.NomeGerente = tipoConta.NomeGerente;
            contaCorrenteDto.LimiteCredito = tipoConta.LimiteCredito;
            contaCorrenteDto.EmpresaId = tipoConta.EmpresaId;
            if (tipoConta.Empresa != null)
            {
                contaCorrenteDto.Empresa = EmpresaDto.Mapear(tipoConta.Empresa);
                contaCorrenteDto.Empresa.Descricao = "- " + contaCorrenteDto.Empresa.NomeFantasia;
            }
            if (banco != null)
            {
                contaCorrenteDto.BancoId = banco.Id;
                contaCorrenteDto.Banco = BancoDto.Mapear(banco, true);
            }
            contaCorrenteDto.AgenciaId = tipoConta.AgenciaId;
            if (tipoConta.Agencia != null)
                contaCorrenteDto.Agencia = AgenciaDto.Mapear(tipoConta.Agencia);
            contaCorrenteDto.TipoContaCorrenteId = tipoConta.TipoContaCorrenteId;
            if (tipoConta.TipoContaCorrente != null)
                contaCorrenteDto.TipoContaCorrente = TipoContaCorrenteDto.Mapear(tipoConta.TipoContaCorrente);
            contaCorrenteDto.IsContaNaoOperacional = tipoConta.IsContaNaoOperacional;
            contaCorrenteDto.Observacao = tipoConta.Observacao;

            return contaCorrenteDto;
        }

        public static ContaCorrente Mapear(ContaCorrenteDto tipoContaDto, bool isService = false)
        {
            ContaCorrente contaCorrente = new ContaCorrente();

            contaCorrente.Id = tipoContaDto.Id;
            contaCorrente.Codigo = tipoContaDto.Codigo;
            contaCorrente.Descricao = tipoContaDto.Descricao;
            contaCorrente.DataAbertura = tipoContaDto.DataAbertura;
            contaCorrente.NomeGerente = tipoContaDto.NomeGerente;
            contaCorrente.LimiteCredito = tipoContaDto.LimiteCredito;
            contaCorrente.EmpresaId = tipoContaDto.EmpresaId;
            contaCorrente.AgenciaId = tipoContaDto.AgenciaId;
            contaCorrente.TipoContaCorrenteId = tipoContaDto.TipoContaCorrenteId;
            contaCorrente.IsContaNaoOperacional = tipoContaDto.IsContaNaoOperacional;
            contaCorrente.Observacao = tipoContaDto.Observacao;

            return contaCorrente;
        }
    }
}
