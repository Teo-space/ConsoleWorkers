global using static Printer;


public static class Printer
{
	public static void print() => Console.WriteLine();
	public static void print(object o) => Console.WriteLine($"[{DateTime.Now.ToString("yyyy.MM.dd:HH:mm:ss.fff")}] {o?.ToString() ?? string.Empty}");
}