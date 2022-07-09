using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{   
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    public string inputName;
    public Text bestScoreText;
    private int bestPoints;
    private string recordName;
    private int recordPoints;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        inputName = MenuManager.Instance.inputText;
        Debug.Log("Nam: " + inputName);

        LoadStats();
        bestScoreText.text = "Best Score: " + recordPoints + " Name: " + recordName;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(m_Points > recordPoints){
            bestPoints= m_Points;
            bestScoreText.text = "Best Score: " + bestPoints + " Name: " + inputName;
            SaveStats();
        }
    }

    [System.Serializable]
    class SaveData{
        public string name;
        public int score;
    }

    public void SaveStats(){
        SaveData data = new SaveData();
        data.name = inputName;
        data.score = bestPoints;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "savefile.json", json);
    }

    public void LoadStats(){
        Debug.Log("Stats load");
        string path = Application.persistentDataPath + "savefile.json";
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SaveData data= JsonUtility.FromJson<SaveData>(json);

            recordName = data.name;
            recordPoints = data.score;
            Debug.Log("Stats loaded already");
        }
    }

    public void BackToMenu(){
        SceneManager.LoadScene(0);
    }

}
