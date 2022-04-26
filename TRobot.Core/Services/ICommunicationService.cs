using System;

namespace Elokon.Eloprotect.Core.Services
{
    public interface ICommunicationService : IService
    {
        event EventHandler Connected;
        event EventHandler Disconnected;

        void Initialize();
        void Connect();
        void Disconnect();
    }
}
