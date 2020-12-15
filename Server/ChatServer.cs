using System;
using System.Threading;
using System.Collections.Generic;
using SimpleSockets.Server;
using SimpleSockets.Messaging.Metadata;

namespace ExtChat.Server
{
    class ChatServer
    {
        SimpleSocketListener listener;
        Dictionary<IClientInfo, string> clients;

        public void Start()
        {
            listener = new SimpleSocketTcpListener();
            clients = new Dictionary<IClientInfo, string>();
            listener.StartListening("127.0.0.1", 2137);
            listener.ServerHasStarted += Listener_ServerHasStarted;
            listener.ClientConnected += Listener_ClientConnected;
            listener.MessageReceived += Listener_MessageReceived;
            Thread.Sleep(-1);
        }

        public void JoinBroadcast(string name)
        {
            Broadcast("JOIN|" + name);
        }

        public void MessageBroadcast(string author, string message)
        {
            Broadcast("MSG|" + author + "|" + message);
        }

        private void Listener_MessageReceived(IClientInfo client, string message)
        {
            var tokens = message.Split("|");
            switch(tokens[0])
            {
                case "JOIN":
                    clients.Add(client, tokens[1]);
                    Console.WriteLine(tokens[1] + " joined");
                    JoinBroadcast(tokens[1]);
                    break;
                case "MSG":
                    Console.WriteLine(message);
                    string name = GetNameByID(client.Id);
                    MessageBroadcast(name, tokens[1]);
                    break;
            }
        }

        public string GetNameByID(int id)
        {
            foreach (var client in clients)
            {
                if (client.Key.Id == id)
                {
                    return client.Value;
                }
            }
            return null;
        }

        public void Broadcast(string msg)
        {
            foreach (var client in clients.Keys)
            {
                listener.SendMessageAsync(client.Id, msg);
            }
        }

        private void Listener_ClientConnected(IClientInfo clientInfo)
        {
            Console.WriteLine(clientInfo.RemoteIPv4 + " connected to the server");
        }

        private void Listener_ServerHasStarted()
        {
            Console.WriteLine("The server has started!");
        }
    }
}
