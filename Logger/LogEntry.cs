//MIT License

//Copyright(c) 2017 nessie1980 (nessie1980@gmx.de)

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Drawing;

namespace Logging
{
    /// <summary>
    ///     This class represents a single log entry
    ///     One entry contains:
    ///     - unique ID of the entry
    ///     - time stamp of the entry
    ///     - state of the entry
    ///     - log of the entry
    /// </summary>
    internal class LogEntry
    {
        #region Variables

        /// <summary>
        ///     Stores the ID of log entry
        /// </summary>
        private int _logID;

        /// <summary>
        ///     Stores the timestamp when the log entry has been made
        /// </summary>
        private DateTime _logTimeStamp;

        /// <summary>
        ///     Stores the state of the log entry (e.g Info)
        /// </summary>
        private string _logState;

        /// <summary>
        ///     Stores the component name of the message creation
        /// </summary>
        private string _logComponentName;

        /// <summary>
        ///     Stores the color for the showing
        /// </summary>
        private Color _logColor;

        /// <summary>
        ///     Stores the log of the log entry
        /// </summary>
        private string _log;

        #endregion Variables

        #region Properties

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public DateTime LogTimeStamp
        {
            get { return _logTimeStamp; }
            set { _logTimeStamp = value; }
        }

        public string LogState
        {
            get { return _logState; }
            set { _logState = value; }
        }

        public string LogComponentName
        {
            get { return _logComponentName; }
            set { _logComponentName = value; }
        }

        public Color LogColor
        {
            get { return _logColor; }
            internal set { _logColor = value; }
        }

        public string Log
        {
            get { return _log; }
            set { _log = value; }
        }

        #endregion Properties

        #region Methodes

        /// <summary>
        ///     This is the constructor for the log entry object
        /// </summary>
        /// <param name="id">ID for the entry</param>
        /// <param name="timeStamp">Timestamp of the entry</param>
        /// <param name="state">State of the entry (e.g. Info)</param>
        /// <param name="componentName">Name of the entry creation component</param>
        /// <param name="color">Color for showing the entry</param>
        /// <param name="log">Log of the entry</param>
        public LogEntry(int id, DateTime timeStamp, string state, string componentName, Color color, string log)
        {
            LogID = id;
            LogTimeStamp = timeStamp;
            LogState = state;
            LogComponentName = componentName;
            LogColor = color;
            Log = log;
        }

        /// <summary>
        ///     This is the constructor for the log entry object
        ///     without giving a timestamp for the log entry.
        ///     The timestamp is set to the current time.
        /// </summary>
        /// <param name="id">ID for the entry</param>
        /// <param name="state">State of the entry (e.g. Info)</param>
        /// <param name="color">Color for showing the entry</param>
        /// <param name="log">Log of the entry</param>
        public LogEntry(int id, string state, Color color, string log)
        {
            LogID = id;
            LogTimeStamp = DateTime.Now;
            LogState = state;
            LogComponentName = @"-";
            LogColor = color;
            Log = log;
        }

        #endregion Methodes

    }
}
