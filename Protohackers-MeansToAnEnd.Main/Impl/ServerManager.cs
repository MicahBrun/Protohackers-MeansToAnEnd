using System.Net;
using System.Net.Sockets;
using Protohackers_MeansToAnEnd.Main.Domain;
using Protohackers_MeansToAnEnd.Main.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Protohackers_MeansToAnEnd.Main.Impl;
class ServerManager : IServerManager
    {
        private const int _PORT = 9999;
        private TcpListener _server;
        private ILogger _logger;
        private IRequestCreator _requestCreator;
        private IRequestHandler _requestHandler;
        public ServerManager(ILogger logger, IRequestCreator requestCreator, IRequestHandler requestHandler)
        {
            _server = new TcpListener(IPAddress.Any, _PORT);
            _logger = logger;
            _requestCreator = requestCreator;
            _requestHandler = requestHandler;
        }

        public void StartServer()
        {
            _server.Start();
            AcceptConnection();
        }

        private void AcceptConnection()
        {
            _server.BeginAcceptTcpClient(HandleConnection, _server);
        }

        private void HandleConnection(IAsyncResult result)
        {
            _logger.Info("Connected.");
            AcceptConnection();
            try
            {
                using (var client = _server.EndAcceptTcpClient(result))
                {
                    var priceStore = new PriceStore();

                    while (client.Connected)
                    {
                        NetworkStream networkStream = client.GetStream();

                        byte[] readByteArray = new byte[9];
                        networkStream.Read(readByteArray, 0, 9);

                        var request = _requestCreator.Create(readByteArray);
                        _logger.Info($"Request {request.GetType()}");

                        var response = _requestHandler.HandleRequest(request, priceStore);
                        if (response != null)
                        {
                            _logger.Info($"Return value {response}.");
                            networkStream.Write(Utils.ToBytes(response.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }