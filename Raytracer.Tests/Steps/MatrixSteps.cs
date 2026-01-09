using System.Linq;
using Reqnroll;
using Shouldly;
using Matrix = Raytracer.Math.Matrix.T;
using Tuple = Raytracer.Math.Tuple.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class MatrixSteps(ScenarioContext ctx) : StepsBase(ctx)
{

    [StepArgumentTransformation()]
    public static Matrix CreateMatrix(DataTable table)
    {
        var elements = table.Header.Concat(table.Rows.SelectMany(r => r.Values)).Select(System.Convert.ToDouble);
        return Math.Matrix.ofSeq(elements);
    }

    [Given(@"^the following \dx\d matrix (M|A|B):$")]
    [Given(@"^the following matrix (A|B):$")]
    public void GivenTheFollowingMatrix(string matrixKey, Matrix matrix)
    {
        Matrix[matrixKey] = matrix;
    }

    [Then(@"^(M)\[(\d+),(\d+)] = (.*)$")]
    public void ThenElementShouldBe(string matrixKey, int row, int col, float expected)
    {
        var actual = Matrix[matrixKey][row, col];
        actual.ShouldBe(expected);
    }

    [Then(@"^(B)\[(\d+),(\d+)] = (-?\d+)/(\d+)$")]
    public void ThenElementShouldBe(string matrixKey, int row, int col, float d, float n)
    {
        var actual = Matrix[matrixKey][row, col];
        var expected = d / n;
        actual.ShouldBe(expected);
    }


    [Then(@"^(A) = (B)$")]
    public void ThenMatrixEqualityShouldBe(string matrixKey1, string matrixKey2)
    {
        var a = Matrix[matrixKey1];
        var b = Matrix[matrixKey2];

        a.ShouldBe(b);
    }

    [Then(@"^(B) is the following \dx\d matrix:$")]
    public void ThenMatrixEqualityShouldBe(string matrixKey, Matrix expected)
    {
        var actual = Matrix[matrixKey];
        actual.ShouldBe(expected);
    }


    [Then(@"^inverse\((A)\) is the following \dx\d matrix:$")]
    public void ThenMatrixInversionShouldBe(string matrixKey, Matrix expected)
    {
        var a = Matrix[matrixKey];
        var actual = Math.Matrix.inverse(a);
        actual.ShouldBe(expected);
    }

    [Then(@"^(A) != (B)$")]
    public void ThenMatrixInequalityShouldBe(string matrixKey1, string matrixKey2)
    {
        var a = Matrix[matrixKey1];
        var b = Matrix[matrixKey2];

        a.ShouldNotBe(b);
    }

    [Then(@"^(A) \* (B) is the following 4x4 matrix:$")]
    public void ThenMatrixMultiplicationShouldBe(string matrixKey1, string matrixKey2, Matrix expected)
    {
        var a = Matrix[matrixKey1];
        var b = Matrix[matrixKey2];
        var actual = a * b;

        actual.ShouldBe(expected);
    }

    [Then(@"^(A) \* (b) = (tuple.*)$")]
    public void ThenMatrixMultiplicationShouldBe(string matrixKey, string tupleKey, Tuple expected)
    {
        var a = Matrix[matrixKey];
        var b = Tuple[tupleKey];
        var actual = a * b;

        actual.ShouldBe(expected);
    }

    [Then(@"^(A) \* identity_matrix = (A)$")]
    public void ThenIdentityMatrixMultiplicationShouldBe(string matrixKey1, string matrixKey2)
    {
        var actual = Matrix[matrixKey1] * Math.Matrix.identity;
        var expected = Matrix[matrixKey2];

        actual.ShouldBe(expected);
    }

    [Then(@"^identity_matrix \* (a) = (a)$")]
    public void ThenIdentityMatrixTupleMultiplicationShouldBe(string tupleKey1, string tupleKey2)
    {
        var actual = Math.Matrix.identity * Tuple[tupleKey1];
        var expected = Tuple[tupleKey2];

        actual.ShouldBe(expected);
    }

    [Then(@"^transpose\((A)\) is the following matrix:$")]
    public void ThenTransposeTheFollowingMatrix(string matrixKey, Matrix expected)
    {
        var actual = Math.Matrix.transpose(Matrix[matrixKey]);
        actual.ShouldBe(expected);
    }

    [Given(@"^(A) ← transpose\(identity_matrix\)")]
    public void GivenTransposeIdentityMatrix(string matrixKey)
    {
        Matrix[matrixKey] = Math.Matrix.transpose(Math.Matrix.identity);
    }

    [Then(@"^(A) = identity_matrix$")]
    public void ThenMatrixShouldBeIdentityMatrix(string matrixKey)
    {
        Matrix[matrixKey].ShouldBe(Math.Matrix.identity);
    }

    [Then(@"^determinant\((A|B)\) = (.*)$")]
    public void ThenDeterminantShouldBe(string matrixKey, double expected)
    {
        var actual = Math.Matrix.determinant(Matrix[matrixKey]);
        actual.ShouldBe(expected);
    }

    [Then(@"^submatrix\((A), (\d), (\d)\) is the following \dx\d matrix:")]
    public void ThenSubmatrixShouldBe(string matrixKey, int row, int col, Matrix expected)
    {
        var m = Matrix[matrixKey];
        var actual = Math.Matrix.submatrix(m, row, col);
        actual.ShouldBe(expected);
    }

    [Given(@"^(B) ← submatrix\((A), (\d), (\d)\)$")]
    public void GivenSubmatrix(string matrixKey1, string matrixKey2, int row, int col)
    {
        var a = Matrix[matrixKey2];
        var b = Math.Matrix.submatrix(a, row, col);
        Matrix[matrixKey1] = b;
    }

    [Then(@"^minor\((A), (\d), (\d)\) = (.*)$")]
    public void ThenMinorShouldBe(string matrixKey, int row, int col, double expected)
    {
        var a = Matrix[matrixKey];
        var actual = Math.Matrix.minor(a, row, col);
        actual.ShouldBe(expected);
    }

    [Then(@"^cofactor\((A), (\d), (\d)\) = (.*)$")]
    public void ThenCofactorShouldBe(string matrixKey, int row, int col, double expected)
    {
        var a = Matrix[matrixKey];
        var actual = Math.Matrix.cofactor(a, row, col);
        actual.ShouldBe(expected);
    }

    [Then(@"^(A) is invertible$")]
    public void ThenIsInvertible(string matrixKey)
    {
        var a = Matrix[matrixKey];
        var d = Math.Matrix.determinant(a);
        d.ShouldNotBe(0);
    }

    [Then(@"^(A) is not invertible$")]
    public void ThenIsNotInvertible(string matrixKey)
    {
        var a = Matrix[matrixKey];
        var d = Math.Matrix.determinant(a);
        d.ShouldBe(0);
    }

    [Given(@"^(B) ← inverse\((A)\)$")]
    public void GivenInverseMatrix(string matrixKey1, string matrixKey2)
    {
        var a = Matrix[matrixKey2];
        var b = Math.Matrix.inverse(a);

        Matrix[matrixKey1] = b;
    }

    [Given(@"^(C) ← (A) \* (B)$")]
    public void GivenMatrixMultiplication(string matrixKey1, string matrixKey2, string matrixKey3)
    {
        var a = Matrix[matrixKey2];
        var b = Matrix[matrixKey3];
        var c = a * b;
        Matrix[matrixKey1] = c;
    }

    [Then(@"^(C) \* inverse\((B)\) = (A)$")]
    public void ThenInverseMatrixMultiplicationShouldBe(string matrixKey1, string matrixKey2, string matrixKey3)
    {
        var c = Matrix[matrixKey1];
        var b = Matrix[matrixKey2];
        var a = Matrix[matrixKey3];

        (c * Math.Matrix.inverse(b)).ShouldBe(a);
    }

}