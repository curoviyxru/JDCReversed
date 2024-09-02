using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JDCReversedCli;

public class KeyIntercept
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static IntPtr _hookID = IntPtr.Zero;

    public Action<ConsoleKey>? KeyPressed { get; set; }

    private static HashSet<KeyIntercept> _registeredIntercepters = [];

    public void Start()
    {
        _registeredIntercepters.Add(this);

        if (_hookID != IntPtr.Zero)
        {
            return;
        }

        _hookID = SetHook(HookCallback);
    }

    public void Stop()
    {
        _registeredIntercepters.Remove(this);

        if (_hookID == IntPtr.Zero || _registeredIntercepters.Count > 0)
        {
            return;
        }

        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using Process curProcess = Process.GetCurrentProcess();
        using ProcessModule? curModule = curProcess.MainModule;

        if (curModule == null)
        {
            return 0;
        }

        return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            
            foreach (var i in _registeredIntercepters) {
                i.KeyPressed?.Invoke((ConsoleKey) vkCode);
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}