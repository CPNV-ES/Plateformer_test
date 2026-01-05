using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:7379";

    public void GetTopPlayers(int count)
    {
        StartCoroutine(GetTopPlayersRoutine(count));
    }

    private IEnumerator GetTopPlayersRoutine(int count)
    {
        string url = $"{baseUrl}/ZREVRANGE/leaderboard/0/{count - 1}/WITHSCORES";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Leaderboard brut : " + request.downloadHandler.text);
                // Tu peux parser ici pour l'UI
            }
        }
    }
}
