﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace RobotUICSharp
{
    //This class wraps a SerialPort Object for easier use 
    internal class SerialConnector
    {
        private SerialPort port;
        private Thread receiveThread;


        public SerialConnector()
        {
            port = new SerialPort();
            port.BaudRate = 115200;
            receiveThread = new Thread(new ThreadStart(this.receiveMessages));
        }

        // Sets the COM-Port that is to be used 
        public void setPort(String portName)
        {
            port.PortName = portName;
        }

        //Returns an Array(String) of available serial ports 
        public String[] getSerialPorts()
        {
            return SerialPort.GetPortNames();
        }

        //Closes the SerialPorts connection 
        public void shutdownPort()
        {
            if (port.IsOpen == true) {
                receiveThread.Join();
                port.Close(); 
            }

        }

        //Opens the Serial Port. Returns true if successful. Returns false if error occurs (Port non-existent, Port in use, or similar) .
        public bool openPort()
        {
            try
            {
                port.Open();
                Console.WriteLine("Opened Port at " + port.PortName);
                receiveThread.Start();
                return true;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private void receiveMessages()
        {
            while (true)
            {
                String received = port.ReadLine();
                Console.WriteLine(received);
                
            }
        }

        //Returns the state of the port as a boolean. 
        public bool isOpen()
        {
            return port.IsOpen;
        }

        //Sends a message via Serial Port 
        public void sendMessage(String message)
        {
            if (port.IsOpen == true)
            {
                try
                {
                    port.WriteLine(message);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    port.Close();
                }
            }
        }


    }
}
