using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoqueEmprestimoDto : CamposPadraoCRUDDto
    {
        public long SisPessoaId { get; set; }
        public string ContatoNome { get; set; }
        public string ContatoTelefone { get; set; }
        public string ContatoEmail { get; set; }
        public SisPessoaDto SisPessoa { get; set; }

        public static EstoqueEmprestimoDto Mapear(EstoqueEmprestimo entity)
        {
            if (entity == null) return null;

            var dto = new EstoqueEmprestimoDto()
            {
                Id = entity.Id,
                ContatoNome = entity.ContatoNome,
                ContatoTelefone = entity.ContatoTelefone,
                ContatoEmail = entity.ContatoEmail,
                SisPessoaId = entity.SisPessoaId,
                SisPessoa = SisPessoaDto.Mapear(entity.SisPessoa)
            };

            return dto;
        }

        public static EstoqueEmprestimo Mapear(EstoqueEmprestimoDto dto)
        {
            if (dto == null) return null;

            var entity = new EstoqueEmprestimo()
            {
                Id = dto.Id,
                ContatoNome = dto.ContatoNome,
                ContatoTelefone = dto.ContatoTelefone,
                ContatoEmail = dto.ContatoEmail,
                SisPessoaId = dto.SisPessoaId
            };

            return entity;
        }
    }

    
}
