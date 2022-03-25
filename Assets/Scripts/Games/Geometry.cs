using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    public enum Geometry_Type
    {
        circle = 0,
        triangle = 1,
        square = 2,
        diamond = 3,
        rectangle = 4,
        pentagon = 5,
        hexagon = 6
    }

    public Geometry_Type _geometryType;

    /*
     * @desc Return the geometrical form written in Spanish
     * @return string - Geometric shape name
     * **/
    public string getGeometryString()
    {
        switch (_geometryType)
        {
            case Geometry_Type.circle:
                return "círculo";

            case Geometry_Type.triangle:
                return "triángulo";

            case Geometry_Type.square:
                return "cuadrado";

            case Geometry_Type.diamond:
                return "diamante";

            case Geometry_Type.rectangle:
                return "rectángulo";

            case Geometry_Type.pentagon:
                return "pentágono";

            case Geometry_Type.hexagon:
                return "hexágono";

            default:
                return "error";
        }
    }
}
