using Reqnroll;
using Shouldly;
using Tuple = Raytracer.Math.Tuple.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class TupleSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation(@"tuple\((.*), (.*), (.*), (.*)\)")]
    public static Tuple ToTuple(double x, double y, double z, double w) => Math.Tuple.tuple(x, y, z, w);

    [StepArgumentTransformation(@"point\((.*), (.*), (.*)\)")]
    public static Tuple ToPoint(double x, double y, double z) => Math.Tuple.point(x, y, z);

    [StepArgumentTransformation(@"vector\((.*), (.*), (.*)\)")]
    public static Tuple ToVector(double x, double y, double z) => Math.Tuple.vector(x, y, z);

    [Given(@"^(a|a1|a2|b) ← (tuple.*)$")]
    [Given(@"^(p|p1|p2) ← (point.*)$")]
    [Given(@"^(v|v1|v2|zero|a|b|n) ← (vector.*)$")]
    [Given(@"^(eyev|normalv|up) ← (vector.*)$")]
    [Given(@"^(position|from|to) ← (point.*)$")]
    public void GivenTuple(string k, Tuple tuple)
    {
        Tuple[k] = tuple;
    }

    [Then("a.x = {float}")]
    public void ThenXShouldBe(double x)
    {
        Tuple["a"].x.ShouldBe(x);
    }

    [Then("a.y = {float}")]
    public void ThenYShouldBe(double x)
    {
        Tuple["a"].y.ShouldBe(x);
    }
    [Then("a.z = {float}")]
    public void ThenZShouldBe(double x)
    {
        Tuple["a"].z.ShouldBe(x);
    }
    [Then("a.w = {float}")]
    public void ThenWShouldBe(double x)
    {
        Tuple["a"].w.ShouldBe(x);
    }

    [Then("a is a point")]
    public void ThenTupleIsAPoint()
    {
        Tuple["a"].w.ShouldBe(1);
    }

    [Then("a is not a point")]
    public void ThenTupleIsNotAPoint()
    {
        Tuple["a"].w.ShouldNotBe(1);
    }

    [Then("a is a vector")]
    public void ThenTupleIsAVector()
    {
        Tuple["a"].w.ShouldBe(0);
    }
    [Then("a is not a vector")]
    public void ThenTupleIsNotAVector()
    {
        Tuple["a"].w.ShouldNotBe(0);
    }

    [Then(@"^(a|p|v) = (tuple.*)$")]
    [Then(@"^(n|r) = (vector.*)$")]
    public void ThenTupleShouldBe(string k, Tuple expected)
    {
        var actual = Tuple[k];
        actual.ShouldBe(expected);
    }

    [Then(@"^\-(a) = (tuple.*)$")]
    public void ThenNegatedTupleShouldBe(string k, Tuple expected)
    {
        var actual = -Tuple[k];
        actual.ShouldBe(expected);
    }

    [Then(@"^(a1) \+ (a2) = (tuple.*)$")]
    public void ThenTupleAddShouldBe(string k1, string k2, Tuple expected)
    {
        var l = Tuple[k1];
        var r = Tuple[k2];
        var actual = l + r;
        actual.ShouldBe(expected);
    }

    [Then(@"^(p1) \- (p2) = (vector.*)$")]
    [Then(@"^(p) \- (v) = (point.*)$")]
    [Then(@"^(v1) \- (v2) = (vector.*)$")]
    [Then(@"^(zero) \- (v) = (vector.*)$")]
    public void ThenTupleSubShouldBe(string k1, string k2, Tuple expected)
    {
        var l = Tuple[k1];
        var r = Tuple[k2];
        var actual = l - r;
        actual.ShouldBe(expected);
    }

    [Then(@"^(a) \* (.*) = (tuple.*)$")]
    public void ThenTupleMultShouldBe(string k, double r, Tuple expected)
    {
        var l = Tuple[k];
        var actual = l * r;
        actual.ShouldBe(expected);
    }

    [Then(@"^(a) / (.*) = (tuple.*)$")]
    public void ThenTupleDivShouldBe(string k, double r, Tuple expected)
    {
        var l = Tuple[k];
        var actual = l / r;
        actual.ShouldBe(expected);
    }

    [Then(@"^magnitude\((v|norm)\) = (.*)$")]
    public void ThenMagnitudeShouldBe(string k, double expected)
    {
        var t = Tuple[k];
        var actual = Math.Tuple.magnitude(t);
        actual.ShouldBe(expected);
    }

    [Then(@"^normalize\((v)\) = (vector.*)$")]
    public void ThenNormalShouldBe(string k, Tuple expected)
    {
        var t = Tuple[k];
        var actual = Math.Tuple.normalize(t);
        actual.ShouldBe(expected);
    }

    //   
    [Then(@"^(n) = normalize\((n)\)$")]
    public void ThenNormalShouldBe(string key1, string key2)
    {
        var actual = Tuple[key1];
        var expected = Math.Tuple.normalize(Tuple[key2]);

        actual.ShouldBe(expected);
    }

    [When(@"^norm ← normalize\(v\)$")]
    public void WhenNormalizeTuple()
    {
        var v = Tuple["v"];
        Tuple["norm"] = Math.Tuple.normalize(v);
    }

    [Then(@"^dot\(a, b\) = 20$")]
    public void ThenDotShouldBe()
    {
        var a = Tuple["a"];
        var b = Tuple["b"];

        Math.Tuple.dot(a, b).ShouldBe(20);
    }

    [Then(@"^cross\((a), (b)\) = (vector.*)$")]
    [Then(@"^cross\((b), (a)\) = (vector.*)$")]
    public void ThenCrossShouldBe(string k1, string k2, Tuple expected)
    {
        var a = Tuple[k1];
        var b = Tuple[k2];

        var actual = Math.Tuple.cross(a, b);
        actual.ShouldBe(expected);
    }

    // When r ← reflect(v, n)
    [When(@"^(r) ← reflect\((v), (n)\)$")]
    public void WhenReflecting(string key, string vectorKey, string normalKey)
    {
        var v = Tuple[vectorKey];
        var n = Tuple[normalKey];
        var r = Math.Tuple.reflect(v, n);
        Tuple[key] = r;
    }
}
