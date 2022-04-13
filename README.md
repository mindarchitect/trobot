# TRobot

TRobot is a robot simulation desktop solution based on Microsoft&copy; .NET Framework 4.x (C#, WPF, WCF).
The project is being actively designed and developed as a playground for various technical/engineering/scientific/programmatic ideas including mathematical movement simulation algorithms for virtual objects.

Currently it provides simulation of a virtual warehouse robot that moves along predefined trajectory having 2 independent X and Y virtual drives which are parameterized by velocity and acceleration. When running, the virtual engine performs 
calculation of a speed and acceleration for each drive independently, acting as a virtual car differential, producing resulting movement speed and acceleration for robot object.

## Key Features

  - Robots are organised in virtual robot factories. Robot factory defines logical container or operating space for robot objects and performs various tasks like trajectory validation based on 2 dimensional segments that are set up during robot trajectory configuration.
  - Robot movement are displayed using Monitor Tool which establishes interprocess communication between Robot factory process and Monitor process using Microsoft WCF (Named pipes).


## Running

You can freely download, compile and run this project using Microsoft&copy; Visual Studio 2013 or later. After compilation is done, run the main project **Trobot.ECU.UI**. Select the robot and press **Start**. For monitoring, go **Tools->Start monitor** which runs Monitor application. Press **Upload settings**. Once trajectory is validated against virtual robot factory, you can press **Start** to run simulation.

Username: User1\
Password: User1


