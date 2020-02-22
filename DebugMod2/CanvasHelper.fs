namespace DebugMod
open Modding
open UnityEngine

[<AbstractClass; Sealed>]
type CanvasHelper private () =
    static member CanvasRes = new Vector2(1920.0f, 1080.0f)

    static member GetLeftX(sprite:Sprite) =
        (float32 sprite.texture.width / 2.0f) / CanvasHelper.CanvasRes.x

    static member GetRightX(sprite:Sprite) =
        1.0f - CanvasHelper.GetLeftX(sprite)

    static member GetBottomY(sprite:Sprite) =
        (float32 sprite.texture.height / 2.0f) / CanvasHelper.CanvasRes.y

    static member GetTopY(sprite:Sprite) =
        1.0f - CanvasHelper.GetBottomY(sprite)

    static member GetRect(sprite:Sprite, vec) =
        new CanvasUtil.RectData(new Vector2(float32 sprite.texture.width, float32 sprite.texture.height),
            Vector2.zero, vec, vec)

    static member AddPixels(baseVec, x, y) =
        baseVec + new Vector2(float32 x / CanvasHelper.CanvasRes.x, float32 y / CanvasHelper.CanvasRes.y)
