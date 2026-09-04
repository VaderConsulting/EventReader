> **Attribution notice:** This project originates from code by **Marc Merritt** (2003). VaderConsulting does not claim the original code as their own work. See [LICENSE](LICENSE) for details.

# EventReader

A lightweight Windows system tray utility that monitors the Windows Event Log in real time and delivers balloon-tip notifications when matching events are written.

**Source last updated:** 2020-01-21

**Initiated:** 2020-01-21 · **Framework:** .NET Framework 4.7.2, Windows Forms · **Solution:** `EventReader.sln`

---

## Features

| Feature | Description |
|---------|-------------|
| System tray icon | Runs minimised; shows the watched log name in the tooltip |
| Event log selection | Dynamically lists all available Windows Event Logs |
| Event type filter | All / Information / Warning / Error / SuccessAudit / FailureAudit |
| Balloon notifications | Pop-up tip displayed when a matching event is written |
| One-click Event Viewer | Clicking a balloon launches `eventvwr.exe` |
| Settings persistence | Selected log and filter saved to the Windows Registry |
| Run on Startup | Optional registry run key for auto-start with Windows |

---

## How It Works

1. Launches hidden (minimised, not shown in taskbar)
2. Reads saved settings from the registry; attaches `NotifyIconEx` to tray
3. Subscribes to the selected Windows `EventLog` using `EventLog.EntryWritten`
4. Matching events trigger a balloon tip with event details
5. Right-click tray icon to open *Configure...* and change watched log or filter

---

## Project Structure

```
EventReader/
+-- frmConfig.cs         # Main form - tray icon, config dialog, event watch loop
+-- NotifyIconEx.cs      # Extended NotifyIcon with balloon click events
+-- RegHelper.cs         # Registry read/write helper
```

## Requirements

- Visual Studio 2017, .NET Framework 4.7.2

