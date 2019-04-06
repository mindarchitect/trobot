using System;
using System.Collections.ObjectModel;

namespace TRobot.Core
{
    public abstract class RobotFactory
    {
        public ObservableCollection<AbstractRobot> Robots
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        protected RobotFactory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Factory name is null or empty");
            }

            Name = name;
            Robots = new ObservableCollection<AbstractRobot>();
        }
    }
}
