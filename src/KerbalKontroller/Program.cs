﻿using KerbalKontroller.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace KerbalKontroller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ServiceConfigurator.Configure(serviceCollection);

            var krpc = serviceProvider.GetService<KRPCClient>();
            var driver = serviceProvider.GetService<ArduinoClient>();

            var vessel = krpc.GetActiveVessel();

            while (true)
            {
                var leftJoystick = driver.ReadLeftJoystick();
                vessel.Control.Yaw = leftJoystick.XValue;
                vessel.Control.Pitch = leftJoystick.YValue;
            }
        }
    }
}