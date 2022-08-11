namespace APPLICATION.DOMAIN.UTILS.Extensions;

public static class CustomErrors
{
    /// <summary>
    /// Retorna exceptions customizadas.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static string CustomExceptionMessage(this string code)
    {
        if (code == "DuplicateUserName") return "O usuário informado já foi cadastrado.";

        if (code == "DuplicateEmail") return "O email informado já foi usado.";

        if (code == "PasswordTooShort") return "A senha têm que ter no mínimo 12 caracteres.";

        if (code == "PasswordRequiresNonAlphanumeric") return "As senhas devem ter pelo menos um caractere não alfanumérico.";

        if (code == "PasswordRequiresDigit") return "As senhas devem ter pelo menos um dígito('0'-'9').";

        if (code == "PasswordRequiresUpper") return "As senhas devem ter pelo menos uma letra maiúscula ('A'-'Z').";

        if (code == "PasswordRequiresLower") return "As senhas devem ter pelo menos uma letra minúscula ('a'-'z').";

        return "Erro não foi tratado no servidor.";
    }
}
