using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    public List<Geometry.Geometry_Type> prueba;
    public int level;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            generateTargets(prueba, level);
    }

    /*
     * @desc Generates a list with geometry as a target for mini-games
     * @param List<Geomtry.Geometry_Type> geometryOptions - Possible geometry to select
     * @param int level - Difficulty level
     * @return List<Geomtry.Geometry_Type> - List of selected geometry
     * **/
    public List<Geometry.Geometry_Type> generateTargets(List<Geometry.Geometry_Type> geometryOptions, int level)
    {
        List<Geometry.Geometry_Type> result = new List<Geometry.Geometry_Type>();
        int numOptions = (level / 3) + 1;
        do
        {
            Geometry.Geometry_Type aux = (Geometry.Geometry_Type)Random.Range(0, geometryOptions.Count);
            if (!result.Contains(aux))
                result.Add(aux);
        } while (result.Count != numOptions);
        return result;
    }
}
