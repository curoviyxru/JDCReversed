using JDCReversedCli;

//KeyIntercept.Start();

new Thread(Client.Start).Start();

Application.Run();

KeyIntercept.Stop();