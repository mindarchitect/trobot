namespace TRobot.Communication.Services
{
    public interface IServiceHostProvider
    {
        IService Service { get; set; }
        void Close();
    }
}
