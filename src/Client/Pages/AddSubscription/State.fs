module Murocast.Client.Pages.AddSubscription.State

open System
open Elmish
open Domain
open Murocast.Client.Server

open Murocast.Shared.Core.Subscriptions.Communication.Queries
open Murocast.Client.SharedView

let getSubscriptions() : Fable.Core.JS.Promise<SubscriptionRendition list> =
    promise {
        return! getJsonPromise "/api/subscriptions"
    }

let getMessages() =
    Ok [{  FeedId = Guid.NewGuid()
           Name = "Logbuch Netzpolitik"
           Url = "https://logbuchnetzpolitik.fm/feed"
           Subscribed = false
           LastEpisodeDate = Some (DateTime(2021,1,12)) }]

let init () =
    {Feeds = []}, []

let update (msg:Msg) (model:Model) : Model * Cmd<Msg> =
    match msg with
    | FindFeeds s ->
        model, Cmd.ofMsg (FoundFeedsLoaded (getMessages()))
    | FoundFeedsLoaded feeds -> model, []