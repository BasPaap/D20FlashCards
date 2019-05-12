using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bas.D20FlashCards.Client.Services
{
    sealed class TestMessageHandler : HttpMessageHandler
    {
        public Collection<HttpRequestMessage> SentRequestMessages { get; } = new Collection<HttpRequestMessage>();
        public HttpResponseMessage ResponseMessageToSend { get; set; }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SentRequestMessages.Add(request);
            return await Task.FromResult(ResponseMessageToSend);
        }
    }
}
