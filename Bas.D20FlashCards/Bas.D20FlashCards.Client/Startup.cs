using System;
using Bas.D20FlashCards.Pathfinder;
using Bas.D20FlashCards.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bas.D20FlashCards.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CardsService>(CardsServiceFactory);
        }

        private CardsService CardsServiceFactory(IServiceProvider arg)
        {
            return new CardsService(new Parser[]
            {
                new ArchivesOfNethysParser(new Uri("https://aonprd.com/"), " - Archives of Nethys: Pathfinder RPG Database"),
                new D20PFSrdParser(new Uri("https://www.d20pfsrd.com"), new Uri("/feats"), new Uri("/skills"))
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
