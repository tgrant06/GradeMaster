using System.Text.Json.Serialization;

namespace GradeMaster.DesktopClient.Json;

/// <summary>
/// JsonSerializerContext for serializing and deserializing ULIDs.
/// Source generated.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(AppPreferencesObject))]
internal partial class AppJsonContext : JsonSerializerContext;
