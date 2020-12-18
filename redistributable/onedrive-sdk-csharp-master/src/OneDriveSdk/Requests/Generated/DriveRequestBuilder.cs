// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Graph;

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    /// <summary>
    /// The type DriveRequestBuilder.
    /// </summary>
    public partial class DriveRequestBuilder : BaseRequestBuilder, IDriveRequestBuilder
    {

        /// <summary>
        /// Constructs a new DriveRequestBuilder.
        /// </summary>
        /// <param name="requestUrl">The URL for the built request.</param>
        /// <param name="client">The <see cref="IBaseClient"/> for handling requests.</param>
        public DriveRequestBuilder(
            string requestUrl,
            IBaseClient client)
            : base(requestUrl, client)
        {
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <returns>The built request.</returns>
        public IDriveRequest Request()
        {
            return this.Request(null);
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <param name="options">The query and header options for the request.</param>
        /// <returns>The built request.</returns>
        public IDriveRequest Request(IEnumerable<Option> options)
        {
            return new DriveRequest(this.RequestUrl, this.Client, options);
        }
    
        /// <summary>
        /// Gets the request builder for Items.
        /// </summary>
        /// <returns>The <see cref="IDriveItemsCollectionRequestBuilder"/>.</returns>
        public IDriveItemsCollectionRequestBuilder Items
        {
            get
            {
                return new DriveItemsCollectionRequestBuilder(this.AppendSegmentToRequestUrl("items"), this.Client);
            }
        }

        /// <summary>
        /// Gets the request builder for Shared.
        /// </summary>
        /// <returns>The <see cref="IDriveSharedCollectionRequestBuilder"/>.</returns>
        public IDriveSharedCollectionRequestBuilder Shared
        {
            get
            {
                return new DriveSharedCollectionRequestBuilder(this.AppendSegmentToRequestUrl("shared"), this.Client);
            }
        }

        /// <summary>
        /// Gets the request builder for Special.
        /// </summary>
        /// <returns>The <see cref="IDriveSpecialCollectionRequestBuilder"/>.</returns>
        public IDriveSpecialCollectionRequestBuilder Special
        {
            get
            {
                return new DriveSpecialCollectionRequestBuilder(this.AppendSegmentToRequestUrl("special"), this.Client);
            }
        }
    
        /// <summary>
        /// Gets the request builder for DriveRecent.
        /// </summary>
        /// <returns>The <see cref="IDriveRecentRequestBuilder"/>.</returns>
        public IDriveRecentRequestBuilder Recent()
        {
            return new DriveRecentRequestBuilder(
                this.AppendSegmentToRequestUrl("oneDrive.recent"),
                this.Client);
        }
    
    }
}
