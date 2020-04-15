using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    float relativeScreenWidth = 0.0f;
    private AudioSource die;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.name == "doc")
        {
            //gameObject.SetActive(false);
            die.Play();
            repositonSelf();
        }
    }
    

    void repositonSelf()
    {
        Vector3 position = new Vector3(Random.Range(-relativeScreenWidth, relativeScreenWidth), Random.Range(-(Camera.main.orthographicSize / 2), 0), -1);
        transform.position = position;
        print(position);
        //gameObject.SetActive(true);
        //die.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        die = GetComponent<AudioSource>();
        
        var cHeight = Camera.main.orthographicSize * 2.0f;
        relativeScreenWidth = (cHeight * Screen.width / Screen.height) / 2;
        repositonSelf();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
