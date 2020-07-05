using System;

namespace TRobot.Core
{
    public abstract class AbstractRobot
    {        
        public string Title { get; set; }
        public Guid Id { get; private set; }       

        protected AbstractRobot()
        {
            Id = Guid.NewGuid();
        }

        public abstract void Start();

        public abstract void Stop();        

        public override string ToString() => Title;
    }
}