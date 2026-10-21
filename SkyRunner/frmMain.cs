// Uncomment the next line to use dummy data (no MSFS required)
#define DUMMY_DATA

///////////////////////////////////////////////////////////////////////////////
// SimConnect integration patterns (per-primitive registration, SimVarString
// wrapper, and message-handler pump) adapted from:
//
//   SimConnectHelper
//   https://github.com/Dragonlaird/SimConnectHelper
//   by Dragonlaird (MIT License)
//
// The original library is a static helper for MSFS 2020 SimConnect.
// We borrowed the technique — not the library — to keep SkyRunner's
// SimConnect layer small and dependency-free.
///////////////////////////////////////////////////////////////////////////////
///
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace SkyRunner
{
    public partial class frmMain : Form
    {
        //////////////////////////////////////////////////////////////////////////////////
        /// SimConnect stuff
        /// 
        //////////////////////////////////////////////////////////////////////////////////
        
        [System.Runtime.InteropServices.StructLayout(
            System.Runtime.InteropServices.LayoutKind.Sequential,
            CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 1)]
        public struct SimVarString
        {
            [System.Runtime.InteropServices.MarshalAs(
                System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Value;
        }

        private SimConnect _sim;
        private MessageHandler _msgHandler;

        private const int WM_USER_SIMCONNECT = 0x0402;

        private enum DEFINITIONS { Title = 1, Lat = 2, Lon = 3, Ground = 4, Parked = 5, Speed = 6 }
        private enum REQUESTS { Title = 1, Lat = 2, Lon = 3, Ground = 4, Parked = 5, Speed = 6 }

        private string _acTitle;
        private double _acLat, _acLon;
        private int _acGround, _acParked;
        private bool _gotTitle, _gotLat, _gotLon, _gotGround, _gotParked;
        private double _acAirspeed;
        private bool _gotSpeed;

        private readonly string _jsonPath;

        //////////////////////////////////////////////////////////////////////////////////
        /// SimConnect stuff
        /// 
        //////////////////////////////////////////////////////////////////////////////////





        private string _currentOriginIcao;
        private double _currentLat;
        private double _currentLon;
        
        private List<Airport> _airports;
        
        private List<Airport> closeby;

        private List<Job> available = new List<Job>();
        private Job currentJob = new Job();
        private List<Job> history = new List<Job>();

        private string HistoryPath => Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "SkyRunner", "history.json");

        private string CurrentJobPath => Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "SkyRunner", "currentjob.json");



        ////////////////////////////////////////////////////////////////////////////////
        /// Beginning of frmMain code
        /// 
        ///
        ///////////////////////////////////////////////////////////////////////////////
        public frmMain(string jsonPath) {
            InitializeComponent();
            _jsonPath = jsonPath;
        }

        private void frmMain_Load(object sender, EventArgs e) {
            _airports = JsonSerializer.Deserialize<List<Airport>>(File.ReadAllText(_jsonPath));
            Console.WriteLine($"Loaded {_airports.Count} airports. First: Icao='{_airports[0].icao}' Name='{_airports[0].name}' Lat={_airports[0].latitude} Lon={_airports[0].longitude}");


            LoadCurrentJob();
            LoadHistory();

            _msgHandler = new MessageHandler();
            _msgHandler.CreateControl();
            _msgHandler.MessageReceived += (s, m) =>
            {
                if (m.Msg == WM_USER_SIMCONNECT && _sim != null) {
                    try { _sim.ReceiveMessage(); } catch { }
                }
            };
        }

        private void LoadCurrentJob() {
            string appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SkyRunner");
            string path = Path.Combine(appFolder, "currentjob.json");

            if (!File.Exists(path)) return;

            currentJob = JsonSerializer.Deserialize<Job>(File.ReadAllText(path));
            if (currentJob != null && currentJob.Status == JobStatus.Current) {
                UpdateCurrentJobLabels();

                RefreshButtonStates();
            }

        }

        private void cmdConnect_Click(object sender, EventArgs e) {
#if DUMMY_DATA
            _acTitle = "Dummy Cessna";
            _acLat = -34.6763;
            _acLon = -58.6428;
            _acGround = 1;
            _acParked = 1;
            _acAirspeed = 0.0;
            _gotTitle = _gotLat = _gotLon = _gotGround = _gotParked = _gotSpeed = true;
            ProcessSnapshot();
            #else
                cmdConnect.Enabled = false;

                try {
                    _sim = new SimConnect("SkyRunner", _msgHandler.Handle,
                        WM_USER_SIMCONNECT, null, 0);

                    _sim.OnRecvOpen += Sim_OnRecvOpen;
                    _sim.OnRecvQuit += Sim_OnRecvQuit;
                    _sim.OnRecvException += Sim_OnRecvException;
                    _sim.OnRecvSimobjectData += Sim_OnRecvSimobjectData;
                }
                catch (Exception ex) {
                    cmdConnect.Enabled = true;
                    MessageBox.Show("Could not connect to MSFS. Is it running?\n\n" + ex.Message,
                        "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            #endif
        }

        private void cmdGenerateJobs_Click(object sender, EventArgs e) {
            double maxNm = trkRadius.Value;   // slider's current value, 50..500

            closeby = FindClosestAirports(
                _currentOriginIcao, _currentLat, _currentLon,
                maxNm, maxCount: 10);


            available.Clear();
            var rng = new Random();

            foreach (Airport airport in closeby) {
                Job aJob = new Job();
                aJob.OriginIcao = _currentOriginIcao;
                aJob.DestinationIcao = airport.icao;
                aJob.DestinationName = airport.name;
                aJob.DistanceNm = DistanceInNauticalMiles(
                                            _currentLat, _currentLon,
                                            airport.latitude, airport.longitude);
                aJob.Status = JobStatus.Available;
                aJob.CreatedUtc = DateTime.UtcNow;

                if (rng.Next(2) == 0) {
                    aJob.TaskType = JobTaskType.Passengers;
                    aJob.PassengerCount = 1 + rng.Next(40);
                    aJob.CargoWeightKg = 0;
                }
                else {
                    aJob.TaskType = JobTaskType.Cargo;
                    aJob.PassengerCount = 0;
                    aJob.CargoWeightKg = 100 + rng.Next(4901);
                }

                available.Add(aJob);
            }

            lstAvailableJobs.Items.Clear();
            foreach (Job job in available) {
                lstAvailableJobs.Items.Add(job);
            }

        }

        private List<Airport> FindClosestAirports(
            string originIcao, double originLat, double originLon,
                double maxNm, int maxCount) {
            
            var candidates = new List<(Airport airport, double distanceNm)>();

            foreach (var airport in _airports) {
                if (airport.icao == originIcao) continue;
                if (string.IsNullOrWhiteSpace(airport.icao)) continue;
                if (string.IsNullOrWhiteSpace(airport.name)) continue;

                double distanceNm = DistanceInNauticalMiles(
                    originLat, originLon, airport.latitude, airport.longitude);

                if (distanceNm <= 0 || distanceNm > maxNm) continue;

                candidates.Add((airport, distanceNm));
            }

            return candidates
                .OrderBy(c => c.distanceNm)
                .Take(maxCount)
                .Select(c => c.airport)
                .ToList();
        }

        private static double DistanceInNauticalMiles(
            double lat1, double lon1, double lat2, double lon2) {
            
            return DistanceInKilometers(lat1, lon1, lat2, lon2) / 1.852;
        }

        private void ProcessSnapshot() {
            if (_acGround == 0) {
                MessageBox.Show("Aircraft is airborne. Land first.",
                    "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CleanupSim();
                return;
            }

            if (_acAirspeed >= 1.0) {
                MessageBox.Show("Aircraft is on the ground but still moving. Come to a full stop and try again.",
                    "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CleanupSim();
                return;
            }

            Airport nearestAirport = _airports
                .OrderBy(a => DistanceInKilometers(_acLat, _acLon, a.latitude, a.longitude))
                .FirstOrDefault();

            if (nearestAirport != null) {
                txtCurrentAirport.Text = $"[{nearestAirport.icao}] - {nearestAirport.name}";
                _currentOriginIcao = nearestAirport.icao;
                _currentLat = _acLat;
                _currentLon = _acLon;
            }

            RefreshButtonStates();
            CleanupSim();
        }

        private void cmdTake_Click(object sender, EventArgs e) {
            if (!(lstAvailableJobs.SelectedItem is Job selectedJob)) {
                MessageBox.Show("Select a job from the list first.",
                    "SkyRunner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentJob = selectedJob;
            currentJob.Status = JobStatus.Current;

            UpdateCurrentJobLabels();
            SaveCurrentJob();

            RefreshButtonStates();
        }

        private void UpdateCurrentJobLabels() {
            lblOriginIcao.Text = "Origin: " + currentJob.OriginIcao;
            lblDestination.Text = "Destination: " + currentJob.DestinationIcao + " (" + currentJob.DestinationName + ")";
            lblDistance.Text = "Distance: " + currentJob.DistanceNm.ToString("F0") + " nm";
            lblJobType.Text = "Type: " + currentJob.TaskType;
            lblPax.Text = "Pax: " + currentJob.PassengerCount;
            lblWeight.Text = "Weight: " + currentJob.CargoWeightKg.ToString("F0") + " kg";

            SetJobLabelsEnabled(true);
        }

        private void SetJobLabelsEnabled(bool enabled) {
            lblCurrentJob.Enabled = enabled;
            lblOriginIcao.Enabled = enabled;
            lblDestination.Enabled = enabled;
            lblDistance.Enabled = enabled;
            lblJobType.Enabled = enabled;
            lblPax.Enabled = enabled;
            lblWeight.Enabled = enabled;
        }

        private void SaveCurrentJob() {
            string appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SkyRunner");
            string path = Path.Combine(appFolder, "currentjob.json");

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, JsonSerializer.Serialize(currentJob, options));
        }

        private void Sim_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data) {
            _gotTitle = _gotLat = _gotLon = _gotGround = _gotParked = _gotSpeed = false;

            _sim.AddToDataDefinition(DEFINITIONS.Title, "TITLE", "number",
                SIMCONNECT_DATATYPE.STRING256, 0, 0);
            _sim.RegisterDataDefineStruct<SimVarString>(DEFINITIONS.Title);

            _sim.AddToDataDefinition(DEFINITIONS.Lat, "PLANE LATITUDE", "degrees",
                SIMCONNECT_DATATYPE.FLOAT64, 0, 0);
            _sim.RegisterDataDefineStruct<double>(DEFINITIONS.Lat);

            _sim.AddToDataDefinition(DEFINITIONS.Lon, "PLANE LONGITUDE", "degrees",
                SIMCONNECT_DATATYPE.FLOAT64, 0, 0);
            _sim.RegisterDataDefineStruct<double>(DEFINITIONS.Lon);

            _sim.AddToDataDefinition(DEFINITIONS.Ground, "SIM ON GROUND", "Bool",
                SIMCONNECT_DATATYPE.INT32, 0, 0);
            _sim.RegisterDataDefineStruct<int>(DEFINITIONS.Ground);

            _sim.AddToDataDefinition(DEFINITIONS.Parked, "PLANE IN PARKING STATE", "Bool",
                SIMCONNECT_DATATYPE.INT32, 0, 0);
            _sim.RegisterDataDefineStruct<int>(DEFINITIONS.Parked);

            _sim.AddToDataDefinition(DEFINITIONS.Speed, "AIRSPEED INDICATED", "knots",
                SIMCONNECT_DATATYPE.FLOAT64, 0, 0);
            _sim.RegisterDataDefineStruct<double>(DEFINITIONS.Speed);
            
            _sim.RequestDataOnSimObject(REQUESTS.Title, DEFINITIONS.Title, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);
            _sim.RequestDataOnSimObject(REQUESTS.Lat, DEFINITIONS.Lat, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);
            _sim.RequestDataOnSimObject(REQUESTS.Lon, DEFINITIONS.Lon, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);
            _sim.RequestDataOnSimObject(REQUESTS.Ground, DEFINITIONS.Ground, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);
            _sim.RequestDataOnSimObject(REQUESTS.Parked, DEFINITIONS.Parked, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);
            _sim.RequestDataOnSimObject(REQUESTS.Speed, DEFINITIONS.Speed, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.ONCE, 0, 0, 0, 0);

        }

        private void lstAvailableJobs_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshButtonStates();
        }

        private void Sim_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data) {
            switch ((REQUESTS)data.dwRequestID) {
                case REQUESTS.Title: _acTitle = ((SimVarString)data.dwData[0]).Value; _gotTitle = true; break;
                case REQUESTS.Lat: _acLat = (double)data.dwData[0]; _gotLat = true; break;
                case REQUESTS.Lon: _acLon = (double)data.dwData[0]; _gotLon = true; break;
                case REQUESTS.Ground: _acGround = (int)data.dwData[0]; _gotGround = true; break;
                case REQUESTS.Parked: _acParked = (int)data.dwData[0]; _gotParked = true; break;
                case REQUESTS.Speed: _acAirspeed = (double)data.dwData[0]; _gotSpeed = true; break;
                default: return;
            }

            if (!(_gotTitle && _gotLat && _gotLon && _gotGround && _gotParked && _gotSpeed))
                return;

            ProcessSnapshot();

            CleanupSim();
        }

        

        private static double DistanceInKilometers(
            double lat1, double lon1, double lat2, double lon2) {
            const double R = 6371.0;
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;
            double rLat1 = lat1 * Math.PI / 180.0;
            double rLat2 = lat2 * Math.PI / 180.0;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                     + Math.Cos(rLat1) * Math.Cos(rLat2)
                     * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private void Sim_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data) {
            CleanupSim();
            RefreshButtonStates();
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
        }

        private void RefreshButtonStates() {
            bool jobActive = currentJob != null && currentJob.Status == JobStatus.Current;

            cmdConnect.Enabled = true;
            cmdGenerateJobs.Enabled = !jobActive && _currentOriginIcao != null;
            cmdTake.Enabled = !jobActive && lstAvailableJobs.SelectedItem is Job;
            cmdJobDone.Enabled = jobActive;

            lstAvailableJobs.Enabled = !jobActive;
            lblJobSet.Visible = jobActive;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F3) {
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SkyRunner", "currentjob.json");

                if (File.Exists(path)) File.Delete(path);

                e.Handled = true;
            }
        }

        

        private void SaveHistory() {
            Directory.CreateDirectory(Path.GetDirectoryName(HistoryPath));
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(HistoryPath, JsonSerializer.Serialize(history, options));
        }

        private void LoadHistory() {
            if (!File.Exists(HistoryPath)) return;

            var loaded = JsonSerializer.Deserialize<List<Job>>(File.ReadAllText(HistoryPath));
            if (loaded != null) history = loaded;
        }

        private void CompleteCurrentJob() {
            if (currentJob == null || currentJob.Status != JobStatus.Current) return;

            currentJob.Status = JobStatus.Done;
            currentJob.CompletedUtc = DateTime.UtcNow;

            history.Add(currentJob);
            SaveHistory();

            // Clear current job (in memory + on disk)
            currentJob = new Job();
            if (File.Exists(CurrentJobPath)) File.Delete(CurrentJobPath);

            SetJobLabelsEnabled(false);
            lblJobSet.Visible = false;
            RefreshButtonStates();
        }

    }
}