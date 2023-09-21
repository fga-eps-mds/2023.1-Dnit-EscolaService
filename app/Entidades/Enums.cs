using System.ComponentModel;

namespace app.Entidades
{
    public enum UF
    {
        [Description("Acre")]
        AC = 1,
        [Description("Alagoas")]
        AL,
        [Description("Amapá")]
        AP,
        [Description("Amazonas")]
        AM,
        [Description("Bahia")]
        BA,
        [Description("Ceará")]
        CE,
        [Description("Espírito Santo")]
        ES,
        [Description("Goiás")]
        GO,
        [Description("Maranhão")]
        MA,
        [Description("Mato Grosso")]
        MT,
        [Description("Mato Grosso do Sul")]
        MS,
        [Description("Minas Gerais")]
        MG,
        [Description("Pará")]
        PA,
        [Description("Paraíba")]
        PB,
        [Description("Paraná")]
        PR,
        [Description("Pernambuco")]
        PE,
        [Description("Piauí")]
        PI,
        [Description("Rio de Janeiro")]
        RJ,
        [Description("Rio Grande do Norte")]
        RN,
        [Description("Rio Grande do Sul")]
        RS,
        [Description("Rondônia")]
        RO,
        [Description("Roraima")]
        RR,
        [Description("Santa Catarina")]
        SC,
        [Description("São Paulo")]
        SP,
        [Description("Sergipe")]
        SE,
        [Description("Tocantins")]
        TO,
        [Description("Distrito Federal")]
        DF
    }

    public enum Localizacao
    {
        Rural = 1,
        Urbana,
    }

    public enum Porte
    {
        [Description("Até 50 matrículas de escolarização")]
        Ate50 = 1,
        [Description("Entre 51 e 200 matrículas de escolarização")]
        Entre51e200 = 4,
        [Description("Entre 201 e 500 matrículas de escolarização")]
        Entre201e500 = 2,
        [Description("Entre 501 e 1000 matrículas de escolarização")]
        Entre501e1000 = 3,
        [Description("Mais de 1000 matrículas de escolarização")]
        Mais1000 = 5,
    }

    public enum Situacao
    {
        [Description("Indicação")]
        Indicacao = 1,
        [Description("Solicitação da escola")]
        SolicitacaoEscola,
        [Description("Jornada de crescimento do professor")]
        Jornada,
        [Description("Escola Crítica")]
        EscolaCritica,
    }

    public enum Rede
    {
        Municipal = 1,
        Estadual,
        Privada,
    }

    public enum EtapaEnsino
    {
        [Description("Educação Infantil")]
        Infantil = 1,
        [Description("Ensino Fundamental")]
        Fundamental,
        [Description("Ensino Médio")]
        Medio,
        [Description("Educação de Jovens Adultos")]
        JovensAdultos,
        [Description("Educação Profissional")]
        Profissional,
    }
}
