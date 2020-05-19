using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MBN.Modules
{
    public class SerialListener
    {
        private readonly Byte[] _startChars;
        private readonly Byte[] _endChars;
        private Boolean _startReceived, _endReceived;
        private readonly AutoResetEvent _receivedEvent;
        private readonly AutoResetEvent _messageSentEvent;
        private readonly Boolean _isArray;
        private Queue _queue;
        private readonly Object _lock;

        public Queue MessagesQueue
        {
            get; private set;
        }

        public delegate void MessageAvailableEventHandler(Object sender, EventArgs e);
        public event MessageAvailableEventHandler MessageAvailable = delegate { };

        #region Constructors
        public SerialListener(Char signal) : this((Byte)signal, (Byte)signal)
        {
        }

        public SerialListener(Byte signal) : this(signal, signal)
        {
        }

        public SerialListener(Char startSignal, Char endSignal) : this((Byte)startSignal, (Byte)endSignal)
        {
        }

        public SerialListener(Byte startSignal, Byte endSignal) : this(new[] { startSignal }, new[] { endSignal })
        {
        }

        public SerialListener(String startSignal, String endSignal) :
            this(Encoding.UTF8.GetBytes(startSignal), Encoding.UTF8.GetBytes(endSignal))
        {
        }

        public SerialListener(Byte[] startSignals, Byte[] endSignals)
        {
            _isArray = startSignals.Length > 1 || endSignals.Length > 1;
            _startChars = startSignals;
            _endChars = endSignals;
            _receivedEvent = new AutoResetEvent(false);
            _messageSentEvent = new AutoResetEvent(false);
            MessagesQueue = new Queue();
            _lock = new Object();
            new Thread(HandleSignals).Start();
        }
        #endregion

        public void Add(Char value) => Add((Byte)value);
        public void Add(Byte[] values)
        {
            for (var i = 0; i < values.Length; i++)
                Add(values[i]);
        }

        public void Add(Byte value)
        {
            if (_isArray)
                MultipleCharsSignal(value);
            else
                SingleCharSignal(value);
        }

        private void MultipleCharsSignal(Byte value) => Debug.WriteLine($"Adding bytes in multiple signals not supported (value {value})");

        private void SingleCharSignal(Byte value)
        {
            if (_startReceived)
            {
                _queue.Enqueue(value);
                _endReceived = _startReceived && value == _endChars[0];
                if (_endReceived)
                {
                    _receivedEvent.Set();
                    _messageSentEvent.WaitOne();
                    _startReceived = _endReceived = false;

                }
            }
            else
            {
                _startReceived = value == _startChars[0] || _startChars[0] == 32;
                if (_startReceived)
                {
                    _queue = new Queue();
                    _queue.Enqueue(value);
                    _receivedEvent.Reset();
                }
            }
        }

        private void HandleSignals()
        {
            while (true)
            {
                _receivedEvent.WaitOne();
                lock (_lock)
                {
                    var _tabtmp = new Byte[_queue.Count];
                    _queue.CopyTo(_tabtmp, 0);
                    MessagesQueue.Enqueue(_tabtmp);
                    _tabtmp = null;
                    _queue = null;
                    _messageSentEvent.Set();
                    MessageAvailableEventHandler messageReadyEvent = MessageAvailable;
                    messageReadyEvent(this, new EventArgs());
                }
            }
        }
    }
}
