using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace SkyRunner
{



    ///////////////////////////////////////////////////////////////
    /// Beginning of frmMain
    ///////////////////////////////////////////////////////////////
    public partial class frmMain : Form
    {

        ///////////////////////////////////////////////////////////////
        /// SimConnect structure for aircraft data. This structure is used to receive data from the flight simulator.
        ///////////////////////////////////////////////////////////////
        [System.Runtime.InteropServices.StructLayout(
        System.Runtime.InteropServices.LayoutKind.Sequential,
        CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 1)]
        public struct AircraftData
        {
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Title;
            public double Latitude;
            public double Longitude;
            public int OnGround;
            public int ParkingState;
        }

        private Microsoft.FlightSimulator.SimConnect.SimConnect _sim;
        private bool _simConnected;
        enum DEFINITIONS { Aircraft }
        enum REQUESTS { Aircraft }

        private void Sim_OnRecvOpen(
            Microsoft.FlightSimulator.SimConnect.SimConnect sender,
            Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_OPEN data) {
                _sim.AddToDataDefinition(DEFINITIONS.Aircraft, "TITLE", null,
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATATYPE.STRING256, 0, 0);
                _sim.AddToDataDefinition(DEFINITIONS.Aircraft, "PLANE LATITUDE", "degrees",
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATATYPE.FLOAT64, 0, 0);
                _sim.AddToDataDefinition(DEFINITIONS.Aircraft, "PLANE LONGITUDE", "degrees",
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATATYPE.FLOAT64, 0, 0);
                _sim.AddToDataDefinition(DEFINITIONS.Aircraft, "SIM ON GROUND", "Bool",
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATATYPE.INT32, 0, 0);
                _sim.AddToDataDefinition(DEFINITIONS.Aircraft, "PLANE IN PARKING STATE", "Bool",
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATATYPE.INT32, 0, 0);

                _sim.RegisterDataDefineStruct<AircraftData>(DEFINITIONS.Aircraft);

                _sim.RequestDataOnSimObject(
                    REQUESTS.Aircraft, DEFINITIONS.Aircraft,
                    SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    Microsoft.FlightSimulator.SimConnect.SIMCONNECT_PERIOD.ONCE,
                    0, 0, 0, 0);
        }

        private void Sim_OnRecvSimobjectData(
        Microsoft.FlightSimulator.SimConnect.SimConnect sender,
        Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_SIMOBJECT_DATA data) {
                if ((REQUESTS)data.dwRequestID != REQUESTS.Aircraft) return;

                var ac = (AircraftData)data.dwData[0];

                if (ac.OnGround == 0) {
                    MessageBox.Show("Aircraft is airborne. Land first.",
                        "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CleanupSim();
                    return;
                }

                if (ac.ParkingState == 0) {
                    MessageBox.Show("Aircraft is on the ground but not parked. Park first.",
                        "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CleanupSim();
                    return;
                }

                // On ground + parked → nearest airport
                // (next segment: loop _airports with Haversine, show the match)

                CleanupSim();
        }

        private void Sim_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data) {
            // Sim closed / connection dropped
            CleanupSim();
        }

        private void Sim_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data) {
            MessageBox.Show("SimConnect exception: " + data.dwException,
                "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Error);
            CleanupSim();
        }

        private void CleanupSim() {
            try { _sim?.Dispose(); } catch { }
            _sim = null;
            cmdConnect.Enabled = true;
            tmrPoll.Enabled = false;
        }






        private readonly string _jsonPath;
        private List<Airport> _airports;

        public frmMain(string jsonPath) {
            InitializeComponent();
            _jsonPath = jsonPath;
        }

        private void frmMain_Load(object sender, EventArgs e) {
            _airports = JsonSerializer.Deserialize<List<Airport>>(File.ReadAllText(_jsonPath));

        }

        private void cmdConnect_Click(object sender, EventArgs e) {
            cmdConnect.Enabled = false;
            tmrPoll.Enabled = true;

            try {
                _sim = new Microsoft.FlightSimulator.SimConnect.SimConnect(
                    "SkyRunner", IntPtr.Zero, 0, null, 0);

                _sim.OnRecvOpen += Sim_OnRecvOpen;
                _sim.OnRecvQuit += Sim_OnRecvQuit;
                _sim.OnRecvException += Sim_OnRecvException;
                _sim.OnRecvSimobjectData += Sim_OnRecvSimobjectData;
            }
            catch (Exception ex) {
                cmdConnect.Enabled = true;
                MessageBox.Show(
                    "Could not connect to MSFS. Is it running?\n\n" + ex.Message,
                    "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrPoll_Tick(object sender, EventArgs e) {
            try { _sim?.ReceiveMessage(); } catch { }
        }
    }
}