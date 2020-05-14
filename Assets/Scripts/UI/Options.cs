﻿/*
 * Created by:
 * Name: Danny Pym-Hember
 * Sid: 1513999
 * Date Created: 29/09/2019
 * Last Modified: 05/10/2019
 *  Modified by: Alexander Watson
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    private ControllerInputDetection controllerInput;

    #region Attributes

    [SerializeField]
    private AudioMixer gameMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private SaveData Save;

    #region Player Pref Key Constants

    private const string Resolution_Pref_Key = "resolution";

    #endregion

    #region Resolution
    /*[SerializeField]
    private Text resolutionText;

    private Resolution[] resolutions;

    private int currentResolutionIndex = 0;*/
    #endregion

    #endregion


    // Start is called before the first frame update
    private void Start()
    {
        controllerInput = GameObject.Find("EventSystem").GetComponent<ControllerInputDetection>();
        Save = GameObject.Find("SaveData").GetComponent<SaveData>();
        //resolutions = Screen.resolutions;

        musicSlider.value = SoundManager.Instance.MusicVol;
        sfxSlider.value = SoundManager.Instance.SoundVol;
        masterSlider.value = SoundManager.Instance.MasterVol;

        #region OLD UNUSED CODE
        /*if (musicSlider && sfxSlider != null)
        {
           musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
           sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }*/

        // SetMusicVolume(PlayerPrefs.GetFloat("musicVol", 0));
        // SetSFXVolume(PlayerPrefs.GetFloat("sfxVol", 0));

        //musicSlider.value = PlayerPrefs.GetFloat("musicVol", 0);
        //sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0);

        //currentResolutionIndex = PlayerPrefs.GetInt(Resolution_Pref_Key, 0);

        //Set resolution Text to screens resolution on start up
        //SetResolutionText(resolutions[currentResolutionIndex]);

        #endregion
    }

    //Cycling through Resolutions
    #region Resolution Cycling

    /*//Setting resolution text
    private void SetResolutionText(Resolution resolution)
    {
        resolutionText.text = resolution.width + "x" + resolution.height;
    }

    //Advancing resolution to the next one 
    public void SetNextResolution()
    {
        currentResolutionIndex = GetNextWrappedIndex(resolutions, currentResolutionIndex);
        SetResolutionText(resolutions[currentResolutionIndex]);
    }

    //Advancing resolution to the previous one
    public void SetPreviousResolution()
    {
        currentResolutionIndex = GetPreviousWrappedIndex(resolutions, currentResolutionIndex);
        SetResolutionText(resolutions[currentResolutionIndex]);
    }*/
    #endregion

    //Applying the Resolution
    /*#region Apply Resolution

    private void SetAndApplyResolution(int newResolutionIndex)
    {
        currentResolutionIndex = newResolutionIndex;
        ApplyCurrentResolution();
    }

    private void ApplyCurrentResolution()
    {
        ApplyResolution(resolutions[currentResolutionIndex]);
    }

    private void ApplyResolution(Resolution resolution)
    {
        SetResolutionText(resolution);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(Resolution_Pref_Key, currentResolutionIndex);
    }

    #endregion

    //Wrapping the Index of Resolutions
    #region Misc Helpers
    #region Index Wrap Helpers
    private int GetNextWrappedIndex<T>(IList<T> collection, int currentIndex)
    {
        if (collection.Count < 1) return 0;
        return (currentIndex + 1) % collection.Count;
    }

    private int GetPreviousWrappedIndex<T>(IList<T> collection, int currentIndex)
    {
        if (collection.Count < 1) return 0;
        if ((currentIndex - 1) < 0) return collection.Count - 1;
        return (currentIndex - 1) % collection.Count;
    }
    #endregion
    #endregion

    //Applying Resolution changes button
    /*public void ApplyChanges()
    {
        SetAndApplyResolution(currentResolutionIndex);
    }*/

    //Applying music settings on button press
    public void ApplyMusicSettings()
    {
        Save.Save();
    }

    //Setting volume levels
    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.SetBGMVol(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVol(volume);
    }

    public void SetMasterVolume(float volume)
    {
        SoundManager.Instance.SetMasterVol(volume);
    }

    //Opens resolution menu
    /*public void ResolutionSettings()
    {
        resolutionSettingsPanel.gameObject.SetActive(true);
        soundSettingsPanel.gameObject.SetActive(false);
    }*/

    //Opens sound menu
    /*public void SoundSettings()
    {
        soundSettingsPanel.gameObject.SetActive(true);
        resolutionSettingsPanel.gameObject.SetActive(false);
    }*/
}
