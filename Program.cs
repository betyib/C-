using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Clock
{
    class Program
    {
        static bool IsRunning;
        private static Stopwatch watch = new Stopwatch();

        static void Main(string[] args)
        {
            ShowMenu();


            CancellationTokenSource cts = new CancellationTokenSource();
            Task timerTask = Task.Run(() => ShowTheWatch(cts.Token));

            // Handle user input
            while (!cts.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var keyInput = Console.ReadKey(true).Key;

                    switch (keyInput)
                    {
                        case ConsoleKey.S:
                            if (!IsRunning)
                            {
                                watch.Start();
                                IsRunning = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                ShowMessage("S pressed: Timer is running.");
                            }
                            break;

                        case ConsoleKey.T:
                            if (IsRunning)
                            {
                                watch.Stop();
                                IsRunning = false;
                                Console.ForegroundColor = ConsoleColor.Red;
                                ShowMessage("T pressed: Timer is stopped.");
                            }
                            break;

                        case ConsoleKey.R:
                            watch.Reset();
                            IsRunning = false;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            ShowMessage("R pressed: Timer reset.");
                            Task.Delay(1000).Wait();
                            ShowMenu();
                            break;

                        case ConsoleKey.Escape:
                            cts.Cancel();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            ShowMessage("ESC pressed: Exiting program.");
                            Task.Delay(1000).Wait();
                            break;
                    }
                }

                Task.Delay(50).Wait();
            }

            timerTask.Wait();
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("=============== STOPWATCH ===============");
            Console.WriteLine("[S] Start the watch");
            Console.WriteLine("[T] Stop the watch");
            Console.WriteLine("[R] Reset the watch");
            Console.WriteLine("[ESC] Quit the program");
        }

        private static void ShowMessage(string message)
        {
            Console.SetCursorPosition(0, 10);
            Console.WriteLine(message.PadRight(Console.WindowWidth - 1));
        }

        private static void ShowTheWatch(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 12);
                if (IsRunning)
                {
                    TimeSpan elapsed = watch.Elapsed;
                    Console.WriteLine($"{elapsed:mm\\:ss\\.fff}".PadRight(Console.WindowWidth - 1));

                }
                else
                {
                    Console.WriteLine("00:00.000".PadRight(Console.WindowWidth - 1));
                }

                Thread.Sleep(100); // Update every 100ms for smooth output
            }
        }
    }
}
