using Microsoft.CognitiveServices.Speech;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Program
{
    public static async Task Main(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription("YourSubscriptionKey", "YourServiceRegion");
        using var recognizer = new SpeechRecognizer(speechConfig);

        Console.WriteLine("Diga 'desligar o computador' para desligar o sistema.");

        recognizer.Recognized += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"Você disse: {e.Result.Text}");
                if (e.Result.Text.ToLower().Contains("desligar o computador"))
                {
                    ShutdownComputer();
                }
            }
            else if (e.Result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine("Nenhuma correspondência encontrada.");
            }
        };

        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
        Console.WriteLine("Pressione qualquer tecla para parar...");
        Console.ReadKey();
        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    }

    private static void ShutdownComputer()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("shutdown", "now");
        }
    }
}
