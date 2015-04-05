using FollowMe.Configuration;
using FollowMe.Enums;

namespace FollowMe.Interfaces
{
    public interface ITargetLocator
    {
        TargetLocation GetTargetLocation(TrackingConfig trackingConfig, bool trackingPreviewEnabled);
    }
}
