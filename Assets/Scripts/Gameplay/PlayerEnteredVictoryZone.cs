using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // Animation victoire
            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;

            // 🔥 ENVOI DES STATS
            StatsExporter statsExporter = Object.FindFirstObjectByType<StatsExporter>();

            if (statsExporter != null)
            {
                statsExporter.OnLevelFinished();
            }
            else
            {
                Debug.LogError("StatsExporter introuvable dans la scène !");
            }
        }
    }
}
