using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;

namespace SparkServer.Test.AsyncCall;

class AsyncCallService : ServiceContext
{
    public class Req
    {
        public string text;
        public Int64 time;
    }
    
    public class Rsp
    {
        public string text;
        public Int64 useTime;
    }

    protected override void Init()
    {
        base.Init();
        RegisterServiceAsyncMethods("OnAsyncCall", OnAsyncCall);
        this.TestSendMsg();
    }

    private void OnAsyncCall(int source, int session, string method, object param)
    {
        Req req = param as Req;
        LoggerHelper.Info(m_serviceAddress, string.Format($">>>>>>>>>>>>>>>>>>>>Request Receive Time: {req.time} info: {req.text}"));
        Rsp rsp = new Rsp
        {
            text = "Async Response.",
            useTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - req.time
        };
        DoAsyncResponse(source, method, rsp, session);
    }
    
    protected async void TimeoutCallback(SSContext context, long currentTime)
    {
        this.TestSendMsg();
        Req req = new Req
        {
            text = "Hello World.",
            time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
        Random random = new System.Random();
        Task<object> tRsp = AsyncCall($"AsyncCallService{random.Next(0, 1000)}", "OnAsyncCall", req);
        Rsp rsp = await tRsp as Rsp;
        LoggerHelper.Info(m_serviceAddress, string.Format($"<<<<<<<<<<<<<<<<<<<<Response Receive Use Time: {rsp.useTime} info: {rsp.text}"));
        Int64 totalUseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - req.time;
        LoggerHelper.Info(m_serviceAddress, string.Format($"<<<<<<<<<<<<<<<<<<<<Response Receive Total UseTime: {totalUseTime} info: {rsp.text}"));
    }

    private void TestSendMsg()
    {
        this.Timeout(new SSContext(), 1, TimeoutCallback);
    }
}