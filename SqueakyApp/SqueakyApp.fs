// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace SqueakyApp

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

module App = 
    type Model = 
        {
            IsLoginVisible: bool
        }

    type Msg = 
        | LoginSuccess
        | KickOffDataUpdate
        | AnyInteraction

    let init () = 
        { IsLoginVisible = true }, Cmd.none

    let update msg model =
        match msg with
        | LoginSuccess ->
            { model with IsLoginVisible = false }

            // Once the login is done, refresh data
            , KickOffDataUpdate |> Cmd.ofMsg
        | KickOffDataUpdate ->
            model, Cmd.none

        | AnyInteraction ->
            model, Cmd.none

    // Steps that were ommited to get here:
    // Click item 
    // -> Push item detail 
    // -> Interaction with item that requires login
    // -> Push login
    let view (model: Model) dispatch =
        View.NavigationPage(
            [
                // Page with a list of items
                View.ContentPage(
                    View.Button("Any event", (fun () -> AnyInteraction |> dispatch))
                )

                // Detail page for item
                View.ContentPage(
                    View.Label("Item detail")
                )

                if model.IsLoginVisible then
                    View.ContentPage(
                        // Press Login and Pop
                        View.Button("Login", (fun () -> LoginSuccess |> dispatch))
                    )
            ]
        )

    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> XamarinFormsProgram.run app
