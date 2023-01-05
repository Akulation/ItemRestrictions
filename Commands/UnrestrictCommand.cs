using System;
using System.Collections.Generic;
using System.Linq;
using ItemRestrictions.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;

namespace ItemRestrictions.Commands
{
    public class UnrestrictCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "unrestrict";

        public string Help => "Unrestrict items";

        public string Syntax => "/unrestrict <ItemID>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("UnrestrictMissingArgs"), ItemRestrictions.Instance.MessageColor);
                return;
            }

            int idresult;
            if (int.TryParse(command[0], out idresult))
            {
                ushort ItemID = Convert.ToUInt16(idresult);
                string itemName = Assets.find(EAssetType.ITEM, ItemID).FriendlyName;

                if (System.IO.File.Exists("Plugins/ItemRestrictions/restrictions.json"))
                {
                    string fileContents = System.IO.File.ReadAllText("Plugins/ItemRestrictions/restrictions.json");
                    List<Restriction> restrictions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restriction>>(fileContents);
                    Restriction restriction = restrictions.FirstOrDefault(r => r.itemID == idresult);
                    if (restriction != null)
                    {
                        restrictions.RemoveAll(r => r.itemID == idresult);

                        string argPerm = restriction.PickupPermission;

                        string updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(restrictions);
                        System.IO.File.WriteAllText("Plugins/ItemRestrictions/restrictions.json", updatedJson);

                        UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("UnrestrictSuccess", itemName, argPerm), ItemRestrictions.Instance.MessageColor);
                    }
                    else
                    {
                        UnturnedChat.Say(caller, ItemRestrictions.Instance.Translate("UnrestrictNotRestricted", itemName), ItemRestrictions.Instance.MessageColor);
                    }
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
