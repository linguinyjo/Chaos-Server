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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chaos
{
    internal sealed class Merchants : IEnumerable<Merchant>
    {
        public IEnumerator<Merchant> GetEnumerator() => MerchantList.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //access merchant by name
        internal Merchant this[string name] => MerchantList[name];
        private Dictionary<string, Merchant> MerchantList { get; }

        internal Merchants()
        {
            MerchantList = new Dictionary<string, Merchant>()
            {
                {
                    "Deliope", new Merchant("Deliope", 61, CreatureType.Merchant, new Point(15, 10), Game.World.Maps[5031], Direction.South, 0,
                    new List<PursuitIds>() { PursuitIds.Revive })
                },
                {
                    "Celeste", new Merchant("Celeste", 57, CreatureType.Merchant, new Point(3, 16), Game.World.Maps[17500], Direction.South, 0,
                    new List<PursuitIds>() { PursuitIds.Revive, PursuitIds.Summon, PursuitIds.SummonAll, PursuitIds.LouresCitizenship, PursuitIds.KillUser, PursuitIds.Teleport }, MenuType.Menu, "I like giant cock.")
                },
                {
                    "Frank The Great", new Merchant("Frank The Great", 34, CreatureType.Merchant, new Point(5, 2), Game.World.Maps[17501], Direction.East, 7,
                    new List<PursuitIds>() { }, MenuType.Dialog)
                }
            };
        }
    }
}