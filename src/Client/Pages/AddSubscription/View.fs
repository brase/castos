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

let searchFormView() =
    Bulma.columns [
        Bulma.column [
            Bulma.field.div[
                Bulma.field.hasAddons
                prop.children [
                    Bulma.control.div [
                        Bulma.input.text [
                            prop.placeholder "Find podcasts"
                        ]
                    ]
                    Bulma.control.div [
                        Bulma.button.a [
                            Bulma.color.isPrimary
                            prop.text "Search"
                        ]
                    ]
                ]
            ]
        ]
    ]

let view = React.functionComponent(fun () ->
    let model, dispatch = React.useElmish(State.init, State.update, [| |])

    searchFormView()
    |> inTemplate
)