using ItemRestrictions.Models;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace ItemRestrictions
{
    public class ItemRestrictions : RocketPlugin<Config>
    {
        public static ItemRestrictions Instance { get; private set; }
        public Color MessageColor { get; set; }
        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            Logger.Log("ItemRestrictions has been successfully loaded!");
            ItemManager.onTakeItemRequested += onTakeItemRequested;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += OnPlayerInventoryAdded;
        }

        protected override void Unload()
        {
            Logger.Log("ItemRestrictions has been successfully unloaded!");
            ItemManager.onTakeItemRequested -= onTakeItemRequested;
        }

        private void onTakeItemRequested(Player player1, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            if (Configuration.Instance.DeleteItems) return;
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(player1);
            if (System.IO.File.Exists("Plugins/ItemRestrictions/restrictions.json"))
            {
                string fileContents = System.IO.File.ReadAllText("Plugins/ItemRestrictions/restrictions.json");
                List<Restriction> restrictions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restriction>>(fileContents);
                Restriction restriction = restrictions.FirstOrDefault(r => r.itemID == itemData.item.id);
                if (restriction != null)
                {
                    if (player.HasPermission(restriction.PickupPermission))
                    {
                        shouldAllow = true;
                        return;
                    }
                    else
                    {
                        string itemName = Assets.find(EAssetType.ITEM, itemData.item.id).FriendlyName;
                        UnturnedChat.Say(player, Translate("ItemRestricted", itemName));
                        shouldAllow = false;
                        return;
                    }
                }
            }
        }

        private void OnPlayerInventoryAdded(UnturnedPlayer player, Rocket.Unturned.Enumerations.InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P)
        {
            if (!Configuration.Instance.DeleteItems) return;
            if (System.IO.File.Exists("Plugins/ItemRestrictions/restrictions.json"))
            {
                string fileContents = System.IO.File.ReadAllText("Plugins/ItemRestrictions/restrictions.json");
                List<Restriction> restrictions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restriction>>(fileContents);
                Restriction restriction = restrictions.FirstOrDefault(r => r.itemID == P.item.id);
                if (restriction != null)
                {
                    if (player.HasPermission(restriction.PickupPermission)) return;
                    else
                    {
                        string itemName = Assets.find(EAssetType.ITEM, P.item.id).FriendlyName;
                        player.Inventory.removeItem((byte)inventoryGroup, inventoryIndex);
                        UnturnedChat.Say(player, Translate("ItemRestricted", itemName));
                        return;
                    }
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "RestrictMissingArgs", "Not enough arguments, correct usage: /restrict <ItemID> <Permission>" },
            { "UnrestrictMissingArgs", "Not enough arguments, correct usage: /unrestrict <ItemID>" },
            { "AlreadyRestricted", "The item `{0}` is already restricted to the permission `{1}`!" },
            { "RestrictSuccess", "You have successfully restricted item `{0}` to the permission `{1}`!" },
            { "UnrestrictSuccess", "You have successfully unrestricted item `{0}` with the permission `{1}`!" },
            { "UnrestrictNotRestricted", "The item `{0}` is not restricted!" },
            { "InvalidItem", "You must specify a valid item ID!" },
            { "ItemRestricted", "The item `{0}` is restricted!" }
        };
    }
}
