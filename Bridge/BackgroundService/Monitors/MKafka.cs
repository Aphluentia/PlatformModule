using Bridge.BackgroundService.Interfaces;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using Confluent.Kafka;
using System.Xml.Linq;

namespace Bridge.BackgroundService.Monitors
{
    public class MKafka: IKafkaConsumer, IKafkaMessageHandler
    {
        private Queue<Message> IncomingMessages { get; set; }

        // Reentrant Lock
        private readonly object _lock = new object();
        private KafkaStatus _status;
        public MKafka(KafkaStatus status) {
            this.IncomingMessages = new Queue<Message>();
            this._status = status;
        }

        public bool AddIncomingMessage(Message _message)
        {
            lock (_lock)
            {
                try
                {

                    _status.MessagesReceivedCounter++;
                    // Critical section where the shared resource is accessed
                    IncomingMessages.Enqueue(_message);
                    _status.MessagesInQueueCounter++;
                    _status.MessagesInQueue.Add(_message);
                    Monitor.PulseAll(_lock);
                }
                catch(Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public Message FetchIncomingMessage()
        {
            var fetchedMessage = new Message();
            lock (_lock)
            {
                try
                {
                    while (IncomingMessages.Count() == 0)
                    {
                        Monitor.Wait(_lock);
                    }
                    // Critical section where the shared resource is accessed
                    fetchedMessage = IncomingMessages.Dequeue();
                    _status.MessagesInQueue.Remove(fetchedMessage);
                    _status.MessagesInQueueCounter--;
                }
                catch (Exception)
                {
                    return null;
                }
              
            }
            return fetchedMessage;
        }
    }
}
