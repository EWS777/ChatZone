using System.Collections.Concurrent;

namespace ChatZone.Core.Services;

public class ChatGroupStore
{
    private static readonly ConcurrentDictionary<string, string> UsersGroups = new();
    private static readonly ConcurrentDictionary<string, bool> IsTypeOfChatSingle = new();

    public static string? GetPersonGroup(string username)
    {
        return UsersGroups.GetValueOrDefault(username);
    }

    public static bool IsSingleChat(string groupName)
    {
        return IsTypeOfChatSingle.FirstOrDefault(x => x.Key == groupName).Value;
    }

    public static void RemovePersonFromGroup(string username)
    {
        UsersGroups.TryGetValue(username, out var groupName);
    }

    public static string GetSecondPersonInSingleChat(string groupName, string firstUsername)
    {
        return UsersGroups.FirstOrDefault(x => x.Value == groupName && x.Key != firstUsername).Key;
    }

    public static void AddPersonToGroup(string username, string groupName)
    {
        UsersGroups.TryAdd(username, groupName);
    }

    public static void AddTypeOfGroup(string groupName, bool isSingleChat)
    {
        IsTypeOfChatSingle.TryAdd(groupName, isSingleChat);
    }
}