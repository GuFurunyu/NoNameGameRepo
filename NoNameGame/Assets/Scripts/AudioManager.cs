using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.audioManager)]
public class AudioManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    int storedAudioClipIndex = -1;

    #region ConstantsUsed
    AudioSource audioSource;

    List<AudioClip> audioClips;

    float normalVolume;
    float volumeFadingOutSpeed;
    float volumeFadingOutThres;
    #endregion

    #region VariablesUsed

    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        audioSource = CONS.audioSource;
        audioClips = CONS.audioClips;
        normalVolume = CONS.normalVolume;
        volumeFadingOutSpeed = CONS.volumeFadingOutSpeed;
        volumeFadingOutThres = CONS.volumeFadingOutThres;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsInNewRoomAllResetOver)
        {
            VARS.curAudioClipIndex = VARS.curFaceIndex - 1;

            //ifEnterNewFacePlayItsBGM
            if (storedAudioClipIndex != VARS.curAudioClipIndex)
            {
                storedAudioClipIndex = VARS.curAudioClipIndex;

                VARS.IsFormerBgmFadingOut = true;
            }
            //formalBgmFadeOut
            if (VARS.IsFormerBgmFadingOut)
            {
                audioSource.volume -= volumeFadingOutSpeed * Time.deltaTime;

                if (audioSource.volume < volumeFadingOutThres)
                {
                    audioSource.volume = normalVolume;

                    audioSource.clip = audioClips[storedAudioClipIndex];
                    audioSource.Play();
                    audioSource.loop = true;

                    VARS.IsFormerBgmFadingOut = false;
                }
            }
        }
    }
}
