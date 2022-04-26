using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace TRobot.Core.Services
{
    public class CommunicationService : ICommunicationService
    {
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public SerialPortSettings SerialPortSettings { get; set; }

        private readonly AutoResetEvent dataReceivedEvent = new AutoResetEvent(false);
        private SerialPortCommunicationProvider communicationProvider;

        private const ushort TIMEOUT = 15000;
        private const ushort REQUEST_TIMEOUT = 50;

        private readonly object dataBufferAccessSynchronizationObject = new object();
        private byte[] dataBuffer;


        public CommunicationService()
        {
        }

        public void Initialize()
        {
            dataBuffer = default;

            dataReceivedEvent.Reset();

            var serialPortSettings = new SerialPortSettings();

            serialPortSettings.PortName = ControllerDeviceConfigurationService.ControllerDeviceDataModel.ControllerDeviceConnectionSettingsModel.SerialPort;
            serialPortSettings.BaudRate = ControllerDeviceConfigurationService.ControllerDeviceDataModel.ControllerDeviceConnectionSettingsModel.BaudRate;
            serialPortSettings.Parity = ControllerDeviceConfigurationService.ControllerDeviceDataModel.ControllerDeviceConnectionSettingsModel.Parity;
            serialPortSettings.DataBits = ControllerDeviceConfigurationService.ControllerDeviceDataModel.ControllerDeviceConnectionSettingsModel.DataBits;
            serialPortSettings.StopBits = ControllerDeviceConfigurationService.ControllerDeviceDataModel.ControllerDeviceConnectionSettingsModel.StopBits;

            communicationProvider = new SerialPortCommunicationProvider(serialPortSettings);
            communicationProvider.ReadTimeout = TIMEOUT;
            communicationProvider.WriteTimeout = TIMEOUT;

            communicationProvider.DataReceived += CommunicationProvider_DataReceived;
            communicationProvider.ErrorReceived += CommunicationProvider_ErrorReceived;
        }

        public void Connect()
        {
            if (communicationProvider != null && !communicationProvider.IsOpen)
            {
                communicationProvider.Open();
                OnConnected(new EventArgs());
            }
        }

        public void Disconnect()
        {
            if (communicationProvider != null && communicationProvider.IsOpen)
            {
                communicationProvider.Close();
                OnDisconnected(new EventArgs());
            }
        }                    

        protected virtual void OnConnected(EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        private void SendData(string data)
        {
            communicationProvider?.Write(data);
        }

        private void SendData(char[] data)
        {
            communicationProvider?.Write(data, 0, data.Length);
        }

        private void SendDataSync(string data)
        {
            SendData(data);
            WaitForData();
        }

        private void SendDataSync(char[] data)
        {
            SendData(data);
            WaitForData();
        }

        private byte ReadByte()
        {
            lock (dataBufferAccessSynchronizationObject)
            {
                if (dataBuffer.Length > 0)
                {
                    return dataBuffer[0];
                }
                else
                {
                    throw new InvalidOperationException("Error reading data from buffer");
                }
            }
        }

        private char[] ReadCharArray()
        {
            lock (dataBufferAccessSynchronizationObject)
            {
                if (dataBuffer.Length > 0)
                {
                    return Encoding.ASCII.GetString(dataBuffer).ToCharArray();
                }
                else
                {
                    throw new InvalidOperationException("Error reading data from buffer");
                }
            }
        }

        private string ReadString()
        {
            lock (dataBufferAccessSynchronizationObject)
            {
                if (dataBuffer.Length > 0)
                {
                    return Encoding.ASCII.GetString(dataBuffer);
                }
                else
                {
                    throw new InvalidOperationException("Error reading data from buffer");
                }
            }
        }

        private void CommunicationProvider_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            lock (dataBufferAccessSynchronizationObject)
            {
                dataBuffer = new byte[serialPort.BytesToRead];
                serialPort.Read(dataBuffer, 0, dataBuffer.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(dataBuffer);
                }
            }

            dataReceivedEvent.Set();
        }

        private void CommunicationProvider_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new InvalidOperationException("Error reading data from serial port");
        }

        private void WaitForData()
        {
            if (!dataReceivedEvent.WaitOne(TimeSpan.FromMilliseconds(TIMEOUT)))
            {
                throw new TimeoutException("Serial port receiving data timeout");
            }
        }
    }
}
