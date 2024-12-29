namespace WorldMap.Common.Math
{
    public static class MoreMath
    {
        public static int Factorial(int i) => Enumerable.Range(1,i<1?1:i).Aggregate((f, x)=>f* x);
    }
}
