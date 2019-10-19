namespace DebugMod
open Modding
open UnityEngine
open UnityEngine.SceneManagement
open UnityEngine.UI

[<AllowNullLiteral>]
type DebugMenu() =
    inherit MonoBehaviour()

    static let mutable _instance = null
    let mutable _canvas = null

    static member Instance
        with get() = _instance
        and private set(value) = _instance <- value

    member private this.Start() =
        DebugMenu.Instance <- this
        
        _canvas <- this.gameObject.AddComponent<Canvas>()
        _canvas.renderMode <- RenderMode.ScreenSpaceOverlay

        let scaler = this.gameObject.AddComponent<CanvasScaler>()
        scaler.uiScaleMode <- CanvasScaler.ScaleMode.ScaleWithScreenSize

        this.gameObject.AddComponent<GraphicRaycaster>() |> ignore
        this.gameObject.AddComponent<CanvasGroup>() |> ignore

        _canvas.CreateImagePanel(SpriteManager.GetSprite("banana"),
            new CanvasUtil.RectData(Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one))
            |> ignore
        this.CheckCanvas()

        UObject.DontDestroyOnLoad(this.gameObject)
        EventManager.RegisterEvent<USceneManager, DebugMenu>(null, "activeSceneChanged", "CheckScene", this)

    member private __.OnDisable() =
        _canvas.enabled <- false

    member private this.OnEnable() =
        this.CheckCanvas()

    member private this.CheckScene(oldScene:Scene, newScene:Scene) =
        this.CheckCanvas()

    member private __.CheckCanvas() =
        _canvas.enabled <- GameManager.instance.IsGameplayScene()
