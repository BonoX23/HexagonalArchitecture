namespace Domain.Guest.Enums
{
    public enum Action
    {
        Pay = 0,
        Finish = 1, //Depois de pago e utilizado
        Cancel = 2, //Não pode cancelar oq já foi pago
        Refound = 3, // Só pode ser reembolsado depois que pago
        Reopen = 4, // Só pode reabrir se cancelar
    }
}
