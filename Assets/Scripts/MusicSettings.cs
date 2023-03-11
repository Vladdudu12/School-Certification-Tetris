using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{


    [SerializeField]
    private GameObject _soundFx;
    [SerializeField]
    private float _soundFxVolume;
    [SerializeField]
    private GameObject _soundFxUnmuted;
    [SerializeField]
    private GameObject _soundFxMuted;


    [SerializeField]
    private GameObject _music;
    [SerializeField]
    private float _musicVolume;
    [SerializeField]
    private GameObject _musicUnmuted;
    [SerializeField]
    private GameObject _musicMuted;

    private Slider _soundSlider;
    private Slider _musicSlider;


    private AudioSource _soundFxSource;
    private AudioSource _musicSource;


    private void Awake()
    {
        _soundFxSource = GameObject.Find("SoundFx").GetComponent<AudioSource>();
        _musicSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
    }
    private void Start()
    {
        _soundSlider = _soundFx.GetComponentInChildren<Slider>();
        _musicSlider = _music.GetComponentInChildren<Slider>();
    }

    public void SoundEffectsMute()
    {
        _soundFxUnmuted.SetActive(false);
        _soundFxMuted.SetActive(true);
        _soundFxVolume = _soundSlider.value;
        _soundSlider.value = 0;

    }

    public void SoundEffectsUnmute()
    {
        _soundFxMuted.SetActive(false);
        _soundFxUnmuted.SetActive(true);
        _soundSlider.value = _soundFxVolume;
    }

    public void MusicMute()
    {

        _musicUnmuted.SetActive(false);
        _musicMuted.SetActive(true);
        _musicVolume = _musicSlider.value;
        _musicSlider.value = 0;
    }

    public void MusicUnmute()
    {
        _musicMuted.SetActive(false);
        _musicUnmuted.SetActive(true);
        _musicSlider.value = _musicVolume;
    }

    public void AdjustSoundFxVolume()
    {
        _soundFxSource.volume = _soundSlider.value;
    }

    public void AdjustMusicVolume()
    {
        _musicSource.volume = _musicSlider.value;
    }

}
