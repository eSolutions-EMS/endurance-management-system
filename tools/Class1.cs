

using ems_tools;

var client = new SignalRClient();
client.Configure("http://localhost:11337");
await client.Start();
await client.FetchStartlist();
