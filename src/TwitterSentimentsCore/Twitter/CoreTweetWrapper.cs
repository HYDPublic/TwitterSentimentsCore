﻿using System;
using System.Threading.Tasks;
using CoreTweet;
using CoreTweet.Core;
using CoreTweet.Rest;

namespace TwitterSentimentsCore.Twitter
{
    public class CoreTweetWrapper
    {
        // API information
        private string consumerKey = "KKkdkUnUX2HikXO0NbxwNK4A1";
        private string accessToken = "2789039840-7EWx1MKjSBNCvUPTxsb2hlTYwm6ZqpLcOYsU1IF";

        // Connection tokens object
        public Tokens tokens;

        public CoreTweetWrapper()
        {
            // Use OAuth to open an authenticated session and make requests
            tokens = Tokens.Create(consumerKey, "8GSJ0HNkPaLJ89jqwYMFgRj015gdSBhscQ46xY6grs8FD9PQXm", accessToken, "WrcRG29RNb4U0bvCpW85E2L0jlmRSKWyIbjzPldxfMHEC", screenName: "JamesMSP");
        }

        // Return the text of the most recent status, given the user name
        public async Task<UserResponse> GetUserMostRecentStatus(string ScreenName)
        {
            return await tokens.Users.ShowAsync(screen_name: ScreenName); // need status text
        }

        // Return the n most recent statuses, provided the user name and number of statuses wanted
        public async Task<ListedResponse<Status>> GetUserTimeline(string ScreenName, int Count)
        {
            return await tokens.Users.IncludedTokens.Statuses.UserTimelineAsync(screen_name: ScreenName, count: Count);
        }

        // Return tweets related to the specified query
        public async Task<SearchResult> GetTweetsByTopic(string Query)
        {
            return await tokens.Search.TweetsAsync(q: Query);
        }
    }
}