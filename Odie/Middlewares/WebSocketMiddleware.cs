using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Odie.Middlewares
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly List<WebSocket> _webSocketCollection;

        public WebSocketMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<WebSocketMiddleware>();
            _webSocketCollection = new List<WebSocket>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();

                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    _webSocketCollection.Add(webSocket);

                    while (webSocket.State == WebSocketState.Open)
                    {
                        try
                        {
                            var buffer = new ArraySegment<Byte>(new Byte[4096]);

                            var received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                            switch (received.MessageType)
                            {
                                case WebSocketMessageType.Text:
                                    var request = Encoding.UTF8
                                        .GetString(buffer.Array, buffer.Offset, buffer.Count)
                                        .Trim(new char[] { '\0' });
                                    var type = WebSocketMessageType.Text;
                                    var data = Encoding.UTF8.GetBytes(request);
                                    buffer = new ArraySegment<Byte>(data, 0, data.Length); // TODO ath
                                    
                                    _webSocketCollection.ForEach(async (s) =>
                                    {
                                        if (s != null && s.State == WebSocketState.Open)
                                        {
                                            await s.SendAsync(buffer, type, true, CancellationToken.None);
                                        }
                                    });
                                    
                                    //await webSocket.SendAsync(buffer, type, true, CancellationToken.None);
                                    break;
                                case WebSocketMessageType.Close:
                                    await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                                    _webSocketCollection.Remove(webSocket);
                                    break;
                            }

                        }
                        catch (Exception e)
                        {
                            // NOTE: https://github.com/aspnet/AspNetCoreModule/issues/77
                            // System.Net.WebSockets.WebSocketException
                            Console.WriteLine("Exception: {0}", e);
                            throw(e);
                        }

                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
            
        }
    }

    public static class RequestLoggerExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketMiddleware>();
        }
    }
}