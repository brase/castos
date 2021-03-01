module Murocast.Client.Pages.AddSubscription.View

open System

open Feliz
open Feliz.Bulma
open Feliz.UseElmish

open Murocast.Client.Router
open Murocast.Client.Forms
open Domain
open Murocast.Client.SharedView
open Murocast.Client.Template

let view = React.functionComponent(fun () ->
    let model, dispatch = React.useElmish(State.init, State.update, [| |])

    Html.span " · "
    |> inTemplate
)