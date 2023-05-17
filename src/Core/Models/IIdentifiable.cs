using System;

namespace Core.Models;

public interface IIdentifiable/* : IEquatable<IIdentifiable>*/
{
    int Id { get; }
}
