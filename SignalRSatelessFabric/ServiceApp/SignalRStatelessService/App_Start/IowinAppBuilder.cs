using Owin;

namespace SignalRStatelessService
{
    public interface IowinAppBuilder
    {
        void Configuration(IAppBuilder appBuilder);
    }
}