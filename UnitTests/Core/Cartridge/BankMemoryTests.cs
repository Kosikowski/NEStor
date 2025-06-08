using System;
using System.Collections.Generic;
using NEStor.Core.Cartridge;
using Xunit;

namespace UnitTests.Core.Cartridge
{
    public class BankMemoryTests
    {
        private BankMemory CreateBankMemory(int bankCount, int bankSize)
        {
            var bankMemory = new BankMemory();

            // Initialize SwapBanks with unique data
            for (int i = 0; i < bankCount; i++)
            {
                var bank = new byte[bankSize];
                for (int j = 0; j < bankSize; j++)
                    bank[j] = (byte)(i * 10 + j);
                bankMemory.SwapBanks.Add(bank);
            }

            // Initialize Banks to point to SwapBanks
            for (int i = 0; i < bankCount; i++)
                bankMemory.Banks.Add(bankMemory.SwapBanks[i]);

            // Set BankSize (triggers internal setup)
            bankMemory.BankSize = bankSize;

            return bankMemory;
        }

        [Fact]
        public void Indexer_GetSet_WorksCorrectly()
        {
            var bankMemory = CreateBankMemory(bankCount: 2, bankSize: 4);

            // Write to first bank, first address
            bankMemory[0] = 42;
            Assert.Equal(42, bankMemory[0]);

            // Write to second bank, first address
            int addr = 4; // 2 banks of size 4, so addr 4 is start of second bank
            bankMemory[addr] = 99;
            Assert.Equal(99, bankMemory[addr]);
        }

        [Fact]
        public void Swap_ChangesBankMapping()
        {
            var bankMemory = CreateBankMemory(bankCount: 2, bankSize: 4);

            // Save original value in bank 0, address 0
            byte original = bankMemory[0];

            // Change value in swap bank 1, address 0
            bankMemory.SwapBanks[1][0] = 123;

            // Swap bank 0 to point to swap bank 1
            bankMemory.Swap(0, 1);

            // Now bank 0 should reflect swap bank 1's value
            Assert.Equal(123, bankMemory[0]);
            Assert.NotEqual(original, bankMemory[0]);
        }

        [Fact]
        public void Swap_NegativeIndex_WrapsCorrectly()
        {
            var bankMemory = CreateBankMemory(bankCount: 2, bankSize: 4);

            // Set a unique value in swap bank 1
            bankMemory.SwapBanks[1][0] = 77;

            // Use negative index to swap (should wrap to index 1)
            bankMemory.Swap(0, -1);

            Assert.Equal(77, bankMemory[0]);
        }

        [Fact]
        public void BankSize_Set_RepartitionsBanks()
        {
            var bankMemory = new BankMemory();

            // Start with 2 swap banks of size 4
            bankMemory.SwapBanks.Add(new byte[] { 1, 2, 3, 4 });
            bankMemory.SwapBanks.Add(new byte[] { 5, 6, 7, 8 });
            bankMemory.Banks.Add(bankMemory.SwapBanks[0]);
            bankMemory.Banks.Add(bankMemory.SwapBanks[1]);

            // Set BankSize to 2 (should split into 4 banks of size 2)
            bankMemory.BankSize = 2;

            Assert.Equal(2, bankMemory.BankSize);
            Assert.Equal(4, bankMemory.SwapBanks.Count);
            Assert.Equal(4, bankMemory.Banks.Count);

            // Check that data is preserved after repartition
            Assert.Equal(1, bankMemory.SwapBanks[0][0]);
            Assert.Equal(2, bankMemory.SwapBanks[0][1]);
            Assert.Equal(3, bankMemory.SwapBanks[1][0]);
            Assert.Equal(4, bankMemory.SwapBanks[1][1]);
            Assert.Equal(5, bankMemory.SwapBanks[2][0]);
            Assert.Equal(6, bankMemory.SwapBanks[2][1]);
            Assert.Equal(7, bankMemory.SwapBanks[3][0]);
            Assert.Equal(8, bankMemory.SwapBanks[3][1]);
        }
    }
}