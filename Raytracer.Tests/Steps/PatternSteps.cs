using Reqnroll;
using Shouldly;

namespace Raytracer.Tests.Steps;

[Binding]
public class PatternSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [Given(@"^(pattern) ← stripe_pattern\((white), (black)\)$")]
    public void GivenStripePattern(string key, string colorKey1, string colorKey2)
    {
        var a = Color[colorKey1];
        var b = Color[colorKey2];

        Pattern[key] = Graphics.Pattern.stripeOf(a, b);
    }

    [Given(@"^(pattern) ← gradient_pattern\((white), (black)\)$")]
    public void GivenGradientPattern(string key, string colorKey1, string colorKey2)
    {
        var a = Color[colorKey1];
        var b = Color[colorKey2];

        Pattern[key] = Graphics.Pattern.gradientOf(a, b);
    }

    [Given(@"^(pattern) ← ring_pattern\((white), (black)\)$")]
    public void GivenRingPattern(string key, string colorKey1, string colorKey2)
    {
        var a = Color[colorKey1];
        var b = Color[colorKey2];

        Pattern[key] = Graphics.Pattern.ringOf(a, b);
    }

    [Given(@"^(pattern) ← checkers_pattern\((white), (black)\)$")]
    public void GivenCheckerPattern(string key, string colorKey1, string colorKey2)
    {
        var a = Color[colorKey1];
        var b = Color[colorKey2];

        Pattern[key] = Graphics.Pattern.checkerOf(a, b);
    }

    [Given(@"^(pattern) ← test_pattern\(\)$")]
    public void GivenTestPattern(string key)
    {
        Pattern[key] = Graphics.Pattern.testOf();
    }

    [Then(@"^(pattern)\.a = (white)$")]
    public void ThenPatternAShouldBe(string key, string expectedKey)
    {
        var expected = Color[expectedKey];
        var actual = Pattern[key];

        actual.a.ShouldBe(expected);
    }

    [Then(@"^(pattern)\.b = (black)$")]
    public void ThenPatternBShouldBe(string key, string expectedKey)
    {
        var expected = Color[expectedKey];
        var actual = Pattern[key];

        actual.b.ShouldBe(expected);
    }

    [Then(@"^pattern_at\((pattern), (point.*)\) = (color.*)$")]
    public void ThenPatternAtShouldBe(string key, Math.Tuple.T point, Graphics.Color.T expected)
    {
        var pattern = Pattern[key];
        var actual = Graphics.Pattern.colorFrom(pattern, point);
        actual.ShouldBe(expected);
    }

    [Then(@"^stripe_at\((pattern), (point.*)\) = (white|black)")]
    [Then(@"^pattern_at\((pattern), (point.*)\) = (white|black)")]
    public void ThenStripeAtShouldBe(string key, Math.Tuple.T point, string expectedKey)
    {
        var expected = Color[expectedKey];
        ThenPatternAtShouldBe(key, point, expected);
    }

    [When(@"^(c) ← stripe_at_object\((pattern), (object), (point.*)\)")]
    [When(@"^(c) ← pattern_at_shape\((pattern), (shape), (point.*)\)")]
    public void WhenStripeColorAt(string colorKey, string patternKey, string shapeKey, Math.Tuple.T point)
    {
        var pattern = Pattern[patternKey];
        var shape = Shape[shapeKey];

        var p = shape.transform.inverse.Value * point;
        var c = Graphics.Pattern.colorFrom(pattern, p);

        Color[colorKey] = c;
    }

    [Given(@"^set_pattern_transform\((pattern), (translation.*)\)$")]
    [Given(@"^set_pattern_transform\((pattern), (scaling.*)\)$")]
    [When(@"^set_pattern_transform\((pattern), (translation.*)\)$")]
    public void GivenPatternTransform(string key, Math.Transformation.T transform)
    {
        var p = Pattern[key];
        p.transform = transform;
    }

    [Then(@"^(pattern)\.transform = (translation.*)$")]
    public void ThenTransformShouldBe(string key, Math.Transformation.T expected)
    {
        Pattern[key].transform.ShouldBe(expected);
    }


    [Then(@"^(pattern)\.transform = identity_matrix$")]
    public void ThenTransformShouldBeIdentity(string key)
    {
        ThenTransformShouldBe(key, Math.Transformation.identity);
    }
}