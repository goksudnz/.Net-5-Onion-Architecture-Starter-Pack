// Copyrights(c) Charqe.io. All rights reserved.

using System;

namespace Domain.Interfaces
{
    public interface IHistoryPersisted
    {
        string CreatedBy { get; set; }
        DateTime CreateDate { get; set; }
    }
}