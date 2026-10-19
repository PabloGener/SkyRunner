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



        private readonly string _jsonPath;
        private List<Airport> _airports;

        public frmMain(string jsonPath) {
            InitializeComponent();
            _jsonPath = jsonPath;
        }

        private void frmMain_Load(object sender, EventArgs e) {
            _airports = JsonSerializer.Deserialize<List<Airport>>(File.ReadAllText(_jsonPath));

        }
    }
}
