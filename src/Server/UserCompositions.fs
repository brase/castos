module Castos.UserCompositions

open Castos
open Castos.Auth
open Castos.Http
open Castos.Users

open System
open Giraffe
open Saturn
open Murocast.Shared
open Murocast.Shared.Core.UserAccount.Domain.Queries

type AddUserRendition =
    {
        EMail: string
        Password: string
    }

let private streamId = sprintf "users"

let private storeUsersEvent eventStore event =
    storeEvent eventStore (fun _ -> streamId) event

let getUsersComposition eventStore =
    let (events, _) = getAllEventsFromStreamById eventStore streamId
    Ok (getUsers events)

let getUserComposition eventStore email =
    match getUsersComposition eventStore with
    | Ok users -> Ok (getUser users email)
    | Error m -> Error m

let getUserAccountComposition (eventStore:CosmoStore.EventStore<_,_>) (user:AuthenticatedUser) =
    Ok user

let addUserComposition eventStore (rendition:AddUserRendition) =
    let salt = generateSalt()
    let hash = calculateHash rendition.Password salt
    UserAdded { Id = Guid.NewGuid()
                Email = rendition.EMail
                PasswordHash = hash
                Salt = salt }
    |> storeUsersEvent eventStore

    Ok ("added user")


let smapiauthComposition (db:Database.DatabaseConnection) eventStore (rendition:SmapiAuthRendition) =
    match getUsersComposition eventStore with
    | Error m -> Error "Users not found"
    | Ok users -> match (getUser users rendition.EMail) with
                       | None -> Error "User not found"
                       | Some u -> let correctPassword = (u.PasswordHash = calculateHash rendition.Password u.Salt)
                                   let authReq = db.GetAuthRequestByLinkToken rendition.LinkCode rendition.HouseholdId
                                   let found = authReq.IsSome
                                   match (correctPassword, found) with
                                   | true, true ->
                                        let updatedReq = { authReq.Value with
                                                            UserId = Some (u.Id) }
                                        db.UpdateAuthRequest updatedReq |> ignore
                                        Ok "Success"
                                   | _ -> Error "Auth not successful"

let usersRouter eventStore db = router {
    //pipe_through authorize  //<-- for all methods of router
    get "" (authorize >=> adminOnly >=>  (processAsync getUsersComposition eventStore))
    get "/userinfo" (authorize >=> returnUser())
    post "" (processDataAsync addUserComposition eventStore)
    post "/smapiauth" (processDataAsync (smapiauthComposition db) eventStore)
}

