using TRobot.Core.Services;

namespace TRobot.Communication.Services
{
    public interface IServiceHostProvider<S> where S: IService
    {
        S Service { get; set; }
        void Close();
    }
}
