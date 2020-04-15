using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ground : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer cloud;

    [SerializeField]
    SpriteRenderer builds;

    [SerializeField]
    SpriteRenderer bottomGround;

    [SerializeField]
    SpriteRenderer overlay;

    [SerializeField]
    SpriteRenderer doc;

    [SerializeField]
    Button playBtn;

    [SerializeField]
    Button tryAgainBtn;

    [SerializeField]
    Text Score;

    [SerializeField]
    Text Instruction;
    
    public int scoreCount = 0;
    private CanvasScaler canvas;
    void Start()
    {
        GenerateCollidersAcrossScreen();
        resizeToWorld(bottomGround);
        resizeToWorld(builds);
        resizeToWorld(cloud);
        tryAgainBtn.gameObject.SetActive(false);
        playBtn.onClick.AddListener(onPlayBtnClicked);
        tryAgainBtn.onClick.AddListener(onPlayAgainBtnClicked);
        canvas = Score.GetComponentInParent<CanvasScaler>();
      
        //Score.rectTransform.anchoredPosition = new Vector3(0, (canvas.referenceResolution.y * 0.76f), 0);
    }

    public void onPlayBtnClicked()
    {
        startPlay();
    }
    public void onPlayAgainBtnClicked()
    {
        startPlay();
    }

    private void startPlay()
    {
        scoreCount = 0;
        //Score.rectTransform.anchoredPosition = new Vector3(0, -(canvas.referenceResolution.y * 0.75f), 0);
        Score.text = "Score ~ " + scoreCount;
        playBtn.gameObject.SetActive(false);
        overlay.gameObject.SetActive(false);
        tryAgainBtn.gameObject.SetActive(false);
        Instruction.gameObject.SetActive(false);
        var docScript = doc.GetComponent<doc>();
        docScript.callStart();
        docScript.isGrounded = true;
        docScript.isRunning = true;
        docScript.waitTime = DateTime.Now.AddSeconds(3);
        if (docScript.onDocOutOFGroundListner == null)
        {
            docScript.onDocOutOFGroundListner = () =>
            {
                //Score.rectTransform.anchoredPosition = new Vector3(0, (canvas.referenceResolution.y * 0.5f), 0);
                docScript.isGrounded = false;
                docScript.isRunning = false;
                overlay.gameObject.SetActive(true);
                tryAgainBtn.gameObject.SetActive(true);
                Instruction.gameObject.SetActive(true);
                return true;
            };
        }

        if(docScript.onDocKilledCoronaListner == null)
        {
            docScript.onDocKilledCoronaListner = () =>
            {
                scoreCount++;
                Score.text = "Score ~ " + scoreCount;
                return true;
            };
        }
    }

    void resizeToWorld(SpriteRenderer sprite)
    {
        var cHeight = Camera.main.orthographicSize * 2.0f;
        var cWidth = cHeight  * Screen.width / Screen.height;
        
        Sprite s = sprite.sprite;
        float unitWidth = s.textureRect.width / s.pixelsPerUnit;
        float newWidth = cWidth / unitWidth;
        float newHeight = sprite.transform.localScale.y * (newWidth / sprite.transform.localScale.x);
        //print($"{newWidth} / {newHeight} $ {sprite.transform.localScale.x}/{sprite.transform.localScale.y}");
        sprite.transform.localScale = new Vector3(newWidth, newHeight, sprite.transform.localScale.z);

    }

    void GenerateCollidersAcrossScreen()
    {
        var camera = Camera.main;
        Vector2 lDCorner = camera.ViewportToWorldPoint(new Vector3(0, 0f, camera.nearClipPlane));
        Vector2 rUCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        Vector2[] colliderpoints;

        EdgeCollider2D upperEdge = new GameObject("upperEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = upperEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        upperEdge.points = colliderpoints;

        EdgeCollider2D lowerEdge = new GameObject("lowerEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = lowerEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        lowerEdge.points = colliderpoints;

        EdgeCollider2D leftEdge = new GameObject("leftEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = leftEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderpoints;

        EdgeCollider2D rightEdge = new GameObject("rightEdge").AddComponent<EdgeCollider2D>();
       
        colliderpoints = rightEdge.points;
        colliderpoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderpoints;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
