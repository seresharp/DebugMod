namespace DebugMod
open System.Collections.Generic
open System.Reflection;
open Modding

[<AbstractClass; Sealed>]
type EventManager private () =
    static let mutable _subscribed = false

    static let _subscriptions = new Dictionary<(EventInfo * System.Object), List<System.Delegate>>()

    static let mutable _logger = SimpleLogger("") :> ILogger
    static member Logger
        with set(value) = _logger <- value
        and get() = _logger

    static member RegisterEvent<'T, 'TInvoke>(inst, event, func, ?obj:'TInvoke) =
        let mutable flags = BindingFlags.Public
        match inst with
            | null -> flags <- flags ||| BindingFlags.Static
            | _    -> flags <- flags ||| BindingFlags.Instance
        let eventInfo = typeof<'T>.GetEvent(event, flags)
        
        let mutable listFound, list = _subscriptions.TryGetValue((eventInfo, inst))

        if not listFound then
            list <- new List<System.Delegate>()
            _subscriptions.[(eventInfo, inst)] <- list

        let declaringType = typeof<'TInvoke>;

        let mi = declaringType.GetMethod(func, BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.Static)

        let del = match obj with
                    | None -> System.Delegate.CreateDelegate(eventInfo.EventHandlerType, mi);
                    | Some value -> System.Delegate.CreateDelegate(eventInfo.EventHandlerType, value, mi)

        list.Add(del)

        if _subscribed then
            EventManager.Subscribe(eventInfo, inst, del)

    static member SubscribeAll() =
        if not _subscribed then
            for pair in _subscriptions do
                for del in pair.Value do
                    let event, inst = pair.Key
                    EventManager.Subscribe(event, inst, del)

            _subscribed <- true

    static member UnsubscribeAll() =
        if _subscribed then
            for pair in _subscriptions do
                for del in pair.Value do
                    let event, inst = pair.Key
                    event.RemoveEventHandler(inst, del)

            _subscribed <- false

    static member private Subscribe(event, inst, del) =
        event.AddEventHandler(inst, del)
