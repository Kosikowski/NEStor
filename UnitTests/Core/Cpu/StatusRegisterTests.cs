using Xunit;
using NEStor.Core.Cpu;

namespace UnitTests.Core.Cpu
{
    public class StatusRegisterTests
    {
        [Fact]
        public void Register_Get_ReturnsCorrectBitPattern()
        {
            var sr = new StatusRegister
            {
                Carry = true,
                Zero = true,
                IrqDisable = true,
                Decimal = true,
                Break = true,
                Unused = true,
                Overflow = true,
                Negative = true
            };

            // All bits set: 0xFF
            Assert.Equal(0xFF, sr.Register);

            sr = new StatusRegister
            {
                Carry = false,
                Zero = false,
                IrqDisable = false,
                Decimal = false,
                Break = false,
                Unused = false,
                Overflow = false,
                Negative = false
            };

            // Only Unused is set to false, so result should be 0x00
            Assert.Equal(0x20, sr.Register);

            sr = new StatusRegister
            {
                Carry = true,
                Zero = false,
                IrqDisable = false,
                Decimal = false,
                Break = false,
                Unused = false,
                Overflow = false,
                Negative = false
            };

            Assert.Equal(0x21, sr.Register);
        }

        [Fact]
        public void Register_Set_SetsFlagsCorrectly()
        {
            var sr = new StatusRegister();

            sr.Register = 0xFF;
            Assert.True(sr.Carry);
            Assert.True(sr.Zero);
            Assert.True(sr.IrqDisable);
            Assert.True(sr.Decimal);
            Assert.True(sr.Break);
            Assert.True(sr.Unused); // Always true after set
            Assert.True(sr.Overflow);
            Assert.True(sr.Negative);

            sr.Register = 0x00;
            Assert.False(sr.Carry);
            Assert.False(sr.Zero);
            Assert.False(sr.IrqDisable);
            Assert.False(sr.Decimal);
            Assert.False(sr.Break);
            Assert.True(sr.Unused); // Always true after set
            Assert.False(sr.Overflow);
            Assert.False(sr.Negative);

            sr.Register = 0xA5; // 0b10100101
            Assert.True(sr.Carry);      // 0x01
            Assert.False(sr.Zero);      // 0x02
            Assert.True(sr.IrqDisable); // 0x04
            Assert.False(sr.Decimal);   // 0x08
            Assert.False(sr.Break);     // 0x10
            Assert.True(sr.Unused);     // Always true
            Assert.False(sr.Overflow);  // 0x40
            Assert.True(sr.Negative);   // 0x80
        }

        [Fact]
        public void Unused_IsAlwaysTrue_AfterSet()
        {
            var sr = new StatusRegister { Unused = false };
            sr.Register = 0x00;
            Assert.True(sr.Unused);

            sr.Unused = false;
            sr.Register = 0xFF;
            Assert.True(sr.Unused);
        }
    }
}