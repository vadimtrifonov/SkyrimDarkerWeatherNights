using System;
using System.Collections.Generic;
using System.Drawing;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace DarkerWeatherNights;

internal sealed class WeatherProcessor
{
    private readonly IPatcherState<ISkyrimMod, ISkyrimModGetter> _state;
    private readonly DarkerWeatherNightsSettings _settings;
    private readonly HashSet<FormKey> _scaledVolumetricLights = new();

    private int _colorChanges;
    private int _weatherChanges;

    public WeatherProcessor(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        DarkerWeatherNightsSettings settings)
    {
        _state = state;
        _settings = settings;
    }

    public void Run()
    {
        foreach (var weatherGetter in _state.LoadOrder.PriorityOrder.Weather().WinningOverrides())
        {
            var patchWeather = _state.PatchMod.Weathers.GetOrAddAsOverride(weatherGetter);
            var beforeColors = _colorChanges;
            var recordChanged = false;

            recordChanged |= ScaleCloudLayers(weatherGetter, patchWeather);
            recordChanged |= ScaleNam0WeatherColors(weatherGetter, patchWeather);
            recordChanged |= ScaleDirectionalAmbientLighting(weatherGetter, patchWeather);
            recordChanged |= ScaleVolumetricLighting(weatherGetter);

            if (recordChanged || beforeColors != _colorChanges)
            {
                _weatherChanges++;
            }
        }

        Console.WriteLine(
            $"Darker Weather Nights: Weather records changed: {_weatherChanges}. Color fields adjusted: {_colorChanges}.");
    }

    private bool ScaleCloudLayers(IWeatherGetter source, Weather destination)
    {
        var srcClouds = source.Clouds;
        var dstClouds = destination.Clouds;
        var count = Math.Min(srcClouds.Count, dstClouds.Length);
        var changed = false;

        for (var i = 0; i < count; i++)
        {
            var srcLayer = srcClouds[i];
            var dstLayer = dstClouds[i];
            if (srcLayer?.Colors == null || dstLayer?.Colors == null)
            {
                continue;
            }

            if (ScaleWeatherColorNight(srcLayer.Colors, dstLayer.Colors, _settings.CloudColorNightMultiplier))
            {
                changed = true;
            }
        }

        return changed;
    }

    private bool ScaleNam0WeatherColors(IWeatherGetter source, Weather destination)
    {
        var changed = false;

        changed |= ScaleWeatherColorNight(source.SkyUpperColor, destination.SkyUpperColor, _settings.SkyUpperNightMultiplier);
        changed |= ScaleWeatherColorNight(source.FogNearColor, destination.FogNearColor, _settings.FogNearNightMultiplier);
        changed |= ScaleWeatherColorNight(source.AmbientColor, destination.AmbientColor, _settings.AmbientNightMultiplier);
        changed |= ScaleWeatherColorNight(source.SunlightColor, destination.SunlightColor, _settings.SunlightNightMultiplier);
        changed |= ScaleWeatherColorNight(source.SkyLowerColor, destination.SkyLowerColor, _settings.SkyLowerNightMultiplier);
        changed |= ScaleWeatherColorNight(source.HorizonColor, destination.HorizonColor, _settings.HorizonNightMultiplier);
        changed |= ScaleWeatherColorNight(source.EffectLightingColor, destination.EffectLightingColor, _settings.EffectLightingNightMultiplier);
        changed |= ScaleWeatherColorNight(source.FogFarColor, destination.FogFarColor, _settings.FogFarNightMultiplier);
        changed |= ScaleWeatherColorNight(source.SkyStaticsColor, destination.SkyStaticsColor, _settings.SkyStaticsNightMultiplier);
        changed |= ScaleWeatherColorNight(source.WaterMultiplierColor, destination.WaterMultiplierColor, _settings.WaterMultiplierNightMultiplier);
        changed |= ScaleWeatherColorNight(source.MoonGlareColor, destination.MoonGlareColor, _settings.MoonGlareNightMultiplier);

        return changed;
    }

