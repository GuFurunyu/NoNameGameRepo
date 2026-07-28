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

    bool hasStartedMainBoardBGM;

    int storedInGameBGMIndex = -1;
    float storedPitch = 1;

    float curAccumulatedChangedPitch;
    bool isPitchIncreasing;

    #region ConstantsUsed
    AudioSource audioSource;

    AudioClip mainBoardBGM;
    List<AudioClip> inGameBGMs;

    float normalVolume;
    float volumeFadingOutSpeed;
    float volumeFadingOutThres;

    float pitchChangingSpeed;
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
        mainBoardBGM = CONS.mainBoardBGM;
        inGameBGMs = CONS.inGameBGMs;
        normalVolume = CONS.normalVolume;
        volumeFadingOutSpeed = CONS.volumeFadingOutSpeed;
        volumeFadingOutThres = CONS.volumeFadingOutThres;
        pitchChangingSpeed = CONS.pitchChangingSpeed;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        //noAudioSet
        audioSource.volume = 0;

        //mainBoard
        if (VARS.IsInMainBoard)
        {
            if (!hasStartedMainBoardBGM)
            {
                audioSource.volume = normalVolume;

                audioSource.clip = mainBoardBGM;
                audioSource.Play();
                audioSource.loop = true;

                hasStartedMainBoardBGM = true;
            }
        }
        else
        {
            if (hasStartedMainBoardBGM)
            {
                audioSource.Pause();

                hasStartedMainBoardBGM = false;
            }
        }

        if (VARS.IsInNewRoomAllResetOver)
        {
            //inGame
            if (!VARS.IsInMainBoard &&
                VARS.HasJumped)
            {
                VARS.curInGameBGMIndex = VARS.curFaceIndex - 1;

                //ifEnterNewFacePlayItsBGM
                if (storedInGameBGMIndex != VARS.curInGameBGMIndex)
                {
                    storedInGameBGMIndex = VARS.curInGameBGMIndex;

                    VARS.IsFormerBgmFadingOut = true;
                }
                //formalBgmFadeOut
                if (VARS.IsFormerBgmFadingOut)
                {
                    audioSource.volume -= volumeFadingOutSpeed * Time.deltaTime;

                    if (audioSource.volume < volumeFadingOutThres)
                    {
                        audioSource.volume = normalVolume;

                        audioSource.clip = inGameBGMs[storedInGameBGMIndex];
                        audioSource.UnPause();
                        audioSource.Play();
                        audioSource.loop = true;

                        VARS.IsFormerBgmFadingOut = false;
                    }
                }

                ////pitch
                //if (!VARS.IsChangingPitch)
                //{
                //    VARS.curPitch = 1f + (float)(VARS.faceDirectionIndexes[VARS.curInGameBGMIndex] / 10f);
                //    //setAudioPitch
                //    if (storedPitch != VARS.curPitch)
                //    {
                //        //storedPitch = VARS.curPitch;

                //        //audioSource.pitch = storedPitch;

                //        //audioSource.Pause();

                //        curAccumulatedChangedPitch = 0;
                //        VARS.curTargetAccumulatedChangedPitch = Mathf.Abs(VARS.curPitch - storedPitch);
                //        if (VARS.curPitch > storedPitch)
                //        {
                //            isPitchIncreasing = true;
                //        }
                //        else
                //        {
                //            isPitchIncreasing = false;
                //        }

                //        //audioSource.Pause();
                //        audioSource.volume = 0;

                //        VARS.IsChangingPitch = true;
                //    }
                //}
                ////else
                ////{
                ////    //audioSource.UnPause();
                ////}
                //else if (VARS.IsChangingPitch)
                //{
                //    curAccumulatedChangedPitch += pitchChangingSpeed * Time.deltaTime;

                //    if (isPitchIncreasing)
                //    {
                //        storedPitch += pitchChangingSpeed * Time.deltaTime;
                //    }
                //    else
                //    {
                //        storedPitch += -pitchChangingSpeed * Time.deltaTime;
                //    }

                //    audioSource.pitch = storedPitch;

                //    audioSource.volume += normalVolume * (pitchChangingSpeed / VARS.curTargetAccumulatedChangedPitch) * Time.deltaTime;                    

                //    if (curAccumulatedChangedPitch > VARS.curTargetAccumulatedChangedPitch)
                //    {
                //        storedPitch = VARS.curPitch;

                //        audioSource.pitch = storedPitch;

                //        audioSource.volume = normalVolume;

                //        //audioSource.UnPause();

                //        VARS.IsChangingPitch = false;
                //    }
                //}
            }
        }
    }
}
