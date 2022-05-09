using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenerateForms : MonoBehaviour
{
//    [Header("GameObjects used")]
//    public new GameObject _starPrefab;
//    public new GameObject _targetGameObject;


//    private GameObject _newStar;
//    private GameObject _target;
//    public Image _objetiveFigureSprite;

//    [Header("Configure Generation Stars")]
//    public int _density;
//    private int _geometry;
//    public List<GameObject> _starsSelected = new List<GameObject>();
//    public List<GameObject> _allStars = new List<GameObject>();
//    public List<Sprite> _targetImage = new List<Sprite>();


//    public string _objetiveFigure;


//    [Header("Positions")]
//    Vector3 _mousePosition;
//    private int _count;
//    private int _countCorrects;
//    Touch _touch;

//    private int _maxCount;

//    [Header("Line configuration")]
//    bool _pressed= false;
//    LineRenderer _line;

//    [Header("Levels")]
//    public int _level;
//    private Telescope_Difficulty.dataDiffilcuty _currentLevel;

   

//    private CalculatePuntuation _calculatePuntuation;

//    int _failCount;
//    int _successCount;
    
//    // Start is called before the first frame update
//    void Start()
//    {
//        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
//        _line = GetComponent<LineRenderer>();
//        _currentLevel = GetComponent<Telescope_Difficulty>().GenerateDataDifficulty(_level);
//        levelDificulty();

//        _calculatePuntuation = GetComponent<CalculatePuntuation>();
        




//    }

//    // Update is called once per frame
////    void Update()
////    {
////       if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing)
////        {
////            mouseController();
////#if UNITY_ANDROID
////        _touch = Input.GetTouch(0);
////#endif
////        } 
////    }





//    /** 

//  * @desc Generate stars randomly by dropping a correct predefined shape

//  * @param 

//  * @return 

//    */

//    void StarGenerate (int _density)
//    {

//        for (int i = 0; i < _density; i++)
//        {
//            int xRand = Random.Range(-25, 30);
//            int yRand = Random.Range(-15, 20);



//            _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//            _newStar.GetComponent<StarPreference>()._preference = false;


//        }


//    }
//    /** 

//* @desc Generate stars  by dropping a correct predefined shape

//* @param int geometry - Is a value generate random for do the Geometry Generator

//* @return 

//*/
//    void geometryGenerator(int geometry)
//    {

//        switch (geometry)
//        {
//            case 1:
//                int xRand = Random.Range(-15, 15);
//                int yRand = Random.Range(-8, 13);
//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Triangle";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 2, yRand - 5, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Triangle";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 2, yRand - 5, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Triangle";

//                _maxCount = 4;
//                _objetiveFigure = "Triangle";
//                _objetiveFigureSprite.sprite = _targetImage[1];



                
//                break;
//            case 2:
//                 xRand = Random.Range(-15, 15);
//                 yRand = Random.Range(-8, 13);
//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Scuare";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 5, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Scuare";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand - 5, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Scuare";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 5, yRand - 5, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Scuare";

//                _maxCount = 5;
//                _objetiveFigure = "Scuare";
//                _objetiveFigureSprite.sprite = _targetImage[0];

//                break;
//            case 3:
//                 xRand = Random.Range(-15, 15);
//                  yRand = Random.Range(-8, 13);

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Diamond";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 3 , yRand -4, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Diamond";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 3, yRand - 4, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Diamond";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand - 8 , 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Diamond";

//                _maxCount = 5;
//                _objetiveFigure = "Diamond";
//                _objetiveFigureSprite.sprite = _targetImage[2];

//                break;
//            case 5:
//                 xRand = Random.Range(-15, 15);
//                 yRand = Random.Range(-8, 13);
//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Pentagon";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 4 , yRand - 3, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Pentagon";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 4, yRand - 3, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Pentagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 3, yRand - 7, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Pentagon";


//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 3, yRand - 7, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Pentagon";

//                _maxCount = 6;
//                _objetiveFigure = "Pentagon";
//                _objetiveFigureSprite.sprite = _targetImage[3];

//                break;

       
                
//            case 6:
//                 xRand = Random.Range(-15, 15);
//                 yRand = Random.Range(-8, 13);

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 2, yRand , 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand + 2 , yRand - 2, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand -4 , yRand - 2, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand - 2, yRand - 4, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _newStar = Instantiate(_starPrefab, new Vector3(xRand, yRand - 4, 0), Quaternion.identity);
//                _newStar.GetComponent<StarPreference>()._preference = true;
//                _newStar.GetComponent<StarPreference>()._objetiveFigure = "Hexagon";

//                _maxCount = 7;
//                _objetiveFigure = "Hexagon";
//                _objetiveFigureSprite.sprite = _targetImage[4];

//                break;

//        }
//        }


//    /** 

//* @desc This is the object detetector of the stars, it move int the same way of the mouse

//* @param 

//* @return 

//*/

//    void onClick()
//    {
      
//        _target =  Instantiate(_targetGameObject, _mousePosition, Quaternion.identity);
//        _target.transform.position = _mousePosition;
        
//        _pressed = true;
//    }
//    /** 

//* @desc In this part detect the collisions with the star and collect information for check if is the right answer.

//* @param Vector3 starPosition - Position of the star collision
//* @param Vector3 starPosition - GameObject of the star collision
//* @param Vector3 objectiveFigure - This name is gave when we create the point for check if the rigth geometrical form  of the star collision

//* @return 

//*/
//    public void onColisionDetected(Vector3 starPosition, GameObject star, string objectiveFigure)
//    {
        
//        if (_countCorrects < _maxCount && objectiveFigure == _objetiveFigure )
//        {
           
//            _starsSelected.Add(star);
           
//                _starsSelected[_countCorrects].gameObject.GetComponent<StarPreference>()._checked = true;

            

//            _line.positionCount = _count + 1;
//            _line.SetPosition(_count, starPosition);
//            _line.positionCount = _count + 2;

//            _count++;
//            _countCorrects++;

//        }
        
//        else 
//        {
           
//            _line.positionCount = _count + 1;

//            _line.SetPosition(_count, starPosition);
//            _line.positionCount = _count + 2;
//            _count++;
//            _failCount++;
//        }

//    }


//    /** 

//* @desc This part is for have a control when the player dosen't touching the screen, reset variables, arrays, list, positions etc. Also for check if the answer is correct

//* @param 

//* @return 

//*/
//    private void mouseController()
//    {



//#if UNITY_EDITOR
//        if (_target)
//        {
//            _mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
//                                  Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
//            _target.transform.position = _mousePosition;
//        }

//        if (Input.GetMouseButtonDown(0))
//        {
//            onClick();
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            _count = 0;
//            if (_target != null && _pressed == true)
//            {
//                Destroy(_target);
//                _pressed = false;
//            }
//        }
//#endif






//        if (_pressed == false)
//        {



//            for (int i = 0; i < _countCorrects; i++)
//            {
//                _starsSelected[i].gameObject.GetComponent<StarPreference>()._checked = false;
//            }

//            _line.positionCount = 0;
//            _countCorrects = 0;
//            if (_target != null)
//            {
//                Destroy(_target);

//            }
//            _starsSelected.Clear();

//        }


//        if (_countCorrects == _maxCount - 1)
//        {
//            _starsSelected[0].gameObject.GetComponent<StarPreference>()._checked = false;

//        }
//        if (_countCorrects == _maxCount && _count == _maxCount && _countCorrects != 0)
//        {
//            Debug.Log("correcto");
            
//            _line.positionCount = 0;
//            for (int i = 0; i < _count; i++)
//            {
//                Destroy(_starsSelected[i].gameObject);
//            }
//            _count = 0;
//            _countCorrects = 0;
//            _starsSelected.Clear();
//            _successCount++;

            
            
//            _calculatePuntuation.Puntuation(_successCount, _failCount);
//            levelDificulty();
           
//            _failCount = 0;
//            _successCount = 0;
//            _pressed = false;
//        }

//        if (_pressed && _count > 0)
//        {
//            _line.SetPosition(_count, _mousePosition);
//        }
//        else
//        {
//            _count = 0;

//        }
//#if UNITY_ANDROID


//        if (_target)
//        {
//            _mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(_touch.position).x,
//                                  Camera.main.ScreenToWorldPoint(_touch.position).y);
//            _target.transform.position = _mousePosition;
//        }

//        if (Input.touchCount > 0)
//        {
//            onClick();
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            _count = 0;

//            if (_target != null && _pressed == true)
//            {
//                Destroy(_target);
//                _pressed = false;
//            }
//        }
//#endif
//    }

//    IEnumerator refreshGeometricalForms()
//    {
//        yield return new WaitForSeconds(.5f);
//        int _rand = Random.Range(0, 6);
//        geometryGenerator(_rand);
//        StarGenerate(_density);
//    }


//    public void levelDificulty()
//    {
//        _density = _currentLevel.numAsteroids ;
//        int _range = _currentLevel.possibleGeometry.Count ;
//        if (_range != 0)
//        {
//            _geometry = Random.Range(1, _range);
           

//        }
//        geometryGenerator((int)_currentLevel.possibleGeometry[_geometry]);
//        StarGenerate(_density);
//    }
    
    
}
