# WindowsToolkit

A small Windows Forms utility for common PC maintenance tasks — clearing out junk files and running Windows' built-in repair tools, all from one window.

## Features

- **Delete Temp Files** — Clears leftover files from `C:\Windows\Temp`, `%Temp%`, and `C:\Windows\Prefetch`.
- **Clear Recycle Bin** — Empties the Recycle Bin.
- **DISM Restore Health** — Runs `DISM /Online /Cleanup-Image /RestoreHealth` to repair the Windows system image.
- **SFC** — Runs `sfc /scannow` to check for and replace corrupted system files.
- **DISM Component Cleanup** — Runs `DISM /Online /Cleanup-Image /StartComponentCleanup` to remove old, superseded component versions that pile up in the component store after Windows updates, freeing up space without touching anything currently in use.
- **CHKDSK** — Runs `chkdsk C: /f` to scan the disk for errors. Requires a restart to complete, and you'll be warned before it's queued.
- **Flush DNS** — Runs `ipconfig /flushdns` to clear the DNS resolver cache.

Check any combination of the boxes above and hit **Run** to execute them in sequence. Use the **Help** button in-app for a quick explanation of each option.

## How it works

The app is a single-window WinForms UI (`MainForm` in `Form1.cs`) with a checkbox for each feature, a **Run** button, a **Help** button, and a read-only log panel on the right that prints progress as plain text and auto-scrolls to the latest line as it comes in.

1. **Startup / elevation check** — On load, the app checks whether it's running as Administrator (`WindowsPrincipal.IsInRole`). If not, it shows a message box and exits; every feature here needs admin rights, so there's no non-admin fallback.
2. **Version check** — Also on load, the app reads its own assembly version and compares it against the latest GitHub release tag (via `CheckForUpdate.GetLatestReleaseVersionAsync()`), logging a heads-up if a newer version is available.
3. **CHKDSK confirmation** — Checking the "Check Disk" box immediately pops a warning that CHKDSK requires a full restart, since scheduling it is a bigger commitment than the other options. Clicking "No" unchecks the box.
4. **Run button** — Clicking **Run**:
   - Refuses to run if no checkboxes are selected.
   - Refuses to run again if a run is already in progress (`isRunning` guard), so you can't queue overlapping jobs.
   - Otherwise runs each selected task **in a fixed order**, not the order you checked them:
     1. **Delete Temp Files** — enumerates and deletes files in `C:\Windows\Temp`, `%Temp%`, and `C:\Windows\Prefetch`. Each file delete is wrapped in its own try/catch so one locked/inaccessible file just gets logged and skipped instead of aborting the whole cleanup.
     2. **Clear Recycle Bin** — empties it via the `SHEmptyRecycleBin` shell API, silently (no confirmation prompt, progress dialog, or sound).
     3. **DISM Restore Health** (`dism.exe /Online /Cleanup-Image /RestoreHealth`)
     4. **SFC** (`sfc.exe /scannow`)
     5. **DISM Component Cleanup** (`dism.exe /Online /Cleanup-Image /StartComponentCleanup`)
     6. **CHKDSK** (`chkdsk.exe C: /f`) — this one needs interactive confirmation, so the app writes `"Y"` to the process's standard input to auto-confirm scheduling the check at next restart.
     7. **Flush DNS** (`ipconfig.exe /flushdns`)
   - The order matters: DISM Restore Health runs before SFC because SFC verifies files against the local component store, and DISM is what repairs that store — so running DISM first means SFC has good data to check against. Component Cleanup runs after both repairs, once the store is confirmed healthy, so it's only ever trimming superseded versions that are safe to remove.
   - Each external tool's stdout is streamed line-by-line into the log box in real time via `OutputDataReceived`. SFC's output is decoded as Unicode specifically, since `sfc.exe` writes UTF-16 to redirected output unlike the other tools here.
   - The whole run is wrapped in `try`/`finally`, so `isRunning` always gets reset back to `false` once the run ends — even if one of the steps throws — instead of leaving the app stuck refusing to run anything until restart.
5. **Help button** — Just shows a static message box summarizing what each checkbox does; it doesn't reflect current state.
6. **Closing mid-run** — If you try to close the window while `isRunning` is still `true`, a confirmation prompt warns that the current task will keep running in the background but no further queued tasks will start. Choosing "No" cancels the close.

## Requirements

- Windows
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) (or the SDK, for building from source)
- Administrator privileges — the app requires elevation. If it isn't run as admin, it shows a message box telling you to close and relaunch it as Administrator; it does not relaunch itself automatically.

## Building

```sh
git clone https://github.com/OdegardXD/WindowsToolkit.git
cd WindowsToolkit
dotnet build
```

The project targets `net10.0-windows` and uses Windows Forms.

## About Development

Yes, AI has been used in this project. I'm not an experienced coder — I write the code myself, and lean on AI for help when I get stuck. Documentation like this README is also AI-assisted.
