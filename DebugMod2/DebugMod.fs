namespace DebugMod
open System.Collections
open Modding
open UnityEngine

[<AllowNullLiteral>]
type DebugMod() =
    inherit Mod()

    static let mutable _instance = null
    static let mutable _initialized = false

    interface ITogglableMod with
        member this.Unload() =
            // Everything dies if the mod is disabled on startup unless you wait a frame
            GameManager.instance.StartCoroutine(this.DisableNextFrame()) |> ignore

    static member Instance
        with get() = _instance
        and private set(value) = _instance <- value
    
    override this.Initialize() = 
        this.LogDebug("Initializing")
        DebugMod.Instance <- this

        if not _initialized then
            EventManager.Logger <- this

            new GameObject("DebugMenu", typeof<DebugMenu>) |> ignore

            _initialized <- true
        else
            DebugMenu.Instance.enabled <- true

        EventManager.SubscribeAll()

    member __.DisableNextFrame() =
        seq {
            yield new WaitForEndOfFrame()
            EventManager.UnsubscribeAll()
            DebugMenu.Instance.enabled <- false
        } :?> IEnumerator
