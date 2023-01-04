using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

namespace ItemRestrictions
{
    public class ItemRestrictions : RocketPlugin<Config>
    {
        protected override void Load()
        {
            Logger.Log("ItemRestrictions has been successfully loaded!");
            ItemManager.onTakeItemRequested += onTakeItemRequested;
        }

        protected override void Unload()
        {
            Logger.Log("ItemRestrictions has been successfully unloaded!");
            ItemManager.onTakeItemRequested -= onTakeItemRequested;
        }

        private void onTakeItemRequested(Player player1, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(player1);
            var Restrictions = Configuration.Instance.ItemRestrictions;
            foreach (var Restriction in Restrictions)
            {
                foreach (var itemID in Restriction.Items)
                {
                    if (itemID != itemData.item.id)
                    {
                        break;
                    }
                    if (player.HasPermission(Restriction.PickupPermission)){
                        shouldAllow = true;
                        break;
                    }
                    string itemName = Assets.find(EAssetType.ITEM, itemData.item.id).FriendlyName;
                    UnturnedChat.Say(player, Translate("ItemRestricted", itemName, Restriction.PickupPermission));
                    shouldAllow = false;
                    break;
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "NotATranslation", "You can use variables {0} and {1} for showing item name and the pickup permission needed." },
            { "ItemRestricted", "The item {0} is restricted." }
        };
    }
}
