module Murocast.Client.View

open Murocast.Client.Router
open Murocast.Client.Domain

open Domain
open Feliz
open Feliz.Bulma
open Feliz.Bulma.PageLoader
open Feliz.Router

let view (model:Model) (dispatch:Msg -> unit) =
    let render =
        if model.IsCheckingUser then
            PageLoader.pageLoader [
                pageLoader.isWhite
                pageLoader.isActive
                prop.children [
                    PageLoader.title "Checking Login"
                ]
            ]
        else
            match model.CurrentPage with
            | Anonymous pg ->
                match pg with
                | Login -> Pages.Login.View.view()
                | LinkSonos (linkcode,householdId) -> Pages.LinkSonos.View.view { LinkCode = linkcode; HouseholdId = householdId }
                | ResetPassword _ | Registration | ForgottenPassword | AccountActivation _ -> failwith "not implemented"

            | Secured (pg, user) ->
                match pg with
                | Subscriptions -> Pages.Subscriptions.View.view ()
                | AddSubscription -> Pages.AddSubscription.View.view ()
                | Feeds | Users | MyAccount -> failwith "Not implemented"

    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children render
    ]