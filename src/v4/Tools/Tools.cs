
using Not.Injection;
using NTS.Persistence.Startup;
using Microsoft.Extensions.DependencyInjection;
//var a = new EventFormService(new EventRepository(new StaffMemberRepository()));
var services = new ServiceCollection();
services.AddPersistence();
services.GetConventionalAssemblies().RegisterConventionalServices();

var provider = services.BuildServiceProvider();