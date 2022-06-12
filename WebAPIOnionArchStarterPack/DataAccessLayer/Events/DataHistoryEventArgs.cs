// Copyrights(c) Charqe.io. All rights reserved.

using System;
using Domain.Entities.LogEntities;

namespace DataAccessLayer.Events
{
    public class DataHistoryEventArgs : EventArgs
    {
        public readonly DataHistory DataHistory;

        /// <summary>
        /// Constructor to set data history.
        /// </summary>
        /// <param name="dataHistory">Data history object.</param>
        public DataHistoryEventArgs(DataHistory dataHistory) => DataHistory = dataHistory;
    }
}