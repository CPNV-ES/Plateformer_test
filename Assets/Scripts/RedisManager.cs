using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RedisManager : MonoBehaviour
{
    // Webdis (PAS Redis direct)
    public string baseUrl = "http://localhost:7379";

    // =========================
    // SAVE STATS (JSON)
    // =========================
    public void SaveStats(LevelStats stats)
    {
        StartCoroutine(SaveStatsRoutine(stats));
        AddScoreToLeaderboard(stats.playerName, stats.score);
    }

    private IEnumerator SaveStatsRoutine(LevelStats stats)
    {
        string json = JsonUtility.ToJson(stats);
        string key = "stats:" + stats.playerName;
        string url = $"{baseUrl}/SET/{key}/{UnityWebRequest.EscapeURL(json)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError(request.error);
            else
                Debug.Log("Stats sauvegardées !");
        }
    }

    // =========================
    // LEADERBOARD
    // =========================
    public void AddScoreToLeaderboard(string playerName, int score)
    {
        StartCoroutine(AddScoreRoutine(playerName, score));
    }

    private IEnumerator AddScoreRoutine(string playerName, int score)
    {
        string url = $"{baseUrl}/ZADD/leaderboard/{score}/{playerName}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError(request.error);
            else
                Debug.Log("Score ajouté au leaderboard !");
        }
    }
}
