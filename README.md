# WindowsToolkit

A small Windows Forms utility for common PC maintenance tasks — clearing out junk files and running Windows' built-in repair tools, all from one window.

> **Warning:** This project is very much a work in progress. Expect rough edges.

## Features

- **Delete Temp Files** — Clears leftover files from `C:\Windows\Temp`, `%Temp%`, and `C:\Windows\Prefetch`.
- **DISM** — Runs `DISM /Online /Cleanup-Image /RestoreHealth` to repair the Windows system image.
- **SFC** — Runs `sfc /scannow` to check for and replace corrupted system files.
- **CHKDSK** — Runs `chkdsk C: /f` to scan the disk for errors. Requires a restart to complete, and you'll be warned before it's queued.

Check any combination of the boxes above and hit **Run** to execute them in sequence. Use the **Help** button in-app for a quick explanation of each option.

## How it works

The app is a single-window WinForms UI (`MainForm` in `Form1.cs`) with a checkbox for each feature, a **Run** button, a **Help** button, and a log panel on the right that prints progress as plain text.

1. **Startup / elevation check** — On load, the app checks whether it's running as Administrator (`WindowsPrincipal.IsInRole`). If not, it shows a message box and exits; every feature here needs admin rights, so there's no non-admin fallback.
2. **CHKDSK confirmation** — Checking the "Check Disk" box immediately pops a warning that CHKDSK requires a full restart, since scheduling it is a bigger commitment than the other options. Clicking "No" unchecks the box.
3. **Run button** — Clicking **Run**:
   - Refuses to run if no checkboxes are selected.
   - Refuses to run again if a run is already in progress (`isRunning` guard), so you can't queue overlapping jobs.
   - Otherwise runs each selected task **in a fixed order**, not the order you checked them:
     1. **Delete Temp Files** — enumerates and deletes files in `C:\Windows\Temp`, `%Temp%`, and `C:\Windows\Prefetch`. Each file delete is wrapped in its own try/catch so one locked/inaccessible file just gets logged and skipped instead of aborting the whole cleanup.
     2. **DISM** (`dism.exe /Online /Cleanup-Image /RestoreHealth`)
     3. **SFC** (`sfc.exe /scannow`)
     4. **CHKDSK** (`chkdsk.exe C: /f`) — this one needs interactive confirmation, so the app writes `"Y"` to the process's standard input to auto-confirm scheduling the check at next restart.
   - DISM intentionally runs before SFC: SFC verifies files against the local component store, and DISM is what repairs that store, so running DISM first means SFC has good data to check against.
   - Each external tool's stdout is streamed line-by-line into the log box in real time via `OutputDataReceived`.
4. **Help button** — Just shows a static message box summarizing what each checkbox does; it doesn't reflect current state.

**Known issue (see comments in `Form1.cs`):** if DISM, SFC, or CHKDSK throws an exception, the handler logs it and `return`s early — but that skips the line that resets `isRunning` back to `false`. The app has to be restarted to run anything again after a failure like that.

## Requirements

- Windows
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) (or the SDK, for building from source)
- Administrator privileges — the app requires elevation and will prompt to relaunch as admin if it isn't already.

## Building

```sh
git clone https://github.com/OdegardXD/WindowsToolkit.git
cd WindowsToolkit
dotnet build
```

The project targets `net10.0-windows` and uses Windows Forms.

## About Development

Yes, AI has been used in this project. I'm not an experienced coder — I write the code myself, and lean on AI for help when I get stuck. Documentation like this README is also AI-assisted.
