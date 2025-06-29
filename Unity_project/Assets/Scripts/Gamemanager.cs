using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public RawImage imageUI;
    public Text infoText;

    [System.Serializable]
    public class ImageData
    {
        public string image;
        public string date;
        public string day;
        public string time;
    }

    public void OnReceiveImageData(string json)
    {
        Debug.Log("📦 受信データ: " + json);
        ImageData data = JsonUtility.FromJson<ImageData>(json);

        infoText.text = $"📅 {data.date}（{data.day}）\n🕒 {data.time}";
        StartCoroutine(LoadImage(data.image));
    }

    IEnumerator LoadImage(string base64)
    {
        byte[] bytes = System.Convert.FromBase64String(base64.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", ""));
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        imageUI.texture = tex;
        yield return null;
    }
}
