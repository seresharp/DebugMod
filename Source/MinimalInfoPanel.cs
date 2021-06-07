using UnityEngine;
using InControl;
using System.Collections.Generic;

namespace DebugMod
{
    public static class MinimalInfoPanel
    {
        public static bool minInfo = false;
        private static CanvasPanel altPanel;
        //private static CanvasPanel panelCurrentSaveState;

        public static void BuildMenu(GameObject canvas)
        {
            altPanel = new CanvasPanel(
                canvas, 
                GUIController.Instance.images["BlankBox"], 
                new Vector2(130f, 230f), 
                Vector2.zero, 
                new Rect(
                    0f,
                    0f,
                    GUIController.Instance.images["BlankBox"].width,
                    GUIController.Instance.images["BlankBox"].height
                )
            );

            //Labels
            altPanel.AddText("Alt vel Label", "Vel", new Vector2(10f, 10f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt pos Label", "Pos", new Vector2(110f, 10f), Vector2.zero, GUIController.Instance.arial, 15);
 
            altPanel.AddText("Alt MP Label", "MP", new Vector2(10f, 30f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt canSuperdash Label", "CanCdash", new Vector2(100f, 30f), Vector2.zero, GUIController.Instance.arial, 15);

            altPanel.AddText("Alt Nail Damage Label", "NailDmg", new Vector2(10f, 50f), Vector2.zero, GUIController.Instance.arial, 15);

            altPanel.AddText("Alt Completion Label", "Completion", new Vector2(10f, 70f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt Grubs Label", "Grubs", new Vector2(140f, 70f), Vector2.zero, GUIController.Instance.arial, 15);
         
            altPanel.AddText("Alt Scene Name Label", "Scene Name", new Vector2(10f, 90f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt Current Save State Lable", "Current SaveState", new Vector2(10f, 110f), Vector2.zero, GUIController.Instance.arial, 15);
            //altPanel.AddText("Alt SaveState AutoSlot", "Autoslot", new Vector2(10f, 130f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt SaveState CurrentSlot", "Current slot", new Vector2(110f, 130f), Vector2.zero, GUIController.Instance.arial, 15);
            altPanel.AddText("Alt WillHardfall", "Hardfall", new Vector2(10f, 130f), Vector2.zero, GUIController.Instance.arial, 15);

            //Values
            altPanel.AddText("Vel", "", new Vector2(40f, 14f), Vector2.zero, GUIController.Instance.trajanNormal);
            altPanel.AddText("Pos", "", new Vector2(140f, 14f), Vector2.zero, GUIController.Instance.trajanNormal);
 
            altPanel.AddText("MP", "", new Vector2(40f, 34f), Vector2.zero, GUIController.Instance.trajanNormal);
            altPanel.AddText("CanCdash", "", new Vector2(190f, 34f), Vector2.zero, GUIController.Instance.trajanNormal);

            altPanel.AddText("NailDmg", "", new Vector2(100f, 54f), Vector2.zero, GUIController.Instance.trajanNormal);

            altPanel.AddText("Completion", "", new Vector2(95f, 74f), Vector2.zero, GUIController.Instance.trajanNormal);
            altPanel.AddText("Grubs", "", new Vector2(195f, 74f), Vector2.zero, GUIController.Instance.trajanNormal);

            altPanel.AddText("Scene Name", "", new Vector2(140f, 94f), Vector2.zero, GUIController.Instance.trajanNormal);
            altPanel.AddText("Current SaveState", "", new Vector2(140f, 114f), Vector2.zero, GUIController.Instance.trajanNormal);
            //altPanel.AddText("Autoslot", "", new Vector2(80f, 134f), Vector2.zero, GUIController.Instance.trajanNormal);
            altPanel.AddText("Current slot", "", new Vector2(200f, 134f), Vector2.zero, GUIController.Instance.trajanNormal);

            altPanel.AddText("Hardfall", "", new Vector2(80f, 134f), Vector2.zero, GUIController.Instance.trajanNormal);
            
            altPanel.FixRenderOrder();
        }

        public static void Update()
        {
 
            if (altPanel == null)
            {
                return;
            }

            if (DebugMod.GM.IsNonGameplayScene())
            {
                if (altPanel.active)
                {
                    altPanel.SetActive(false, true);
                }
                return;
            }

            // Not intended min/full info panel logic, but should show the two panels one at a time
            if (DebugMod.settings.MinInfoPanelVisible && !altPanel.active)
            {
                altPanel.SetActive(true, false);
            }
            else if (!DebugMod.settings.MinInfoPanelVisible && altPanel.active)
            {
                altPanel.SetActive(false, true);
            }

            if (altPanel.active)
            {
                PlayerData.instance.CountGameCompletion();

                altPanel.GetText("Vel").UpdateText(HeroController.instance.current_velocity.ToString());
                altPanel.GetText("Pos").UpdateText(GetHeroPos());

                altPanel.GetText("MP").UpdateText((PlayerData.instance.MPCharge + PlayerData.instance.MPReserve).ToString());
                altPanel.GetText("NailDmg").UpdateText(DebugMod.RefKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value + " (Flat " + PlayerData.instance.nailDamage + ", x" + DebugMod.RefKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value + ")");
                altPanel.GetText("CanCdash").UpdateText(GetStringForBool(HeroController.instance.CanSuperDash()));

                altPanel.GetText("Completion").UpdateText(PlayerData.instance.completionPercentage.ToString() + "%");
                altPanel.GetText("Grubs").UpdateText(PlayerData.instance.grubsCollected + " / 46");

                altPanel.GetText("Scene Name").UpdateText(DebugMod.GetSceneName());

                if (SaveStateManager.quickState.IsSet())
                {
                    //string[] temp = ;
                    //altPanel.GetText("Current SaveState").UpdateText(string.Format("{0}\n{1}", temp[2], temp[1]));
                    altPanel.GetText("Current SaveState").UpdateText(SaveStateManager.quickState.GetSaveStateID());
                }
                else
                {
                    altPanel.GetText("Current SaveState").UpdateText("No savestate");
                }

                string slotSet = SaveStateManager.GetCurrentSlot().ToString();
                if (slotSet == "-1")
                {
                    slotSet = "unset";
                }

                //altPanel.GetText("Autoslot").UpdateText(string.Format("{0}",
                //            GetStringForBool(SaveStateManager.GetAutoSlot())));
                altPanel.GetText("Current slot").UpdateText(string.Format("{0}", slotSet));

                altPanel.GetText("Hardfall").UpdateText(GetStringForBool(HeroController.instance.cState.willHardLand));
            }
        }

        private static string GetHeroPos()
        {
            float HeroX = (float)DebugMod.RefKnight.transform.position.x;
            float HeroY = (float)DebugMod.RefKnight.transform.position.y;

            return $"({HeroX}, {HeroY})";
        }

        private static string GetStringForBool(bool b)
        {
            return b ? "✓" : "X";
        }
    }
}
