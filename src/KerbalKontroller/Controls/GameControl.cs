﻿using KerbalKontroller.Clients;
using KerbalKontroller.Interfaces;
using KerbalKontroller.Resources;
using KerbalKontroller.Resources.Helpers;
using KRPC.Client.Services.SpaceCenter;
using Serilog.Core;
using System.Collections.Generic;
using System.Linq;

namespace KerbalKontroller.Controls
{
    public class GameControl
    {
        private readonly IEnumerable<IControl> controls;
        private readonly KRPCClient kRPCClient;
        private readonly Logger logger;

        public GameControl(IEnumerable<IControl> controls, KRPCClient kRPCClient, Logger logger)
        {
            this.controls = controls;
            this.kRPCClient = kRPCClient;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Information("Controls added - starting KerbalKontroller");

            Vessel activeVessel;
            ControlType? activeControl = null;
            IControl control;

            while (true)
            {
                if (!kRPCClient.IsInFlight() || kRPCClient.IsGamePaused()) activeControl = null;
                else
                {
                    activeVessel = kRPCClient.GetActiveVessel();
                    activeControl = ActiveControlHelper.SelectControlType(activeVessel);
                }

                try
                {
                    control = controls.FirstOrDefault(_ => _.ControlType == activeControl);
                    if (control != null) control.ControlLoop();
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, $"Fatal error - {activeControl} control type failed");
                    throw;
                }
            }
        }
    }
}