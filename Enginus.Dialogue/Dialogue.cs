using Enginus.Control;
using Enginus.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Enginus.Dialogue
{
	public static class Dialogue
    {
        #region Properties

        public static List<Speaker> ConversationSpeakers = new List<Speaker>();
        public static ContentManager Content;
        public static SpriteFont spriteFont;
        //public static Song Voice;
        public static AudioManager AudioEngine; 
        public static bool InConversation = false;
        public static bool Expired = true;
        public static string Text
        {
            get { return text; }
            set { text = value; }
        }

        private static int currentSpeakerIndex = 0;
        private static string text;
        private static string[] revealedMessage;
        private const string texturePath = "Dialogues";
        //bubble textures
        private static Texture2D bottomBorder;
        private static Texture2D innerBackground;
        private static Texture2D leftBorder;
        private static Texture2D leftBottomCorner;
        private static Texture2D leftTopCorner;
        private static Texture2D rightBorder;
        private static Texture2D rightBottomCorner;
        private static Texture2D rightTopCorner;
        private static Texture2D topBorder;
        private static Texture2D continueGraphic;
        private static Texture2D pointer;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes the Conversation Class
        /// </summary>
        /// <param name="font">Font to display text with</param>
        public static void Initialize(ContentManager content, AudioManager Audio)
        {
            AudioEngine = Audio;
            Content = content;

            spriteFont = content.Load<SpriteFont>(string.Format("{0}/DialoguesTahoma", texturePath));

            innerBackground = content.Load<Texture2D>(string.Format("{0}/interior", texturePath));
            bottomBorder = content.Load<Texture2D>(string.Format("{0}/bottomBorder", texturePath));
            leftBorder = content.Load<Texture2D>(string.Format("{0}/leftBorder", texturePath));
            leftBottomCorner = content.Load<Texture2D>(string.Format("{0}/leftBottomCorner", texturePath));
            leftTopCorner = content.Load<Texture2D>(string.Format("{0}/leftTopCorner", texturePath));
            rightBorder = content.Load<Texture2D>(string.Format("{0}/rightBorder", texturePath));
            rightBottomCorner = content.Load<Texture2D>(string.Format("{0}/rightBottomCorner", texturePath));
            rightTopCorner = content.Load<Texture2D>(string.Format("{0}/rightTopCorner", texturePath));
            topBorder = content.Load<Texture2D>(string.Format("{0}/topBorder", texturePath));
            pointer = content.Load<Texture2D>(string.Format("{0}/pointer", texturePath));
            continueGraphic = content.Load<Texture2D>(string.Format("{0}/more", texturePath));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts a new Conversation
        /// </summary>
        /// <param name="sceneNo">Scene number to use</param>
        public static void StartConversation(int sceneNo, int dialogueIndex)
        {
            Expired = false;
            LoadConversation(sceneNo, dialogueIndex);

            revealedMessage = constrainText(ConversationSpeakers[currentSpeakerIndex].Dialogue, ConversationSpeakers[currentSpeakerIndex].Width);
            AudioEngine.LoadSound(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
            AudioEngine.PlaySound(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
            //Voice = Content.Load<Song>(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
            //MediaPlayer.Play(Voice);
        }
        public static void HandleInput(InputState input)
        {
            if (input.MouseClicked && !Expired)
            {
                ShowNext();
            }
        }
        public static void Update(GameTime gameTime)
        {
            if (!Expired)
            {
                InConversation = true;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!Expired)
            {
                DrawBalloon(spriteBatch, ConversationSpeakers[currentSpeakerIndex]);
            }
        }
        private static void DrawBalloon(SpriteBatch sb, Speaker speaker)
        {
            //top
            sb.Draw(leftTopCorner, speaker.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);
            sb.Draw(topBorder, new Rectangle((int)speaker.Position.X + leftTopCorner.Width, (int)speaker.Position.Y, speaker.Width - leftTopCorner.Width * 2, leftTopCorner.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.998f);
            sb.Draw(rightTopCorner, speaker.Position + new Vector2(speaker.Width - rightTopCorner.Width, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);

            //lines
            for (int i = 0; i < revealedMessage.Length + (speaker.HasContinue ? 1 : 0); i++)
            {
                sb.Draw(leftBorder, new Vector2(speaker.Position.X, speaker.Position.Y + leftTopCorner.Height + (i * leftBorder.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);
                sb.Draw(innerBackground, new Rectangle((int)speaker.Position.X + leftBorder.Width,
                                        (int)speaker.Position.Y + leftTopCorner.Height + (i * leftBorder.Height),
                                        speaker.Width - leftBorder.Width * 2,
                                        leftBorder.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.998f);
                sb.Draw(rightBorder, new Vector2(speaker.Position.X + speaker.Width - rightBorder.Width, speaker.Position.Y + leftTopCorner.Height + (i * leftBorder.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);

                //leave space for more graphic if necessary
                if (i < revealedMessage.Length)
                    sb.DrawString(spriteFont, revealedMessage[i], new Vector2((int)speaker.Position.X + leftBorder.Width, (int)speaker.Position.Y + leftTopCorner.Height + (i * leftBorder.Height)), Color.Black, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

            //bottom
            sb.Draw(leftBottomCorner, speaker.Position + new Vector2(0, leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);

            switch (speaker.PointerType)
            {
                case PointerType.Left:
                    {
                        sb.Draw(pointer, speaker.Position + new Vector2(leftBottomCorner.Width, leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);
                        sb.Draw(bottomBorder, new Rectangle((int)speaker.Position.X + leftBorder.Width + pointer.Width,
                                                            (int)speaker.Position.Y + leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height,
                                                            speaker.Width - leftBorder.Width - pointer.Width - rightBorder.Width,
                                                            bottomBorder.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.998f);
                        break;
                    }
                case PointerType.Right:
                    {
                        sb.Draw(bottomBorder, new Rectangle((int)speaker.Position.X + leftBorder.Width,
                                                            (int)speaker.Position.Y + leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height,
                                                            speaker.Width - leftBorder.Width - pointer.Width - rightBorder.Width,
                                                            bottomBorder.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.998f);
                        sb.Draw(pointer, speaker.Position + new Vector2(speaker.Width - pointer.Width - rightBorder.Width, leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);
                        break;
                    }
                case PointerType.None:
                    {
                        sb.Draw(bottomBorder, new Rectangle((int)speaker.Position.X + leftBorder.Width,
                                                            (int)speaker.Position.Y + leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height,
                                                            speaker.Width - leftBorder.Width - rightBorder.Width,
                                                            bottomBorder.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.998f);
                        break;
                    }
            }

            sb.Draw(rightBottomCorner, speaker.Position + new Vector2(speaker.Width - rightBottomCorner.Width, leftTopCorner.Height + (revealedMessage.Length + (speaker.HasContinue ? 1 : 0)) * leftBorder.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.998f);

            if (speaker.HasContinue)
                sb.Draw(continueGraphic, new Vector2(speaker.Position.X + speaker.Width - rightBorder.Width - continueGraphic.Width, speaker.Position.Y + leftTopCorner.Height + (revealedMessage.Length * leftBorder.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads a Conversation File
        /// </summary>
        private static void LoadConversation(int sceneNo, int dialogueIndex)
        {
            ConversationSpeakers = null;
            ConversationSpeakers = Content.Load<List<Speaker>>(@"Dialogues\" + sceneNo);
            currentSpeakerIndex = dialogueIndex;
        }
        /// <summary>
        /// Continue the Conversation
        /// </summary>
        private static void ShowNext()
        {
            MediaPlayer.Stop();

            // Dialogue WITHOUT Answer
            if (!ConversationSpeakers[currentSpeakerIndex].HasAnswer)
            {
                Expired = true;
                InConversation = false;
                currentSpeakerIndex = -1;
                revealedMessage = null;
            }
            // Dialogue WITH Answer
            else
            {
                // Going to Answer Item in list
                currentSpeakerIndex = ConversationSpeakers[currentSpeakerIndex].AnswerIndex.Value;
                revealedMessage = constrainText(ConversationSpeakers[currentSpeakerIndex].Dialogue, ConversationSpeakers[currentSpeakerIndex].Width);
                AudioEngine.LoadSound(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
                AudioEngine.PlaySound(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
                //Voice = Content.Load<Song>(ConversationSpeakers[currentSpeakerIndex].VoiceFileName);
                //MediaPlayer.Play(Voice);
            }
        }
        /// <summary>
        /// Breaks up a string so it fits in the box. Will split long messages into two or three if necessary
        /// </summary>
        /// <param name="message">Speaker Message String</param>
        /// <returns>Formatted String</returns>
        private static string[] constrainText(string message, int dialogWidth)
        {
            string line = "";
            string returnString = "";
            string[] wordArray = message.Split(' ');

            // Go through each word in string
            foreach (string word in wordArray)
            {
                // If we add the next word to the current line and go beyond the width...
                if (spriteFont.MeasureString(line + word).X > dialogWidth)
                {
                    returnString += line + "|";
                    line = "";
                }
                line += word + " ";
            }

            returnString += line;

            return returnString.Split('|');
        }

        #endregion
    }
}