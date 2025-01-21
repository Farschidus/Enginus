using Enginus.Global;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Enginus.GameState
{
	public class PalyerProfile
    {
        public Vector2 PlayerPosition
        {
            get
            {
                // Player Position for Bakery in New Game
                return new Vector2(1425, 960);
            }
        }
        public Enums.Direction PlayerDirection
        {
            get
            {
                return Enums.Direction.East;
            }
        }
        public float PlayerLayerDepth
        {
            get
            {
                return 0.9f;
            }
        }
        public Dictionary<string, bool> Checkers { get; set; }

        public PalyerProfile()
        {
            Checkers = new Dictionary<string, bool>();
            Checkers.Add("BakeryFirstVisit", true);
            Checkers.Add("TalkedWithGirl", false);
            Checkers.Add("TalkedWithBadGuy", false);
            Checkers.Add("TalkedWithPeter", false);
            Checkers.Add("FlowershopFirstVisit", false);
            Checkers.Add("LobbyFirstVisit", false);
            Checkers.Add("CardPuzzleSolved", false);
            Checkers.Add("MapTaken", false);
            Checkers.Add("Floor2FirstVisit", false);
            Checkers.Add("DoughnutGiven", false);
            Checkers.Add("GirldDialogue3 ", false);
            Checkers.Add("StoneAndCanCombined ", false);
            Checkers.Add("MizukiTired ", false);
            Checkers.Add("SafetyboxSolved ", false);
            Checkers.Add("FilmMagGiven ", false);
            Checkers.Add("DollhouseOpen ", false);
        }
    }
}