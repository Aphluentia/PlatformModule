using Bridge.BackgroundService.Interfaces;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using System.Xml.Linq;

namespace Bridge.BackgroundService.Monitors
{
    public class MKafka: IKafkaConsumer, IKafkaMessageHandler
    {
        private Queue<Message> IncomingMessages { get; set; }

        // Reentrant Lock
        private readonly object _lock = new object();
        public MKafka() {
            this.IncomingMessages = new Queue<Message>();
        }

        public bool AddIncomingMessage(Message _message)
        {
            lock (_lock)
            {
                // Use Monitor.Enter() to acquire the lock
                Monitor.Enter(_lock);

                try
                {
                    // Critical section where the shared resource is accessed
                    IncomingMessages.Enqueue(_message);
                    KafkaStatus.MessagesInQueueCounter++;
                    KafkaStatus.MessagesInQueue.Add(_message);
                    Monitor.Pulse(this);
                }
                catch(Exception e)
                {
                    return false;
                }
                finally
                {
                    // Use Monitor.Exit() to release the lock
                    Monitor.Exit(_lock);
                }
            }
            return true;
        }

        public Message FetchIncomingMessage()
        {
            var fetchedMessage = new Message();
            lock (_lock)
            {
                // Use Monitor.Enter() to acquire the lock
                Monitor.Enter(_lock);

                try
                {
                    while (IncomingMessages.Count() == 0)
                    {
                        Monitor.Wait(this);
                    }
                    // Critical section where the shared resource is accessed
                    fetchedMessage = IncomingMessages.Dequeue();
                    KafkaStatus.MessagesInQueueCounter--;
                    KafkaStatus.MessagesInQueue.Remove(fetchedMessage);
                    
                }
                catch (Exception e)
                {
                    return null;
                }
                finally
                {
                    // Use Monitor.Exit() to release the lock
                    Monitor.Exit(_lock);
                }
            }
            return fetchedMessage;
        }
    }
}
