//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealmManager.entities
{
    public struct Stats : IEnumerable<int>
    {
        private int[] stats;

        public Stats(int len)
        {
            stats = new int[len];
        }

        public int this[Stat index]
        {
            get { return this[(int)index]; }
            set { this[(int)index] = value; }
        }

        public int this[int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
    
        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)stats).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return stats.GetEnumerator();
        }
    }

    public enum Stat : int
    {
        MAX_HP = 0,
        MAX_MP = 1,
        HP = 0,
        MP = 1,
        ATT = 2,
        DEF = 3,
        SPD = 4,
        VIT = 5,
        WIS = 6,
        DEX = 7
    }
}
