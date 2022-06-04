using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tapa : MonoBehaviour
{
    public GameObject[] engranajes = new GameObject[6];
    public Camera mainCamera;
    public bool presionado;
   
   
    public GameObject tapon;

    public Animation anim;

    private void Start()
    {
        //anim = gameObject.GetComponent<Animation>();
    }
    void Update()
    {
        RaycastHit2D hit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (hit = Physics2D.Raycast(ray.origin, new Vector2(0, 50)))
        {
         
            // transform.position = new Vector3(0f, 0.001f, 0f);
            Debug.Log(hit.collider.name);
            if (presionado == false)
            {
                
                tapon.transform.localPosition = new Vector3(0, tapon.transform.localPosition.y + 300 , 0);
                Show();
            }
            presionado = true;
           
            
            
           

        }

        void Show ()
        {
            presionado = true;
            for (int i = 0; i < engranajes.Length; i++)
            {
                engranajes[i].SetActive(true);
            }
        }
           
    }

}
    

   
