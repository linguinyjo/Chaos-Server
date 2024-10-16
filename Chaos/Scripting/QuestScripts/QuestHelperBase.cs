using Chaos.Common.Definitions;
using Chaos.Models.World;

namespace Chaos.Scripting.QuestScripts;

// public abstract class QuestHelper<TQuestStatus> where TQuestStatus : struct, Enum
// {
//     public static TQuestStatus GetQuestStatus(Aisling player)
//     {
//         return player.Trackers.Enums.TryGetValue<TQuestStatus>(out var status) ? status : default;
//     }
//
//     public bool IsQuestAvailable(Aisling player)
//     {
//         return player.HasClass(BaseClass.Peasant) && !Equals(GetQuestStatus(player), GetCompletedStatus());
//     }
//
//     public static void IncrementQuestStage(Aisling player)
//     {
//         var questStatus = GetQuestStatus(player);
//
//         if (Equals(questStatus, GetCompletedStatus()))
//             return;
//
//         int nextStatus = Convert.ToInt32(questStatus) + 1;
//         player.Trackers.Enums.Set((TQuestStatus)Enum.ToObject(typeof(TQuestStatus), nextStatus));
//     }
//     
//     // Abstract methods to be implemented in the derived classes
//     public virtual void StartQuest(Aisling player) {}
//     public virtual void CompleteQuest(Aisling player) {}
//     protected abstract TQuestStatus GetCompletedStatus();
//     protected abstract TQuestStatus GetInitialStatus();
// }
