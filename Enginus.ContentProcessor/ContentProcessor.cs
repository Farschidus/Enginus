using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Audio;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Enginus.ContentProcessor;

public class ContentProcessor
{
    private readonly string _outputDir;

    public ContentProcessor(string outputDir)
    {
        _outputDir = outputDir;
        Directory.CreateDirectory(_outputDir); // Ensure output directory exists
    }

    public void ProcessTexture(string inputPath)
    {
        try
        {
            Console.WriteLine($"Processing Texture: {inputPath}");

            using var stream = new FileStream(inputPath, FileMode.Open);
            var textureContent = new Texture2DContent();
            var bitmapContent = new PixelBitmapContent<Color>(1, 1); // Use a concrete implementation of BitmapContent
            textureContent.Faces[0].Add(bitmapContent);

            // Convert to XNB
            using var outputStream = new FileStream(GetOutputPath(inputPath), FileMode.Create);
            var compiler = new ContentCompiler();
            compiler.Compile(outputStream, textureContent, GetTargetPlatform(), GraphicsProfile.HiDef, false, null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing texture {inputPath}: {ex.Message}");
        }
    }

    public void ProcessSound(string inputPath)
    {
        try
        {
            Console.WriteLine($"Processing Sound: {inputPath}");

            using var stream = new FileStream(inputPath, FileMode.Open);
            var audioContent = new AudioContent(Path.GetFileNameWithoutExtension(inputPath), GetAudioFileType(inputPath));

            // Convert to XNB
            using var outputStream = new FileStream(GetOutputPath(inputPath), FileMode.Create);
            var compiler = new ContentCompiler();
            compiler.Compile(outputStream, audioContent, GetTargetPlatform(), GraphicsProfile.HiDef, false, null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing sound {inputPath}: {ex.Message}");
        }
    }

    private static AudioFileType GetAudioFileType(string inputPath)
    {
        string extension = Path.GetExtension(inputPath).ToLowerInvariant();
        return extension switch
        {
            ".mp3" => AudioFileType.Mp3,
            ".wav" => AudioFileType.Wav,
            ".wma" => AudioFileType.Wma,
            ".ogg" => AudioFileType.Ogg,
            _ => throw new Exception("Audio file is unknown!"),
        };
    }

    private string GetOutputPath(string inputPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(inputPath) + ".xnb";
        return Path.Combine(_outputDir, fileName);
    }

    private static TargetPlatform GetTargetPlatform()
    {
        return OperatingSystem.IsWindows() ? TargetPlatform.Windows :
               OperatingSystem.IsLinux() ? TargetPlatform.DesktopGL :
               OperatingSystem.IsMacOS() ? TargetPlatform.MacOSX :
               throw new NotSupportedException("Unsupported platform");
    }
}
