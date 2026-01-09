using Reqnroll;
using Raytracer.Graphics;
using Material = Raytracer.Graphics.Material.T;
using Shouldly;

namespace Raytracer.Tests.Steps;

[Binding]
public class MaterialSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation(@"material\(\)")]
    public static Material Create() => DefaultsBuilder.Material();

    [Given(@"^(m) ← (material\(\))$")]
    public void GivenMaterial(string key, Material m)
    {
        Material[key] = m;
    }

    [Then(@"^(m)\.color = (color.*)$")]
    public void ThenColorShouldBe(string key, Color.T expected)
    {
        var actual = Material[key];
        actual.color.ShouldBe(expected);
    }

    [Then(@"^(m)\.ambient = (.*)$")]
    public void ThenAmbientShouldBe(string key, double expected)
    {
        var actual = Material[key];
        actual.ambient.ShouldBe(expected);
    }

    [Then(@"^(m)\.diffuse = (.*)$")]
    public void ThenDiffuseShouldBe(string key, double expected)
    {
        var actual = Material[key];
        actual.diffuse.ShouldBe(expected);
    }

    [Then(@"^(m)\.specular = (.*)$")]
    public void ThenSpecularShouldBe(string key, double expected)
    {
        var actual = Material[key];
        actual.specular.ShouldBe(expected);
    }

    [Then(@"^(m)\.shininess = (.*)$")]
    public void ThenShininessShouldBe(string key, double expected)
    {
        var actual = Material[key];
        actual.shininess.ShouldBe(expected);
    }

    [Given(@"^(result) ← lighting\((m), (light), (position), (eyev), (normalv)\)$")]
    [When(@"^(result) ← lighting\((m), (light), (position), (eyev), (normalv)\)$")]
    public void WhenCreatingLightning(string key, string materialKey, string lightKey, string positionKey, string eyevKey, string normalvKey)
    {
        var material = Material[materialKey];
        var light = Light[lightKey];
        var position = Tuple[positionKey];
        var eyev = Tuple[eyevKey];
        var normalv = Tuple[normalvKey];

        Color[key] = Graphics.Material.lightning(material, light, position, eyev, normalv);
    }

}
