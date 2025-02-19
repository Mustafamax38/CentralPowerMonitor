using CentralPowerMonitor.Configuration;
using PingApp;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace CentralPowerMonitor
{
    public partial class CentralForm : Form
    {
        private TcpListener server;
        private bool isRunning = false;
        private int port;
        private Dictionary<string, string> pcList = new Dictionary<string, string>();
        private string logFilePath;
        private List<string> history = new List<string>(); // Declare and initialize history
        private Dictionary<string, ListViewItem> pcItems = new Dictionary<string, ListViewItem>(); // Declare and initialize pcItems
        private System.Windows.Forms.Timer timer;

        private void LoadPcListFromConfig()
        {
            var section = (PcConfigSection)ConfigurationManager.GetSection("pcList");
            foreach (PcElement pc in section.Pcs)
            {
                pcList.Add(pc.IpAddress, pc.Alias);
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public CentralForm()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            InitializeComponent();
            FormClosing += CentralForm_FormClosing;

            // Read settings from App.config
            port = int.Parse(ConfigurationManager.AppSettings["port"]);
            logFilePath = ConfigurationManager.AppSettings["logFilePath"];
            LoadPcListFromConfig();
            LoadHistory();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000; // Check every 5 seconds
            timer.Tick += Timer_Tick;
            timer.Start();

            StartServer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckPcStatuses();
        }

        private void CheckPcStatuses()
        {
            foreach (KeyValuePair<string, string> pc in pcList)
            {
                string ipAddress = pc.Key;
                string alias = pc.Value;

                Ping ping = new Ping();
                ping.PingCompleted += (sender, e) => Ping_PingCompleted(sender, e, alias);
                ping.SendAsync(ipAddress, 1000, ipAddress);
            }
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e, string alias)
        {
            string ipAddress = (string)e.UserState;
            string status;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                status = "Online";
            }
            else
            {
                status = "Offline";
            }

            string logEntry = $"{time} - {alias} ({ipAddress}) - {status}";
            UpdatePcStatus(alias, status, time);
            UpdateLog(logEntry);
            WriteLogToFile(logEntry);
            AddToHistory(logEntry);
        }

        private async void StartServer()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                isRunning = true;
                AppendLog($"Server started on port {port}.{Environment.NewLine}");

                while (isRunning)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    _ = HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error starting server: {ex.Message}{Environment.NewLine}");
            }
        }

        private void AppendLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(AppendLog), message);
            }
            else
            {
                if (!txtLog.IsDisposed)
                {
                    txtLog.AppendText(message);
                }
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) return;

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessMessage(message);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error handling client: {ex.Message}{Environment.NewLine}");
            }
            finally
            {
                client.Close();
            }
        }

        private void ProcessMessage(string message)
        {
            string pcName = GetValue(message, "PC");
            string status = GetValue(message, "Status");
            string time = GetValue(message, "Time");

            string logEntry = $"{time} - {pcName} - {status}";

            UpdatePcStatus(pcName, status, time);
            UpdateLog(logEntry);
            WriteLogToFile(logEntry);
            AddToHistory(logEntry);
        }

        private string GetValue(string message, string key)
        {
            int startIndex = message.IndexOf($"{key}:") + key.Length + 1;
            int endIndex = message.IndexOf(',', startIndex);
            if (endIndex == -1)
                endIndex = message.Length;

            return message.Substring(startIndex, endIndex - startIndex).Trim();
        }

        private void UpdatePcStatus(string pcName, string status, string time)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => UpdatePcStatus(pcName, status, time)));
            }
            else
            {
                if (pcItems.ContainsKey(pcName))
                {
                    ListViewItem item = pcItems[pcName];
                    item.SubItems[1].Text = status;
                    item.SubItems[2].Text = time;
                }
                else
                {
                    ListViewItem item = new ListViewItem(pcName);
                    item.SubItems.Add(status);
                    item.SubItems.Add(time);
                    lvwPcStatus.Items.Add(item);
                    pcItems.Add(pcName, item);
                }
            }
        }

        private void UpdateLog(string logEntry)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => UpdateLog(logEntry)));
                }
                else
                {
                    txtLog.AppendText(logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error updating log: {ex.Message}{Environment.NewLine}");
            }
        }

        private void WriteLogToFile(string logEntry)
        {
            try
            {
                string detailedLog = $"{logEntry} - Checked at interval: {timer.Interval}ms";
                File.AppendAllText(logFilePath, detailedLog + Environment.NewLine);
            }
            catch (Exception ex)
            {
                AppendLog($"Error writing to log file: {ex.Message}{Environment.NewLine}");
            }
        }

        private void CentralForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            server?.Stop();
            timer?.Stop();
            SaveHistory();
        }

        private void LoadHistory()
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    history = new List<string>(File.ReadAllLines(logFilePath));
                    foreach (var logEntry in history)
                    {
                        txtLog.AppendText(logEntry + Environment.NewLine);
                    }
                }
                else
                {
                    AppendLog($"Log file not found at: {logFilePath}{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error loading history: {ex.Message}{Environment.NewLine}");
            }
        }

        private void SaveHistory()
        {
            try
            {
                File.WriteAllLines(logFilePath, history);
            }
            catch (Exception ex)
            {
                AppendLog($"Error saving history to log file: {ex.Message}{Environment.NewLine}");
            }
        }

        private void AddToHistory(string logEntry)
        {
            history.Add(logEntry);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private static void buttonOpenForm_Click(object sender, EventArgs e)
        {
            // Create an instance of the form you want to open
            Form1 form1 = new Form1();

            // Show the form
            form1.Show();
        }

        private void lvwPcStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
