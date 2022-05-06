using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public RectTransform _alienPosition;

    [Header("Instructions")]
    public int _legInstructions;
    public int _armInstructions;
    public int _eyeInstructions;
    public int _mouthInstructions;


    // Start is called before the first frame update
    void Start()
    {
        Restart();
    }

    private void Update()
    {
        
    }

    private void Restart()
    {
        GenerateNewValues();
        GenerateAlien(_armInstructions, _legInstructions, _eyeInstructions, _mouthInstructions);
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Cuenta las partes del alien", true);

        _eyeText.GetComponent<Text>().text = ""+0;
        _legText.GetComponent<Text>().text = ""+0;
        _armText.GetComponent<Text>().text = ""+0;
        _mouthText.GetComponent<Text>().text = ""+0;
    }

    private void GenerateNewValues()
    {
        _legInstructions = Random.Range(0, 4);
        _armInstructions = Random.Range(0, 4);
        _eyeInstructions = Random.Range(0, 4);
        _mouthInstructions = Random.Range(0, 4);
}

    private void GenerateAlien(int arm, int leg, int mouth , int eye)
    {
        
        GameObject[] armPositionsArr = GameObject.FindGameObjectsWithTag("ArmSpawner");
        List<GameObject> armPositions = new List<GameObject>(armPositionsArr);

        GameObject[] legPositionsArr = GameObject.FindGameObjectsWithTag("LegSpawner");
        List<GameObject> legPositions = new List<GameObject>(legPositionsArr);

        GameObject[] eyePositionsArr = GameObject.FindGameObjectsWithTag("EyeSpawner");
        List<GameObject> eyePositions = new List<GameObject>(eyePositionsArr);

        GameObject[] mouthPositionsArr = GameObject.FindGameObjectsWithTag("MouthSpawner");
        List<GameObject> mouthPositions = new List<GameObject>(mouthPositionsArr);

        for (int i = 0; i < arm; i++)
        {
            GameObject newArm = Instantiate(_alienArm, _alienPosition);
            int index= Random.Range(0, armPositions.Count);
            Vector2 newPosition = armPositions[index].transform.position;
            armPositions.RemoveAt(index);
            newArm.GetComponent<RectTransform>().position = newPosition;
        }

        for (int i = 0; i < leg; i++)
        {
            GameObject newLeg = Instantiate(_alienLeg, _alienPosition);
            int indexLeg = Random.Range(0, legPositions.Count);
            Vector2 newPositionLeg = legPositions[indexLeg].transform.position;
            legPositions.RemoveAt(indexLeg);
            newLeg.GetComponent<RectTransform>().position = newPositionLeg;
        }

        for (int i = 0; i < eye; i++)
        {
            GameObject newEye = Instantiate(_alienEye, _alienPosition);
            int indexEye = Random.Range(0, eyePositions.Count);
            Vector2 newPositionEye = eyePositions[indexEye].transform.position;
            eyePositions.RemoveAt(indexEye);
            newEye.GetComponent<RectTransform>().position = newPositionEye;
        }

        for (int i = 0; i < mouth; i++)
        {
            GameObject newMouth = Instantiate(_alienMouth, _alienPosition);
            int indexMouth = Random.Range(0, mouthPositions.Count);
            Vector2 newPositionMouth = mouthPositions[indexMouth].transform.position;
            mouthPositions.RemoveAt(indexMouth);
            newMouth.GetComponent<RectTransform>().position = newPositionMouth;
            int degrees = Random.Range(0, 21);
            int negative = Random.Range(0, 2);

            if (negative == 0)
                newMouth.transform.rotation = Quaternion.Euler(0, 0, degrees);
            else
                newMouth.transform.rotation = Quaternion.Euler(0, 0, -degrees);
        }

    }

    public void CheckAnswer()
    {
        int numEye = int.Parse(_eyeText.GetComponent<Text>().text);

        int numLeg = int.Parse(_legText.GetComponent<Text>().text);

        int numArm = int.Parse(_armText.GetComponent<Text>().text);

        int numMouth = int.Parse(_mouthText.GetComponent<Text>().text);

        if (numEye == _eyeInstructions && numArm == _armInstructions && numLeg == _legInstructions && numMouth == _mouthInstructions)
        {
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
            cleanAlien();
            Restart();
        }else
        {
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
    }

    private void cleanAlien()
    {
        RectTransform[] allChildren = _alienBase.GetComponentsInChildren<RectTransform>();
        List<RectTransform> allChindrenCopy = new List<RectTransform>(allChildren);
        for (int i = 0; i < allChindrenCopy.Count; i++)
        {
            if(allChildren[i].name != "AlienParts")
            {
                Destroy(allChildren[i].gameObject);
            }
        }
    }

}
