using System;

namespace BuzzIO
{
    public class UsbEventService : IUsbEventService
    {
        public void Register(IntPtr hwnd)
        {
            Win32Usb.RegisterForUsbEvents(hwnd, Win32Usb.HIDGuid);
        }

        public bool Unregister(IntPtr hwnd)
        {
            return Win32Usb.UnregisterForUsbEvents(hwnd);
        }
    }
}
