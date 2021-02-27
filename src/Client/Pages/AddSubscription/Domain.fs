module Murocast.Client.Pages.AddSubscription.Domain

open Murocast.Shared.Errors
open Murocast.Shared.Auth.Communication
open Murocast.Shared.Core.Subscriptions.Communication.Queries
open Murocast.Client.Forms

type Model = {
    Feeds : FoundFeedRendition list
}

type Msg =
    | FindFeeds of string
    | FoundFeedsLoaded of ServerResult<FoundFeedRendition list>
