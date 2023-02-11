
if(args.Length >= 2)
{
	int timeToSleep = int.Parse(args[0]);
	var number = int.Parse(args[1]);

	Console.WriteLine($"Worker:{number}	 Sleep:({timeToSleep})");
	Thread.Sleep(timeToSleep);
	throw new Exception($"Worker:{number} Exception");
}
else
{
	Console.WriteLine($"Worker!!!!!!!!!!!!!!!!!!!");
	throw new Exception($"Worker Exception");
}



Console.WriteLine("End");
