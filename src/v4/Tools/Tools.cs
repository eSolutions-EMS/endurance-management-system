
using Common.Conventions;
using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;
using EMS.Judge.Setup.Events;
using EMS.Persistence.Startup;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddPersistence();
services.GetConventionalAssemblies().RegisterConventionalServices();

var provider = services.BuildServiceProvider();
var eventService = provider.GetRequiredService<IUpdateEvent>();
;