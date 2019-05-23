using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public RawImage image;
    public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        Application.runInBackground = true;
        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        //WWW www = new WWW(VideoURL);
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //We want to play from video clip not from url
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
        //videoPlayer.url = "https://r8---sn-ab02a0nfpgxapox-bh2es.googlevideo.com/videoplayback?initcwndbps=7757500&ipbits=0&dur=252.168&key=yt6&lmt=1536932379214742&expire=1555599091&pcm2cms=yes&gir=yes&itag=248&id=o-ABO53_TJpJFWwvomF6VYQ4wAlwt9m7GIQZ-SHf3Mf-Up&c=WEB&keepalive=yes&fvip=3&sparams=aitags%2Cclen%2Cdur%2Cei%2Cgir%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Ckeepalive%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpcm2cms%2Cpl%2Crequiressl%2Csource%2Cexpire&mn=sn-ab02a0nfpgxapox-bh2es%2Csn-ogueln7d&mm=31%2C26&clen=71489908&ms=au%2Conr&ei=kzq4XPvUC4esqQHbhpfgCQ&pl=22&mv=m&mt=1555577389&source=youtube&ip=113.198.217.67&aitags=133%2C134%2C135%2C136%2C137%2C160%2C242%2C243%2C244%2C247%2C248%2C278%2C394%2C395%2C396%2C397%2C398&mime=video%2Fwebm&requiressl=yes";
        //videoPlayer.source = VideoSource.VideoClip;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video to play then prepare audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while(!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 seconds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }

        Debug.Log("Done preparing Video");

        //Assign the texture from video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        Debug.Log("Playing video");
        while (videoPlayer.isPlaying)
        {
            //Debug.LogWarning("Video Time : " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
