using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgMusic;
    public AudioSource fx;

    [Header("Sound FX Audio Clips")]
    public AudioClip placeholder;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is another thing that is not the audio manager instance, destroying it.");
            Destroy(this);
        }

        instance = this;
        //singleton ref of this script
    }


}
