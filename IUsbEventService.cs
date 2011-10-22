using System;

namespace BuzzIO
{
    public interface IUsbEventService
    {
        void Register(IntPtr hwndHandle);
        bool Unregister(IntPtr hwndHandle);
    }
}
