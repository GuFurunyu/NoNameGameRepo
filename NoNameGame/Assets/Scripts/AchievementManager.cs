using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.achievementManager)]
public class AchievementManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    GameObject[] roomPlanes = new GameObject[54];
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
        roomPlanes = CONS.roomPlanes;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (SteamManager.Initialized)
        {
            //rotate
            if (!VARS.isAchievementRotateUnlocked &&
                VARS.IsRotating)
            {
                SteamUserStats.SetAchievement("Achievement_Rotate");
                SteamUserStats.StoreStats();

                VARS.isAchievementRotateUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //twist
            if (!VARS.isAchievementTwistUnlocked &&
                VARS.IsTwisting)
            {
                SteamUserStats.SetAchievement("Achievement_Twist");
                SteamUserStats.StoreStats();

                VARS.isAchievementTwistUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //gearsOfDestiny
            if (!VARS.isAchievementGearsOfDestinyUnlocked &&
                VARS.IsTwisting &&
                !VARS.IsInCenter)
            {
                SteamUserStats.SetAchievement("Achievement_GearsOfDestiny");
                SteamUserStats.StoreStats();

                VARS.isAchievementGearsOfDestinyUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //portal
            if (!VARS.isAchievementPortalUnlocked &&
                VARS.IsJustTransported)
            {
                SteamUserStats.SetAchievement("Achievement_Portal");
                SteamUserStats.StoreStats();

                VARS.isAchievementPortalUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //pivot
            if (!VARS.isAchievementPivotUnlocked &&
                VARS.curAccessedCenterSavePointPositions.Count >= 6)
            {
                SteamUserStats.SetAchievement("Achievement_Pivot");
                SteamUserStats.StoreStats();

                VARS.isAchievementPivotUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //fullAppearance
            if (!VARS.isAchievementFullAppearanceUnlocked &&
                !VARS.IsRoomExplored.Contains(false))
            {
                SteamUserStats.SetAchievement("Achievement_FullAppearance");
                SteamUserStats.StoreStats();

                VARS.isAchievementFullAppearanceUnlocked = true;

                VARS.IsToWriteAchievementData= true;
            }
            //red
            if (!VARS.isAchievementRedUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[5] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Red");
                SteamUserStats.StoreStats();

                VARS.isAchievementRedUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //yellow
            if (!VARS.isAchievementYellowUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[0] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Yellow");
                SteamUserStats.StoreStats();

                VARS.isAchievementYellowUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //blue
            if (!VARS.isAchievementBlueUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[3] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Blue");
                SteamUserStats.StoreStats();

                VARS.isAchievementBlueUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //orange
            if (!VARS.isAchievementOrangeUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[2] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Orange");
                SteamUserStats.StoreStats();

                VARS.isAchievementOrangeUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //green
            if (!VARS.isAchievementGreenUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[4] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Green");
                SteamUserStats.StoreStats();

                VARS.isAchievementGreenUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //purple
            if (!VARS.isAchievementPurpleUnlocked &&
                VARS.curOneColorFragmentCollectedNumbers[1] >= 8)
            {
                SteamUserStats.SetAchievement("Achievement_Purple");
                SteamUserStats.StoreStats();

                VARS.isAchievementPurpleUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //CUBE_
            if (!VARS.isAchievementCUBE_Unlocked &&
                VARS.curAllColorsFragmentCollectedNumber >= 48)
            {
                SteamUserStats.SetAchievement("Achievement_CUBE_");
                SteamUserStats.StoreStats();

                VARS.isAchievementCUBE_Unlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //connected
            if (!VARS.isAchievementConnectedUnlocked &&
                VARS.curKeysAndLocksCollectedNumber >= 96)
            {
                SteamUserStats.SetAchievement("Achievement_Connected");
                SteamUserStats.StoreStats();

                VARS.isAchievementConnectedUnlocked = true;

                VARS.IsToWriteAchievementData = true;
            }
            //solved
            if (!VARS.isAchievementSolvedUnlocked &&
                !VARS.IsRoomExplored.Contains(false))
            {
                for (int i = 0; i < 54; i++)
                {
                    if (roomPlanes[i].transform.parent != roomPlanes[(i / 9) * 9].transform.parent)
                    {
                        break;
                    }

                    if (i == 53)
                    {
                        SteamUserStats.SetAchievement("Achievement_Solved");
                        SteamUserStats.StoreStats();

                        VARS.isAchievementSolvedUnlocked = true;

                        VARS.IsToWriteAchievementData = true;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("SteamManager is not initialized. Achievements will not be tracked.");
        }
    }
}
