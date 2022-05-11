using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienGenerator : MonoBehaviour
{
    #region Variables
    [Header("ForTextComprobation")]
    public GameObject _eyeText;
    public GameObject _armText;
    public GameObject _legText;
    public GameObject _mouthText;

    public GameObject _hand;

    [Header("Configuration")]
    public GameObject _alienBase;

    public RectTransform _alienPosition;

    [Header("AlienEyes")]
    public GameObject _alienEye;

    public List<GameObject> _eyesList;

    [Header("AlienBody")]
    public GameObject _alienBody;

    public List<GameObject> _bodyList;

    [Header ("AlienArms")]
    public GameObject _alienArm;

    public List<GameObject> _armList;

    [Header ("AlienLegs")]
    public GameObject _alienLeg;

    public List<GameObject> _LegList;

    [Header ("AlienMouths")]
    public GameObject _alienMouth;

    public List<GameObject> _mouthList;

    [Header("Instructions")]
    public int _legInstructions;
    public int _armInstructions;
    public int _eyeInstructions;
    public int _mouthInstructions;
    #endregion


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
        PartsSelection();
        GenerateAlien(_armInstructions, _legInstructions, _mouthInstructions, _eyeInstructions);
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Cuenta las partes del alien", true);

        
        _eyeText.GetComponent<Text>().text = ""+0;
        _legText.GetComponent<Text>().text = ""+0;
        _armText.GetComponent<Text>().text = ""+0;
        _mouthText.GetComponent<Text>().text = ""+0;
    }

    /// <summary>
    /// Random pick quantity of spawners
    /// </summary>
    private void GenerateNewValues()
    {
        _legInstructions = Random.Range(0, 4);
        _armInstructions = Random.Range(0, 4);
        _eyeInstructions = Random.Range(0, 4);
        _mouthInstructions = Random.Range(0, 4);
    }

    private void PartsSelection()
    {
        int _alienEyeSelect = Random.Range(0, _eyesList.Count);
        _alienEye = _eyesList[_alienEyeSelect];

        int _alienBodySelect = Random.Range(0, _bodyList.Count);
        _alienBody = _bodyList[_alienBodySelect];

        int _alienArmSelect = Random.Range(0, _armList.Count);
        _alienArm = _armList[_alienArmSelect];

        int _alienLegSelect = Random.Range(0, _LegList.Count);
        _alienLeg = _LegList[_alienLegSelect];

        int _alienMouthSelect = Random.Range(0, _mouthList.Count);
        _alienMouth = _mouthList[_alienMouthSelect];
    }

    private void GenerateAlien(int arm, int leg, int mouth , int eye)
    {
        GameObject newBody = Instantiate(_alienBody, _alienPosition);

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

            if (newArm.GetComponent<RectTransform>().position.x > _alienBase.GetComponent<RectTransform>().position.x)
            {
                newArm.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        for (int i = 0; i < leg; i++)
        {
            GameObject newLeg = Instantiate(_alienLeg, _alienPosition);
            int indexLeg = Random.Range(0, legPositions.Count);
            Vector2 newPositionLeg = legPositions[indexLeg].transform.position;
            legPositions.RemoveAt(indexLeg);
            newLeg.GetComponent<RectTransform>().position = newPositionLeg;

            if (newLeg.GetComponent<RectTransform>().position.x > _alienBase.GetComponent<RectTransform>().position.x)
            {
                newLeg.transform.localScale = new Vector3(-1, 1, 1);
            }

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
    /// <summary>
    /// Compare number extracted from text.
    /// </summary>
    public void CheckAnswer()
    {
        _hand.GetComponent<Animator>().Play("HandWritting_anim");

        int numEye = int.Parse(_eyeText.GetComponent<Text>().text);

        int numLeg = int.Parse(_legText.GetComponent<Text>().text);

        int numArm = int.Parse(_armText.GetComponent<Text>().text);

        int numMouth = int.Parse(_mouthText.GetComponent<Text>().text);

        if (numEye == _eyeInstructions && numArm == _armInstructions && numLeg == _legInstructions && numMouth == _mouthInstructions)
        {
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
            StartCoroutine(cleanAlien());
        }else
        {
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
    }

    /// <summary>
    /// Delete alien parts for restart.
    /// </summary>
    private IEnumerator cleanAlien()
    {
        RectTransform[] allChildren = _alienBase.GetComponentsInChildren<RectTransform>();
        List<RectTransform> allChindrenCopy = new List<RectTransform>(allChildren);
        for (int i = 0; i < allChindrenCopy.Count; i++)
        {
            if(allChildren[i].name != "Alien")
            {
                Destroy(allChildren[i].gameObject);
            }
        }
        yield return new WaitForSeconds(1.5f);
        Restart();
    }

}
