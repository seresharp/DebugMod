using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DebugMod
{
    public static class InfoPanel
    {
        private static CanvasPanel panel;

        public static bool visible;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.instance.images["StatusPanelBG"], new Vector2(0f, 223f), Vector2.zero, new Rect(0f, 0f, GUIController.instance.images["StatusPanelBG"].width, GUIController.instance.images["StatusPanelBG"].height));

            //Labels
            panel.AddText("Hero State Label", "Hero State", new Vector2(10f, 20f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Velocity Label", "Velocity", new Vector2(10f, 40f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Nail Damage Label", "Naildmg", new Vector2(10f, 60f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("HP Label", "HP", new Vector2(10f, 80f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("MP Label", "MP", new Vector2(10f, 100f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Completion Label", "Completion", new Vector2(10f, 178f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Grubs Label", "Grubs", new Vector2(10f, 198f), Vector2.zero, GUIController.instance.arial, 15);
            
            panel.AddText("isInvuln Label", "isInvuln", new Vector2(10f, 276f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Invincible Label", "Invincible", new Vector2(10f, 296f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Invincitest Label", "invinciTest", new Vector2(10f, 316f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Damage State Label", "Damage State", new Vector2(10f, 336f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Dead State Label", "Dead State", new Vector2(10f, 356f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Hazard Death Label", "Hazard Death", new Vector2(10f, 376f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Scene Name Label", "Scene Name", new Vector2(10f, 454), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Transition Label", "Transition", new Vector2(10f, 474f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Trans State Label", "Trans State", new Vector2(10f, 494f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("is Gameplay Label", "Is Gameplay", new Vector2(10f, 514f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Game State Label", "Game State", new Vector2(10f, 534f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("UI State Label", "UI State", new Vector2(10f, 554f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Hero Paused Label", "Hero Paused", new Vector2(10f, 574f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Camera Mode Label", "Camera Mode", new Vector2(10f, 594f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Accept Input Label", "Accept Input", new Vector2(300f, 30f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Relinquished Label", "Relinquished", new Vector2(300f, 50f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("atBench Label", "atBench", new Vector2(300f, 70f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Dashing Label", "Dashing", new Vector2(300f, 120f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Jumping Label", "Jumping", new Vector2(300f, 140f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Superdashing Label", "Superdashing", new Vector2(300f, 160f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Falling Label", "Falling", new Vector2(300f, 180f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Hardland Label", "Hardland", new Vector2(300f, 200f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Swimming Label", "Swimming", new Vector2(300f, 220f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Recoiling Label", "Recoiling", new Vector2(300f, 240f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Wall lock Label", "Wall lock", new Vector2(300f, 290f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Wall jumping Label", "Wall jumping", new Vector2(300f, 310f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Wall touching Label", "Wall touching", new Vector2(300f, 330f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Wall sliding Label", "Wall sliding", new Vector2(300f, 350f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Wall left Label", "Wall left", new Vector2(300f, 370f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("Wall right Label", "Wall right", new Vector2(300f, 390f), Vector2.zero, GUIController.instance.arial, 15);

            panel.AddText("Attacking Label", "Attacking", new Vector2(300f, 440f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canCast Label", "canCast", new Vector2(300f, 460f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canSuperdash Label", "canSuperdash", new Vector2(300f, 480f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canQuickmap Label", "canQuickmap", new Vector2(300f, 500f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canInventory Label", "canInventory", new Vector2(300f, 520f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canWarp Label", "canWarp", new Vector2(300f, 540f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("canDGate Label", "canDGate", new Vector2(300f, 560f), Vector2.zero, GUIController.instance.arial, 15);
            panel.AddText("gateAllow Label", "gateAllow", new Vector2(300f, 580f), Vector2.zero, GUIController.instance.arial, 15);

            //Values
            panel.AddText("Hero State", "", new Vector2(150f, 20f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Velocity", "", new Vector2(150f, 40f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Nail Damage", "", new Vector2(150f, 60f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("HP", "", new Vector2(150f, 80f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("MP", "", new Vector2(150f, 100f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Completion", "", new Vector2(150f, 178f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Grubs", "", new Vector2(150f, 198f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("isInvuln", "", new Vector2(150f, 276f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Invincible", "", new Vector2(150f, 296f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Invincitest", "", new Vector2(150f, 316f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Damage State", "", new Vector2(150f, 336f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Dead State", "", new Vector2(150f, 356f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Hazard Death", "", new Vector2(150f, 376f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Scene Name", "", new Vector2(150f, 454), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Transition", "", new Vector2(150f, 474f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Trans State", "", new Vector2(150f, 494f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("is Gameplay", "", new Vector2(150f, 514f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Game State", "", new Vector2(150f, 534f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("UI State", "", new Vector2(150f, 554f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Hero Paused", "", new Vector2(150f, 574f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Camera Mode", "", new Vector2(150f, 594f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Accept Input", "", new Vector2(440f, 30f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Relinquished", "", new Vector2(440f, 50f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("atBench", "", new Vector2(440f, 70f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Dashing", "", new Vector2(440f, 120f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Jumping", "", new Vector2(440f, 140f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Superdashing", "", new Vector2(440f, 160f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Falling", "", new Vector2(440f, 180f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Hardland", "", new Vector2(440f, 200f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Swimming", "", new Vector2(440f, 220f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Recoiling", "", new Vector2(440f, 240f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Wall lock", "", new Vector2(440f, 290f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Wall jumping", "", new Vector2(440f, 310f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Wall touching", "", new Vector2(440f, 330f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Wall sliding", "", new Vector2(440f, 350f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Wall left", "", new Vector2(440f, 370f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("Wall right", "", new Vector2(440f, 390f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.AddText("Attacking", "", new Vector2(440f, 440f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canCast", "", new Vector2(440f, 460f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canSuperdash", "", new Vector2(440f, 480f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canQuickmap", "", new Vector2(440f, 500f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canInventory", "", new Vector2(440f, 520f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canWarp", "", new Vector2(440f, 540f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("canDGate", "", new Vector2(440f, 560f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddText("gateAllow", "", new Vector2(440f, 580f), Vector2.zero, GUIController.instance.trajanNormal);

            panel.FixRenderOrder();
        }

        public static void Update()
        {
            if (panel == null)
            {
                return;
            }

            if (visible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!visible && panel.active)
            {
                panel.SetActive(false, true);
            }

            if (panel.active)
            {
                PlayerData.instance.CountGameCompletion();

                panel.GetText("Hero State").UpdateText(HeroController.instance.hero_state.ToString());
                panel.GetText("Velocity").UpdateText(HeroController.instance.current_velocity.ToString());
                panel.GetText("Nail Damage").UpdateText(DebugMod.refKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value + " (Flat " + PlayerData.instance.nailDamage + ", x" + DebugMod.refKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value + ")");
                panel.GetText("HP").UpdateText(PlayerData.instance.health + " / " + PlayerData.instance.maxHealth);
                panel.GetText("MP").UpdateText((PlayerData.instance.MPCharge + PlayerData.instance.MPReserve).ToString());

                panel.GetText("Completion").UpdateText(PlayerData.instance.completionPercentage.ToString());
                panel.GetText("Grubs").UpdateText(PlayerData.instance.grubsCollected + " / 46");

                panel.GetText("isInvuln").UpdateText(GetStringForBool(HeroController.instance.cState.invulnerable));
                panel.GetText("Invincible").UpdateText(GetStringForBool(PlayerData.instance.isInvincible));
                panel.GetText("Invincitest").UpdateText(GetStringForBool(PlayerData.instance.invinciTest));
                panel.GetText("Damage State").UpdateText(HeroController.instance.damageMode.ToString());
                panel.GetText("Dead State").UpdateText(GetStringForBool(HeroController.instance.cState.dead));
                panel.GetText("Hazard Death").UpdateText(HeroController.instance.cState.hazardDeath.ToString());

                panel.GetText("Scene Name").UpdateText(DebugMod.GetSceneName());
                panel.GetText("Transition").UpdateText(GetStringForBool(HeroController.instance.cState.transitioning));

                string transState = HeroController.instance.transitionState.ToString();
                if (transState == "WAITING_TO_ENTER_LEVEL") transState = "LOADING";
                if (transState == "WAITING_TO_TRANSITION") transState = "WAITING";

                panel.GetText("Trans State").UpdateText(transState);
                panel.GetText("is Gameplay").UpdateText(GetStringForBool(DebugMod.gm.IsGameplayScene()));
                panel.GetText("Game State").UpdateText(GameManager.instance.gameState.ToString());
                panel.GetText("UI State").UpdateText(UIManager.instance.uiState.ToString());
                panel.GetText("Hero Paused").UpdateText(GetStringForBool(HeroController.instance.cState.isPaused));
                panel.GetText("Camera Mode").UpdateText(DebugMod.refCamera.mode.ToString());

                panel.GetText("Accept Input").UpdateText(GetStringForBool(HeroController.instance.acceptingInput));
                panel.GetText("Relinquished").UpdateText(GetStringForBool(HeroController.instance.controlReqlinquished));
                panel.GetText("atBench").UpdateText(GetStringForBool(PlayerData.instance.atBench));

                panel.GetText("Dashing").UpdateText(GetStringForBool(HeroController.instance.cState.dashing));
                panel.GetText("Jumping").UpdateText(GetStringForBool((HeroController.instance.cState.jumping || HeroController.instance.cState.doubleJumping)));
                panel.GetText("Superdashing").UpdateText(GetStringForBool(HeroController.instance.cState.superDashing));
                panel.GetText("Falling").UpdateText(GetStringForBool(HeroController.instance.cState.falling));
                panel.GetText("Hardland").UpdateText(GetStringForBool(HeroController.instance.cState.willHardLand));
                panel.GetText("Swimming").UpdateText(GetStringForBool(HeroController.instance.cState.swimming));
                panel.GetText("Recoiling").UpdateText(GetStringForBool(HeroController.instance.cState.recoiling));

                panel.GetText("Wall lock").UpdateText(GetStringForBool(HeroController.instance.wallLocked));
                panel.GetText("Wall jumping").UpdateText(GetStringForBool(HeroController.instance.cState.wallJumping));
                panel.GetText("Wall touching").UpdateText(GetStringForBool(HeroController.instance.cState.touchingWall));
                panel.GetText("Wall sliding").UpdateText(GetStringForBool(HeroController.instance.cState.wallSliding));
                panel.GetText("Wall left").UpdateText(GetStringForBool(HeroController.instance.touchingWallL));
                panel.GetText("Wall right").UpdateText(GetStringForBool(HeroController.instance.touchingWallR));

                panel.GetText("Attacking").UpdateText(GetStringForBool(HeroController.instance.cState.attacking));
                panel.GetText("canCast").UpdateText(GetStringForBool(HeroController.instance.CanCast()));
                panel.GetText("canSuperdash").UpdateText(GetStringForBool(HeroController.instance.CanSuperDash()));
                panel.GetText("canQuickmap").UpdateText(GetStringForBool(HeroController.instance.CanQuickMap()));
                panel.GetText("canInventory").UpdateText(GetStringForBool(HeroController.instance.CanOpenInventory()));
                panel.GetText("canWarp").UpdateText(GetStringForBool(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Warp Allowed").Value));
                panel.GetText("canDGate").UpdateText(GetStringForBool(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Can Dream Gate").Value));
                panel.GetText("gateAllow").UpdateText(GetStringForBool(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Gate Allowed").Value));
            }
        }

        private static string GetStringForBool(bool b)
        {
            return b ? "✓" : "X";
        }
    }
}
