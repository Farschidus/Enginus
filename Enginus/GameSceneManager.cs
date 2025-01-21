using Enginus.Control;
using Enginus.Global;
using Enginus.Inventory;
using Enginus.InventorySystem;
using Enginus.Navigation;
using Enginus.Particle;
using Enginus.SceneObject;
using Enginus.Screen;
using Enginus.StateMachine;
using Jint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus
{
	public class GameSceneManager : GameScene
    {
        Icon InventoryIcon;
        MapIcon mapIcon;
        public GameSceneManager(string sceneName, string background, string music, Vector2 playerPosition, Enums.Direction playerDirection, float playerLayerDepth)
            : base(sceneName, background, music, playerPosition, playerDirection, playerLayerDepth)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();
            InventoryIcon = new Icon(Content);
            mapIcon = new MapIcon(Content);
            NavMesh meshi = this.SceneMesh;
            
            ActionFunctions af = new ActionFunctions();
            ScreenManager.JSEngine.SetValue("Actions", af);
            ScreenManager.JSEngine.SetValue("Scene", this);

            RegisterScriptsObjects();
            //Dialogue.Initialize(content, this.ScreenManager.Audio);
            //Dialogue.StartConversation(2, 0);
        }
        private void RegisterScriptsObjects()
        {
            ScreenManager.JSEngine.SetValue("Player", this.player);
            foreach (ExitObject exit in SceneExits)
            {
                ScreenManager.JSEngine.SetValue(exit.Name, exit);
                exit.FSMInit(this);
            }
            foreach (InteractiveObject inter in SceneInteractives)
            {
                ScreenManager.JSEngine.SetValue(inter.Name, inter);
                inter.FSMInit(this);
            }
            foreach (PopupTriggerObject mini in SceneMiniGameTrigers)
            {
                ScreenManager.JSEngine.SetValue(mini.Name, mini);
                mini.FSMInit(this);
            }
            foreach (LoopAnimationObject auto in SceneAutoSprites)
            {
                ScreenManager.JSEngine.SetValue(auto.Name, auto);
                auto.FSMInit(this);
            }
            foreach (NpcObject npc in SceneCharacters)
            {
                ScreenManager.JSEngine.SetValue(npc.Name, npc);
                npc.FSMInit(this);
            }
            foreach (InventoryObject item in SceneInventoryItems)
            {
                ScreenManager.JSEngine.SetValue(item.Name, item);
                item.FSMInit(this);
            }
        }
        public override void UnloadContent()
        {
            //TODO: Why I did this?
            ScreenManager.JSEngine = new Engine();
            GameState.StateManager State = GameState.StateManager.GetGameState();
            ScreenManager.JSEngine.SetValue("State", State);
            base.UnloadContent();
        }
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input); 
            //Dialogue.HandleInput(input);
            foreach (ExitObject exit in SceneExits)
            {
                exit.HandleInput(input, ScreenManager.Cursor);
            }
            foreach (InteractiveObject InteractiveObj in SceneInteractives)
            {
                InteractiveObj.HandleInput(input, ScreenManager.Cursor);
            }
            foreach (PopupTriggerObject miniGameObj in SceneMiniGameTrigers)
            {
                miniGameObj.HandleInput(input, ScreenManager.Cursor);
            }
            foreach (NpcObject SceneCharacter in SceneCharacters)
            {
                SceneCharacter.HandleInput(input, ScreenManager.Cursor);
            }
            foreach (InventoryObject Item in SceneInventoryItems)
            {
                Item.HandleInput(input, ScreenManager.Cursor);
            }

            InventoryIcon.HandleInput(input);
            if (InventoryIcon.IsHover && input.MouseClicked)
            {
                ScreenManager.AddScreen(new Screen.Inventory(), ControllingPlayer);
            }
            mapIcon.HandleInput(input);
            if (mapIcon.IsHover && input.MouseClicked)
            {
                ScreenManager.AddScreen(new Map(), ControllingPlayer);
            }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen, InputState input)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen, input);
            if (IsActive)
            {
                //Dialogue.Update(gameTime);
                if(plotter.Mizuki)
                    player.Update(gameTime, elapsedTime, input, SceneMesh);

                foreach (ExitObject exit in SceneExits)
                {
                    exit.Update(gameTime, this);
                }
                foreach (InteractiveObject InteractiveObj in SceneInteractives)
                {
                    InteractiveObj.Update(gameTime, this);
                }
                foreach (PopupTriggerObject miniGameObj in SceneMiniGameTrigers)
                {
                    miniGameObj.Update(this);
                }
                foreach (LoopAnimationObject SceneAuto in SceneAutoSprites)
                {
                    SceneAuto.Update(gameTime, this);
                }
                foreach (NpcObject npc in SceneCharacters)
                {
                    npc.Update(gameTime, this);
                }
                foreach (InventoryObject Item in SceneInventoryItems)
                {
                    //Item.Update(input, ScreenManager.Cursor, ScreenManager.InventoryManager);
                    Item.Update(gameTime, this);
                }
                foreach (ParticleEngine particle in SceneParticles)
                {
                    particle.Update(gameTime);
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Resolution.getScaleMatrix());

            spriteBatch.Draw(backgroundTexture, backgroundRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.05f);
            //Dialogue.Draw(spriteBatch);
            foreach (ExitObject Exit in SceneExits)
            {
                Exit.Draw(gameTime, spriteBatch);
            }
            foreach (InteractiveObject InteractiveObj in SceneInteractives)
            {
                InteractiveObj.Draw(gameTime, spriteBatch);
            }
            foreach (PopupTriggerObject miniGameObj in SceneMiniGameTrigers)
            {
                miniGameObj.Draw(gameTime, spriteBatch);
            }
            foreach (NpcObject character in SceneCharacters)
            {
                character.Draw(gameTime, spriteBatch);
            }
            foreach (LoopAnimationObject SceneAuto in SceneAutoSprites)
            {
                SceneAuto.Draw(gameTime, spriteBatch);
            }
            foreach (InventoryObject Item in SceneInventoryItems)
            {
                if (!ScreenManager.InventoryManager.HasItem(Item.itemId))
                    Item.Draw(gameTime, spriteBatch);
            }
            foreach (ForgroundObject Obstacle in SceneForgrounds)
            {
                Obstacle.Draw(gameTime, spriteBatch);
            }
            foreach (ParticleEngine particle in SceneParticles)
            {
                particle.Draw(gameTime, spriteBatch);
            }  
            Player.Draw(gameTime, spriteBatch);
            
            plotter.Draw(gameTime, spriteBatch);
            InventoryIcon.Draw(gameTime, spriteBatch);
            mapIcon.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }

        public static GameScene InitNextScene(string SceneName, ScreenManager screenManager, Vector2 playerPosition, Enums.Direction playerDirection, float playerLayerDepth)
        {
            ContentManager content = new ContentManager(screenManager.Game.Services, "Content");
            GameAssets gameAssets = content.Load<GameAssets>(Constants.Scenes_GameAssets_File);
            Scene sceneAssets = gameAssets.Scenes.Find(x => x.SceneName.Equals(SceneName));
            GameSceneManager nextScene = new GameSceneManager(sceneAssets.SceneName, sceneAssets.Background, sceneAssets.SceneMusic, playerPosition, playerDirection, playerLayerDepth);

            foreach (Exit exit in sceneAssets.Exits)
                nextScene.SceneExits.Add(new ExitObject(exit.Rectangle, content, Constants.Image_ExitObjects, exit.NextSceneName, exit.PlayerPosition, exit.PlayerDirection, exit.PlayerLayerDepth, exit.InitGroup.Trim()));
            foreach (LoopAnimation auto in sceneAssets.LoopAnimations)
            {
                LoopAnimationObject loopAnimation = new LoopAnimationObject(auto.Name, content, true, auto.LayerDepth);
                foreach (AnimationSprite animation in auto.AnimationSprites)
                {
                    loopAnimation.AddAnimation(new Animation.Animator(animation.Name, animation.Rectangle, content, animation.SpriteFile, animation.FramesRange, animation.Fps, animation.LoopCount, animation.Delay, animation.RowFrameCount, animation.FileType, animation.AnimType, animation.IsMoving, animation.MoveSpeed, animation.Destination, auto.LayerDepth));
                }
                loopAnimation.SetCurrentAnimation(0);
                nextScene.SceneAutoSprites.Add(loopAnimation);
            }
            foreach (NPC npc in sceneAssets.NPCs)
            {
                NpcObject character = new NpcObject(npc.Name, content, npc.LayerDepth, npc.IdleGroup);
                foreach (AnimationSprite animation in npc.AnimationSprites)
                {
                    character.AddAnimation(new Animation.Animator(animation.Name, animation.Rectangle, content, animation.SpriteFile, animation.FramesRange, animation.Fps, animation.LoopCount, animation.Delay, animation.RowFrameCount, animation.FileType, animation.AnimType, animation.IsMoving, animation.MoveSpeed, animation.Destination, npc.LayerDepth));
                }
                nextScene.SceneCharacters.Add(character);
            }
            foreach (InventoryItem item in sceneAssets.InventoryItems)
                nextScene.SceneInventoryItems.Add(new InventoryObject(item.ItemId, item.Name, item.Rectangle, item.Texture, content, item.LayerDepth));
            foreach (Interactive interactive in sceneAssets.Interactives)
                nextScene.SceneInteractives.Add(new InteractiveObject(interactive.Name, interactive.Rectangle, interactive.Texture, content, interactive.LayerDepth));
            foreach (MiniTriger miniTriger in sceneAssets.MiniGameTrigers)
                nextScene.SceneMiniGameTrigers.Add(new PopupTriggerObject(miniTriger.MiniGameID, miniTriger.Name, miniTriger.Rectangle, miniTriger.Texture, content, miniTriger.LayerDepth));
            foreach (Forground forground in sceneAssets.Forgrounds)
                nextScene.SceneForgrounds.Add(new ForgroundObject(forground.Name, forground.Rectangle, forground.Texture, content, forground.LayerDepth));
            foreach (ParticleClass particle in sceneAssets.Particles)
            {
                //TODO: using DI/IoC here
                //ParticleEngine particleInstance = (ParticleEngine)(Activator.CreateInstance(Type.GetType(string.Format("Enginus.Particle.{0}", particle.ClassName), true)));
                //particleInstance.content = content;
                //particleInstance.Initialize();
                //nextScene.SceneParticles.Add(particleInstance);
            }
            foreach (ConvexPolygon polygon in sceneAssets.MeshPolygons)
            {
                for (int i = 0; i < polygon.Vertices.Count; i++)
                    polygon.Vertices[i] = new Point(polygon.Vertices[i].X, polygon.Vertices[i].Y);
                polygon.GenerateEdges();
                nextScene.SceneMesh.AddPolygon(polygon);
            }
            foreach (MeshLink link in sceneAssets.MeshLinks)
            {
                PolygonLink polygonLink = new PolygonLink(
                    nextScene.SceneMesh.PolygonList[link.StartPoly],
                    new IndexedEdge(link.EdgesStartPoly[0], link.EdgesStartPoly[1]),
                    nextScene.SceneMesh.PolygonList[link.EndPoly],
                    new IndexedEdge(link.EdgesEndPoly[0], link.EdgesEndPoly[1]));
                nextScene.SceneMesh.AddLink(polygonLink);
            }
            return nextScene;
        }
    }
}