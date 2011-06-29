using System;

namespace BuzzIO
{
    /// <summary>
    /// Arguments for button changed event
    /// </summary>
    public class BuzzButtonChangedEventArgs : EventArgs
    {
        /// <summary>Current states of the buttons</summary>
        public readonly ButtonStates[] Buttons;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arrStates">State of the buttons</param>
        public BuzzButtonChangedEventArgs(ButtonStates[] arrStates)
        {
            Buttons = arrStates;
        }
    }
    /// <summary>
    /// Delegate for button event
    /// </summary>
    public delegate void BuzzButtonChangedEventHandler(object sender, BuzzButtonChangedEventArgs args);
}