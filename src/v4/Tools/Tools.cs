
using Common.Application.Forms;
using Common.Conventions;
using EMS.Domain.Setup.Entities;
using EMS.Persistence.Startup;
using Microsoft.Extensions.DependencyInjection;
//var a = new EventFormService(new EventRepository(new StaffMemberRepository()));
var services = new ServiceCollection();
services.AddPersistence();
services.GetConventionalAssemblies().RegisterConventionalServices();

var provider = services.BuildServiceProvider();
var eventService = provider.GetRequiredService<IManage<Official>>();
;