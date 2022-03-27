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
        private static void EnterSpiderTownTrap(int index) //Deepnest_Spider_Town
        {
            string goName = "RestBench Spider";
            string websFsmName = "Fade";
            string benchFsmName = "Bench Control Spider";
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
            if (index == 2)
            {
                websFSM.SendEvent("FINISHED");
            }
        }
        public static void BreakTHKChains(int index)
        {
            string fsmName = "Control";
            string goName1 = "hollow_knight_chain_base";
            string goName2 = "hollow_knight_chain_base 2";
            string goName3 = "hollow_knight_chain_base 3";
            string goName4 = "hollow_knight_chain_base 4";
            PlayMakerFSM fsm1 = FindFsmGlobally(goName1, fsmName);
            PlayMakerFSM fsm2 = FindFsmGlobally(goName2, fsmName);
            PlayMakerFSM fsm3 = FindFsmGlobally(goName3, fsmName);
            PlayMakerFSM fsm4 = FindFsmGlobally(goName4, fsmName);
            fsm1.SetState("Break");
            fsm2.SetState("Break");
            fsm3.SetState("Break");
            fsm4.SetState("Break");
        }
        public static void DoRoomSpecific(string scene, int index)//index only used if multiple functionallities in one room, safe to ignore for now.
        {
            switch (scene)
            {
                case "Deepnest_Spider_Town":
                    EnterSpiderTownTrap(index);
                    break;
                case "Final_Boss_Core":
                    BreakTHKChains(index);
                    break;
                default:
                    Console.AddLine("No Room Specific Function Found In: " + scene);
                    break;
            }
        }
        private static PlayMakerFSM FindFsmGlobally(string gameObjectName, string fsmName)
        {
            return GameObject.Find(gameObjectName).LocateMyFSM(fsmName);
        }
    }
}
