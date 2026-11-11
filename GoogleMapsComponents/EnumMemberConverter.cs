using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents;

internal class EnumMemberConverter<T> : JsonConverter<T> where T : IComparable, IFormattable, IConvertible
{
    // The enum members are read with reflection once per enum type, not for every value that is (de)serialized.
    // Only the members that have an EnumMember attribute are kept, in declaration order.
    private static readonly (string Name, string? MemberValue, T Value)[] Members = GetMembers();

    private static readonly Dictionary<string, string?> MemberValuesByName = GetMemberValuesByName();

    private static (string Name, string? MemberValue, T Value)[] GetMembers()
    {
        var members = new List<(string Name, string? MemberValue, T Value)>();

        foreach (var fi in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var description = (EnumMemberAttribute?)fi.GetCustomAttribute(typeof(EnumMemberAttribute), false);

            if (description != null)
            {
                members.Add((fi.Name, description.Value, (T)fi.GetValue(null)!));
            }
        }

        return members.ToArray();
    }

    private static Dictionary<string, string?> GetMemberValuesByName()
    {
        var memberValues = new Dictionary<string, string?>(Members.Length);

        foreach (var member in Members)
        {
            memberValues[member.Name] = member.MemberValue;
        }

        return memberValues;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonValue = reader.GetString();

        foreach (var member in Members)
        {
            if (string.Equals(member.MemberValue, jsonValue, StringComparison.OrdinalIgnoreCase))
            {
                return member.Value;
            }
        }

        throw new JsonException($"string {jsonValue} was not found as a description in the enum {typeToConvert}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        MemberValuesByName.TryGetValue(value.ToString() ?? string.Empty, out var memberValue);

        writer.WriteStringValue(memberValue);
    }
}