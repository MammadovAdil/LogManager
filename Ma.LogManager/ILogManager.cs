using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ma.LogManager
{
    /// <summary>
    /// Log manager interface.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Write information to log.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When EventLog and logFolder.
        /// </exception>
        /// <param name="info">Information to log.</param>
        void Log(string info);

        /// <summary>
        /// Write details of exception to log.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When EventLog and logFolder.
        /// </exception>
        /// <param name="exceptionDetails">Details of exception.</param>
        void Log(ExceptionDetails exceptionDetails);
    }
}
