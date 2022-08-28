using Chaos.Entities.Networking;

namespace Chaos.Networking.Interfaces;

public interface IRedirectManager
{
    void Add(Redirect redirect);
    bool TryGetRemove(uint id, out Redirect redirect);
}