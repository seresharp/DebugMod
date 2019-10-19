namespace DebugMod
open SeanprCore

[<AbstractClass; Sealed>]
type SpriteManager private () =
    static let _sprites = ResourceHelper.GetSprites("DebugMod2.Resources.")

    static member GetSprite(name) =
        let found, sprite = _sprites.TryGetValue(name)

        match found with
            | true -> sprite
            | false -> SpriteManager.GetSprite("MissingTex")
