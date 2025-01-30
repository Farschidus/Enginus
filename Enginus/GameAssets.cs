using Microsoft.Xna.Framework;
using Enginus.Animation;
using Enginus.Core;
using Enginus.Navigation;
using System.Collections.Generic;

namespace Enginus
{
    public class GameAssets
    {
        public List<Scene> Scenes;
    }

    public class Scene
    {
        public string SceneName;
        public string Background;
        public string SceneMusic;
        public List<Exit> Exits;
        public List<LoopAnimation> LoopAnimations;
        public List<NPC> NPCs;
        public List<Forground> Forgrounds;
        public List<Interactive> Interactives;
        public List<InventoryItem> InventoryItems;
        public List<ParticleClass> Particles;
        public List<MiniTriger> MiniGameTrigers;
        public List<ConvexPolygon> MeshPolygons;
        public List<MeshLink> MeshLinks;
    }

    public class Exit
    {
        public Rectangle Rectangle;
        public string NextSceneName;
        public Vector2 PlayerPosition;
        public Direction PlayerDirection;
        public float PlayerLayerDepth;
        public string InitGroup;
    }
    public class LoopAnimation
    {
        public string Name;
        public float LayerDepth;
        public AnimationSprite[] AnimationSprites;
    }
    public class NPC
    {
        public string Name;
        public float LayerDepth;
        public AnimationSprite[] AnimationSprites;
        public string InitGroup;
        public string IdleGroup;
    }
    public class Forground
    {
        public string Name;
        public Rectangle Rectangle;
        public string Texture;
        public float LayerDepth;
        public string InitGroup;
    }
    public class Interactive
    {
        public string Name;
        public Rectangle Rectangle;
        public string Texture;
        public float LayerDepth;
    }
    public class InventoryItem
    {
        public int ItemId;
        public string Name;
        public Rectangle Rectangle;
        public string Texture;
        public float LayerDepth;
        public string InitGroup;
    }
    public class ParticleClass
    {
        public string ClassName;
    }
    public class MiniTriger
    {
        public int MiniGameID;
        public string Name;
        public Rectangle Rectangle;
        public string Texture;
        public float LayerDepth;
        public string InitGroup;
    }
    public class MeshLink
    {
        public int StartPoly;
        public int EndPoly;
        public int[] EdgesStartPoly;
        public int[] EdgesEndPoly;
    }


    public class AnimationSprite
    {
        public string Name;
        public Rectangle Rectangle;
        public SpriteFile SpriteFile;
        public FrameRange FramesRange;
        public float Fps;
        public float Delay;
        public int LoopCount;
        public int[] RowFrameCount;
        public AnimationFileType FileType;
        public AnimationType AnimType;
        public bool IsMoving;
        public float MoveSpeed;
        public Vector2 Destination;
    }


    public class PuzzlePopups
    {
        public List<PuzzlePopup> Popups;
    }

    public class PuzzlePopup
    {
        public int Id;
        public string SceneName;
        public string Background;
        public List<LoopAnimation> LoopAnimations;
        public List<Forground> Forgrounds;
        public List<Interactive> Interactives;
    }
}