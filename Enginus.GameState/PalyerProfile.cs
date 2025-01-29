using Enginus.Global;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Enginus.GameState
{
	public class PalyerProfile
    {
        public static Vector2 PlayerPosition => new(1425, 960); // Initial player position in Bakery when start a new game
        public static Enums.Direction PlayerDirection => Enums.Direction.East;
        public static float PlayerLayerDepth => 0.9f;
        public Dictionary<string, bool> Checkers { get; set; }

        public PalyerProfile()
        {
            Checkers = new Dictionary<string, bool>
            {
                { "BakeryFirstVisit", true },
                { "TalkedWithGirl", false },
                { "TalkedWithBadGuy", false },
                { "TalkedWithPeter", false },
                { "FlowershopFirstVisit", false },
                { "LobbyFirstVisit", false },
                { "CardPuzzleSolved", false },
                { "MapTaken", false },
                { "Floor2FirstVisit", false },
                { "DoughnutGiven", false },
                { "GirldDialogue3 ", false },
                { "StoneAndCanCombined ", false },
                { "MizukiTired ", false },
                { "SafetyboxSolved ", false },
                { "FilmMagGiven ", false },
                { "DollhouseOpen ", false }
            };
        }
    }
}