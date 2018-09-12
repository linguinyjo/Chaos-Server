// ****************************************************************************
// This file belongs to the Chaos-Server project.
// 
// This project is free and open-source, provided that any alterations or
// modifications to any portions of this project adhere to the
// Affero General Public License (Version 3).
// 
// A copy of the AGPLv3 can be found in the project directory.
// You may also find a copy at <https://www.gnu.org/licenses/agpl-3.0.html>
// ****************************************************************************

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chaos
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class Panel<T> : IEnumerable<T> where T : PanelObject
    {
        private readonly object Sync = new object();
        [JsonProperty]
        private Dictionary<byte, T> Objects;
        [JsonProperty]
        private readonly byte[] Invalid;
        [JsonProperty]
        private readonly byte length;

        internal byte Length => (byte)(length - 1);
        internal T this[EquipmentSlot slot] => this[(byte)slot];
        internal T this[string name] => Objects.Values.FirstOrDefault(obj => obj.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> safeEnum = Objects.Values.GetEnumerator();

            lock (Sync)
                while (safeEnum.MoveNext())
                    yield return safeEnum.Current;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        internal T this[byte slot]
        {
            get => Valid(slot) ? Objects[slot] : null;
            private set
            {
                if (value.Slot == slot && Valid(slot))
                    Objects[slot] = value;
            }
        }

        /// <summary>
        /// Base constructor for an object representing an in-game panel. Skill/Spell/Inventory/Equipment
        /// </summary>
        /// <param name="length">The number of objects that can fit in the panel.</param>
        internal Panel(byte length)
        {
            this.length = length;
            Objects = new Dictionary<byte, T>();
            for (byte i = 0; i < length; i++)
                Objects[i] = null;

            if (length == 90) //skillbook and spellbook
                Invalid = new byte[] { 36, 72 };
            else if (length == 61) //inventory
                Invalid = new byte[] { };
            else if (length == 20) //equipment
                Invalid = new byte[] { };
        }

        /// <summary>
        /// Json & Master constructor for an object representing an in-game panel.
        /// </summary>
        [JsonConstructor]
        internal Panel(byte length, Dictionary<byte, T> objects, byte[] invalid)
        {
            this.length = length;
            Objects = objects;
            Invalid = invalid;
        }

        /// <summary>
        /// Synchronously checks if the panel contains an object.
        /// </summary>
        /// <param name="obj">Object to check if the panel contains</param>
        internal bool Contains(T obj)
        {
            lock (Sync)
                return Objects.Values.Contains(obj);
        }

        /// <summary>
        /// Synchronously checks of the panel is full.
        /// </summary>
        internal bool IsFull
        {
            get
            {
                lock (Sync)
                    return !Objects.Any(kvp => Valid(kvp.Key) && kvp.Value == null);
            }
        }

        /// <summary>
        /// A list of valid slots within the given panel.
        /// </summary>
        /// <param name="slot">Slot to check.</param>
        private bool Valid(byte slot) => slot > 0 && !Invalid.Contains(slot) && slot < Length;

        /// <summary>
        /// Attempts to synchronously add a stackable item.
        /// </summary>
        /// <param name="item">Object to try adding.</param>
        internal bool TryAddStack(T item)
        {
            lock (Sync)
            {
                if (item is Item tItem)
                {
                    Item existingItem = Objects.Values.OfType<Item>().FirstOrDefault(obj => obj.Sprite == tItem.ItemSprite.InventorySprite && obj.Name.Equals(tItem.Name) && obj.Stackable == true);
                    if (tItem.Stackable && existingItem?.Stackable == true)
                    {
                        tItem.Count += existingItem.Count;
                        tItem.Slot = existingItem.Slot;
                        Objects[existingItem.Slot] = item;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Attempts to synchronously equip the given item. Sends an item out if an item was replaced in the slot.
        /// </summary>
        /// <param name="item">The item to try equiping.</param>
        /// <param name="outItem">The object you were previously wearing in that slot.</param>
        /// <returns></returns>
        internal bool TryEquip(T item, out T outItem)
        {
            lock (Sync)
            {
                if (item is Item tItem)
                {
                    outItem = null;
                    EquipmentSlot slot = tItem.EquipmentSlot;

                    if (slot == EquipmentSlot.None || !Valid((byte)slot))
                    {
                        outItem = null;
                        return false;
                    }

                    if (!(Objects[(byte)slot] != null && !TryUnequip(slot, out outItem)))
                    {
                        item.Slot = (byte)slot;
                        TryAdd(item);
                    }

                    return Objects[(byte)slot] == item;
                }
                outItem = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to synchronously unequip an item and return it.
        /// </summary>
        /// <param name="slot">The equipment slot to remove from.</param>
        /// <param name="item">The item returns by unequipping it.</param>
        /// <returns></returns>
        internal bool TryUnequip(EquipmentSlot slot, out T item)
        {
            lock (Sync)
                return TryGetRemove((byte)slot, out item);
        }

        /// <summary>
        /// Attempts to synchronously add an object.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        internal bool TryAdd(T obj)
        {
            lock (Sync)
            {
                if (TryAddStack(obj))
                    return Objects[obj.Slot] == obj;

                if (Objects[obj.Slot] == null)
                    this[obj.Slot] = obj;

                return Objects[obj.Slot] == obj;
            }
        }

        /// <summary>
        /// Attempts to synchronously add an object to the next available slot. Handles stackable items.
        /// </summary>
        /// <param name="obj">Object to add</param>
        internal bool AddToNextSlot(T obj)
        {
            lock (Sync)
            {
                if (TryAddStack(obj))
                    return Objects[obj.Slot] == obj;

                foreach (byte key in Objects.Keys)
                {
                    if (!Valid(key))
                        continue;

                    obj.Slot = key;
                    if (TryAdd(obj))
                        return true;
                }

                return Objects[obj.Slot] == obj;
            }
        }

        /// <summary>
        /// Attempts to synchronously remove an object from the panel. Sets the value to null.
        /// </summary>
        /// <param name="slot">Slot to remove.</param>
        internal bool TryRemove(byte slot)
        {
            lock (Sync)
            {
                if (Valid(slot))
                    Objects[slot] = null;

                return Objects[slot] == null;
            }
        }

        /// <summary>
        /// Attempts to synchronously remove an object and return it.
        /// </summary>
        /// <param name="slot">Slot to remove.</param>
        /// <param name="obj">Return object if successful.</param>
        internal bool TryGetRemove(byte slot, out T obj)
        {
            lock (Sync)
            {
                if (TryGet(slot, out obj) && TryRemove(slot))
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Attempts to synchronously return a reference to an existing object.
        /// </summary>
        /// <param name="slot">Slot to retreive from.</param>
        /// <param name="obj">Obj reference to set.</param>
        internal bool TryGet(byte slot, out T obj)
        {
            lock (Sync)
            {
                obj = Objects[slot];
                return Valid(slot) && obj != null;
            }
        }

        /// <summary>
        /// Attempts to synchronously swap two items slots.
        /// If it fails, items will be put back.
        /// </summary>
        /// <param name="slot1">First slot to swap.</param>
        /// <param name="slot2">Second slot to swap.</param>
        internal bool TrySwap(byte slot1, byte slot2)
        {
            lock (Sync)
            {
                if (TryGetRemove(slot1, out T one))
                {
                    TryGetRemove(slot2, out T two);
                    if (one != null)
                        one.Slot = slot2;
                    if (two != null)
                        two.Slot = slot1;

                    if (one != null)
                        Objects[slot2] = one;
                    if (two != null)
                        Objects[slot1] = two;

                    return true;
                } //puts the first object back if it succeeded on the first operation, but failed on the second
                else if (one != null)
                    TryAdd(one);

                return false;
            }
        }
    }
}
