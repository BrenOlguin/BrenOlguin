using Microsoft.Extensions.Logging;

namespace Sa.Demo.Common.LogsEvents
{
    public class ServiceEvents
    {
        public static EventId EnteringToCreateCountryCommandHandler { get; } = new EventId(1000, "Llamando al metodo Handle() en Sa.Demo.Application.Commands");
        public static EventId ExceptionInCreateCountryCommandHandler { get; } = new EventId(1001, "Se produjo una excepcion llamando al metodo Handle() en Sa.Demo.Application.Commands");
        public static EventId ErrorInCreateCountryCommandHandler { get; } = new EventId(1001, "Se produjo una excepcion llamando al metodo Handle() en Sa.Demo.Application.Commands");
    }
}
