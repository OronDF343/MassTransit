namespace MassTransit.MessageData.Values
{
    using System;
    using System.Threading.Tasks;
    using Serialization.JsonConverters;


    public class InlineMessageData<T> :
        MessageData<T>,
        IInlineMessageData
    {
        readonly IInlineMessageData _messageData;

        public InlineMessageData(Uri address, T value, IInlineMessageData messageData)
        {
            Value = Task.FromResult(value);
            Address = address;

            _messageData = messageData;
        }

        public void Set(MessageDataReference reference)
        {
            _messageData.Set(reference);
        }

        public Uri Address { get; }
        public bool HasValue => true;
        public Task<T> Value { get; }
    }
}
