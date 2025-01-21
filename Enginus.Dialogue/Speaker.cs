using Enginus.Global;
using Microsoft.Xna.Framework;

namespace Enginus.Dialogue
{
	public enum PointerType
    {
        None,
        Left,
        Right,
        Center
    }

    public class Speaker
    {
        #region Properties

        public Vector2 Position;
        public int Width;
        public PointerType PointerType;
        public bool HasContinue;
        public Enums.Characters Character;
        public string Dialogue;
        public int? AnswerIndex;
        public string VoiceFileName;
        public string AnimationName;
        public bool HasAnswer
        {
            get { return (AnswerIndex != null); }
        }
        public byte CharacterID
        {
            get { return (byte)Character; }
        }
        

        #endregion

        #region Constructor

        /// <summary>
        /// Add a new Speaker to a Conversation
        /// </summary>
        public Speaker(Vector2 BalloonPosition, int BalloonWidth, PointerType Type, bool hasContinue, Enums.Characters character, string dialogue, int? answerIndex, string voiceFileName, string animationName)
        {
            Position = BalloonPosition;
            Width = BalloonWidth;
            PointerType = Type;
            HasContinue = hasContinue;
            Character = character;
            Dialogue = dialogue;
            AnswerIndex = answerIndex;
            VoiceFileName = voiceFileName;
            AnimationName = animationName;
        }
        /// <summary>
        /// Needed for Serialization. Not intended for use.
        /// </summary>
        public Speaker()
        {
            Position = Vector2.Zero;
            Width = 0;
            HasContinue = false;
            PointerType = PointerType.None;
            Character = Enums.Characters.Mizuki;
            Dialogue = VoiceFileName = AnimationName = "";
            AnswerIndex = null;
        }

        #endregion
    }
}