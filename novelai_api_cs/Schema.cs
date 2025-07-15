using System.Text.Json.Serialization;

enum ImageModels
{
  [JsonPropertyName("nai-diffusion-3")]
  AnimeV3 = 1,
  [JsonPropertyName("nai-diffusion-3-furry")]
  FurryV3 = 2,
  [JsonPropertyName("nai-diffusion-4-curated")]
  AnimeV4Curated = 3,
  [JsonPropertyName("nai-diffusion-4-full")]
  AnimeV4 = 0,
  [JsonPropertyName("nai-diffusion-45-curated")]
  AnimeV4_5Curated = 4,
  [JsonPropertyName("nai-diffusion-45-full")]
  AnimeV4_5 = 5,
}

enum Sampler
{
  [JsonPropertyName("k_euler")]
  Euler = 1,
  [JsonPropertyName("k_euler_ancestral")]
  EulerAncestral = 0,
  [JsonPropertyName("k_dpmpp_2m")]
  Dpmpp2m = 2,
  [JsonPropertyName("k_dpmpp_2m_sde")]
  Dpmpp2mSde = 3,
  [JsonPropertyName("k_dpmpp_2s_ancestral")]
  Dpmpp2sAncestral = 4,
  [JsonPropertyName("k_dpmpp_sde")]
  DpmppSde = 5,
  [JsonPropertyName("k_dpmpp_2")]
  Dpm2 = 6,
  [JsonPropertyName("k_dpmpp_2_ancestral")]
  Dpm2Ancestral = 7,
}

enum Noise
{
  [JsonPropertyName("kerras")]
  Kerras = 0,
  [JsonPropertyName("native")]
  Native = 1,
  [JsonPropertyName("exponential")]
  Exponential = 2,
  [JsonPropertyName("polyexponential")]
  PolyExponential = 3,
}

class Caption
{
  public required string BaseCaption { get; set; }
  public required string[] CharCaptions { get; set; }
}

class V4Prompt
{
  public required Caption Caption { get; set; }
  public required bool UseCoords { get; set; }
  public required bool UseOrder { get; set; }
  public required bool LegacyUc { get; set; }
}

class ImageParameters
{
  public string? NegativePrompt { get; set; }
  public int? Samples { get; set; }
  public Sampler? Sampler { get; set; }
  public int? Steps { get; set; }
  public float? Scale { get; set; }
  public float? CfgScale { get; set; }
  public int? Seed { get; set; }
  public bool? QualityToggle { get; set; }
  public int? UcPreset { get; set; }
  public V4Prompt? V4Prompt { get; set; }
  public V4Prompt? V4NegativePrompt { get; set; }
  public float? UncondScale { get; set; }
  public int? Width { get; set; }
  public int? Height { get; set; }
  public float? ControlnetStrength { get; set; }
  public bool? DeliberateEulerAncestralBug { get; set; }
  public bool? PreferBrownian { get; set; }
}

class ImageConfig
{
  public required string Input { get; set; }
  public required string Prompt { get; set; }
  public required string Parameters { get; set; }
}