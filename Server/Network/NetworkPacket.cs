using System;
using System.Text;
using Server.Utilite;

namespace Server.Network
{
    public struct NetworkPacket
    {
        private const int DefaultOverflowValue = 128;
        private byte[] _buffer;
        private int _offset;
        private readonly bool _receivedPacket;

        public NetworkPacket(int headerOffset, byte[] buffer)
        {
            _receivedPacket = true;
            _buffer = buffer;
            _offset = headerOffset;
        }
        
        public NetworkPacket(params byte[] opcodes)
        {
            _receivedPacket = false;
            _buffer = opcodes;
            _offset = opcodes.Length;
        }

        public unsafe void WriteByte(byte v)
        {
            ValidateBufferSize(sizeof(byte));

            fixed (byte* buf = _buffer)
                *(buf + _offset++) = v;
        }

        public void WriteByte(params byte[] v)
        {
            WriteByteArray(v);
        }

        public void WriteByteArray(byte[] v)
        {
            int length = v.Length;

            ValidateBufferSize(length);

            MemoryBuffer.Copy(v, 0, _buffer, _offset, length);
            _offset += length;
        }

        public unsafe void WriteShort(short v)
        {
            ValidateBufferSize(sizeof(short));

            fixed (byte* buf = _buffer)
                *(short*)(buf + _offset) = v;

            _offset += sizeof(short);
        }

        public unsafe void WriteShort(params short[] v)
        {
            int length = v.Length * sizeof(short);

            ValidateBufferSize(length);

            fixed (byte* buf = _buffer)
            {
                fixed (short* w = v)
                    MemoryBuffer.UnsafeCopy(w, length, buf, ref _offset);
            }
        }

        public unsafe void WriteInt(int v)
        {
            ValidateBufferSize(sizeof(int));

            fixed (byte* buf = _buffer)
                *(int*)(buf + _offset) = v;

            _offset += sizeof(int);
        }

        public unsafe void WriteInt(params int[] v)
        {
            int length = v.Length * sizeof(int);

            ValidateBufferSize(Length);

            fixed (byte* buf = _buffer)
            {
                fixed (int* w = v)
                    MemoryBuffer.UnsafeCopy(w, length, buf, ref _offset);
            }
        }

        public unsafe void WriteIntArray(int[] v)
        {
            int length = v.Length * sizeof(int);

            ValidateBufferSize(Length);

            fixed (byte* buf = _buffer)
            {
                fixed (int* w = v)
                    MemoryBuffer.UnsafeCopy(w, length, buf, ref _offset);
            }
        }

        public unsafe void WriteDouble(double v)
        {
            ValidateBufferSize(sizeof(double));

            fixed (byte* buf = _buffer)
                *(double*)(buf + _offset) = v;

            _offset += sizeof(double);
        }

        public unsafe void WriteDouble(params double[] v)
        {
            int length = v.Length * sizeof(double);

            ValidateBufferSize(length);

            fixed (byte* buf = _buffer)
            {
                fixed (double* w = v)
                    MemoryBuffer.UnsafeCopy(w, length, buf, ref _offset);
            }
        }

        public unsafe void WriteLong(long v)
        {
            ValidateBufferSize(sizeof(long));

            fixed (byte* buf = _buffer)
                *(long*)(buf + _offset) = v;

            _offset += sizeof(long);
        }

        public unsafe void WriteLong(params long[] v)
        {
            int length = v.Length * sizeof(long);

            ValidateBufferSize(length);

            fixed (byte* buf = _buffer)
            {
                fixed (long* w = v)
                    MemoryBuffer.UnsafeCopy(w, length, buf, ref _offset);
            }
        }

        public unsafe void WriteString(string s)
        {
            byte[] buffer = Encoding.Default.GetBytes(s);
            WriteInt(buffer.Length);
            WriteByteArray(buffer);
        }

        public void InternalWriteBool(bool v)
        {
            WriteByte(v ? (byte)0x01 : (byte)0x00);
        }

        public void InternalWriteDateTime(DateTime v)
        {
            WriteLong(v.Ticks);
        }

        public unsafe byte ReadByte()
        {
            fixed (byte* buf = _buffer)
                return *(buf + _offset++);
        }

        public unsafe byte[] ReadBytesArray(int length)
        {
            byte[] dest = new byte[length];

            fixed (byte* buf = _buffer, dst = dest)
                MemoryBuffer.Copy(buf, length, dst, ref _offset);
            return dest;
        }

        public byte[] ReadByteArrayAlt(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(GetBuffer(), _offset, result, 0, length);
            _offset += length;
            return result;
        }

        public unsafe short ReadShort()
        {
            fixed (byte* buf = _buffer)
            {
                short v = *(short*)(buf + _offset);
                _offset += sizeof(short);
                return v;
            }
        }

        public unsafe int ReadInt()
        {
            fixed (byte* buf = _buffer)
            {
                int v = *(int*)(buf + _offset);
                _offset += sizeof(int);
                return v;
            }
        }

        public unsafe double ReadDouble()
        {
            fixed (byte* buf = _buffer)
            {
                double v = *(double*)(buf + _offset);
                _offset += sizeof(double);
                return v;
            }
        }

        public unsafe long ReadLong()
        {
            fixed (byte* buf = _buffer)
            {
                long v = *(long*)(buf + _offset);
                _offset += sizeof(long);
                return v;
            }
        }

        public unsafe string ReadString(int length)
        {
            byte[] buffer = new byte[length];

            for (int iterator = 0; iterator < length; iterator++)
                buffer[iterator] = ReadByte();

            return Encoding.Default.GetString(buffer, 0, length);
        }

        public bool InternalReadBool()
        {
            return ReadByte() == 0x01;
        }

        public DateTime InternalReadDateTime()
        {
            return new DateTime(ReadLong());
        }

        private void ValidateBufferSize(int nextValueLength)
        {
            if ((_offset + nextValueLength) > _buffer.Length)
                MemoryBuffer.Extend(ref _buffer, nextValueLength + DefaultOverflowValue);
        }

        public unsafe void Prepare(int headerSize)
        {
            _offset += headerSize;

            MemoryBuffer.Extend(ref _buffer, headerSize, _offset);

            fixed (byte* buf = _buffer)
            {
                if (headerSize == sizeof(short))
                    *(short*)buf = (short)_offset;
                else
                    *(int*)buf = _offset;
            }
        }

        public byte[] GetBuffer()
        {
            return _buffer;
        }

        public byte[] GetBuffer(int skipFirstBytesCount)
        {
            return MemoryBuffer.Copy(_buffer, skipFirstBytesCount, new byte[_buffer.Length - skipFirstBytesCount], 0, _buffer.Length - skipFirstBytesCount);
        }

        public void MoveOffset(int size)
        {
            _offset += size;
        }

        public unsafe byte FirstOpcode
        {
            get
            {
                fixed (byte* buf = _buffer)
                    return *buf;
            }
        }

        public unsafe int SecondOpcode
        {
            get
            {
                fixed (byte* buf = _buffer)
                    return *(buf + 1);
            }
        }

        public int Length => _receivedPacket ? _buffer.Length : _offset;

        public override string ToString()
        {
            return MemoryBuffer.ToString(_buffer);
        }
    }
}