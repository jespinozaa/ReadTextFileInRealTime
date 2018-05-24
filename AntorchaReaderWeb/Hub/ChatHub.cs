using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AntorchaReaderWeb
{
    public class ChatHub : Hub
    {
        public void Send()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".");
            fsw.Filter = @"\\192.168.2.2\0.MargenDx";
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s, e) => wh.Set();

            var fs = new FileStream(@"\\192.168.2.2\0.MargenDx\log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                var s = "";
                while (true)
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        Clients.All.broadcastMessage("Transmitting: " + s);
                        wh.WaitOne(1000);
                    }
                    //else
                    //{
                    //    Clients.All.broadcastMessage( "Sleeping...");
                    //    wh.WaitOne(1000);
                    //}

                }
            }
        }
    }
}