using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// Sends stats to Redis on level completion.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        [Header("Database Connection")]
        [Tooltip("Drag your RedisManager GameObject here")]
        public RedisManager redisManager;

        [Header("Player Settings")]
        [Tooltip("Player name saved in PlayerPrefs")]
        public string playerPrefsKey = "PlayerName";

        void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // ===========================
                // 1️⃣ Collect stats
                // ===========================
                LevelStats stats = new LevelStats
                {
                    playerName = PlayerPrefs.GetString(playerPrefsKey, "Unknown"),
                    score = GameStats.CalculateScore(),
                    kills = GameStats.Kills,
                    gems = GameStats.Gems,
                    timePlayed = Time.timeSinceLevelLoad
                };

                // ===========================
                // 2️⃣ Send to Redis
                // ===========================
                if (redisManager != null)
                {
                    redisManager.SaveStats(stats);
                    Debug.Log($"Stats sent to Redis: {stats.playerName} | Score: {stats.score}");
                }
                else
                {
                    Debug.LogWarning("RedisManager is not assigned in the VictoryZone Inspector!");
                }

                // ===========================
                // 3️⃣ Execute original victory logic
                // ===========================
                var ev = Schedule<PlayerEnteredVictoryZone>();
                ev.victoryZone = this;
            }
        }
    }
}
