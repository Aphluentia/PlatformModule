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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.BackgroundService.Threads
{
    public class TKafkaConsumer
    {
        private BackgroundWorker worker;
        private readonly KafkaConfigSection _kafkaConfig;
        private bool StopFlag;
        private IKafkaConsumer mKafka;
        public TKafkaConsumer(IOptions<KafkaConfigSection> kafkaConfig, MKafka mKafka)
        {
            this._kafkaConfig = kafkaConfig.Value;
            StopFlag = false;
            this.mKafka = mKafka;

            worker = new BackgroundWorker();
            worker.DoWork += RunAsync;
            worker.RunWorkerCompleted += RunWorkerCompleted;
        }

        public void Start()
        {
            worker.RunWorkerAsync();
        }

        private void RunAsync(object sender, DoWorkEventArgs e)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                GroupId = _kafkaConfig.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_kafkaConfig.Topic);

                while (!StopFlag)
                {
                    Message newMessageAsJson = null;
                    try
                    {
                        var consumed = consumer.Consume(CancellationToken.None).Message.Value;
                        newMessageAsJson = JsonConvert.DeserializeObject<Message>(consumed);
                    }
                    catch (Exception ex) {
                        Debug.WriteLine(ex);
                    }
                    
                   
                    if (newMessageAsJson!=null) 
                        mKafka.AddIncomingMessage(newMessageAsJson);

                }

                consumer.Close();
            }
           
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Handle completion
        }
    }
}
