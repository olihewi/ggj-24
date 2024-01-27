using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CaptchaGame.Utilities
{
    public class NotificationMarker : Marker, INotification
    {
        public PropertyName id { get; }
    }
}