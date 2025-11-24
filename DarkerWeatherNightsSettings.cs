using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DarkerWeatherNights;

public sealed class DarkerWeatherNightsSettings
{
    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double CloudColorNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    public double SkyUpperNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    public double FogNearNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.25)]
    public double AmbientNightMultiplier { get; set; } = 0.25;

    [Range(0, 1)]
    [DefaultValue(0.25)]
    public double SunlightNightMultiplier { get; set; } = 0.25;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    public double SkyLowerNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    public double HorizonNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(1.0)]
    public double EffectLightingNightMultiplier { get; set; } = 1.0;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    public double FogFarNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double SkyStaticsNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double WaterMultiplierNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double MoonGlareNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double AmbientSidesNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double AmbientDownNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double AmbientUpNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    public double AmbientSpecularNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.10)]
    public double VolumetricLightingNightMultiplier { get; set; } = 0.10;

    public void Validate()
    {
        ValidateRange(CloudColorNightMultiplier, nameof(CloudColorNightMultiplier));
        ValidateRange(SkyUpperNightMultiplier, nameof(SkyUpperNightMultiplier));
        ValidateRange(FogNearNightMultiplier, nameof(FogNearNightMultiplier));
        ValidateRange(AmbientNightMultiplier, nameof(AmbientNightMultiplier));
        ValidateRange(SunlightNightMultiplier, nameof(SunlightNightMultiplier));
        ValidateRange(SkyLowerNightMultiplier, nameof(SkyLowerNightMultiplier));
        ValidateRange(HorizonNightMultiplier, nameof(HorizonNightMultiplier));
        ValidateRange(EffectLightingNightMultiplier, nameof(EffectLightingNightMultiplier));
        ValidateRange(FogFarNightMultiplier, nameof(FogFarNightMultiplier));
        ValidateRange(SkyStaticsNightMultiplier, nameof(SkyStaticsNightMultiplier));
        ValidateRange(WaterMultiplierNightMultiplier, nameof(WaterMultiplierNightMultiplier));
        ValidateRange(MoonGlareNightMultiplier, nameof(MoonGlareNightMultiplier));
        ValidateRange(AmbientSidesNightMultiplier, nameof(AmbientSidesNightMultiplier));
        ValidateRange(AmbientDownNightMultiplier, nameof(AmbientDownNightMultiplier));
        ValidateRange(AmbientUpNightMultiplier, nameof(AmbientUpNightMultiplier));
        ValidateRange(AmbientSpecularNightMultiplier, nameof(AmbientSpecularNightMultiplier));
        ValidateRange(VolumetricLightingNightMultiplier, nameof(VolumetricLightingNightMultiplier));
    }

    private static void ValidateRange(double value, string name)
    {
        if (value < 0d || value > 1d)
        {
            throw new ValidationException($"{name} must be between 0 and 1 (inclusive).");
        }
    }
}
