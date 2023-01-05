using System;
using System.Collections.Generic;
using System.Linq;
using ItemRestrictions.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;

namespace ItemRestrictions.Commands
{
   public class RestrictCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "restrict";

        public string Help => "Restrict items";

        public string Syntax => "/restrict <ItemID> <Permission>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 2)
            {
                UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("RestrictMissingArgs"), ItemRestrictions.Instance.MessageColor);
                return;
            }

            int idresult;
            if (int.TryParse(command[0], out idresult))
            {
                string argPerm = command[1];

                Restriction restriction = new Restriction
                {
                    PickupPermission = argPerm,
                    itemID = idresult
                };

                ushort ItemID = Convert.ToUInt16(idresult);
                string itemName = Assets.find(EAssetType.ITEM, ItemID).FriendlyName;

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(restriction);

                if (System.IO.File.Exists("Plugins/ItemRestrictions/restrictions.json"))
                {
                    string fileContents = System.IO.File.ReadAllText("Plugins/ItemRestrictions/restrictions.json");
                    List<Restriction> restrictions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restriction>>(fileContents);

                    if (restrictions.Any(r => r.itemID == idresult))
                    {
                        UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("AlreadyRestricted", itemName, argPerm), ItemRestrictions.Instance.MessageColor);
                    }
                    else
                    {
                        restrictions.Add(restriction);

                        string updatedJson = "[" + string.Join(",", restrictions.Select(r => Newtonsoft.Json.JsonConvert.SerializeObject(r))) + "]";
                        System.IO.File.WriteAllText("Plugins/ItemRestrictions/restrictions.json", updatedJson);

                        UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("RestrictSuccess", itemName, argPerm), ItemRestrictions.Instance.MessageColor);
                    }
                }
                else
                {
                    System.IO.File.WriteAllText("Plugins/ItemRestrictions/restrictions.json", "[" + json + "]");
                    UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("RestrictSuccess", itemName, argPerm), ItemRestrictions.Instance.MessageColor);
                }
            }
            else
            {
                UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("InvalidItem"), ItemRestrictions.Instance.MessageColor);
                return;
            }
        }
    }
}
