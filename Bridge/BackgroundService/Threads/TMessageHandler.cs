using Bridge.BackgroundService.Interfaces;
using Bridge.BackgroundService.Monitors;
using Bridge.ConfigurationSections;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.BackgroundService.Threads
{
    public class TMessageHandler
    {
        private BackgroundWorker _worker;
        private bool StopFlag;
        private readonly IKafkaMessageHandler _kafkaMonitor;
        private readonly IConnectionManagerMessageHandler _connectionManager;
        private KafkaStatus _status;
        public TMessageHandler(MKafka mKafka, MConnectionManager mConnectionManager, KafkaStatus status)
        {
            _kafkaMonitor = mKafka;
            _connectionManager = mConnectionManager;
            _status = status;

            StopFlag = false;
            _worker = new BackgroundWorker();
            _worker.DoWork += RunAsync;
            _worker.RunWorkerCompleted += RunWorkerCompleted;
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        private void RunAsync(object sender, DoWorkEventArgs e)
        {
            while (!StopFlag)
            {

                var message = this._kafkaMonitor.FetchIncomingMessage();
                if (message != null)
                {
                    var connection = new Connection()
                    {
                        WebPlatformId = message.TargetId,
                        ModuleId = message.SourceId,
                        ModuleType = message.SourceModuleType
                    };
                    switch (message.Action)
                    {
                        case Dtos.Enum.Action.Create_Connection:
                            this._connectionManager.CreateConnection(connection);
                            break;
                        case Dtos.Enum.Action.Close_Connection:
                            this._connectionManager.CloseConnection(connection);
                            break;
                        case Dtos.Enum.Action.Ping_Connection:
                            this._connectionManager.PingConnection(connection);
                            break;
                        case Dtos.Enum.Action.Update_Section:
                            this._connectionManager.UpdateSection(connection);
                            break;
                    };


                    _status.MessagesProcessed.Add(message);
                    _status.MessagesProcessedCounter++;
                    
                    
                    
                }
                

            }
            

        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Handle completion
        }
    }
}
