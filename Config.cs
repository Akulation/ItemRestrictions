using Rocket.API;

namespace ItemRestrictions
{
    public class Config : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool DeleteItems { get; set; }
        public void LoadDefaults()
        {
            MessageColor = "yellow";
            DeleteItems = false;
        }
    }
}
