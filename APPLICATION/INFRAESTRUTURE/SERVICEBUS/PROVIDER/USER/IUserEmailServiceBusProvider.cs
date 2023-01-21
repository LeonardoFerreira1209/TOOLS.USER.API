using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;

/// <summary>
/// Interface de UserEmailServiceBusSenderProvider
/// </summary>
public interface IUserEmailServiceBusSenderProvider : IServiceBusSenderProvider { };

/// <summary>
/// Interface de IUserEmailServiceBusReceiverProvider
/// </summary>
public interface IUserEmailServiceBusReceiverProvider : IServiceBusReceiverProvider { };