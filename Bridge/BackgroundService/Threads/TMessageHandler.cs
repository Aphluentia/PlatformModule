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
        public TMessageHandler(IKafkaMessageHandler mKafka, IConnectionManagerMessageHandler mConnectionManager)
        {
            _kafkaMonitor = mKafka;
            _connectionManager = mConnectionManager;

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

                KafkaStatus.MessagesInQueue.Remove(message);
                KafkaStatus.MessagesInQueueCounter--;
                switch (message.Action)
                {
                    case Dtos.Enum.Action.Create_Connection:
                        this._connectionManager.CreateConnection
                        break;
                    case Dtos.Enum.Action.Close_Connection:
                        break;
                    case Dtos.Enum.Action.Ping_Connection:
                        break;
                    case Dtos.Enum.Action.Update_Section:
                        break;
                };
                KafkaStatus.MessagesProcessed.Add(message);
                KafkaStatus.MessagesProcessedCounter++;

            }
            

        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Handle completion
        }
    }
}
