namespace DebugMod
open System.Runtime.CompilerServices
open Modding
open UnityEngine
open UnityEngine.UI

[<AbstractClass; Sealed; Extension>]
type CanvasExtensions() =
    [<Extension>]
    static member CreateImagePanel(obj, sprite, rect) =
        let panel = CanvasUtil.CreateImagePanel(obj, sprite, rect)
        panel.GetComponent<Image>().preserveAspect <- false

        panel

    [<Extension>]
    static member CreateTextPanel(obj:GameObject, text:string, fontSize:int, textAnchor:TextAnchor, rectData:CanvasUtil.RectData, font:Font) =
        CanvasUtil.CreateTextPanel(obj, text, fontSize, textAnchor, rectData, font)
