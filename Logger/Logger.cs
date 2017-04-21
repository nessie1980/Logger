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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Logging
{
    public class Logger
    {
        #region Variables

        #region Enums

        public enum EInitState
        {
            InitializationFailed = -9,
            LogPathCreationFailed = -8,
            WriteStartupFailed = -7,
            WrongSize = -6,
            ColorsMaxCount = -5,
            ComponentNamesMaxCount = -4,
            ComponentLevelInvalid = -3,
            StateLevelInvalid = -2,
            StatesMaxCount = -1,
            NotInitialized = 0,
            Initialized = 1,
        }

        public enum ELoggerState
        {
            CleanUpLogFilesFailed = -5,
            NewEntryAddFailed = -4,
            ComponentNameIndexInvalid = -3,
            StateIndexInvalid = -2,
            NotInitialized = -1,
            Initialized = 0,
            LoggingDisabled = 1,
            NewEntryAddSuccessful = 2,
            CleanUpLogFilesSuccessful = 3
        }

        public enum ELoggerStateLevels
        {
            State0 = 0,
            State1 = 1,
            State2 = 2,
            State3 = 4,
            State4 = 8,
            State5 = 16,
            State6 = 32,
            State7 = 64,
            State8 = 128,
            State9 = 256,
            State10 = 512,
            State11 = 1024,
            State12 = 2048,
            State13 = 4096,
            State14 = 8192,
            State15 = 16384,
            State16 = 32768
        }

        public enum ELoggerComponentLevels
        {
            Component0 = 0,
            Component1 = 1,
            Component2 = 2,
            Component3 = 4,
            Component4 = 8,
            Component5 = 16,
            Component6 = 32,
            Component7 = 64,
            Component8 = 128,
            Component9 = 256,
            Component10 = 512,
            Component11 = 1024,
            Component12 = 2048,
            Component13 = 4096,
            Component14 = 8192,
            Component15 = 16384,
            Component16 = 32768
        }

        #endregion Enums

        #region Member variables

        /// <summary>
        ///     Stores the level of the states  which should be logged
        /// </summary>
        private int _logLevelStates;

        /// <summary>
        ///     Stores the level of the components  which should be logged
        /// </summary>
        private int _logLevelComponent;

        /// <summary>
        ///     Stores the list with the states (e.g. Info)
        /// </summary>
        private List<string> _logStates;

        /// <summary>
        ///     Stores the list with the colors (e.g. Red)
        /// </summary>
        private List<Color> _logColors;

        /// <summary>
        ///     Maximum count for the log states
        /// </summary>
        private const int MaxLogLevelsState = 16;

        /// <summary>
        ///     Stores the list with the component names
        /// </summary>
        private List<string> _logComponentNames;

        /// <summary>
        ///     Maximum count for the log components
        /// </summary>
        private const int MaxLogLevelsComponents = 16;

        /// <summary>
        ///     Stores the default maximum size for the log entry list
        /// </summary>
        private const int DefaultMaxSize = 50;

        /// <summary>
        ///     Stores the size of the log entry list
        /// </summary>
        private int _logSize = -1;

        /// <summary>
        ///     Stores the state of the logger
        /// </summary>
        private EInitState _initState = EInitState.NotInitialized;

        /// <summary>
        ///     Stores the logger state
        /// </summary>
        private ELoggerState _loggerState = ELoggerState.NotInitialized;

        /// <summary>
        ///     Stores the log entries
        /// </summary>
        private List<LogEntry> _logEntryList;

        /// <summary>
        ///     Stores if the logging to file is enabled or disabled
        /// </summary>
        private bool _loggingToFileEnabled;

        /// <summary>
        ///     Stores if the new log entries should be added to the file or not
        /// </summary>
        private bool _loggingAppendToFile;

        /// <summary>
        ///     Stores the path and file name of the log file
        /// </summary>
        private string _loggingPathAndFileName;

        /// <summary>
        ///     Stores the path for the log file
        /// </summary>
        private string _loggingPath;

        /// <summary>
        ///     Stores the log file stream writer
        /// </summary>
        private StreamWriter _streamWriterLogFile;

        #endregion Member variables

        #endregion Variables

        #region Properties

        public int LoggerLogLevelStates
        {
            get { return _logLevelStates; }
            internal set { _logLevelStates = value; }
        }

        public int LoggerLogLevelComponents
        {
            get { return _logLevelComponent; }
            internal set { _logLevelComponent = value; }
        }

        public List<string> LoggerStatesList
        {
            get { return _logStates; }
            internal set
            {
                if (value != null && value.Count > 0)
                    _logStates = value;
                else
                    _logStates = null;
            }
        }

        public List<string> LoggerComponentNamesList
        {
            get { return _logComponentNames; }
            internal set
            {
                if (value != null && value.Count > 0)
                    _logComponentNames = value;
                else
                    _logComponentNames = null;
            }
        }

        public List<Color> LoggerColorList
        {
            get { return _logColors; }
            internal set
            {
                if (value != null && value.Count > 0)
                    _logColors = value;
                else
                    _logColors = null;
            }
        }

        public EInitState InitState
        {
            get { return _initState; }
            internal set { _initState = value; }
        }

        public ELoggerState LoggerState
        {
            get { return _loggerState; }
            internal set { _loggerState = value; }
        }

        public int LoggerSize
        {
            get { return _logSize; }
            internal set
            {
                if (value > 0)
                    _logSize = value;
                else
                    _logSize = DefaultMaxSize;
            }
        }

        public bool LoggerToFileEnabled
        {
            get { return _loggingToFileEnabled; }
            internal set { _loggingToFileEnabled = value; }
        }

        public bool LoggerAppendToFile
        {
            get { return _loggingAppendToFile; }
            internal set { _loggingAppendToFile = value; }
        }

        public string LoggerPathAndFileName
        {
            get { return _loggingPathAndFileName; }
            internal set
            {
                if (value == null || value == @"")
                {
                    if (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) != null &&
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) != @"")
                    {
                        _loggingPathAndFileName =
                            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Log.txt");
                    }
                }
                else
                {
                    _loggingPathAndFileName = value;
                }

                // Set log path property
                LoggerPath = Path.GetDirectoryName(_loggingPathAndFileName);
            }
        }

        public string LoggerPath
        {
            get { return _loggingPath; }
            internal set { _loggingPath = value; }
        }

        #endregion Properties

        #region Methodes

        /// <summary>
        ///     This function initialized the logger
        /// </summary>
        /// <param name="logLevelStates">Log level that controls which states are logged or not</param>
        /// <param name="logLevelComponents">Log level that controls which components are logged or not</param>
        /// <param name="stateList">List with the states (e.g. Info)</param>
        /// <param name="componentNameList">List with the component names</param>
        /// <param name="colors">List with the colors for the GUI visualization</param>
        /// <param name="size">Size of the log entries for the GUI visualization</param>
        /// <param name="fileLogging">Enables or disable logging to file</param>
        /// <param name="loggingPathAndFileName">Path and file name for the log file</param>
        /// <param name="startupMessage">Message for the logger startup</param>
        /// <param name="appendToLogFile">Flag if the message should be appended to a existing file or not</param>
        public void LoggerInitialize(int logLevelStates = 0, int logLevelComponents = 0, List<string> stateList = null, List<string> componentNameList = null, List<Color> colors = null,
            bool fileLogging = false, int size = 0, string loggingPathAndFileName = @"", string startupMessage = @"",
            bool appendToLogFile = false)
        {
            try
            {
                LoggerLogLevelStates = logLevelStates;
                LoggerLogLevelComponents = logLevelComponents;
                LoggerStatesList = stateList;
                LoggerComponentNamesList = componentNameList;
                LoggerColorList = colors;
                LoggerSize = size;
                LoggerToFileEnabled = fileLogging;
                LoggerAppendToFile = appendToLogFile;
                LoggerPathAndFileName = loggingPathAndFileName;

                // Check if no states are given or if the count is of the given states is valid
                if (LoggerStatesList == null || LoggerStatesList.Count <= MaxLogLevelsState)
                {
                    // Check if no states are given or if the given state log level is valid
                    if (LoggerStatesList == null || LoggerLogLevelStates < Math.Pow(2, LoggerStatesList.Count))
                    {
                        // Check if no component names are given or if the count of the given component names is valid
                        if (LoggerComponentNamesList == null || LoggerComponentNamesList.Count <= MaxLogLevelsComponents)
                        {
                            // Check if no component names are given or if the given component log level is valid
                            if (LoggerComponentNamesList == null || LoggerLogLevelComponents < Math.Pow(2, LoggerComponentNamesList.Count))
                            {
                                // Check if logging is enabled via log levels
                                if (LoggerLogLevelStates > 0 && LoggerLogLevelComponents > 0)
                                {
                                    // Check if the size is not "0"
                                    if (LoggerSize > 0)
                                    {
                                        // If no startup message is given set the default startup message
                                        if (startupMessage == null || startupMessage == @"")
                                            startupMessage = @"-";

                                        if (_logEntryList == null)
                                            _logEntryList = new List<LogEntry>();
                                        else
                                        {
                                            _logEntryList.Clear();
                                            _logEntryList = new List<LogEntry>();
                                        }

                                        _logEntryList.Add(new LogEntry(0, DateTime.Now, @"Start", @"Logger", Color.Black,
                                            startupMessage));

                                        // Write to file
                                        if (LoggerToFileEnabled)
                                        {
                                            // Check if the given path exists if not try to create it
                                            if (!Directory.Exists(LoggerPath))
                                                Directory.CreateDirectory(LoggerPath);

                                            // Check if the creation was successfull
                                            if (!Directory.Exists(LoggerPath))
                                            {
                                                InitState = EInitState.LogPathCreationFailed;
                                                LoggerState = ELoggerState.NotInitialized;
                                                throw (new LoggerException(InitState, LoggerState,
                                                    @"Log path does not exist."));
                                            }
                                            else
                                            {
                                                using (
                                                    _streamWriterLogFile =
                                                        new StreamWriter(LoggerPathAndFileName, LoggerAppendToFile,
                                                            Encoding.UTF8))
                                                {
                                                    // Write log entries to the file
                                                    foreach (var logEntry in _logEntryList)
                                                    {
                                                        if (WriteLogEntry(_streamWriterLogFile, logEntry))
                                                        {
                                                            InitState = EInitState.Initialized;
                                                            LoggerState = ELoggerState.Initialized;

                                                        }
                                                        else
                                                        {
                                                            InitState = EInitState.WriteStartupFailed;
                                                            LoggerState = ELoggerState.NotInitialized;
                                                            throw (new LoggerException(InitState, LoggerState,
                                                                @"Write startup messages failed."));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            InitState = EInitState.Initialized;
                                            LoggerState = ELoggerState.Initialized;
                                        }
                                    }
                                    else
                                    {
                                        InitState = EInitState.WrongSize;
                                        LoggerState = ELoggerState.NotInitialized;
                                        throw (new LoggerException(InitState, LoggerState,
                                            @"Wrong log entry size given."));
                                    }
                                }
                                else
                                {
                                    InitState = EInitState.Initialized;
                                    LoggerState = ELoggerState.LoggingDisabled;
                                }
                            }
                            else
                            {
                                InitState = EInitState.ComponentLevelInvalid;
                                LoggerState = ELoggerState.NotInitialized;
                                throw (new LoggerException(InitState, LoggerState,
                                    @"Invalid component log level given."));
                            }
                        }
                        else
                        {
                            InitState = EInitState.ComponentNamesMaxCount;
                            LoggerState = ELoggerState.NotInitialized;
                            throw (new LoggerException(InitState, LoggerState, @"Count of given components is too big.."));
                        }
                    }
                    else
                    {
                        InitState = EInitState.StateLevelInvalid;
                        LoggerState = ELoggerState.NotInitialized;
                        throw (new LoggerException(InitState, LoggerState, @"Invalid state log level given."));
                    }
                }
                else
                {
                    InitState = EInitState.StatesMaxCount;
                    LoggerState = ELoggerState.NotInitialized;
                    throw (new LoggerException(InitState, LoggerState, @"Count of given states is too big."));
                }
            }
            catch (OutOfMemoryException ex)
            {
                InitState = EInitState.InitializationFailed;
                LoggerState = ELoggerState.NotInitialized;

                throw (new LoggerException(InitState, LoggerState, @"Out of memory exception occurred.", ex));
            }
            catch (IOException ex)
            {
                InitState = EInitState.InitializationFailed;
                LoggerState = ELoggerState.NotInitialized;

                throw (new LoggerException(InitState, LoggerState, @"IO exception occurred.", ex));
            }
            catch (UnauthorizedAccessException ex)
            {
                InitState = EInitState.InitializationFailed;
                LoggerState = ELoggerState.NotInitialized;

                throw (new LoggerException(InitState, LoggerState, @"UnauthorizedAccess exception occurred.", ex));
            }
            catch (ArgumentException ex)
            {
                InitState = EInitState.InitializationFailed;
                LoggerState = ELoggerState.NotInitialized;

                throw (new LoggerException(InitState, LoggerState, @"Argument exception occurred.", ex));
            }
            catch (NotSupportedException ex)
            {
                InitState = EInitState.InitializationFailed;
                LoggerState = ELoggerState.NotInitialized;

                throw (new LoggerException(InitState, LoggerState, @"NotSupported exception occurred.", ex));
            }
        }

        /// <summary>
        ///     Add a new log entry to the log list
        /// </summary>
        /// <param name="logMessage">Message of the log entry</param>
        /// <param name="logStateId">State id of the log entry</param>
        /// <param name="logComponentNameId">ComponentName id of the log entry</param>
        /// <exception cref="LoggerException">Catched logger excepetion</exception>
        public void AddEntry(string logMessage, ELoggerStateLevels logStateId = 0, ELoggerComponentLevels logComponentNameId = 0)
        {
            try
            {
                int indexStateList = -1;
                int indexComponentList = -1;

                // Check if the logger is initialized
                if (InitState != EInitState.Initialized)
                {
                    // Check if the logger is initialized or not
                    if (InitState == EInitState.NotInitialized)
                    {
                        LoggerState = ELoggerState.NotInitialized;
                        throw (new LoggerException(InitState, LoggerState, @"Logger is not initialized."));
                    }
                }

                // Check if logging is disabled
                if (InitState == EInitState.Initialized && LoggerState == ELoggerState.LoggingDisabled)
                    return;

                // Reset values
                int ID = 0;
                DateTime timeStamp = DateTime.Now;
                LoggerState = ELoggerState.Initialized;

                // Get state level and component level index
                if (logStateId > 0)
                    indexStateList = (int)(Math.Log10((int)logStateId) / Math.Log10(2)) + 1;
                if (logComponentNameId > 0)
                    indexComponentList = (int)(Math.Log10((int)logComponentNameId) / Math.Log10(2)) + 1;

                // Get next log entry ID
                if (_logEntryList.Count > 0)
                {
                    ID = _logEntryList[_logEntryList.Count - 1].LogID + 1;
                }

                // Get state name
                string logStateIdValue = @"-";
                // Check if states exsists
                if (LoggerStatesList != null && indexStateList > 0)
                {
                    if (LoggerStatesList.Count >= indexStateList)
                        logStateIdValue = LoggerStatesList[indexStateList - 1];
                    else
                    {
                        LoggerState = ELoggerState.StateIndexInvalid;
                        throw (new LoggerException(InitState, LoggerState, @"Given state index is invalid."));
                    }
                }

                // Get component name
                string logComponentNameIdValue = @"-";
                // Check if component names exists
                if (LoggerComponentNamesList != null && indexComponentList > 0)
                {
                    if (LoggerComponentNamesList.Count >= indexComponentList)
                        logComponentNameIdValue = LoggerComponentNamesList[indexComponentList - 1];
                    else
                    {
                        LoggerState = ELoggerState.ComponentNameIndexInvalid;
                        throw (new LoggerException(InitState, LoggerState, @"Given component name is invalid."));
                    }
                }

                // Get color value
                Color logColorValue = Color.Black;
                // Check if colors exists
                if (LoggerColorList != null)
                {
                    if (LoggerColorList.Count >= indexStateList)
                        logColorValue = LoggerColorList[indexStateList - 1];
                }

                // Check if the state should be logged
                if ((LoggerLogLevelStates & (int)logStateId) == (int)logStateId)
                {
                    // Check if the component should be logged
                    if ((LoggerLogLevelComponents & (int)logComponentNameId) == (int)logComponentNameId)
                    {

                        // Create a new log entry object
                        LogEntry entry = new LogEntry(ID, timeStamp, logStateIdValue, logComponentNameIdValue, logColorValue,
                            logMessage);

                        // Check if the maximum size of the list has been reached and then remove the frist element in the list
                        if (_logEntryList.Count >= LoggerSize)
                        {
                            _logEntryList.RemoveAt(0);
                        }

                        // Add new log entry to the list
                        _logEntryList.Add(entry);

                        // Write to file
                        if (LoggerToFileEnabled)
                        {
                            using (
                                _streamWriterLogFile =
                                    new StreamWriter(LoggerPathAndFileName, LoggerAppendToFile, Encoding.UTF8)
                                )
                            {

                                if (WriteLogEntry(_streamWriterLogFile, _logEntryList.Last()))
                                {
                                    LoggerState = ELoggerState.NewEntryAddSuccessful;
                                }
                                else
                                {
                                    LoggerState = ELoggerState.NewEntryAddFailed;
                                    throw (new LoggerException(InitState, LoggerState,
                                        @"Writting log entry failed."));
                                }
                            }
                        }
                    }
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"UnauthorizedAccess exception occurred.", ex));
            }
            catch (ArgumentException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"Argument exception occurred.", ex));
            }
            catch (DirectoryNotFoundException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"DirectoryNotFound exception occurred.", ex));
            }
            catch (PathTooLongException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"PathTooLong exception occurred.", ex));
            }
            catch (OutOfMemoryException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"Out of memory exception occurred.", ex));
            }
            catch (System.Security.SecurityException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"Security exception occurred.", ex));
            }
            catch (IOException ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"IO exception occurred.", ex));
            }
            catch (Exception ex)
            {
                LoggerState = ELoggerState.NewEntryAddFailed;
                throw (new LoggerException(InitState, LoggerState, @"Exception occurred.", ex));
            }
        }

        /// <summary>
        ///     This function retrievs the color of the given state level
        ///     If the state level is invalid the color "Black" is given
        /// </summary>
        /// <param name="stateLevel"></param>
        /// <returns></returns>
        public Color GetColorOfStateLevel(ELoggerStateLevels stateLevel)
        {
            try
            {
                int indexStateList = -1;

                // Get state level and component level index
                if (stateLevel > 0)
                    indexStateList = (int)(Math.Log10((int)stateLevel) / Math.Log10(2)) + 1;

                // Get color value
                Color logColorValue = new Color();
                // Check if colors exists
                if (LoggerColorList != null)
                {
                    if (LoggerColorList.Count >= indexStateList)
                        logColorValue = LoggerColorList[indexStateList - 1];
                }

                return logColorValue;

            }
            catch
            {
                return Color.Black;
            }
        }

        /// <summary>
        ///     This function write the log entry to the file
        /// </summary>
        /// <param name="stream">Stream for the log file</param>
        /// <param name="logEntry">Log entry which should be writen</param>
        /// <returns></returns>
        internal bool WriteLogEntry(StreamWriter stream, LogEntry logEntry)
        {
            try
            {
                if (stream != null)
                {
                    stream.WriteLine("{0:0000}\t{1} {2,-15} {3,-20} {4}", logEntry.LogID, logEntry.LogTimeStamp.ToShortDateString() + " " + logEntry.LogTimeStamp.ToLongTimeString(), logEntry.LogState, logEntry.LogComponentName, logEntry.Log);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     This function cleans up the log directory.
        ///     The logger creates for every day a new log file.
        ///     The logger leaves the given LoggerKeptLogFiles count of log files and
        ///     deletes the other.
        /// </summary>
        /// <param name="iStoredLogFiles">Value of the kept log files</param>
        /// <exception cref="LoggerException">Catched logger excepetion</exception>
        /// <returns>Value of the deleted log files.</returns>
        public int CleanUpLogFiles(int iStoredLogFiles )
        {
            try
            {
                int iDeletedFiles = 0;

                // Check if the logger is initialized
                if (InitState != EInitState.Initialized)
                {
                    // Check if the logger is initialized or not
                    if (InitState == EInitState.NotInitialized)
                    {
                        LoggerState = ELoggerState.NotInitialized;
                        throw (new LoggerException(InitState, LoggerState, @"Logger is not initialized."));
                    }
                }

                // Build list of files
                string[] fileList = Directory.GetFiles(LoggerPath);
                Array.Sort(fileList, new ReverseComparer());

                // Delete files
                for (int i = iStoredLogFiles; i < fileList.Length; i++)
                {
                    File.Delete(fileList[i]);

                    iDeletedFiles++;
                }

                LoggerState = ELoggerState.CleanUpLogFilesSuccessful;
                return iDeletedFiles;
            }
            catch (PathTooLongException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"PathTooLong exception occurred.", ex));
            }
            catch (DirectoryNotFoundException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"DirectoryNotFound exception occurred.", ex));
            }
            catch (IOException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"IO exception occurred.", ex));
            }
            catch (UnauthorizedAccessException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"UnauthorizedAccess exception occurred.", ex));
            }
            catch (ArgumentException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"Argument exception occurred.", ex));
            }
            catch (NotSupportedException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"NotSupported exception occurred.", ex));
            }
            catch (RankException ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"Rank exception occurred.", ex));
            }
            catch (Exception ex)
            {
                LoggerState = ELoggerState.CleanUpLogFilesFailed;
                throw (new LoggerException(InitState, LoggerState, @"Exception occurred.", ex));
            }
        }

        public class ReverseComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                // Compare y and x in reverse order.
                return y.CompareTo(x);
            }
        }

        #endregion Methodes
    }
}