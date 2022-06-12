// Copyrights(c) Charqe.io. All rights reserved.

using System;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser, ISoftDelete, IHistoryPersisted
    {
        public string Name { get; set; }
        public new string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public ProviderType ProviderType { get; set; }
        
        #region Interface Injects
        
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        
        #endregion
    }
}