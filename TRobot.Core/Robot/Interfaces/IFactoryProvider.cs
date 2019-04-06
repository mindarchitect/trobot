using System.Collections.ObjectModel;

namespace TRobot.Core
{
    public interface IFactoryProvier<F> where F : RobotFactory    
    {
        F Factory { get; set; }
    }
}
