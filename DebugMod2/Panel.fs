namespace DebugMod
open Modding
open UnityEngine

[<AbstractClass>]
type Panel() =
    let mutable _hidden = false
    let mutable _panel = null : GameObject

    member __.Hidden
        with get() = _hidden
        and set(value) =
            _hidden <- value
            _panel.SetActive(not _hidden)

    member this.Initialize(parent:Canvas) =
        _panel <- CanvasUtil.CreateBasePanel(parent.gameObject,
            new CanvasUtil.RectData(Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one))

        this.CreateUI(_panel)

    abstract member CreateUI: GameObject -> unit

    abstract member Update: unit -> unit
