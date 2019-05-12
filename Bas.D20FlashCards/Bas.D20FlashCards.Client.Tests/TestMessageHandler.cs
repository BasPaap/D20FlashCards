using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bas.D20FlashCards.Client.Services
{
    sealed class TestMessageHandler : HttpMessageHandler
    {
        public Collection<HttpRequestMessage> SentRequestMessages { get; } = new Collection<HttpRequestMessage>();
        private Dictionary<Uri, HttpResponseMessage> responsesToSend = new Dictionary<Uri, HttpResponseMessage>();

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SentRequestMessages.Add(request);                
            return await Task.FromResult(responsesToSend[request.RequestUri] ?? new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        public void AddResponseMessageToReturnForUri(Uri uri, HttpResponseMessage responseMessageToSend)
        {
            this.responsesToSend.Add(uri, responseMessageToSend);
        }
    }
}
