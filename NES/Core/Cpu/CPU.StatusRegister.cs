using System;

namespace NEStor.Core.Cpu
{
    public class StatusRegister
    {
#if DEBUG
        // Old implementation: individual bool fields
        public bool Carry;
        public bool Zero;
        public bool IrqDisable;
        public bool ToggleIrqDisable;
        public bool Decimal;
        public bool Break;
        public bool Unused = true;
        public bool Overflow;
        public bool Negative;
        public int Register
        {
            get
            {
                var result = 0;
                if (Carry)      result |= 0x01;
                if (Zero)       result |= 0x02;
                if (IrqDisable) result |= 0x04;
                if (Decimal)    result |= 0x08;
                if (Break)      result |= 0x10;
                if (Unused)     result |= 0x20;
                if (Overflow)   result |= 0x40;
                if (Negative)   result |= 0x80;
                return result;
            }
            set
            {
                Carry =      (value & 0x01) != 0;
                Zero =       (value & 0x02) != 0;
                IrqDisable = (value & 0x04) != 0;
                Decimal =    (value & 0x08) != 0;
                Break =      (value & 0x10) != 0;
                Unused =     true;
                Overflow =   (value & 0x40) != 0;
                Negative =   (value & 0x80) != 0;
            }
        }
#else
        // New implementation: single byte with bitwise access - much faster
        private byte _flags = 0x20; // Unused bit is always set

        public bool Carry
        {
            get => (_flags & 0x01) != 0;
            set => _flags = value ? (byte)(_flags | 0x01) : (byte)(_flags & ~0x01);
        }
        public bool Zero
        {
            get => (_flags & 0x02) != 0;
            set => _flags = value ? (byte)(_flags | 0x02) : (byte)(_flags & ~0x02);
        }
        public bool IrqDisable
        {
            get => (_flags & 0x04) != 0;
            set => _flags = value ? (byte)(_flags | 0x04) : (byte)(_flags & ~0x04);
        }
        public bool ToggleIrqDisable { get; set; }
        public bool Decimal
        {
            get => (_flags & 0x08) != 0;
            set => _flags = value ? (byte)(_flags | 0x08) : (byte)(_flags & ~0x08);
        }
        public bool Break
        {
            get => (_flags & 0x10) != 0;
            set => _flags = value ? (byte)(_flags | 0x10) : (byte)(_flags & ~0x10);
        }
        public bool Unused
        {
            get => true;
            set { /* do nothing, always true */ }
        }
        public bool Overflow
        {
            get => (_flags & 0x40) != 0;
            set => _flags = value ? (byte)(_flags | 0x40) : (byte)(_flags & ~0x40);
        }
        public bool Negative
        {
            get => (_flags & 0x80) != 0;
            set => _flags = value ? (byte)(_flags | 0x80) : (byte)(_flags & ~0x80);
        }

        public int Register
        {
            get => _flags | 0x20; // Ensure unused bit is always set
            set => _flags = (byte)((value & 0xDF) | 0x20); // Always set unused bit
        }
#endif
    }
}
