namespace DebugMod
open System.Runtime.CompilerServices
open Modding
open UnityEngine
open UnityEngine.UI

[<AbstractClass; Sealed; Extension>]
type CanvasExtensions() =
    [<Extension>]
    static member CreateImagePanel(obj:Canvas, sprite, rect) =
        let panel = CanvasUtil.CreateImagePanel(obj.gameObject, sprite, rect)
        panel.GetComponent<Image>().preserveAspect <- false

        panel