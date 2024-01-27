using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace CaptchaGame
{
    public class VideoCaptchaHelper : MonoBehaviour, INotificationReceiver
    {
        public VideoPlayer videoPlayer;

        private void Start()
        {
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            Debug.Log("Hello world");
            videoPlayer.playbackSpeed = 1.0F;
            videoPlayer.SetDirectAudioVolume(0, 0.0F);
            videoPlayer.Play();
            yield return null;
            videoPlayer.SetDirectAudioVolume(0, 1.0F);
            videoPlayer.playbackSpeed = 0.0F;
        }
    
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            videoPlayer.time = 0.0F;
            videoPlayer.playbackSpeed = 1.0F;
            videoPlayer.Play();
        }
    }
}
