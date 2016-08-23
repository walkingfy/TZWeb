using System;

namespace Tz.Domain.Entity
{
    public class RolePermission:AggregateRoot
    {
        public RolePermission()
        { }

        public RolePermission(Guid roleId, Guid moduleId)
        {
            this.RoleId = roleId;
            this.ModuleId = moduleId;
        }


        #region Public Properties

        public Role Role { get; set; }

        public Guid RoleId { get; set; }

        public Module Module { get; set; }

        public Guid ModuleId { get; set; }

        #endregion
    }
}