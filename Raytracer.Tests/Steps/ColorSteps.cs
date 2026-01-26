using Reqnroll;
using Shouldly;
using Color = Raytracer.Graphics.Color.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class ColorSteps(ScenarioContext ctx) : StepsBase(ctx)
{

    [StepArgumentTransformation(@"color\((.*), (.*), (.*)\)")]
    public static Color Create(double r, double g, double b) => Graphics.Color.create(r, g, b);

    [Given(@"^(c|c1|c2|c3|red|black|white) ← (color.*)$")]
    [Given(@"^(intensity) ← (color.*)$")]
    public void GivenColor(string k, Color color)
    {
        Color[k] = color;
    }

    [Then(@"^c\.red = (.*)$")]
    public void ThenRedShouldBe(double expected)
    {
        var actual = Color["c"];
        actual.r.ShouldBe(expected);
    }

    [Then(@"^c\.green = (.*)$")]
    public void ThenGreenShouldBe(double expected)
    {
        var actual = Color["c"];
        actual.g.ShouldBe(expected);
    }

    [Then(@"^c\.blue = (.*)$")]
    public void ThenBlueShouldBe(double expected)
    {
        var actual = Color["c"];
        actual.b.ShouldBe(expected);
    }

    [Then(@"^(c1) \+ (c2) = (color.*)$")]
    public void ThenColorAdditionShouldBe(string k1, string k2, Color expected)
    {
        var c1 = Color[k1];
        var c2 = Color[k2];
        var actual = c1 + c2;
        actual.ShouldBe(expected);
    }

    [Then(@"^(c1) \- (c2) = (color.*)$")]
    public void ThenColorSubtractionShouldBe(string k1, string k2, Color expected)
    {
        var c1 = Color[k1];
        var c2 = Color[k2];
        var actual = c1 - c2;
        actual.ShouldBe(expected);
    }

    [Then(@"^(c1) \* (c2) = (color.*)$")]
    public void ThenColorMultiplicationShouldBe(string k1, string k2, Color expected)
    {
        var c1 = Color[k1];
        var c2 = Color[k2];
        var actual = c1 * c2;
        actual.ShouldBe(expected);
    }

    [Then(@"^(c) \* (.*) = (color.*)$")]
    public void ThenColorMultiplicationShouldBe(string k1, float s, Color expected)
    {
        var c1 = Color[k1];
        var actual = c1 * s;
        actual.ShouldBe(expected);
    }

    [Then(@"^(result|c) = (color.*)$")]
    public void ThenColorShouldBe(string key, Color expected)
    {
        var actual = Color[key];
        actual.ShouldBe(expected);
    }

    [Then(@"^(c) = (white)$")]
    public void ThenColorShouldBe(string key, string expectedKey)
    {
        var expected = Color[expectedKey];
        ThenColorShouldBe(key, expected);
    }

}