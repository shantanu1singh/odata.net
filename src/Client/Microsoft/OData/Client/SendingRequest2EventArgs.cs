//   OData .NET Libraries ver. 6.9
//   Copyright (c) Microsoft Corporation
//   All rights reserved. 
//   MIT License
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to use,
//   copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the
//   Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:

//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.

//   THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Microsoft.OData.Client
{
    using System;
    using Microsoft.OData.Core;

    /// <summary> Event args for the SendingRequest2 event. </summary>
    public class SendingRequest2EventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of SendingRequest2EventsArgs
        /// </summary>
        /// <param name="requestMessage">request message.</param>
        /// <param name="descriptor">Descriptor that represents this change.</param>
        /// <param name="isBatchPart">True if this args represents a request within a batch, otherwise false.</param>
        internal SendingRequest2EventArgs(IODataRequestMessage requestMessage, Descriptor descriptor, bool isBatchPart)
        {
            this.RequestMessage = requestMessage;
            this.Descriptor = descriptor;
            this.IsBatchPart = isBatchPart;
        }

        /// <summary>The web request reported through this event. The handler may modify or replace it.</summary>
        public IODataRequestMessage RequestMessage
        {
            get;
            private set;
        }

        /// <summary>The request header collection.</summary>
        public Descriptor Descriptor
        {
            get;
            private set;
        }

        /// <summary> Returns true if this event is fired for request within a batch, otherwise returns false. </summary>
        public bool IsBatchPart
        {
            get;
            private set;
        }
    }
}