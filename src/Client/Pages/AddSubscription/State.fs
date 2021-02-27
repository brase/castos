module Murocast.Client.Pages.AddSubscription.State

open Elmish
open Domain
open Murocast.Client.Server

open Murocast.Shared.Core.Subscriptions.Communication.Queries
open Murocast.Client.SharedView

let getSubscriptions() : Fable.Core.JS.Promise<SubscriptionRendition list> =
    promise {
        return! getJsonPromise "/api/subscriptions"
    }

let init () =
    {Feeds = []}, []

let update (msg:Msg) (model:Model) : Model * Cmd<Msg> =
    match msg with
    | _ -> model, []