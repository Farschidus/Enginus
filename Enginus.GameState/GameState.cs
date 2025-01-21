using Enginus.Global;
using Microsoft.Xna.Framework;
using System;

namespace Enginus.GameState
{
	public class StateManager
    {
        public Vector2 PlayerPosition
        {
            get
            {
                return profile.PlayerPosition;
            }
        }
        public Enums.Direction PlayerDirection
        {
            get
            {
                return profile.PlayerDirection;
            }
        }
        public float PlayerLayerDepth
        {
            get
            {
                return profile.PlayerLayerDepth;
            }
        }
		public bool VarA { get; set; }

		private PalyerProfile profile;
        private static StateManager instance;
        private static object syncLock = new object();

        public static StateManager GetGameState()
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new StateManager();
                    }
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