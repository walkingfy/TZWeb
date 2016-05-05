namespace Tz.Domain
{
    /// <summary>
    /// 聚合根
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        byte[] Version { get; }
    }
}
