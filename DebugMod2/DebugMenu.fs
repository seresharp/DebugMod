namespace DebugMod
open System.Collections
open System.Reflection
open GlobalEnums
open Modding
open SeanprCore
open UnityEngine
open UnityEngine.SceneManagement
open UnityEngine.UI

[<AllowNullLiteral>]
type DebugMenu() =
    inherit MonoBehaviour()

    static let mutable _instance = null
    let mutable _canvas = null

    let _panels : Panel[] = [|new InfoPanel(); new FunctionPanel()|]

    static member Instance
        with get() = _instance
        and private set(value) = _instance <- value

    member private this.Start() =
        DebugMenu.Instance <- this
        
        _canvas <- this.gameObject.AddComponent<Canvas>()
        _canvas.renderMode <- RenderMode.ScreenSpaceOverlay

        let scaler = this.gameObject.AddComponent<CanvasScaler>()
        scaler.uiScaleMode <- CanvasScaler.ScaleMode.ScaleWithScreenSize
        scaler.referenceResolution <- CanvasHelper.CanvasRes

        this.gameObject.AddComponent<GraphicRaycaster>() |> ignore
        this.gameObject.AddComponent<CanvasGroup>() |> ignore

        for panel in _panels do
            panel.Initialize(_canvas)

        this.CheckCanvas()

        UObject.DontDestroyOnLoad(this.gameObject)
        EventManager.RegisterEvent<USceneManager, DebugMenu>(null, "activeSceneChanged", "CheckScene", this)
        EventManager.RegisterEvent<ModHooks, DebugMenu>(ModHooks.Instance, "HeroUpdateHook", "OnUpdate", this)

    member this.OnUpdate() =
        if this.enabled && _canvas.enabled then
            for panel in _panels do
                if not panel.Hidden then
                    panel.Update()

        if Input.GetKeyDown(KeyCode.F1) then
            this.StartCoroutine(seq {
                Ref.Hero.spellControl.SetState("Fireball Recoil")
                yield new WaitForEndOfFrame()
                Ref.HeroRigidbody.gravityScale <- 0.79f
            } :?> IEnumerator) |> ignore

        if Input.GetKeyDown(KeyCode.F2) then
            this.StartCoroutine(this.EnablePFloat() : IEnumerator) |> ignore

        if Input.GetKeyDown(KeyCode.F3) then
            if Ref.Hero.hero_state = ActorStates.no_input && Ref.Hero.transitionState = HeroTransitionState.DROPPING_DOWN then
                typeof<HeroController>.GetMethod("FinishedEnteringScene", BindingFlags.Instance ||| BindingFlags.NonPublic).Invoke(Ref.Hero, [|true; false|]) |> ignore
            Ref.Hero.spellControl.SetState("Spell End")

        if Input.GetKeyDown(KeyCode.F4) then
            this.StartCoroutine(seq {
                yield this.EnablePFloat()
                Ref.HeroRigidbody.velocity <- new Vector2(Ref.HeroRigidbody.velocity.x, 0.0f)
                Ref.Hero.cState.onGround <- true
            } :?> IEnumerator) |> ignore

        if Input.GetKeyDown(KeyCode.F5) then
            if Ref.UI.uiState = UIState.PLAYING || Ref.UI.uiState = UIState.PAUSED then
                if Ref.UI.uiState = UIState.PLAYING then
                    this.StartCoroutine(Ref.GM.PauseGameToggle()) |> ignore
                if Time.timeScale <= Mathf.Epsilon then
                    Time.timeScale <- 1.0f
                    if Ref.Hero.cState.facingRight then
                        Ref.Hero.RecoilLeft()
                    else
                        Ref.Hero.RecoilRight()

        if (Input.GetKeyDown(KeyCode.F6)) then
            this.StartCoroutine(seq {
                Ref.Hero.spellControl.SetState("Q On Ground")
                yield new WaitForEndOfFrame()
                Ref.Hero.RegainControl()
                Ref.Hero.StartAnimationControl()
            } :?> IEnumerator) |> ignore

    member private __.OnDisable() =
        _canvas.enabled <- false

    member private this.OnEnable() =
        this.CheckCanvas()

    member private this.CheckScene(oldScene:Scene, newScene:Scene) =
        this.CheckCanvas()

    member private __.CheckCanvas() =
        _canvas.enabled <- GameManager.instance.IsGameplayScene()

    member private __.EnablePFloat() =
        seq {
            // Activate pfloat
            Ref.Hero.spellControl.SetState("Fireball Recoil")
            yield new WaitForEndOfFrame()
            Ref.Hero.spellControl.SendEvent("LEAVING SCENE")

            // 1f of falling to trigger the grounded check
            Ref.HeroRigidbody.velocity <- new Vector2(Ref.HeroRigidbody.velocity.x, -0.001f)
            yield new WaitForEndOfFrame()
            Ref.HeroRigidbody.velocity <- new Vector2(Ref.HeroRigidbody.velocity.x, 0.0f)
        } :?> IEnumerator
