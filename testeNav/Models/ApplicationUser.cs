using Microsoft.AspNetCore.Identity;

namespace testeNav.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? foto { get; set; }
        public TipoUsuario Uf { get; set; }


        // Propriedades específicas para PessoaFisica
        public string? CPF { get; set; }
        public string? Cidade { get; set; }

        // Propriedades específicas para Empresa
        public string? CNPJ { get; set; }

    }
    public enum TipoUsuario
    {
        AC,
        AL,
        AP,
        AM,
        BA,
        CE,
        DF,
        ES,
        GO,
        MA,
        MT,
        MS,
        MG,
        PA,
        PB,
        PR,
        PE,
        PI,
        RJ,
        RN,
        RS,
        RO,
        RR,
        SC,
        SP,
        SE,
        TO
    }

}