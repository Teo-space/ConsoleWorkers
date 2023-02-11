using System.Diagnostics;


namespace ConsoleWorkers;


public record WorkerContainer(string name, string path, string args)
{
	public static WorkerContainer Create(string name, string path, string args)
		=> new WorkerContainer(name, path, args);


	Process process = new Process();
	public WorkerContainer Init()
	{
		process.EnableRaisingEvents = true;
		process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(ProcessOutputDataReceived);
		process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(ProcessErrorDataReceived);
		process.Exited += new System.EventHandler(ProcessExited);

		process.StartInfo.FileName = path;
		process.StartInfo.Arguments = args;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.RedirectStandardOutput = true;

		return this;
	}

	public WorkerContainer Start()
	{
		process.Start();
		process.BeginErrorReadLine();
		process.BeginOutputReadLine();
		return this;
	}


	/// <summary>
	/// below line is optional if we want a blocking call
	/// </summary>
	public void WaitForExit() => process.WaitForExit();


	bool Logging = false;
	public WorkerContainer EnableLogging()
	{
		Logging = true;
		return this;
	}
	public WorkerContainer DisableLogging()
	{
		Logging = false;
		return this;
	}


	public delegate void OutputDelegate(string workerName, string message);

	List<OutputDelegate> OutputDataReceivedSubs = new();
	public WorkerContainer AddOnOutput(OutputDelegate d)
	{
		OutputDataReceivedSubs.Add(d);
		return this;
	}

	public delegate void ErrorDelegate(string workerName, string message);
	List<ErrorDelegate> ErrorDataReceivedSubs = new();
	public WorkerContainer AddOnError(ErrorDelegate d)
	{
		ErrorDataReceivedSubs.Add(d);
		return this;
	}

	public delegate void ProcessExitedDelegate(string workerName, int Code);
	List<ProcessExitedDelegate> ProcessExitedSubs = new();
	public WorkerContainer AddOnExited(ProcessExitedDelegate d)
	{
		ProcessExitedSubs.Add(d);
		return this;
	}




	void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (!string.IsNullOrEmpty(e.Data))
		{
			if (Logging)
			{
				print($"[WorkerContainer] [{name}] OutputDataReceived Data:{e.Data}");
			}

			foreach (var sub in OutputDataReceivedSubs)
			{
				sub?.Invoke(name, $"{e.Data}");
			}
		}
	}


	void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (!string.IsNullOrEmpty(e.Data))
		{
			if (Logging)
			{
				print($"[WorkerContainer] [{name}] ErrorDataReceived Data:{e.Data}");
			}
			foreach (var sub in ErrorDataReceivedSubs)
			{
				sub?.Invoke(name, $"{e.Data}");
			}
		}
	}


	void ProcessExited(object sender, EventArgs e)
	{
		if (Logging)
		{
			print($"[WorkerContainer] [{name}] process exited with code {process.ExitCode}");
		}
		foreach (var sub in ProcessExitedSubs)
		{
			sub?.Invoke(name, process.ExitCode);
		}
	}


}


