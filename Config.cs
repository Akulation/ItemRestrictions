using Rocket.API;
using ItemRestrictions.Models;

namespace ItemRestrictions
{
    public class Config : IRocketPluginConfiguration
    {
        public Restriction[] ItemRestrictions { get; set; }

        public void LoadDefaults()
        {
            ItemRestrictions = new Restriction[]
            {
                new Restriction()
                {
                    PickupPermission = "ItemRestrictions.Shadowstalkers",
                    Items = new ushort[]
                    {
                        300,
                        1441
                    }
                },
                new Restriction()
                {
                    PickupPermission = "ItemRestrictions.InvisibleGuns",
                    Items = new ushort[]
                    {
                        1300,
                        1394,
                        1471
                    }
                }
            };
        }
    }
}
