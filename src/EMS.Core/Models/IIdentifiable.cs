using System;

namespace EMS.Core.Models;

public interface IIdentifiable/* : IEquatable<IIdentifiable>*/
{
    int Id { get; }
}
