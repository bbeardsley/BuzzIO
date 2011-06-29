using System;
using System.Text;

namespace BuzzIO
{
    /// <summary>
    /// Class which defines the state of the buttons
    /// </summary>
    public class ButtonStates
    {
        /// <summary>True if red pressed</summary>
        public bool Red;
        /// <summary>True if blue pressed</summary>
        public bool Blue;
        /// <summary>True if orange pressed</summary>
        public bool Orange;
        /// <summary>True if green pressed</summary>
        public bool Green;
        /// <summary>True if yellow pressed</summary>
        public bool Yellow;

        public bool Any
        {
            get { return Red || Blue || Orange || Green || Yellow; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Red)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append("Red");
            }

            if (Blue)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append("Blue");
            }

            if (Orange)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append("Orange");
            }

            if (Green)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append("Green");
            }

            if (Yellow)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append("Yellow");
            }

            return sb.ToString();
        }
    }
}
