namespace AgendamentoApi.Helpers;

public class PaymentHelper
{
    private readonly ILogger<PaymentHelper> _logger;

    public PaymentHelper(ILogger<PaymentHelper> logger)
    {
        _logger = logger;
    }

    public async Task<string> CreateFakePaymentLink(decimal amount, string description, string paymentMethod)
    {
        await Task.Delay(500); //Simulating a api request
        return
            $"https://fake-payment.com/pay?amount={{amount}}&desc={{Uri.EscapeDataString(description)}}&method={{paymentMethod}}";
    }
}