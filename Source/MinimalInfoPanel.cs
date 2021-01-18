using UnityEngine;
using InControl;
using System.Collections.Generic;

namespace DebugMod
{
    public static class MinimalInfoPanel
    {
        private static CanvasPanel panel;
        private static CanvasPanel panelCurrentSaveState;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(
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
            panelCurrentSaveState = new CanvasPanel(
                canvas,
                GUIController.Instance.images["BlankBox"],
                new Vector2(500f, 200f),
                Vector2.zero,
                new Rect(
                    0f,
                    0f,
                    GUIController.Instance.images["BlankBox"].width,
                    GUIController.Instance.images["BlankBox"].height
                )
            );

            //Labels
            panel.AddText("Velocity Label", "Vel", new Vector2(10f, 10f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Position Label", "Hero Pos", new Vector2(70f, 10f), Vector2.zero, GUIController.Instance.arial);
 
            panel.AddText("MP Label", "MP", new Vector2(10f, 30f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Nail Damage Label", "Naildmg", new Vector2(70f, 30f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("canSuperdash Label", "CanCdash", new Vector2(130f, 30f), Vector2.zero, GUIController.Instance.arial, 15);

            panel.AddText("Completion Label", "Completion", new Vector2(10f, 50f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Grubs Label", "Grubs", new Vector2(90f, 50f), Vector2.zero, GUIController.Instance.arial, 15);
         
            panel.AddText("Scene Name Label", "Scene Name", new Vector2(10f, 70f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Current Save State Lable", "Current", new Vector2(10f, 90f), Vector2.zero, GUIController.Instance.arial, 15);

            panelCurrentSaveState.AddText("Current Save State Lable", "Current", new Vector2(10f, 10f), Vector2.zero, GUIController.Instance.arial, 15);

            //Values
            panel.AddText("Vel", "", new Vector2(40f, 10f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("Hero Pos", "", new Vector2(120f, 10f), Vector2.zero, GUIController.Instance.trajanNormal);
 
            panel.AddText("MP", "", new Vector2(50f, 50f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("Naildmg", "", new Vector2(50f, 30f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("CanCdash", "", new Vector2(440f, 30f), Vector2.zero, GUIController.Instance.trajanNormal);

            panel.AddText("Completion", "", new Vector2(60f, 50f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("Grubs", "", new Vector2(120f, 50f), Vector2.zero, GUIController.Instance.trajanNormal);

            panel.AddText("Scene Name", "", new Vector2(50f, 70f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("Current", "", new Vector2(50f, 90f), Vector2.zero, GUIController.Instance.trajanNormal);

            panelCurrentSaveState.AddText("Current", "", new Vector2(40f, 10f), Vector2.zero, GUIController.Instance.trajanNormal);
            // Current SaveState might work better as a panel?

            panel.FixRenderOrder();
        }

        public static void Update()
        {


            if (panel == null /*|| panelCurrentSaveState == null*/)
            {
                return;
            }

            if (DebugMod.GM.IsNonGameplayScene())
            {
                if (panel.active)
                {
                    panel.SetActive(false, true);
                }
                if (panelCurrentSaveState.active)
                {
                    panelCurrentSaveState.SetActive(false, true);
                }
                return;
            }

            // Not intended min/full info panel logic, but should show the two panels one at a time
            if (DebugMod.settings.MinInfoPanelVisible)
            {
                if (!panel.active)
                    panel.SetActive(true, false);
                if (DebugMod.settings.InfoPanelVisible) {
                    DebugMod.settings.InfoPanelVisible = false;
                }
                if (SaveStateManager.HasFiles() && !panelCurrentSaveState.active)
                {
                    panelCurrentSaveState.SetActive(true, false);
                }
                else
                {
                    panelCurrentSaveState.SetActive(false, true);
                }
            }
            else if (!DebugMod.settings.MinInfoPanelVisible && panel.active)
            {
                panel.SetActive(false, true);
                if (panelCurrentSaveState.active)
                    panelCurrentSaveState.SetActive(false, true);
            }

            if (panel.active)
            {
                PlayerData.instance.CountGameCompletion();

                panel.GetText("Vel").UpdateText(HeroController.instance.current_velocity.ToString());
                panel.GetText("Pos").UpdateText(DebugMod.RefKnight.transform.position.ToString());

                panel.GetText("NailDmg").UpdateText(DebugMod.RefKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value + " (Flat " + PlayerData.instance.nailDamage + ", x" + DebugMod.RefKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value + ")");

                panel.GetText("MP").UpdateText((PlayerData.instance.MPCharge + PlayerData.instance.MPReserve).ToString());
                panel.GetText("CanCdash").UpdateText(GetStringForBool(HeroController.instance.CanSuperDash()));

                panel.GetText("Completion").UpdateText(PlayerData.instance.completionPercentage.ToString());
                panel.GetText("Grubs").UpdateText(PlayerData.instance.grubsCollected + " / 46");

                panel.GetText("Scene Name").UpdateText(DebugMod.GetSceneName());

                if (SaveStateManager.memoryState.IsSet())
                {
                    string[] temp = SaveStateManager.memoryState.GetSaveStateInfo();
                    panel.GetText("Current").UpdateText(string.Format("{0}\n{1}", temp[2], temp[1]));
                }
                else
                {
                    panel.GetText("Current").UpdateText("No savestate");
                }

                if (SaveStateManager.HasFiles())
                {
                    string slotSet = SaveStateManager.GetCurrentSlot().ToString();
                    if (slotSet == "-1") slotSet = "unset";

                    panelCurrentSaveState.GetText("Current").UpdateText(
                        string.Format(
                            "Auto-select: {0}/n Current slot: {1}",
                            GetStringForBool(SaveStateManager.GetAutoSlot()),
                            slotSet));
                }
            }
        }

        private static string GetStringForBool(bool b)
        {
            return b ? "✓" : "X";
        }
    }
}
