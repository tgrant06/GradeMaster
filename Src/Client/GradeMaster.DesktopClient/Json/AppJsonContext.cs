using System.Text.Json.Serialization;

namespace GradeMaster.DesktopClient.Json;

/// <summary>
/// JsonSerializerContext for serializing and deserializing <see cref="AppPreferencesObject"/>.
/// Source generated.
/// </summary>
#if DEBUG
[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default)]
#else
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default)]
#endif
[JsonSerializable(typeof(AppPreferencesObject))]
internal partial class AppJsonContext : JsonSerializerContext;
