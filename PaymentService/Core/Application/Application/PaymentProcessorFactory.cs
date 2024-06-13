using Application.Booking.Dtos;
using Application.MercadoPago;
using Application.Payment.Ports;

namespace Payments.Application
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider)
        {
            switch (selectedPaymentProvider)
            {
                case SupportedPaymentProviders.MercadoPago:
                    return new MercadoPagoAdapter();

                default: return new NotImplementedPaymentProvider();
            }
        }
    }
}
