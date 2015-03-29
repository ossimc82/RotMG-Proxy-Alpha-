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
    public interface IContainer
    {
        Inventory Inventory { get; }
    }

    public class InventoryChangedEventArgs : EventArgs
    {
        public InventoryChangedEventArgs(int index, Item old, Item @new)
        {
            Index = index;
            OldItem = old;
            NewItem = @new;
        }

        public int Index { get; private set; }
        public Item OldItem { get; private set; }
        public Item NewItem { get; private set; }
    }

    public class Inventory : IEnumerable<Item>
    {
        private Item[] items;
        private IContainer parent;

        public Inventory(IContainer parent) : this(parent, new Item[12]) { }
        public Inventory(IContainer parent, Item[] items)
        {
            this.parent = parent;
            this.items = items;
        }

        public IContainer Parent { get; private set; }
        public bool Locked { get; private set; }
        public int Length { get { return items.Length; } }

        public void SetItems(Item[] items)
        {
            this.items = items;
            if (InventoryChanged != null)
                InventoryChanged(this, new InventoryChangedEventArgs(-1, null, null));
        }

        public bool Contains(int objType)
        {
            foreach (var i in this)
                if (i != null && i.ObjectType == (ushort)objType) return true;
            return false;
        }

        public int? GetFirstItemIndex(ushort objType)
        {
            for (int i = 0; i < this.Length; i++)
                if (this[i] != null && this[i].ObjectType == objType) return i;

            return null;
        }

        public Item this[int index]
        {
            get { return items[index]; }
            set
            {
                if (items[index] != value)
                {
                    var e = new InventoryChangedEventArgs(index, items[index], value);
                    items[index] = value;
                    if (InventoryChanged != null)
                        InventoryChanged(this, e);
                }
            }
        }

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        public IEnumerator<Item> GetEnumerator()
        {
            return ((IEnumerable<Item>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