    private bool ScaleDirectionalAmbientLighting(IWeatherGetter source, Weather destination)
    {
        var srcColors = source.DirectionalAmbientLightingColors;
        var dstColors = destination.DirectionalAmbientLightingColors;
        if (srcColors == null || dstColors == null)
        {
            return false;
        }

        var srcNight = srcColors.Night;
        var dstNight = dstColors.Night;
        if (srcNight == null || dstNight == null)
        {
            return false;
        }

        var changed = false;
        changed |= ScaleColorByFactor(
            srcNight.DirectionalXPlus,
            dstNight.DirectionalXPlus,
            _settings.AmbientSidesNightMultiplier,
            color => dstNight.DirectionalXPlus = color);
        changed |= ScaleColorByFactor(
            srcNight.DirectionalXMinus,
            dstNight.DirectionalXMinus,
            _settings.AmbientSidesNightMultiplier,
            color => dstNight.DirectionalXMinus = color);
        changed |= ScaleColorByFactor(
            srcNight.DirectionalYPlus,
            dstNight.DirectionalYPlus,
            _settings.AmbientSidesNightMultiplier,
            color => dstNight.DirectionalYPlus = color);
        changed |= ScaleColorByFactor(
            srcNight.DirectionalYMinus,
            dstNight.DirectionalYMinus,
            _settings.AmbientSidesNightMultiplier,
            color => dstNight.DirectionalYMinus = color);
        changed |= ScaleColorByFactor(
            srcNight.DirectionalZMinus,
            dstNight.DirectionalZMinus,
            _settings.AmbientDownNightMultiplier,
            color => dstNight.DirectionalZMinus = color);
        changed |= ScaleColorByFactor(
            srcNight.DirectionalZPlus,
            dstNight.DirectionalZPlus,
            _settings.AmbientUpNightMultiplier,
            color => dstNight.DirectionalZPlus = color);
        changed |= ScaleColorByFactor(
            srcNight.Specular,
            dstNight.Specular,
            _settings.AmbientSpecularNightMultiplier,
            color => dstNight.Specular = color);

        return changed;
    }

    private bool ScaleVolumetricLighting(IWeatherGetter source)
    {
        var nightLink = source.VolumetricLighting?.Night;
        if (nightLink == null)
        {
            return false;
        }

        if (_scaledVolumetricLights.Contains(nightLink.FormKey))
        {
            return false;
        }

        var baseline = nightLink.TryResolve(_state.LinkCache);
        if (baseline == null)
        {
            return false;
        }

        if (!TryGetVolumetricColorChannels(baseline, out var r, out var g, out var b))
        {
            return false;
        }

        if (IsAchromatic(r, g, b))
        {
            return false;
        }

        var scaledR = ScaleChannel(r, _settings.VolumetricLightingNightMultiplier);
        var scaledG = ScaleChannel(g, _settings.VolumetricLightingNightMultiplier);
        var scaledB = ScaleChannel(b, _settings.VolumetricLightingNightMultiplier);

        if (scaledR == r && scaledG == g && scaledB == b)
        {
            return false;
        }

        var overrideRecord = _state.PatchMod.VolumetricLightings.GetOrAddAsOverride(baseline);
        overrideRecord.ColorR = scaledR;
        overrideRecord.ColorG = scaledG;
        overrideRecord.ColorB = scaledB;

        _scaledVolumetricLights.Add(nightLink.FormKey);
        _colorChanges++;
        return true;
    }

    private bool ScaleWeatherColorNight(IWeatherColorGetter? source, WeatherColor? destination, double multiplier)
    {
        if (source == null || destination == null)
        {
            return false;
        }

        return ScaleColorByFactor(source.Night, destination.Night, multiplier, color => destination.Night = color);
    }

    private bool ScaleColorByFactor(Color baseline, Color current, double multiplier, Action<Color> apply)
    {
        if (!HasColorData(baseline))
        {
            return false;
        }

        var scaled = ScaleColor(baseline, multiplier);
        if (ColorsEqual(scaled, current))
        {
            return false;
        }

        apply(scaled);
        _colorChanges++;
        return true;
    }

    private static Color ScaleColor(Color baseline, double multiplier)
    {
        var r = ScaleChannel(baseline.R, multiplier);
        var g = ScaleChannel(baseline.G, multiplier);
        var b = ScaleChannel(baseline.B, multiplier);
        return Color.FromArgb(baseline.A, r, g, b);
    }

    private static byte ScaleChannel(byte value, double multiplier)
    {
        var scaled = value * multiplier;
        return ClampByte(scaled);
    }

    private static byte ClampByte(double value)
    {
        return (byte)Math.Clamp((int)Math.Round(value, MidpointRounding.AwayFromZero), 0, 255);
    }

    private static bool HasColorData(Color color)
    {
        if (color.IsEmpty)
        {
            return false;
        }

        var isBlack = color.R == 0 && color.G == 0 && color.B == 0;
        var isWhite = color.R == 255 && color.G == 255 && color.B == 255;
        return !(isBlack || isWhite);
    }

    private static bool ColorsEqual(Color left, Color right)
    {
        return left.ToArgb() == right.ToArgb();
    }

    private static bool TryGetVolumetricColorChannels(
        IVolumetricLightingGetter source,
        out byte r,
        out byte g,
        out byte b)
    {
        if (!source.ColorR.HasValue || !source.ColorG.HasValue || !source.ColorB.HasValue)
        {
            r = g = b = 0;
            return false;
        }

        r = ClampByte(source.ColorR.Value);
        g = ClampByte(source.ColorG.Value);
        b = ClampByte(source.ColorB.Value);
        return true;
    }

    private static bool IsAchromatic(byte r, byte g, byte b)
    {
        return (r == 0 && g == 0 && b == 0) ||
               (r == 255 && g == 255 && b == 255);
    }
}
