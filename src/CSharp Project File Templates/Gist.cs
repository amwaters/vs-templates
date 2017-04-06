// File fetched from: https://gist.github.com/amwaters/ade95805292888fb84f53d1e3e425ae8/raw/a2235b502badaee972ca4e4337b45cfa522208f3/Debugx.cs

// Debugging helpers for .NET and Unity 5
// (c) 2017 Aaron Waters, All Rights Reserved.

/* Released under MIT license:
  Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:
  The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BoogieHouse
{
    /// <summary>
    /// Sends debug and trace messages to default outputs as available
    /// </summary>
    public static class Debugx
    {
        /// <summary>
        /// Outputs to standard trace.
        /// Does not output to Unity.
        /// </summary>
        [Conditional("TRACE")]
        public static void Trace(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);

            for (int i = 0; i < threadTraceLog.Count; i++)
                threadTraceLog[i].Append(message);
        }

        /// <summary>
        /// Outputs to standard debug.
        /// Also outputs to Unity debug.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
#if UNITY_5
            UnityEngine.Debug.Log(message);
#endif
            for (int i = 0; i < threadDebugLog.Count; i++)
                threadDebugLog[i].Append(message);
        }

        /// <summary>
        /// Outputs an error to standard debug.
        /// Also outputs to Unity debug.
        /// Also sends a stack trace to standard trace.
        /// </summary>
        public static void Error(string message)
        {
            // to do: add non-debug log output
            System.Diagnostics.Debug.WriteLine(message);
#if UNITY_5
            UnityEngine.Debug.LogError(message);
#endif
#if TRACE
            System.Diagnostics.Trace.WriteLine(
                string.Format("Error logged: '{0}'\r\nStack trace:\r\n{1}",
                    message, Environment.StackTrace));
#endif
            for (int i = 0; i < threadErrorLog.Count; i++)
                threadErrorLog[i].Append(message);
        }

        #region Formatting overloads

        /// <summary>
        /// Outputs to standard trace.
        /// Does not output to Unity.
        /// </summary>
        [Conditional("TRACE")]
        public static void Trace(string format, params object[] args)
        {
            Trace(string.Format(format, args));
        }

        /// <summary>
        /// Outputs to standard debug.
        /// Also outputs to Unity debug.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }


        /// <summary>
        /// Outputs an error to standard debug.
        /// Also outputs to Unity debug.
        /// Also sends a stack trace to standard trace.
        /// </summary>
        public static void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        #endregion

        #region Func overloads

        /// <summary>
        /// Outputs to standard trace.
        /// Does not output to Unity.
        /// </summary>
        [Conditional("TRACE")]
        public static void Trace(Func<string> messageGetter)
        {
            Trace(messageGetter());
        }

        /// <summary>
        /// Outputs to standard debug.
        /// Also outputs to Unity debug.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Debug(Func<string> messageGetter)
        {
            Debug(messageGetter());
        }

        /// <summary>
        /// Outputs an error to standard debug.
        /// Also outputs to Unity debug.
        /// Also sends a stack trace to standard trace.
        /// </summary>
        public static void Error(Func<string> messageGetter)
        {
            Error(messageGetter());
        }

        #endregion

        #region Custom logging

        [ThreadStatic]
        static List<Logger<string>> threadTraceLog = new List<Logger<string>>();
        [ThreadStatic]
        static List<Logger<string>> threadDebugLog = new List<Logger<string>>();
        [ThreadStatic]
        static List<Logger<string>> threadErrorLog = new List<Logger<string>>();

        public static ILogger<string> CreateThreadTraceLog()
        {
            var c = new Logger<string>(a => threadTraceLog.Remove(a));
            threadTraceLog.Add(c);
            return c;
        }

        public static ILogger<string> CreateThreadDebugLog()
        {
            var c = new Logger<string>(a => threadDebugLog.Remove(a));
            threadDebugLog.Add(c);
            return c;
        }

        public static ILogger<string> CreateThreadErrorLog()
        {
            var c = new Logger<string>(a => threadErrorLog.Remove(a));
            threadErrorLog.Add(c);
            return c;
        }

        public interface ILogger<T> : IDisposable
        {
            int Count { get; }
            IList<T> Log { get; }
        }

        class Logger<T> : ILogger<T>
        {
            public Logger(Action<Logger<T>> onDispose)
            {
                this.onDispose = onDispose;
                this.Log = _Log.AsReadOnly();
            }

            public Action<Logger<T>> onDispose;
            List<T> _Log = new List<T>();
            public IList<T> Log { get; private set; }
            public int Count { get { return Log.Count; } }

            public void Append(T data)
            {
                _Log.Add(data);
            }

            public void Dispose()
            {
                onDispose(this);
            }
        }

        #endregion
    }
}