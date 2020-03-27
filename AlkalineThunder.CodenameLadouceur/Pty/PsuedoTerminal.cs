using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlkalineThunder.Nucleus.Pty
{
    public sealed class PseudoTerminal : Stream
    {
        private ThreadSafeFifoBuffer _input = null;
        private ThreadSafeFifoBuffer _output = null;

        private bool _master = false;

        private PseudoTerminal(ThreadSafeFifoBuffer input, ThreadSafeFifoBuffer output, bool isMaster)
        {
            _input = input;
            _output = output;
            _master = isMaster;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => -1;

        public override long Position { get => -1; set => throw new NotSupportedException(); }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(_master)
            {
                while (_input.Length == 0) ;
                return _input.Read(buffer, offset, count);
            }
            else
            {
                return _output.Read(buffer, offset, count);
            }
        }

        private void WriteInput(byte value)
        {
            _input.WriteByte(value);
            _output.WriteByte(value);
        }

        private void WriteOutput(byte value)
        {
            _output.WriteByte(value);
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
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (offset < 0 || offset + count > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            for(int i = offset; i < offset + count; i++)
            {
                byte v = buffer[i];

                if(_master)
                {
                    WriteOutput(v);
                }
                else
                {
                    WriteInput(v);
                }
            }
        }

        public static void CreatePair(out PseudoTerminal master, out PseudoTerminal slave)
        {
            var input = new ThreadSafeFifoBuffer();
            var output = new ThreadSafeFifoBuffer();

            master = new PseudoTerminal(input, output, true);
            slave = new PseudoTerminal(input, output, false);
        }
    }
}
