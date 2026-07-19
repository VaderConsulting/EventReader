using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using static Utilities.Extensions;

namespace JCMLib
{
    public class NotifyIconEx : Control //System.ComponentModel.Component
    {
        #region Constants

        #endregion

        #region Delegates

        #endregion

        #region Events

        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event EventHandler BalloonClick;

        #endregion

        #region Enums

        public enum NotifyInfoFlags { None = 0x00, Info = 0x01, Warning = 0x02, Error = 0x03 }
        private enum NotifyCommand { Add = 0x00, Delete = 0x02, Modify = 0x01 }
        private enum NotifyFlags { Message = 0x01, Icon = 0x02, Tip = 0x04, Info = 0x10, State = 0x08 }
        private enum NotifyState { Hidden = 0x01 }

        #endregion

        #region DLL Imports

        #region Platform Invoke

        [StructLayout(LayoutKind.Sequential)]
        private struct NotifyIconData
        {
            public uint cbSize; // DWORD

            public System.IntPtr hWnd; // HWND

            public uint uID; // UINT

            public NotifyFlags uFlags; // UINT

            public uint uCallbackMessage; // UINT

            public System.IntPtr hIcon; // HICON

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]

            public string szTip; // char[128]

            public NotifyState dwState; // DWORD

            public NotifyState dwStateMask; // DWORD

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

            public string szInfo; // char[256]

            public int dwTimeoutOrVersion; // UINT

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]

            public string szInfoTitle; // char[64]

