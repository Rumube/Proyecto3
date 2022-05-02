using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGenerator : MonoBehaviour
{
    [Header("ForTextComprobation")]
    public GameObject _eyeText;
    public GameObject _armText;
    public GameObject _legText;
    public GameObject _mouthText;

    [Header("AlienParts")]
    public GameObject _alienBase;
    public GameObject _alienEye;
    public GameObject _alienArm;
    public GameObject _alienLeg;
    public GameObject _alienMouth;

    private RectTransform alienposition;

    // Start is called before the first frame update
    void Start()
    {
        alienposition = _alienBase.GetComponent<RectTransform>();
        GenerateAlien(2, 3, 5, 4);
    }

    private void Update()
    {
        
    }

    private void GenerateAlien(int arm, int leg, int mouth , int eye)
    {
        for (int i = 0; i < arm; i++)
        {
            Instantiate(_alienArm, alienposition);
        }
        for (int i = 0; i < leg; i++)
        {
            Instantiate(_alienLeg, alienposition);
        }
        for (int i = 0; i < mouth; i++)
        {
            Instantiate(_alienMouth, alienposition);
        }
        for (int i = 0; i < eye; i++)
        {
            Instantiate(_alienEye, alienposition);
        }

    }

}
