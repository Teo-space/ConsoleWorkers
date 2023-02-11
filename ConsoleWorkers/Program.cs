using ConsoleWorkers;
using System.Diagnostics;
using System.Xml.Linq;



///Demonstration Parrallel running Console workers
///with receiving messages
///and Catching errors


print("App running");


int WorkersEnded = 0;

ThreadPool.QueueUserWorkItem((o) =>
{
	print("Run Worker1");

	WorkerContainer.Create("Worker1", "Worker.exe", "2000 1")
	//.EnableLogging()
	.AddOnOutput((name, message) => print($"[Worker:{name}] Output {message}"))
	.AddOnError((name, message) => print($"[Worker:{name}] OnError {message}"))
	.AddOnExited((name, code) =>
	{
		print($"[Worker:{name}] Exit Code {code}");
		WorkersEnded ++;
	})
	.Init()
	.Start();

});

ThreadPool.QueueUserWorkItem((o) =>
{
	print("Run Worker2");

	WorkerContainer.Create("Worker1", "Worker.exe", "1000 2")
	//.EnableLogging()
	.AddOnOutput((name, message) => print($"[Worker:{name}] Output {message}"))
	.AddOnError((name, message) => print($"[Worker:{name}] OnError {message}"))
	.AddOnExited((name, code) =>
	{
		print($"[Worker:{name}] Exit Code {code}");
		WorkersEnded ++;
	})
	.Init()
	.Start();

});




while(WorkersEnded < 2) { Thread.Sleep(66); }

print();
print();
print($"Done Workers: {WorkersEnded}");











