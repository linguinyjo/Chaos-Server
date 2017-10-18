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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chaos
{
    internal sealed class Exchange
    {
        private readonly object Sync = new object();
        internal Server Server { get; }
        internal int ExchangeId { get; }
        internal User User1 { get; }
        internal User User2 { get; }
        internal List<Item> User1Items { get; set; }
        internal List<Item> User2Items { get; set; }
        private uint User1Gold;
        private uint User2Gold;
        private bool User1Accept { get; set; }
        private bool User2Accept { get; set; }
        private bool IsActive = false;
        internal User OtherUser(User user) => User1 == user ? User2 : User1;

        internal Exchange(User sender, User receiver)
        {
            ExchangeId = Interlocked.Increment(ref Server.NextId);
            User1 = sender;
            User2 = receiver;
            User1Items = new List<Item>();
            User2Items = new List<Item>();
            User1Gold = 0;
            User2Gold = 0;
            Server = sender.Client.Server;
        }

        internal void Activate()
        {
            lock (Sync)
            {
                //set each player's exchange to this
                User1.Exchange = this;
                User2.Exchange = this;

                //active exchange window on each client
                User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.StartExchange, User2.Id, User2.Name));
                User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.StartExchange, User1.Id, User1.Name));

                IsActive = true;
            }
        }

        internal void AddItem(User user, byte slot)
        {
            lock (Sync)
            {
                Item item;
                byte index;
                if (!IsActive || !user.Inventory.TryGet(slot, out item) || item.AccountBound)
                    return;

                if (!item.Stackable)
                {
                    //remove item from their inventory
                    user.Inventory.TryRemove(slot);
                    user.Client.Enqueue(Server.Packets.RemoveItem(slot));

                    bool source = user == User2;
                    //add item to exchange
                    if (!source)
                    {
                        User1Items.Add(item);
                        index = (byte)(User1Items.IndexOf(item) + 1);
                    }
                    else
                    {
                        User2Items.Add(item);
                        index = (byte)(User2Items.IndexOf(item) + 1);
                    }

                    //update exchange window
                    User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.AddItem, source, index, item.SpritePair.Item2, item.Color, item.Name));
                    User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.AddItem, !source, index, item.SpritePair.Item2, item.Color, item.Name));
                }
                else //if it's stackable, send a prompty asking for how many
                    user.Client.Enqueue(Server.Packets.Exchange(ExchangeType.RequestAmount, item.Slot));
            }
        }

        internal void AddStackableItem(User user, byte slot, byte count)
        {
            lock (Sync)
            {
                Item item;
                Item splitItem;
                int index;

                //if slot is null, or not stackable, or invalid count, then return
                if (!IsActive || !user.Inventory.TryGet(slot, out item) || !item.Stackable || count > item.Count || item.AccountBound)
                    return;

                //remove the item if we're exchanging all that we have
                if (item.Count == count)
                {
                    user.Inventory.TryGetRemove(slot, out splitItem);
                    user.Client.Enqueue(Server.Packets.RemoveItem(slot));
                }
                else
                {
                    //otherwise split the item stack and update the user's inventory
                    splitItem = item.Split(count);
                    user.Client.Enqueue(Server.Packets.AddItem(item));
                }

                //depending on which user is activating this, do different things
                if (user == User1)
                {
                    //if there's a stackable item with this name already, grab it
                    Item oldItem = User1Items.FirstOrDefault(itm => itm.Name.Equals(splitItem.Name));

                    //if it was successfully grabbed
                    if (oldItem != null)
                    {
                        //combine what we took from the inventory with the old amount
                        splitItem.Count += oldItem.Count;
                        //set the old item as the new item with the new count
                        User1Items[User1Items.IndexOf(oldItem)] = splitItem;
                    }
                    else
                        //if no old item, just add the new item stack
                        User1Items.Add(splitItem);

                    //index of this item inthe trade is it's index in the item list + 1 (cuz no zero index)
                    index = User1Items.IndexOf(splitItem) + 1;
                }
                else
                {
                    //do the same thing for the other use, using it's lists
                    Item oldItem = User2Items.FirstOrDefault(itm => itm.Name.Equals(splitItem.Name));

                    if (oldItem != null)
                    {
                        splitItem.Count += oldItem.Count;
                        User2Items[User2Items.IndexOf(oldItem)] = splitItem;
                    }
                    else
                        User2Items.Add(splitItem);

                    index = User2Items.IndexOf(splitItem) + 1;
                }

                //update exchange window
                bool source = user == User2;
                User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.AddItem, source, (byte)index, splitItem.SpritePair.Item2, splitItem.Color, $@"{splitItem.Name}[{splitItem.Count}]"));
                User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.AddItem, !source, (byte)index, splitItem.SpritePair.Item2, splitItem.Color, $@"{splitItem.Name}[{splitItem.Count}]"));
            }
        }

        internal void SetGold(User user, uint amount)
        {
            lock (Sync)
            {
                if (!IsActive)
                    return;

                //if the user already had gold entered, give it back (because this is a set, not an addition)
                user.Attributes.Gold += user == User1 ? User1Gold : User2Gold;

                //if the amount they want to set is greater than what they have, return
                if (amount > user.Attributes.Gold)
                    return;

                //do things depending on which user is requesting
                if (User1 == user)
                {
                    //subtract the gold we want to add from the user
                    User1.Attributes.Gold -= amount;
                    //set the gold in the exchange
                    User1Gold = amount;
                    //update the user's gold
                    User1.Client.SendAttributes(StatUpdateFlags.ExpGold);
                }
                else
                {
                    //do same thing for other user
                    User2.Attributes.Gold -= amount;
                    User2Gold = amount;
                    User2.Client.SendAttributes(StatUpdateFlags.ExpGold);
                }

                //update exchange window
                bool source = user == User2;
                User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.SetGold, source, source ? User1Gold : User2Gold));
                User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.SetGold, !source, !source ? User1Gold : User2Gold));
            }
        }

        internal void Cancel(User user)
        {
            lock(Sync)
            {
                if (!IsActive)
                    return;

                //give the items and gold back to their owners
                User1.Attributes.Gold += User1Gold;
                User1.Client.SendAttributes(StatUpdateFlags.ExpGold);
                foreach (Item item in User1Items)
                {
                    User1.Inventory.TryAdd(item);
                    User1.Client.Enqueue(Server.Packets.AddItem(item));
                }

                User2.Attributes.Gold += User2Gold;
                User2.Client.SendAttributes(StatUpdateFlags.ExpGold);
                foreach (Item item in User2Items)
                {
                    User2.Inventory.TryAdd(item);
                    User2.Client.Enqueue(Server.Packets.AddItem(item));
                }

                //send cancel packet to close the exchange
                bool source = user == User2;
                User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Cancel, source));
                User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Cancel, !source));

                //destroy the exchange object from the server
                Destroy();
            }
        }

        internal void Accept(User user)
        {
            lock(Sync)
            {
                if (!IsActive)
                    return;

                //keep track of which user has hit accept
                if (user == User1)
                    User1Accept = true;
                else
                    User2Accept = true;

                bool source = user == User2;
                //only send the opposite user a false-sourced accept packet (accept button on other side)
                if (source)
                    User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Accept, false));
                else
                    User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Accept, false));

                //if both players accepted, give eachother the items and gold in eachother's lists
                if (User1Accept && User2Accept)
                {
                    User2.Attributes.Gold += User1Gold;
                    User2.Client.SendAttributes(StatUpdateFlags.ExpGold);
                    foreach (Item item in User1Items)
                    {
                        User2.Inventory.AddToNextSlot(item);
                        User2.Client.Enqueue(Server.Packets.AddItem(item));
                    }

                    User1.Attributes.Gold += User2Gold;
                    User1.Client.SendAttributes(StatUpdateFlags.ExpGold);
                    foreach (Item item in User2Items)
                    {
                        User1.Inventory.AddToNextSlot(item);
                        User1.Client.Enqueue(Server.Packets.AddItem(item));
                    }

                    //update exchange window (to close it)
                    User1.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Accept, true));
                    User2.Client.Enqueue(Server.Packets.Exchange(ExchangeType.Accept, true));

                    //destroy the exchange object from the server
                    Destroy();
                }
            }
        }

        private void Destroy()
        {
            //remove the exchange from existence
            User1.Exchange = null;
            User2.Exchange = null;
            Exchange exOut;
            Game.World.Exchanges.TryRemove(ExchangeId, out exOut);
            exOut = null;
            IsActive = false;
        }
    }
}
