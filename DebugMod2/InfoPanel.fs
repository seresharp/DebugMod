namespace DebugMod
open Modding
open SeanprCore
open UnityEngine
open UnityEngine.UI

type InfoPanel() =
    inherit Panel()

    let mutable _text = null
    let _info =
        [|
            "Position",
                (fun () ->  let pos = Ref.Hero.transform.position
                            let x = pos.x
                            let y = pos.y

                            x.ToString("0.00") + ", " + y.ToString("0.00")
                );
            "Velocity",
                (fun () ->  let vel = Ref.HeroRigidbody.velocity
                            let x = vel.x
                            let y = vel.y

                            x.ToString("0.00") + ", " + y.ToString("0.00")
                );
            "", fun () ->   ""; // Category separator
            "Damage",
                (fun () ->  let baseDamage = Ref.PD.nailDamage
                            let damageDealt = Ref.HeroNailFSM.FsmVariables.GetFsmInt("damageDealt").Value |> float32
                            let multiplier = Ref.HeroNailFSM.FsmVariables.GetFsmFloat("Multiplier").Value
                            let totalDamage = (damageDealt * multiplier) |> Mathf.RoundToInt

                            string(totalDamage) + " (" + string(baseDamage) + " base)"
                );
            "Health", 
                (fun () ->  let hp = Ref.PD.health
                            let hpBlue = Ref.PD.healthBlue
                            let hpMax = Ref.PD.maxHealthBase

                            string(hp + hpBlue) + " (" + string(hpMax) + " base)"
                );
            "Soul", 
                fun () ->   string(Ref.PD.MPCharge + Ref.PD.MPReserve);
            "", fun () ->   ""; // Category separator
            "Hard Land",
                (fun () ->  let curr = Ref.Hero.fallTimer
                            let max = Ref.Hero.BIG_FALL_TIME
                            if curr >= max || Ref.Hero.cState.willHardLand then
                                max.ToString("0.00") + " / " + max.ToString("0.00")
                            else
                                curr.ToString("0.00") + " / " + max.ToString("0.00")
                );
            "Nail Art",
                (fun () ->  let curr = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "nailChargeTimer")
                            let max = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "nailChargeTime")
                            if curr >= max then
                                max.ToString("0.00") + " / " + max.ToString("0.00")
                            else
                                curr.ToString("0.00") + " / " + max.ToString("0.00")
                );
            "Dash",
                (fun () ->  let curr = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "dashCooldownTimer")
                            let max = if Ref.PD.GetBool("equippedCharm_31") then Ref.Hero.DASH_COOLDOWN_CH else Ref.Hero.DASH_COOLDOWN
                            if curr >= max then
                                max.ToString("0.00") + " / " + max.ToString("0.00")
                            else
                                curr.ToString("0.00") + " / " + max.ToString("0.00")
                );
            "Shade Dash",
                (fun () ->  let curr = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "shadowDashTimer")
                            let max = Ref.Hero.SHADOW_DASH_COOLDOWN
                            if curr >= max then
                                max.ToString("0.00") + " / " + max.ToString("0.00")
                            else
                                curr.ToString("0.00") + " / " + max.ToString("0.00")
                );
            "Attack",
                (fun () ->  let curr = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "attack_time")
                            let max = ReflectionHelper.GetAttr<HeroController, float32>(Ref.Hero, "attackDuration")
                            if curr >= max then
                                max.ToString("0.00") + " / " + max.ToString("0.00")
                            else
                                curr.ToString("0.00") + " / " + max.ToString("0.00")
                );

            //"Hero State",
            //    fun () -> string Ref.Hero.hero_state;
        |]

    override __.CreateUI(parent:GameObject) =
        let sprite = SpriteManager.GetSprite("Backgrounds.Info")
        let spriteSize = new Vector2(float32 sprite.texture.width, float32 sprite.texture.height)
        let pos = new Vector2(CanvasHelper.GetLeftX(sprite), 0.5f)

        let labelPos = CanvasHelper.AddPixels(pos, 5, 0)
        parent.CreateImagePanel(sprite, CanvasHelper.GetRect(sprite, pos)) |> ignore
        parent.CreateTextPanel("\n" + (_info |> Array.map(fun x -> fst x) |> String.concat("\n")), 15, TextAnchor.UpperLeft, new CanvasUtil.RectData(spriteSize,
                                Vector2.zero, labelPos, labelPos), Fonts.Get("Arial")).GetComponent<Text>().lineSpacing <- 1.2f

        let textPos = CanvasHelper.AddPixels(pos, 110, 0)
        _text <- parent.CreateTextPanel("\nThe mod broke", 15, TextAnchor.UpperLeft, new CanvasUtil.RectData(spriteSize, Vector2.zero, textPos, textPos),
                                        Fonts.Get("Arial")).GetComponent<Text>()
        _text.lineSpacing <- 1.2f

    override __.Update() =
        _text.text <- "\n" + (_info |> Array.map(fun x -> (snd x)()) |> String.concat("\n"))
