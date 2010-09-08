using System;
using System.Windows.Forms;

namespace BuzzWin
{
    /// <summary>
    /// Defines a simple base form class that registers itself for device notifications.
    /// </summary>
    public class UsbAwareForm : Form
    {
        #region Member variables
        /// <summary>HID Guid</summary>
        private Guid m_gDeviceClass;
        /// <summary>Handle returned by RegisterForUsbEvents - need it when we unregister</summary>
        private IntPtr m_hUsbEventHandle;
        #endregion

        /// <summary>
        /// Constructor. No handle yet so can't register for USB events here
        /// </summary>
        public UsbAwareForm()
        {
            InitializeComponent();
            m_gDeviceClass = Win32Usb.HIDGuid;
        }

        /// <summary>
        /// Accessor for device class guid
        /// </summary>
        public Guid DeviceClassGuid
        {
            get { return m_gDeviceClass; }
        }
        /// <summary>Event called when a new device is detected</summary>
        public event EventHandler DeviceArrived;
        /// <summary>Event called when a device is removed</summary>
        public event EventHandler DeviceRemoved;

        /// <summary>
        /// Overridable 'On' method called when a new device is detected
        /// </summary>
        protected virtual void OnDeviceArrived(EventArgs args)
        {
            if (DeviceArrived != null)
            {
                DeviceArrived(this, args);
            }
        }
        /// <summary>
        /// Overridable 'On' method called when a device is removed
        /// </summary>
        protected virtual void OnDeviceRemoved(EventArgs args)
        {
            if (DeviceRemoved != null)
            {
                DeviceRemoved(this, args);
            }
        }

        #region Overrides
        /// <summary>
        /// Override called when the window handle has been created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            m_hUsbEventHandle = Win32Usb.RegisterForUsbEvents(Handle, m_gDeviceClass);
        }
        /// <summary>
        /// Override WndProc to handle incoming Windows messages.
        /// </summary>
        /// <param name="m">Message from Windows</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32Usb.WM_DEVICECHANGE)	// we got a device change message! A USB device was inserted or removed
            {
                switch (m.WParam.ToInt32())	// Check the W parameter to see if a device was inserted or removed
                {
                    case Win32Usb.DEVICE_ARRIVAL:	// inserted
                        OnDeviceArrived(new EventArgs());
                        break;
                    case Win32Usb.DEVICE_REMOVECOMPLETE:	// removed
                        OnDeviceRemoved(new EventArgs());
                        break;
                }
            }
            base.WndProc(ref m);	// pass message on to base form
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Win32Usb.UnregisterForUsbEvents(m_hUsbEventHandle);
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "UsbAwareForm";
        }
        #endregion
    }
}