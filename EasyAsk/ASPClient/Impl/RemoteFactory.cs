namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;

    public class RemoteFactory
    {
        public static IRemoteEasyAsk create(string hostName, int port, string dictionary)
        {
            return new RemoteEasyAsk(hostName, port, dictionary);
        }
    }
}

