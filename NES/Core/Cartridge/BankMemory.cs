﻿using System;
using System.Collections.Generic;

namespace NEStor.Core.Cartridge
{
    public interface IBankMemory
    {
        byte this[int addr] { get; set; }
        int BankSize { get; set; }
        void Swap(int bankIdx, int swapBankIdx);
        //void SwapMasked(int bankIdx, int swapBankIdx);
    }

    class BankMemory: IBankMemory
    {
        public List<byte[]> Banks = new List<byte[]>();
        public List<byte[]> SwapBanks = new List<byte[]>();
        int bankSize;
        int sizeBits;
        int sizeMask;
        public int BankSize
        {
            get { return bankSize; }
            set
            {
                bankSize = SwapBanks[0].Length;
                var expanded = new byte[SwapBanks.Count * bankSize];
                for (int i = 0; i < SwapBanks.Count; i++)
                    Array.Copy(SwapBanks[i], 0, expanded, i * bankSize, bankSize);
                bankSize = Math.Min(value, expanded.Length);
                var bankCount = expanded.Length / bankSize;
                SwapBanks.Clear();
                for (int i = 0; i < bankCount; i++)
                {
                    SwapBanks.Add(new byte[bankSize]);
                    Array.Copy(expanded, i * bankSize, SwapBanks[i], 0, bankSize);
                }
                bankCount = Banks.Count * Banks[0].Length / bankSize;
                Banks.Clear();
                for (int i = 0; i < bankCount; i++)
                    Banks.Add(SwapBanks[i % SwapBanks.Count]);
                sizeBits = 0;
                while (value > 1)
                {
                    value >>= 1;
                    sizeBits++;
                }
                sizeMask = bankSize - 1;
            }
        }

        public byte this[int addr]
        {
            get { return Banks[addr >> sizeBits][addr & sizeMask]; }
            set { Banks[addr >> sizeBits][addr & sizeMask] = value; }
            //get { return Banks[addr / bankSize][addr % bankSize]; }
            //set { Banks[addr / bankSize][addr % bankSize] = value; }
        }

        public void Swap(int bankIdx, int swapBankIdx)
        {
            if (swapBankIdx < 0) swapBankIdx += SwapBanks.Count;
            Banks[bankIdx] = SwapBanks[swapBankIdx % SwapBanks.Count];
        }

        //public void SwapMasked(int bankIdx, int swapBankIdx)
        //{
        //    var mask = Masks[bankIdx];
        //    if (mask != 0) {
        //        var shift = 0;
        //        while ((mask >> shift & 1) == 0) shift++;
        //        swapBankIdx = (swapBankIdx & mask) >> shift;
        //        if (swapBankIdx < 0) swapBankIdx += SwapBanks.Count;
        //        swapBankIdx %= SwapBanks.Count;
        //        if (DoubleSized)
        //        {
        //            Banks[bankIdx << 1] = SwapBanks[swapBankIdx];
        //            Banks[bankIdx << 1 | 1] = SwapBanks[swapBankIdx + 1];
        //        }
        //        else
        //            Banks[bankIdx] = SwapBanks[swapBankIdx];
        //    }
        //}
    }
}
