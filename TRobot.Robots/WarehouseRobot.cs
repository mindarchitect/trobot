using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.Robot.Events;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Models;

namespace TRobot.Robots
{
    public class WarehouseRobot : AbstractRobot, IMovable, IFactoryProvier<DescartesRobotFactory>
    {          
        public event EventHandler<PositionChangedEventArguments> PositionChanged;
        private WarehouseRobotControlPanel controlPanel;

        public RobotSettings Settings { get; set; }       

        public WarehouseRobot(DescartesRobotFactory factory)
        {            
            Factory = factory;

            Settings = new RobotSettings();
            Image = @"~\..\..\Images\robot.jpg";
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

        public double CurrentVelocity
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

        public override void Start()
        {
            Engine = new WarehouseRobotEngine(this);
            Controller = new WarehouseRobotController(this);
            Controller.Initialize();
     
            if (controlPanel == null)
            {
                controlPanel = new WarehouseRobotControlPanel(this);
            }
            
            controlPanel.Show();
        }      

        public override void Stop()
        {
            Controller?.Terminate();            
            controlPanel?.Close();
            controlPanel = null;
        } 

        internal void UploadTrajectory(IList<DescartesCoordinatesItem> coordinates)
        {
            Controller.UploadTrajectory(coordinates);
        }
    }
}
