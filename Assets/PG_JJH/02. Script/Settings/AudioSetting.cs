using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using JJH;

namespace JJH
{
    public class AudioSetting : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Slider bgmSlider;
        public Slider sfxSlider;

        string bgm = "BGM";
        string sfx = "SFX";


        private void Start()
        {
            bgmSlider.onValueChanged.AddListener(SetBGMvlolume);
            sfxSlider.onValueChanged.AddListener(SetSFXvolume);

            //  슬라이더초기화
            bgmSlider.value = PlayerPrefs.GetFloat(bgm, 0.4f); // 두 번째 매개변수는 디폴트값. -->없으면
            sfxSlider.value = PlayerPrefs.GetFloat(sfx, 0.7f);

        }

        public void SetBGMvlolume(float volume)
        {

            if (volume == 0)
            {
                audioMixer.SetFloat(bgm, -80f);
            }
            else
            {
                //오디오의크기는 데시벨을 이용 --> 데시벨은 log 사용. *20 하면 데시벨로 변환됨. 
                audioMixer.SetFloat(bgm, Mathf.Log10(volume) * 20);

            }
            PlayerPrefs.SetFloat(sfx, volume);
        }

        public void SetSFXvolume(float volume)
        {
            if (volume == 0)
            {
                audioMixer.SetFloat(sfx, -80f);
            }
            else
            {
                audioMixer.SetFloat(sfx, Mathf.Log10(volume) * 20);

            }
            PlayerPrefs.SetFloat(sfx, volume);
        }
    }
}


