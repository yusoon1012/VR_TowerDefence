using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static CSVReader instance = null;

    #region 싱글턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion

    public const char DELIMITER = ','; // CSV 파일에서 사용하는 구분자 (기본값은 콤마)

    // csv 파일의 정보를 행과 열로 구분하여 저장할 딕셔너리
    // 행은 키 값이 되고, 열은 키 값 내부의 값이 된다.
    public Dictionary<string, List<string>> dataDictionary = default;

    // CSV 파일을 읽는 함수
    // csvFileName에는 "MusicList"와 같이 파일의 이름을 입력한다. (확장자 제외)

    public Dictionary<string, List<string>> ReadCSVFile(string csvFileName)
    {
        dataDictionary = new Dictionary<string, List<string>>();

        TextAsset csvTextAsset = Resources.Load<TextAsset>(csvFileName);

        if (csvTextAsset != null)
        {
            string[] lines = csvTextAsset.text.Split('\n');

            if (lines.Length > 0)
            {
                string[] headers = lines[0].Split(DELIMITER);

                foreach (string header in headers)
                {
                    dataDictionary.Add(header, new List<string>());
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(DELIMITER);

                    for (int j = 0; j < values.Length; j++)
                    {
                        dataDictionary[headers[j]].Add(values[j]);
                    }
                }
            }
            else
            {
                Debug.LogError("CSV file is empty.");
            }
        }
        else
        {
            Debug.LogError("CSV file not found: " + csvFileName);
        }

        return dataDictionary;
    }

    // 변환된 csv 파일의 정보가 저장된 딕셔너리 내부의 값을 출력하는 함수.
    // "행:열1,열2,열3"과 같이 출력된다.
    // 매개 변수로 받을 딕셔너리의 구조는 <string, List<string>> 이어야 한다.
    public void PrintData(Dictionary<string, List<string>> dictionary)
    {
        // 딕셔너리의 각 항목을 출력
        foreach (KeyValuePair<string, List<string>> entry in dictionary)
        {
            string category = entry.Key;
            List<string> values = entry.Value;

            Debug.Log(category + ": " + string.Join(", ", values));
        }
    }

    public void WriteCSVFile(string csvFileName)
    {
        // 리소스 폴더와는 별도의 폴더에 저장할 절대 경로 설정
        string folderPath = Path.Combine(Application.dataPath, "CSVFiles");
        string filePath = Path.Combine(folderPath, csvFileName);

        // 폴더가 없으면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        StringBuilder csvContent = new StringBuilder();

        // 헤더 라인 작성
        csvContent.AppendLine(string.Join(DELIMITER.ToString(), dataDictionary.Keys));

        // 최대 데이터 행 개수 확인
        int maxRowCount = dataDictionary.Values.Max(list => list.Count);

        for (int i = 0; i < maxRowCount; i++)
        {
            List<string> row = new List<string>();
            foreach (var key in dataDictionary.Keys)
            {
                // 행 개수보다 작은 범위에서 값 가져오기
                if (i < dataDictionary[key].Count)
                {
                    row.Add(dataDictionary[key][i]);
                }
                else
                {
                    row.Add(""); // 값이 없는 경우 빈 문자열 추가
                }
            }
            csvContent.AppendLine(string.Join(DELIMITER.ToString(), row));
        }

        // 파일에 내용 쓰기
        File.WriteAllText(filePath, csvContent.ToString());

        Debug.Log("CSV file saved to: " + filePath);
    }

    // CSV 데이터를 PlayerPrefs에 저장하는 함수
    public void SaveCSVDataToPlayerPrefs(string csvFileName)
    {
        ReadCSVFile(csvFileName); // CSV 파일 읽기

        // 딕셔너리의 각 항목을 순회하면서 PlayerPrefs에 저장
        foreach (KeyValuePair<string, List<string>> entry in dataDictionary)
        {
            string category = entry.Key;
            List<string> values = entry.Value;

            // List<string>을 문자열로 변환하여 PlayerPrefs에 저장
            string valuesString = string.Join(",", values.ToArray());
            PlayerPrefs.SetString(category, valuesString);
        }

        Debug.Log("CSV data saved to PlayerPrefs.");

        // <외부에서 저장 예시>
        // CSV 데이터를 PlayerPrefs에 저장하기
        //Park_CSVReader.instance.SaveCSVDataToPlayerPrefs("MusicList");
    }
}