            public NotifyInfoFlags dwInfoFlags; // DWORD
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("shell32.Dll")]
        private static extern int Shell_NotifyIcon(NotifyCommand cmd, ref NotifyIconData data);

        [DllImport("User32.Dll", CharSet = CharSet.Auto)]
        private static extern int TrackPopupMenuEx(System.IntPtr hMenu,
            uint uFlags,
            int x,
            int y,
            System.IntPtr hWnd,
            System.IntPtr ignore);


        [DllImport("User32.Dll")]
        private static extern int GetCursorPos(ref POINT point);

        [DllImport("User32.Dll")]
        private static extern int SetForegroundWindow(System.IntPtr hWnd);

        #endregion

        #endregion

        #region Fields

        private uint _ID = 0; // each icon in the notification area has an id
        private IntPtr _Handle; // save the handle so that we can remove icon
        private static NotifyIconTarget _MessageSink = new NotifyIconTarget();
        private static uint _NextId = 1;
        private string _Text = "";
        private Icon _Icon = null;
        private ContextMenu _ContextMenu = null;
        private bool _Visible = false;
        private bool _DoubleClick = false; // fix for extra mouse up message we want to discard

        #endregion

        #region Properties

        public string Text
        {
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    CreateOrUpdate();
                }
            }
            get
            {
                return _Text;
            }
        }

        public Icon Icon
        {
            set
            {
                _Icon = value;
                CreateOrUpdate();
            }
            get
            {
                return _Icon;
            }
        }

        public ContextMenu ContextMenu
        {
            set
            {
                _ContextMenu = value;
            }
            get
            {
                return _ContextMenu;
            }
        }

        public bool Visible
        {
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    CreateOrUpdate();
                }
            }
            get
            {
                return _Visible;
            }
        }

        #endregion

        #region Constructors and Destructor

        public NotifyIconEx()
        {
        }

        #endregion

        #region Event Handlers

        private void OnClick(object sender, uint id)
        {
            if (id == _ID)
            {
                if (!_DoubleClick && Click != null)
                {
                    Click(this, EventArgs.Empty);
                }

                _DoubleClick = false;
            }
        }

        private void OnRightClick(object sender, uint id)
        {
            if (id == _ID)
            {
                // show context menu
                if (_ContextMenu != null)
                {
                    POINT point = new POINT();
                    GetCursorPos(ref point);

                    SetForegroundWindow(_MessageSink.Handle); // this ensures that if we show the menu and then click on another window the menu will close

                    // call non public member of ContextMenu
                    _ContextMenu.GetType().InvokeMember("OnPopup",
                        BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance,
                        null, _ContextMenu, new object[] { System.EventArgs.Empty });

                    TrackPopupMenuEx(_ContextMenu.Handle, 64, point.x, point.y, _MessageSink.Handle, IntPtr.Zero);

                    // PostMessage(m_messageSink.Handle, 0, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        private void OnDoubleClick(object sender, uint id)
        {
            if (id == _ID)
            {
                _DoubleClick = true;
                if (DoubleClick != null)
                {
                    DoubleClick(this, EventArgs.Empty);
                }
            }
        }

        private void OnClickBalloon(object sender, uint id)
        {
            if (id == _ID)
            {
                if (BalloonClick != null)
                {
                    BalloonClick(this, EventArgs.Empty);
                }
            }
        }

        private void OnTaskbarCreated(object sender, EventArgs e)
        {
            if (_ID != 0)
            {
                Create(_ID); // keep the id the same
            }
        }

        #endregion

        #region Private Methods

        // this method adds the notification icon if it has not been added and if we have enough data to do so
        private void CreateOrUpdate()
        {
            if (this.DesignMode)
            {
                return;
            }

            if (_ID == 0)
            {
                if (_Icon != null)
                {
                    // create icon using available properties
                    Create(_NextId++);
                }
            }
            else
            {
                // update notify icon
                Update();
            }
        }

        private void Create(uint id)
        {
            NotifyIconData data = new NotifyIconData();
            data.cbSize = (uint)Marshal.SizeOf(data);

            _Handle = _MessageSink.Handle;
            data.hWnd = _Handle;
            _ID = id;
            data.uID = _ID;

            data.uCallbackMessage = 0x400;
            data.uFlags |= NotifyFlags.Message;

            data.hIcon = _Icon.Handle; // this should always be valid
            data.uFlags |= NotifyFlags.Icon;

            data.szTip = _Text;
            data.uFlags |= NotifyFlags.Tip;

            if (!_Visible)
            {
                data.dwState = NotifyState.Hidden;
            }

            data.dwStateMask |= NotifyState.Hidden;

            Shell_NotifyIcon(NotifyCommand.Add, ref data);

            // add handlers
            _MessageSink.ClickNotify += new NotifyIconTarget.NotifyIconHandler(OnClick);
            _MessageSink.DoubleClickNotify += new NotifyIconTarget.NotifyIconHandler(OnDoubleClick);
            _MessageSink.RightClickNotify += new NotifyIconTarget.NotifyIconHandler(OnRightClick);
            _MessageSink.ClickBalloonNotify += new NotifyIconTarget.NotifyIconHandler(OnClickBalloon);
            _MessageSink.TaskbarCreated += new EventHandler(OnTaskbarCreated);
        }

        // update an existing icon
        private new void Update()
        {
            NotifyIconData data = new NotifyIconData();
            data.cbSize = (uint)Marshal.SizeOf(data);

            data.hWnd = _MessageSink.Handle;
            data.uID = _ID;

            data.hIcon = _Icon.Handle; // this should always be valid
            data.uFlags |= NotifyFlags.Icon;

            data.szTip = _Text;
            data.uFlags |= NotifyFlags.Tip;
            data.uFlags |= NotifyFlags.State;

            if (!_Visible)
            {
                data.dwState = NotifyState.Hidden;
            }

            data.dwStateMask |= NotifyState.Hidden;

            Shell_NotifyIcon(NotifyCommand.Modify, ref data);
        }

        /// <summary>
        /// Helper method to determine if invoke required, if so will rerun method on correct thread.
        /// if not do nothing.
        /// </summary>
        /// <param name="c">Control that might require invoking</param>
        /// <param name="a">action to preform on control thread if so.</param>
        /// <returns>true if invoke required</returns>
        private bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new MethodInvoker(delegate { a(); }));
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Public Methods

        protected override void Dispose(bool disposing)
        {
            Remove();
            base.Dispose(disposing);
        }

        public void Remove()
        {
            if (_ID != 0)
            {
                // remove the notify icon
                NotifyIconData data = new NotifyIconData();
                data.cbSize = (uint)Marshal.SizeOf(data);

                data.hWnd = _Handle;
                data.uID = _ID;

                Shell_NotifyIcon(NotifyCommand.Delete, ref data);

                _ID = 0;
            }
        }

        public void ShowBalloon(string title, string text, NotifyInfoFlags type, int timeoutInMilliSeconds)
        {
            if (timeoutInMilliSeconds < 0)
            {
                throw new ArgumentException("The parameter must be positive", "timeoutInMilliseconds");
            }

            NotifyIconData data = new NotifyIconData();
            data.cbSize = (uint)Marshal.SizeOf(data);

            _MessageSink.OnUIThread(() =>
                                    data.hWnd = _MessageSink.Handle
                                   );

            data.uID = _ID;

            data.uFlags = NotifyFlags.Info;
            data.dwTimeoutOrVersion = timeoutInMilliSeconds; // this value does not seem to work - any ideas?
            data.szInfoTitle = title;
            data.szInfo = text;
            data.dwInfoFlags = type;

            Shell_NotifyIcon(NotifyCommand.Modify, ref data);

        }

        #endregion

        #region Classes

        // By my own convention all classes should be in their own file, however sometimes it makes sense to include a class within the same file as it's parent Namespace
        #region Notify Icon Target Window

        private class NotifyIconTarget : System.Windows.Forms.Form
        {
            public NotifyIconTarget()
            {
                this.Text = "Hidden NotifyIconTarget Window";
            }

            protected override void DefWndProc(ref Message msg)
            {
                if (msg.Msg == 0x400) // WM_USER
                {
                    uint msgId = (uint)msg.LParam;
                    uint id = (uint)msg.WParam;

                    switch (msgId)
                    {
                        case 0x201: // WM_LBUTTONDOWN
                            break;

                        case 0x202: // WM_LBUTTONUP
                            if (ClickNotify != null)
                            {
                                ClickNotify(this, id);
                            }

                            break;

                        case 0x203: // WM_LBUTTONDBLCLK
                            if (DoubleClickNotify != null)
                            {
                                DoubleClickNotify(this, id);
                            }

                            break;

                        case 0x205: // WM_RBUTTONUP
                            if (RightClickNotify != null)
                            {
                                RightClickNotify(this, id);
                            }

                            break;

                        case 0x200: // WM_MOUSEMOVE
                            break;

                        case 0x402: // NIN_BALLOONSHOW
                            break;

                        // this should happen when the balloon is closed using the x
                        // - we never seem to get this message!
                        case 0x403: // NIN_BALLOONHIDE
                            break;

                        // we seem to get this next message whether the balloon times
                        // out or whether it is closed using the x
                        case 0x404: // NIN_BALLOONTIMEOUT
                            break;

                        case 0x405: // NIN_BALLOONUSERCLICK
                            if (ClickBalloonNotify != null)
                            {
                                ClickBalloonNotify(this, id);
                            }

                            break;
                    }
                }
                else if (msg.Msg == 0xC086) // WM_TASKBAR_CREATED
                {
                    if (TaskbarCreated != null)
                    {
                        TaskbarCreated(this, System.EventArgs.Empty);
                    }
                }
                else
                {
                    base.DefWndProc(ref msg);
                }
            }

            public delegate void NotifyIconHandler(object sender, uint id);

            public event NotifyIconHandler ClickNotify;
            public event NotifyIconHandler DoubleClickNotify;
            public event NotifyIconHandler RightClickNotify;
            public event NotifyIconHandler ClickBalloonNotify;
            public event EventHandler TaskbarCreated;
        }

        #endregion

        #endregion




    }
}

