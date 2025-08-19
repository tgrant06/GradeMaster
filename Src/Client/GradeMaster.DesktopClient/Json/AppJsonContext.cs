using System.Text.Json.Serialization;

namespace GradeMaster.DesktopClient.Json;

/// <summary>
/// JsonSerializerContext for serializing and deserializing <see cref="AppPreferencesObject"/>.
/// Source generated.
/// </summary>
[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(AppPreferencesObject))]
internal partial class AppJsonContext : JsonSerializerContext;
