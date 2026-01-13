using Reqnroll;
using Raytracer.Graphics;
using Shouldly;

namespace Raytracer.Tests.Steps;

[Binding]
public class MaterialSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    private bool in_shadow;

    [StepArgumentTransformation(@"material\(\)")]
    public static Material.T Create() => DefaultsBuilder.Material();

    [Given(@"^(m) ← (material\(\))$")]
    public void GivenMaterial(string key, Material.T m)
    {
        Material[key] = m;
    }

    [Given(@"^(m)\.ambient ← (\d+)$")]
    public void GivenAmbient(string key, double ambient)
    {
        var m = Material[key];
        m.ambient = ambient;
    }

    [Then(@"^(m) = (material\(\))$")]
    public void ThenMaterialShouldBe(string key, Material.T expected)
    {
        var actual = Material[key];

        actual.ShouldBe(expected);
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
    [When(@"^(result) ← lighting\((m), (light), (position), (eyev), (normalv), in_shadow\)$")]
    public void WhenCreatingLightning(string key, string materialKey, string lightKey, string positionKey, string eyevKey, string normalvKey)
    {
        var material = Material[materialKey];
        var light = Light[lightKey];
        var position = Tuple[positionKey];
        var eyev = Tuple[eyevKey];
        var normalv = Tuple[normalvKey];

        Color[key] = Graphics.Material.lightningFrom(material, light, position, eyev, normalv, this.in_shadow);
    }

    [Given(@"^in_shadow ← (true|false)$")]
    public void GivenInShadow(bool shadow)
    {
        this.in_shadow = shadow;
    }

}   
