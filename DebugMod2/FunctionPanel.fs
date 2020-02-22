namespace DebugMod
open UnityEngine

type FunctionPanel() =
    inherit Panel()

    override __.CreateUI(parent:GameObject) =
        parent |> ignore

    override __.Update() = ()
