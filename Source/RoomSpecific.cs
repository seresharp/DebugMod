using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using UnityEngine;

namespace DebugMod
{
    public static class RoomSpecific
    {
        //This class is intended to recreate some scenarios, with more accuracy than that of the savestate class. 
        //This should be eventually included to compatible with savestates, stored in the same location for easier access.
        private static IEnumerator EnterSpiderTownTrap(int index) //Deepnest_Spider_Town
        {
            string supportedScene = "Deepnest_Spider_Town";
            string goName = "RestBench Spider";
            string websFsmName = "Fade";
            string benchFsmName = "Bench Control Spider";
            if (DebugMod.GM.sceneName == supportedScene)
            {
                PlayMakerFSM websFSM = FindFsmGlobally(goName, websFsmName);
                PlayMakerFSM benchFSM = FindFsmGlobally(goName, benchFsmName);
                benchFSM.SetState("Start Rest");
                benchFSM.SendEvent("WAIT");
                benchFSM.SendEvent("FINISHED");
                benchFSM.SendEvent("STRUGGLE");
                websFSM.SendEvent("FIRST STRUGGLE");
                websFSM.SendEvent("FINISHED");
                websFSM.SendEvent("FINISHED");
                websFSM.SendEvent("FINISHED");
                websFSM.SendEvent("LAND");
                websFSM.SendEvent("FINISHED");
                if(index == 2)
                {
                    websFSM.SendEvent("FINISHED");
                }
            }
            yield break;
        }
        public static void DoRoomSpecific(string scene,int index)//index only used if multiple functionallities in one room, safe to ignore for now.
        {
            switch (scene)
            {
                case "Deepnest_Spider_Town":
                    DebugMod.GM.StartCoroutine(EnterSpiderTownTrap(index));
                    break;
                default:
                    Console.AddLine("No Room Specific Function Found In: " + scene);
                    break;
            }
        }
        private static PlayMakerFSM FindFsmGlobally(string gameObjectName,string fsmName)
        {
            return GameObject.Find(gameObjectName).LocateMyFSM(fsmName);
        }
    }
}
