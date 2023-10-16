using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static CSVReader instance = null;

    #region �̱���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion

    public const char DELIMITER = ','; // CSV ���Ͽ��� ����ϴ� ������ (�⺻���� �޸�)

    // csv ������ ������ ��� ���� �����Ͽ� ������ ��ųʸ�
    // ���� Ű ���� �ǰ�, ���� Ű �� ������ ���� �ȴ�.
    public Dictionary<string, List<string>> dataDictionary = default;

    // CSV ������ �д� �Լ�
    // csvFileName���� "MusicList"�� ���� ������ �̸��� �Է��Ѵ�. (Ȯ���� ����)

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

    // ��ȯ�� csv ������ ������ ����� ��ųʸ� ������ ���� ����ϴ� �Լ�.
    // "��:��1,��2,��3"�� ���� ��µȴ�.
    // �Ű� ������ ���� ��ųʸ��� ������ <string, List<string>> �̾�� �Ѵ�.
    public void PrintData(Dictionary<string, List<string>> dictionary)
    {
        // ��ųʸ��� �� �׸��� ���
        foreach (KeyValuePair<string, List<string>> entry in dictionary)
        {
            string category = entry.Key;
            List<string> values = entry.Value;

            Debug.Log(category + ": " + string.Join(", ", values));
        }
    }

    public void WriteCSVFile(string csvFileName)
    {
        // ���ҽ� �����ʹ� ������ ������ ������ ���� ��� ����
        string folderPath = Path.Combine(Application.dataPath, "CSVFiles");
        string filePath = Path.Combine(folderPath, csvFileName);

        // ������ ������ ����
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        StringBuilder csvContent = new StringBuilder();

        // ��� ���� �ۼ�
        csvContent.AppendLine(string.Join(DELIMITER.ToString(), dataDictionary.Keys));

        // �ִ� ������ �� ���� Ȯ��
        int maxRowCount = dataDictionary.Values.Max(list => list.Count);

        for (int i = 0; i < maxRowCount; i++)
        {
            List<string> row = new List<string>();
            foreach (var key in dataDictionary.Keys)
            {
                // �� �������� ���� �������� �� ��������
                if (i < dataDictionary[key].Count)
                {
                    row.Add(dataDictionary[key][i]);
                }
                else
                {
                    row.Add(""); // ���� ���� ��� �� ���ڿ� �߰�
                }
            }
            csvContent.AppendLine(string.Join(DELIMITER.ToString(), row));
        }

        // ���Ͽ� ���� ����
        File.WriteAllText(filePath, csvContent.ToString());

        Debug.Log("CSV file saved to: " + filePath);
    }

    // CSV �����͸� PlayerPrefs�� �����ϴ� �Լ�
    public void SaveCSVDataToPlayerPrefs(string csvFileName)
    {
        ReadCSVFile(csvFileName); // CSV ���� �б�

        // ��ųʸ��� �� �׸��� ��ȸ�ϸ鼭 PlayerPrefs�� ����
        foreach (KeyValuePair<string, List<string>> entry in dataDictionary)
        {
            string category = entry.Key;
            List<string> values = entry.Value;

            // List<string>�� ���ڿ��� ��ȯ�Ͽ� PlayerPrefs�� ����
            string valuesString = string.Join(",", values.ToArray());
            PlayerPrefs.SetString(category, valuesString);
        }

        Debug.Log("CSV data saved to PlayerPrefs.");

        // <�ܺο��� ���� ����>
        // CSV �����͸� PlayerPrefs�� �����ϱ�
        //Park_CSVReader.instance.SaveCSVDataToPlayerPrefs("MusicList");
    }
}
