/*
*Author:walkingfy
*/
using System;

namespace Tz.Domain
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TKey">标识类型</typeparam>
    public interface IEntity
    {
        /// <summary>
        /// 标识
        /// </summary>
        Guid Id { get; }
    }
}
