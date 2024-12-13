namespace Matuning.Domain.ValueObjects;

public class Email
{
    public string Address { get; private set; }

    private Email() { }

    public Email(string address)
    {
        if (!IsValid(address))
            throw new ArgumentException("Invalid email address.");

        Address = address;
    }

    private bool IsValid(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return false;

        try
        {
            // Простая валидация, можно использовать MailAddress или другой механизм
            var addr = new System.Net.Mail.MailAddress(address);
            return addr.Address == address;
        }
        catch
        {
            return false;
        }
    }

    // Переопределение Equals и GetHashCode для сравнения
    public override bool Equals(object obj)
    {
        return obj is Email email &&
               Address == email.Address;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Address);
    }
}