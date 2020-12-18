// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;

using Microsoft.Graph;

using Newtonsoft.Json;

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    /// <summary>
    /// The interface IShareItemsCollectionPage.
    /// </summary>
    [JsonConverter(typeof(InterfaceConverter<ShareItemsCollectionPage>))]
    public interface IShareItemsCollectionPage : ICollectionPage<Item>
    {
        /// <summary>
        /// Gets the next page <see cref="IShareItemsCollectionRequest"/> instance.
        /// </summary>
        IShareItemsCollectionRequest NextPageRequest { get; }

        /// <summary>
        /// Initializes the NextPageRequest property.
        /// </summary>
        void InitializeNextPageRequest(IBaseClient client, string nextPageLinkString);
    }
}
