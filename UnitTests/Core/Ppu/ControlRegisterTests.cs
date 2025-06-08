using Xunit;
using NEStor.Core.Ppu;

namespace UnitTests.Core.Ppu
{
    public class ControlRegisterTests
    {
        [Fact]
        public void Register_Get_ReturnsCorrectBitPattern()
        {
            var reg = new ControlRegister
            {
                NameTableX = 1,
                NameTableY = 1,
                Increment = 32,
                PatternSprite = 1,
                PatternBg = 1,
                SpriteSize = 16,
                SlaveMode = true,
                EnableNMI = true
            };

            // 0b11111111 = 0xFF
            Assert.Equal(0xFF, reg.Register);

            reg = new ControlRegister
            {
                NameTableX = 0,
                NameTableY = 0,
                Increment = 1,
                PatternSprite = 0,
                PatternBg = 0,
                SpriteSize = 8,
                SlaveMode = false,
                EnableNMI = false
            };

            // 0b00000000 = 0x00
            Assert.Equal(0x00, reg.Register);
        }

        [Theory]
        [InlineData(0x00, 0, 0, 1, 0, 0, 8, false, false)]
        [InlineData(0xFF, 1, 1, 32, 1, 1, 16, true, true)]
        [InlineData(0x84, 0, 0, 32, 0, 0, 8, false, true)]
        [InlineData(0x20, 0, 0, 1, 0, 0, 16, false, false)]
        [InlineData(0x0A, 0, 1, 1, 1, 0, 8, false, false)]
        public void Register_Set_SetsFieldsCorrectly(
            int value, int expectedX, int expectedY, int expectedInc,
            int expectedSprite, int expectedBg, int expectedSize, bool expectedSlave, bool expectedNmi)
        {
            var reg = new ControlRegister();
            reg.Register = value;

            Assert.Equal(expectedX, reg.NameTableX);
            Assert.Equal(expectedY, reg.NameTableY);
            Assert.Equal(expectedInc, reg.Increment);
            Assert.Equal(expectedSprite, reg.PatternSprite);
            Assert.Equal(expectedBg, reg.PatternBg);
            Assert.Equal(expectedSize, reg.SpriteSize);
            Assert.Equal(expectedSlave, reg.SlaveMode);
            Assert.Equal(expectedNmi, reg.EnableNMI);
        }

        [Fact]
        public void DataLatch_CanBeSetAndRead()
        {
            var reg = new ControlRegister();
            reg.DataLatch = 0xAB;
            Assert.Equal(0xAB, reg.DataLatch);
        }
    }
}