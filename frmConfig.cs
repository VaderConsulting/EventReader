using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Xml;
using JCMLib;

namespace EventReader
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmConfig : Form
    {
        private ContextMenu contextMenu;
        private MenuItem menuItemConfigure;
        private MenuItem menuItemExit;

        private GroupBox groupBox1;
        private CheckBox checkBoxMem;
        private Button btnUpdate;
        private Button btnCancel;
        private MenuItem menuItemVwr;
        private MenuItem menuItem2;
        private Label label1;
        private ComboBox cbFilter;
        private Label label2;
        private ComboBox cbLogs;
        private CheckBox checkBoxROS;
        private static object Lock = new object();

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public frmConfig()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            SetInitValues();

            // start watching
            StartWatch();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmConfig));
            contextMenu = new ContextMenu();
            menuItemConfigure = new MenuItem();
            menuItemVwr = new MenuItem();
            menuItem2 = new MenuItem();
            menuItemExit = new MenuItem();
            groupBox1 = new GroupBox();
            checkBoxROS = new CheckBox();
            label2 = new Label();
            cbLogs = new ComboBox();
            cbFilter = new ComboBox();
            label1 = new Label();
            checkBoxMem = new CheckBox();
            btnUpdate = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenu
            // 
            contextMenu.MenuItems.AddRange(new MenuItem[] {
            menuItemConfigure,
            menuItemVwr,
            menuItem2,
            menuItemExit});
            // 
            // menuItemConfigure
            // 
            menuItemConfigure.DefaultItem = true;
            menuItemConfigure.Index = 0;
            menuItemConfigure.Text = "Configure...";
            menuItemConfigure.Click += new EventHandler(menuItemConfigure_Click);
            // 
            // menuItemVwr
            // 
            menuItemVwr.Index = 1;
            menuItemVwr.Text = "Event Viewer";
            menuItemVwr.Click += new EventHandler(menuItemVwr_Click);
            // 
            // menuItem2
            // 
            menuItem2.Index = 2;
            menuItem2.Text = "-";
            // 
            // menuItemExit
            // 
            menuItemExit.Index = 3;
            menuItemExit.Text = "Exit";
            menuItemExit.Click += new EventHandler(menuItemExit_Click);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBoxROS);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cbLogs);
            groupBox1.Controls.Add(cbFilter);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(checkBoxMem);
            groupBox1.Location = new Point(8, 8);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(248, 144);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // checkBoxROS
            // 
            checkBoxROS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxROS.Location = new Point(112, 120);
            checkBoxROS.Name = "checkBoxROS";
            checkBoxROS.Size = new Size(120, 16);
            checkBoxROS.TabIndex = 8;
            checkBoxROS.Text = "Run On Startup";
            // 
            // label2
            // 
            label2.Location = new Point(16, 24);
            label2.Name = "label2";
            label2.Size = new Size(216, 16);
            label2.TabIndex = 7;
            label2.Text = "Select log to monitor:";
            // 
            // cbLogs
            // 
            cbLogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbLogs.Location = new Point(16, 40);
            cbLogs.Name = "cbLogs";
            cbLogs.Size = new Size(216, 21);
            cbLogs.TabIndex = 6;
            // 
            // cbFilter
            // 
            cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbFilter.Items.AddRange(new object[] {
            "All Event Types",
            "Information",
            "Warning",
            "Error",
            "SuccessAudit",
            "FailureAudit"});
            cbFilter.Location = new Point(16, 88);
            cbFilter.Name = "cbFilter";
            cbFilter.Size = new Size(216, 21);
            cbFilter.TabIndex = 4;
            // 
            // label1
            // 
            label1.Location = new Point(16, 72);
            label1.Name = "label1";
            label1.Size = new Size(216, 16);
            label1.TabIndex = 3;
            label1.Text = "Filter by these event types:";
            // 
            // checkBoxMem
            // 
            checkBoxMem.Checked = true;
            checkBoxMem.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxMem.Location = new Point(16, 120);
            checkBoxMem.Name = "checkBoxMem";
            checkBoxMem.Size = new Size(96, 16);
            checkBoxMem.TabIndex = 2;
            checkBoxMem.Text = "Save Settings";
            // 
            // btnUpdate
            // 
            btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnUpdate.Location = new Point(200, 160);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(56, 23);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "Apply";
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Location = new Point(136, 160);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(56, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // Config
            // 
            AcceptButton = btnUpdate;
            AutoScaleBaseSize = new Size(5, 13);
            CancelButton = btnCancel;
            ClientSize = new Size(265, 192);
            ContextMenu = contextMenu;
            ControlBox = false;
            Controls.Add(btnCancel);
            Controls.Add(btnUpdate);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "Config";
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Configuration";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new frmConfig());
        }

        private void SetInitValues()
        {
            RegHelper reg = new RegHelper();
            watchLog = reg.WatchLog;
            eventFilter = reg.EventFilter;
            appIcon = new Icon(GetType(), "events.ico");
            localPath = System.Environment.CurrentDirectory;

            WindowState = FormWindowState.Minimized;
            Hide();

            // populate cbLogs with available event logs
            ArrayList alLogs = GetAvailEventLogs();
            foreach (EventLog log in GetAvailEventLogs())
            {
                cbLogs.Items.Add(log.LogDisplayName);
            }

            NotifyIcon = new NotifyIconEx();
            NotifyIcon.Icon = appIcon;
            NotifyIcon.Text = tipText + "(" + watchLog + ")";
            NotifyIcon.Visible = true;
            NotifyIcon.ContextMenu = contextMenu;

            cbLogs.SelectedItem = watchLog;
            if (eventFilter.Length > 0)
            {
                cbFilter.SelectedItem = eventFilter;
            }
            else
            {
                cbFilter.SelectedIndex = 0;
            }

            // add event handlers
            NotifyIcon.BalloonClick += new EventHandler(OnClickBalloon);
            NotifyIcon.DoubleClick += new EventHandler(OnDoubleClickIcon);
        }

        /// <summary>
        /// Retrieves available event logs on the local machine
        /// </summary>
        /// <returns>ArrayList of available EventLog objects</returns>
        private ArrayList GetAvailEventLogs()
        {
            ArrayList alLogs = new ArrayList();
            foreach (EventLog log in EventLog.GetEventLogs())
            {
                alLogs.Add(log);
            }

            return alLogs;
        }

        /// <summary>
        /// Retrieves available event logs on the specified machine
        /// </summary>
        /// <param name="machine"></param>
        /// <returns>ArrayList of available EventLog objects</returns>
        private ArrayList GetAvailEventLogs(string machine)
        {
            ArrayList alLogs = new ArrayList();
            foreach (EventLog log in EventLog.GetEventLogs(machine))
            {
                alLogs.Add(log);
            }

            return alLogs;
        }

        private void OnClickBalloon(object sender, EventArgs e)
        {
            // start Event Viewer
            Process.Start("eventvwr.exe");
        }

        private void OnDoubleClickIcon(object sender, EventArgs e)
        {
            ShowConfiguration();
        }

        #region "Properties"
        private NotifyIconEx NotifyIcon
        {
            get { return notifyIconA; }

            set { notifyIconA = value; }
        }
        /// <summary>
        /// Tip text displayed on mouseover icon event
        /// </summary>
        private string TipText
        {
            get { return tipText; }
        }
        /// <summary>
        /// Event log entry category
        /// </summary>
        private string LogCategory
        {
            get { return logCategory; }
        }
        /// <summary>
        /// Event log entry message
        /// </summary>
        private string LogMessage
        {
            get { return logMessage; }
        }
        /// <summary>
        /// The machine the log entry was generated on
        /// </summary>
        private string LogMachine
        {
            get { return logMachine; }
        }
        /// <summary>
        /// The event log entry source
        /// </summary>
        private string LogSource
        {
            get { return logSource; }
        }
        /// <summary>
        /// The event log entry type
        /// Can be "Success Audit" "Failure Audit" "Error" "Warning" or "Information"
        /// </summary>
        private string LogType
        {
            get { return logType; }
        }
        /// <summary>
        /// The event log entry ID
        /// </summary>
        private string EventID
        {
            get { return eventID; }
        }
        /// <summary>
        /// The user associated with the log entry, if any.
        /// </summary>
        private string User
        {
            get { return user; }
        }
        /// <summary>
        /// Time the event was generated
        /// </summary>
        private string LogTime
        {
            get { return logTime; }
        }
        #endregion

        #region "Private Variables"
        private string watchLog = "";
        private string eventFilter = "";
        private string tipText = "Event Log Watcher";
        private Icon appIcon;
        private NotifyIconEx notifyIconA;
        private string logMessage = "";
        private string logMachine = "";
        private string logSource = "";
        private string logType;
        private string eventID = "";
        private string user = "";
        private string logTime = "";
        private string logCategory = "";
        private string localPath = "";
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
            cbLogs.SelectedItem = watchLog;
            if (eventFilter.Length > 0)
            {
                cbFilter.SelectedItem = eventFilter;
            }
            else
            {
                cbFilter.SelectedIndex = 0;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (LogComboOK())
            {
                localPath += "\\" + Assembly.GetExecutingAssembly().GetName().Name +
                    ".exe";

                Hide(); // hide from Alt-Tab

                if (eventFilter == "All Event Types")
                {
                    eventFilter = "";
                }

                // save settings
                if (checkBoxMem.Checked)
                {
                    RegHelper.SetKey(watchLog, eventFilter);
                }

                if (checkBoxROS.Checked)
                {
                    RegHelper.SetRunOnStartup(localPath, false);
                }
                else
                {
                    RegHelper.SetRunOnStartup(localPath, true);
                }

                cbLogs.SelectedItem = watchLog;

                NotifyIcon.Text = tipText + "(" + watchLog + ")";
                StartWatch();
            }
            else
            {
                MessageBox.Show(this,
                    "The filter is invalid for this event log type",
                    "Invalid Combination",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Determine if the log combination is valid.
        /// </summary>
        /// <returns>T/F</returns>
        private bool LogComboOK()
        {
            bool comboOK = false;
            eventFilter = cbFilter.SelectedItem.ToString();
            watchLog = cbLogs.SelectedItem.ToString();
            switch (watchLog)
            {
                // Security can only have success or failure events
                case "Security":
                    if (eventFilter == "All Event Types" || eventFilter == "SuccessAudit" ||
                        eventFilter == "FailureAudit")
                    {
                        comboOK = true;
                    }

                    break;
                default:
                    comboOK = true;
                    break;
            }
            return comboOK;
        }

        private void menuItemConfigure_Click(object sender, EventArgs e)
        {
            ShowConfiguration();
        }

        private void menuItemVwr_Click(object sender, EventArgs e)
        {
            Process.Start("eventvwr.exe");
        }

        /// <summary>
        /// Displays the configuration window
        /// </summary>
        private void ShowConfiguration()
        {
            Show();
            WindowState = FormWindowState.Normal;

            if (RegHelper.IsRunOnStartup())
            {
                checkBoxROS.Checked = true;
            }
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            // cleanup
            NotifyIcon.Remove();
            NotifyIcon = null;
            Close();
        }

        private void StartWatch()
        {
            EventLog myLog = new EventLog(watchLog);

            // set event handler
            myLog.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);

            try
            {
                myLog.EnableRaisingEvents = true;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Retrieves the stats on the last event log entry
        /// </summary>
        /// <param name="logName">Event log to open</param>
        private bool GetLogEntryStats(string logName)
        {
            int e = 0;

            try
            {
                EventLog log = new EventLog(logName);
                e = log.Entries.Count - 1; // last entry

                logMessage = log.Entries[e].Message;
                logMachine = log.Entries[e].MachineName;
                logSource = log.Entries[e].Source;
                logCategory = log.Entries[e].Category;
                logType = Convert.ToString(log.Entries[e].EntryType);
                eventID = log.Entries[e].InstanceId.ToString(); // Was EventID

                user = log.Entries[e].UserName;
                logTime = log.Entries[e].TimeGenerated.ToShortTimeString();
                log.Close();    // close log
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void OnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            string logName = watchLog;
            NotifyIconEx.NotifyInfoFlags FlagType = NotifyIconEx.NotifyInfoFlags.None;

            // Not all logs exist on all machines
            if (GetLogEntryStats(watchLog))
            {
                switch (LogType)
                {
                    case "Error":
                        FlagType = NotifyIconEx.NotifyInfoFlags.Error;
                        break;
                    case "Warning":
                        FlagType = NotifyIconEx.NotifyInfoFlags.Warning;
                        break;
                    case "Information":
                        FlagType = NotifyIconEx.NotifyInfoFlags.Info;
                        break;
                    case "SuccessAudit":
                        FlagType = NotifyIconEx.NotifyInfoFlags.Info;
                        break;
                    case "FailureAudit":
                        FlagType = NotifyIconEx.NotifyInfoFlags.Error;
                        break;
                    default:
                        FlagType = NotifyIconEx.NotifyInfoFlags.Info;
                        break;
                }
                /*
                    Error = 1,
                    Warning = 2,
                    Information = 4,
                    SuccessAudit = 8,
                    FailureAudit = 16
                */

                if (logType == eventFilter || eventFilter.Length == 0)
                {
                    // show balloon
                    NotifyIcon.ShowBalloon("Event Log Monitor",
                        "An event was written to the " + logName + " event log." +
                        "\nType: " + LogType +
                        "\nSource: " + LogSource +
                        "\nCategory: " + LogCategory +
                        "\nEventID: " + EventID +
                        "\nUser: " + User,
                        FlagType,
                        5000);

                    LogNotification();
                }
            }
        }

        private void LogNotification()
        {
            string filename = "alerts.xml";

            if (!System.IO.File.Exists(filename))
            {
                lock (Lock)
                {
                    WriteAlertDoc(filename);
                }
            }

            XmlDocument doc = new XmlDocument();

            lock (Lock)
            {
                doc.Load(filename);
            }

            XmlElement elem = doc.CreateElement("alert");
            elem.SetAttribute("time", LogTime);
            elem.SetAttribute("type", LogType);
            elem.SetAttribute("category", LogCategory);
            elem.SetAttribute("source", LogSource);
            elem.SetAttribute("eventid", EventID);
            elem.SetAttribute("user", User);
            elem.InnerText = LogMessage;

            doc.DocumentElement.AppendChild(elem);

            lock (Lock)
            {
                doc.Save(filename);
            }
        }

        private void WriteAlertDoc(string filename)
        {
            XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            writer.WriteStartElement("alerts");
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }
    }
}
