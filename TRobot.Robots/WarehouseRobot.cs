using System;
using System.Windows;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.UI.Commands;

namespace TRobot.Robots
{
    public class WarehouseRobot : AbstractRobot, IMovable, IFactoryProvier<DescartesRobotFactory>
    {          
        public event EventHandler<PositionChangedEventArguments> PositionChanged;
        private WarehouseRobotControlPanel controlPanel;

        public WarehouseRobot(DescartesRobotFactory factory)
        {            
            Factory = factory;           

            Engine = new WarehouseRobotEngine();
            Controller = new WarehouseRobotController(this);          
            controlPanel = new WarehouseRobotControlPanel(this);
        }       

        private ICommand startCommand;

        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                {
                    startCommand = new RelayCommand<object>(                      
                        param => Start()
                    );
                }
                return startCommand;
            }
        }

        private ICommand stopCommand;

        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new RelayCommand<object>(
                        param => Stop()
                    );
                }
                return stopCommand;
            }
        }

        public Point CurrentPosition
        {
            get;
            set;
        }

        public uint Velocity
        {
            get;
            set;
        }

        public uint Acceleration
        {
            get;
            set;
        }

        public DescartesRobotFactory Factory
        {
            get;
            set;
        }

        internal WarehouseRobotEngine Engine
        {
            get;
            set;
        }

        internal WarehouseRobotController Controller
        {
            get;
            set;
        }

        void OnPositionChanged(PositionChangedEventArguments e)
        {
            PositionChanged?.Invoke(this, e);
        }

        public void Move()
        {
            Engine.Start();          
        }

        public override void Start()
        {           
            controlPanel.Show();
        }

        public override void Stop()
        {      
            if (controlPanel != null)
            {
                controlPanel.Hide();
            }
        }
    }
}
