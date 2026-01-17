using Reqnroll;
using Shouldly;
using Raytracer.Math;
using System.Linq;
using Raytracer.Geometry;

namespace Raytracer.Tests.Steps;

[Binding]
public class TransformationSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation()]
    public static Matrix.M4.T CreateMatrix(DataTable table)
    {
        var elements = table.Header.Concat(table.Rows.SelectMany(r => r.Values)).Select(System.Convert.ToDouble).ToArray();
        return Math.Matrix.M4.ofArray(elements);
    }

    [StepArgumentTransformation(@"translation\((.*), (.*), (.*)\)")]
    public static Transformation.T ToTranslation(double x, double y, double z) => Geometry.Transformation.translationOf(x, y, z);

    [StepArgumentTransformation(@"scaling\((.*), (.*), (.*)\)")]
    public static Transformation.T ToScaling(double x, double y, double z) => Geometry.Transformation.scalingOf(x, y, z);

    [StepArgumentTransformation(@"shearing\((.*), (.*), (.*), (.*), (.*), (.*)\)")]
    public static Transformation.T ToShearing(double xy, double xz, double yx, double yz, double zx, double zy) => Geometry.Transformation.shearingOf(xy, xz, yx, yz, zx, zy);

    [StepArgumentTransformation(@"rotation_x\((.*)\)")]
    public static Transformation.T ToRotationX(double radians) => Geometry.Transformation.rotationXOf(radians);

    [StepArgumentTransformation(@"rotation_y\((.*)\)")]
    public static Transformation.T ToRotationY(double radians) => Geometry.Transformation.rotationYOf(radians);

    [StepArgumentTransformation(@"rotation_z\((.*)\)")]
    public static Transformation.T ToRotationZ(double radians) => Geometry.Transformation.rotationZOf(radians);

    [StepArgumentTransformation(@"(π / \d|π/\d|\-?√\d/\d|√\d+)")]
    public static double ToDouble(string arg) => IO.Parser.floatFrom(arg);

    [Given(@"^(transform|C|m|t) ← (translation.*)$")]
    [Given(@"^(transform|B|m) ← (scaling\([^)]*\))$")]
    [Given(@"^(transform) ← (shearing.*)$")]
    [Given(@"(half_quarter|full_quarter|A) ← (rotation_x.*)")]
    [Given(@"(half_quarter|full_quarter) ← (rotation_y.*)")]
    [Given(@"(half_quarter|full_quarter) ← (rotation_z.*)")]
    public void GivenTranslation(string transformationKey, Transformation.T translation)
    {
        Transformation[transformationKey] = translation;
    }

    [Given(@"^(inv) ← inverse\((transform|half_quarter)\)$")]
    public void GivenInverseTransformation(string key, string targetKey)
    {
        var target = Transformation[targetKey];

        Transformation[key] = Geometry.Transformation.init(target.inverse.Value);
    }

    [Then(@"^(transform|inv|half_quarter|full_quarter|T) \* (p) = (point.*)$")]
    [Then(@"^(transform|inv) \* (v) = (vector.*)$")]
    public void ThenTransformationMultiplicationsShouldBe(string transformationKey, string tupleKey, Tuple.T expected)
    {
        var t = Transformation[transformationKey];
        var p = Tuple[tupleKey];
        var actual = t.source * p;

        actual.ShouldBe(expected);
    }

    [Then(@"^(transform) \* (v) = (v)$")]
    public void ThenTransformationMultiplicationsShouldBe(string transformationKey, string tupleKey, string expectedKey)
    {
        var t = Transformation[transformationKey];
        var p = Tuple[tupleKey];
        var actual = t.source * p;
        var expected = Tuple[expectedKey];

        actual.ShouldBe(expected);
    }

    [When(@"^(p2|p3|p4) ← (A|B|C) \* (p|p2|p3)$")]
    public void WhenTransformationMultiplication(string tupleKey1, string transformKey, string tupleKey2)
    {
        var t = Transformation[transformKey];
        var p = Tuple[tupleKey2];
        Tuple[tupleKey1] = t.source * p;
    }

    [When(@"^T ← C \* B \* A$")]
    public void WhenChainingTransformations()
    {
        var c = Transformation["C"];
        var b = Transformation["B"];
        var a = Transformation["A"];
        var t = c * b * a;
        Transformation["T"] = t;
    }

    [Then(@"^(p2|p3|p4) = (point.*)$")]
    public void ThenPointShouldBe(string tupleKey, Tuple.T expected)
    {
        var actual = Tuple[tupleKey];
        actual.ShouldBe(expected);
    }

    [Given(@"^(m) ← (scaling.*) \* (rotation_z.*)$")]
    public void GivenTransformationAsProduct(string key, Transformation.T scaling, Transformation.T rotation)
    {
        Transformation[key] = scaling * rotation;
    }

    [Then(@"^(t) = (scaling.*)$")]
    [Then(@"^(t) = (translation.*)$")]
    public void ThenTransformShouldBe(string key, Transformation.T expected)
    {
        var actual = Transformation[key];
        actual.ShouldBe(expected);
    }

    [When(@"^(t) ← view_transform\((from), (to), (up)\)$")]
    public void ViewTransform(string key, string fromKey, string toKey, string upKey)
    {
        var from = Tuple[fromKey];
        var to = Tuple[toKey];
        var up = Tuple[upKey];

        var t = Geometry.Transformation.viewOf(from, to, up);
        Transformation[key] = t;
    }

    [Then(@"^(t) = identity_matrix$")]
    public void ThenTransformationShouldBeIdentityMatrix(string key)
    {
        Transformation[key].ShouldBe(Geometry.Transformation.identity);
    }

    [Then(@"^(t) is the following \dx\d matrix:$")]
    public void ThenTransformationEqualityShouldBe(string key, Matrix.M4.T expected)
    {
        var actual = Transformation[key];
        actual.source.ShouldBe(expected);
    }
}
