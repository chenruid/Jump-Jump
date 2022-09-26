using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float Factor = 1;

    public float MaxDistance = 2;

    public GameObject Stage;
    public GameObject currentStage;

    public Transform Camera;

    public Text ScoreText;

    private Rigidbody rigibody;
    private float _startTime;
    private Collider lastCollisionCollider;
    private Vector3 cameraRelativePosition;

    private int _score = 0;

    private float GetRelativePosition(Vector3 origin, Vector3 position)
    {
        Vector3 distance = origin - position;
        return distance.y;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        rigibody = GetComponent<Rigidbody>();
        rigibody.centerOfMass = Vector3.zero;

        //调用stage
        currentStage = Stage;
        lastCollisionCollider = currentStage.GetComponent<Collider>();
        SpawnStage();

        cameraRelativePosition = Camera.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //检测按下空格的时间
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //记录按下空格的时间
            _startTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            var elapse = Time.time - _startTime;
            OnJump(elapse);
        }
    }
        void OnJump(float elapse)
        {
            rigibody.AddForce(new Vector3(1, 1, 0) * elapse * Factor, ForceMode.Impulse); 
        }

        void SpawnStage()
        {   
            var stage = Instantiate(Stage);
            stage.transform.position = currentStage.transform.position + new Vector3(Random.Range(1.1f, MaxDistance), 0, 0);
        }

       void OnCollisionEnter(Collision collision)
       {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name.Contains("Stage") && collision.collider != lastCollisionCollider)
            {
                lastCollisionCollider = collision.collider;
                currentStage = collision.gameObject;
                SpawnStage();
                Movecamera();

            //如果落在中心的位置 score +2
            Debug.Log("GameObject.Position: " + collision.gameObject.transform.position.ToString());
            Debug.Log("Rigibody.Position: " + rigibody.position.ToString());
            if (collision.gameObject != null && GetRelativePosition(collision.gameObject.transform.position, rigibody.position) <= 0.3)
            {
                _score = _score + 2;
                ScoreText.text = _score.ToString();
            }
            else
            {
                _score++;
                ScoreText.text = _score.ToString();
            } 


            }

            if(collision.gameObject.name == "Ground")
            {
                SceneManager.LoadScene(0);
                
            }
       }

       void Movecamera()
       {
            Camera.DOMove(transform.position + cameraRelativePosition, 1);
            
       }

}
