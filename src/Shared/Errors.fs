module Murocast.Shared.Errors

open System
open Validation

type AuthenticationError =
    | InvalidLoginOrPassword
    | InvalidOrExpiredToken
    | EmailAlreadyRegistered
    | AccountAlreadyActivatedOrNotFound
    | InvalidPasswordResetKey

module AuthenticationError =
    let explain = function
        | InvalidLoginOrPassword -> "InvalidLoginOrPassword"
        | InvalidOrExpiredToken -> "InvalidOrExpiredToken"
        | EmailAlreadyRegistered -> "EmailAlreadyRegistered"
        | AccountAlreadyActivatedOrNotFound -> "AccountAlreadyActivatedOrNotFound"
        | InvalidPasswordResetKey -> "InvalidPasswordResetKey"

type DomainError =
    | UserNotActivated

module DomainError =
    let explain = function
        | UserNotActivated -> "UserNotActivated"

type ServerError =
    | Exception of string
    | Validation of ValidationError list
    | Authentication of AuthenticationError
    | DatabaseItemNotFound of Guid
    | Domain of DomainError

type ServerResult<'a> = Result<'a, ServerError>

exception ServerException of ServerError

module ServerError =
    let explain = function
        | Exception e -> e
        | Validation errs ->
            errs
            |> List.map ValidationError.explain
            |> String.concat ", "
        | Authentication e -> e |> AuthenticationError.explain
        | DatabaseItemNotFound i -> sprintf "Položka s ID %A nebyla nalezena v databázi." i
        | Domain e -> e |> DomainError.explain

    let failwith (er:ServerError) = raise (ServerException er)

    let ofOption err (v:Option<_>) =
        v
        |> Option.defaultWith (fun _ -> err |> failwith)

    let ofResult<'a> (v:Result<'a,ServerError>) =
        match v with
        | Ok v -> v
        | Error e -> e |> failwith

    let validate (validationFn:'a -> ValidationError list) (value:'a) =
        match value |> validationFn with
        | [] -> value
        | errs -> errs |> Validation |> failwith