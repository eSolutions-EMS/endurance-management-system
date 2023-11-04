
using Common.Application.Forms;
using Common.Conventions;
using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;
using EMS.Judge.Setup.Events;
using EMS.Persistence.Adapters;
using EMS.Persistence.Startup;
using Microsoft.Extensions.DependencyInjection;
//var a = new EventFormService(new EventRepository(new StaffMemberRepository()));
var services = new ServiceCollection();
services.AddPersistence();
services.GetConventionalAssemblies().RegisterConventionalServices();

var provider = services.BuildServiceProvider();
var eventService = provider.GetRequiredService<IUpdateForm<StaffMember>>();
;