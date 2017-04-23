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

namespace Logging
{
    /// <summary>
    ///     Exception which occurs in the logger.
    ///     <para>EInitState contains the state of the logger initialization</para>
    ///     <para>ELoggerState contains the state of the logger</para>
    /// </summary>
    public class LoggerException: Exception
    {
        #region Variables

        /// <summary>
        ///     Stores the logger initialization state
        /// </summary>
        private Logger.EInitState _loggerInitState;

        /// <summary>
        ///     Stores the logger state
        /// </summary>
        private Logger.ELoggerState _loggerState;

        #endregion Variables

        #region Properties

        public Logger.EInitState InitState
        {
            get { return _loggerInitState; }
            internal set { _loggerInitState = value; }
        }

        public Logger.ELoggerState LoggerState
        {
            get { return _loggerState; }
            internal set { _loggerState = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="loggerInitState">Initialization state of the logger</param>
        /// <param name="loggerState">State of the logger</param>
        public LoggerException(Logger.EInitState loggerInitState, Logger.ELoggerState loggerState, string Message)
            : base(Message)
        {
            InitState = loggerInitState;
            LoggerState = loggerState;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="loggerInitState">Initialization state of the logger</param>
        /// <param name="loggerState">State of the logger</param>
        public LoggerException(Logger.EInitState loggerInitState, Logger.ELoggerState loggerState, string Message, Exception inner ) : base (Message, inner)
        {
            InitState = loggerInitState;
            LoggerState = loggerState;
        }

        #endregion Methodes
    }
}
