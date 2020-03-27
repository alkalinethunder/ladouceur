using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace AlkalineThunder.Nucleus.Pty
{
    public sealed class ThreadSafeFifoBuffer : Stream
    {
        private List<byte> _data = new List<byte>();
        private int _readPos = 0;
        private Mutex _mutex = new Mutex();

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => _data.Count - _readPos;

        public override long Position { get => 0; set => throw new NotSupportedException(); }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || offset + count > buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));

            int bytesRead = 0;

            lock (_mutex)
            {
                for (int i = offset; i < offset + count; i++)
                {
                    if (_readPos == _data.Count) break;

                    buffer[i] = _data[_readPos];
                    _readPos++;
                    bytesRead++;
                }
            }
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || offset + count > buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));

            lock (_mutex)
            {
                for (int i = offset; i < offset + count; i++)
                {
                    _data.Add(buffer[i]);
                }
            }
        }
    }
}
