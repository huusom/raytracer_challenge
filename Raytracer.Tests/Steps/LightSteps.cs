using Reqnroll;
using Shouldly;
using System;
namespace Raytracer.Tests.Steps;

[Binding]
public class LightSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [When(@"(light) ← point_light\((position), (intensity)\)$")]
    public void WhenCreatingPointLight(string key, string positionKey, string intensityKey)
    {
        var position = Tuple[positionKey];
        var intensity = Color[intensityKey];

        Light[key] = Graphics.Light.pointLightOf(position, intensity);
    }

    [When(@"^(light) ← point_light\((point.*), (color.*)\)$")]
    [Given(@"^(light) ← point_light\((point.*), (color.*)\)$")]
    public void WhenCreatingPointLight(string key, Math.Tuple.T position, Graphics.Color.T intensity)
    {
        Light[key] = Graphics.Light.pointLightOf(position, intensity);
    }

    [Then(@"^(light)\.position = (position)$")]
    public void ThenPositionShouldBe(string key, string expectedKey)
    {
        var light = Light[key];
        var expected = Tuple[expectedKey];

        light.position.ShouldBe(expected);
    }

    [Then(@"^(light)\.intensity = (intensity)$")]
    public void ThenIntensityShouldBe(string key, string expectedKey)
    {
        var light = Light[key];
        var expected = Color[expectedKey];

        light.intensity.ShouldBe(expected);
    }

}