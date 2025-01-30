using Enginus.Core;
using Microsoft.Xna.Framework;
using System;

namespace Enginus.GameState
{
    public class StateManager
    {
        public Vector2 PlayerPosition => PalyerProfile.PlayerPosition;
        public Direction PlayerDirection => PalyerProfile.PlayerDirection;
        public float PlayerLayerDepth => PalyerProfile.PlayerLayerDepth;
        public bool VarA { get; set; }

		private PalyerProfile profile;
        private static StateManager instance;
        private readonly static object syncLock = new();

        public static StateManager GetGameState()
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    instance ??= new StateManager();
                }
            }
            return instance;
        }
        protected StateManager()
        {
            VarA = true;
            profile = new PalyerProfile();
        }

        public void LoadProfile(PalyerProfile playerProfile)
        {
            profile = playerProfile;
        }
        public void SaveProfile()
        {
            throw new NotImplementedException();
        }

        public void ChangeState(string key, bool value)
        {
            profile.Checkers[key] = value;
        }
    }
}