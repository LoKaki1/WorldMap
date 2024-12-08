using System.Drawing;
using System.Numerics;
namespace WorldMap.Common.Transformations.Interfaces;

public interface ITransformation
{
    PointF Transform(PointF point);
    IEnumerable<PointF> Transform(IEnumerable<PointF> points);
}
