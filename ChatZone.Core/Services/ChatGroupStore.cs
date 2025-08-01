using System.Collections.Concurrent;

namespace ChatZone.Core.Services;

public class ChatGroupStore
{
    private static readonly ConcurrentDictionary<int, int> UsersGroups = new();
    private static readonly ConcurrentDictionary<int, bool> IsTypeOfChatSingle = new();

    public static int? GetPersonGroup(int idPerson)
    {
        return UsersGroups.GetValueOrDefault(idPerson);
    }

    public static bool IsSingleChat(int? idGroup)
    {
        return IsTypeOfChatSingle.FirstOrDefault(x => x.Key == idGroup).Value;
    }

    public static void RemovePersonFromGroup(int idPerson)
    {
        UsersGroups.TryGetValue(idPerson, out _);
    }

    public static int GetSecondPersonInSingleChat(int? idGroup, int firstIdPerson)
    {
        return UsersGroups.FirstOrDefault(x => x.Value == idGroup && x.Key != firstIdPerson).Key;
    }

    public static void AddPersonToGroup(int idPerson, int idGroup)
    {
        UsersGroups.TryAdd(idPerson, idGroup);
    }

    public static void AddTypeOfGroup(int idGroup, bool isSingleChat)
    {
        IsTypeOfChatSingle.TryAdd(idGroup, isSingleChat);
    }
}