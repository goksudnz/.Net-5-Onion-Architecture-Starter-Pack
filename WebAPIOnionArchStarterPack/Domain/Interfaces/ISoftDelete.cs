// Copyrights(c) Charqe.io. All rights reserved.

namespace Domain.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}