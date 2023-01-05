# ItemRestrictions
Unturned Rocketmod Plugin that lets you restrict items

Instead of deleting items like many other restrictor plugins, it simply leaves the item on the ground!

# Features
You can create and remove restrictions using commands ingame, no need to configure any files!
You can make as many restrictions as you want and each of them can have a different permission!
 
# Example Configuration
```
  <ItemRestrictions>
    <Restriction>
      <PickupPermission>ItemRestrictions.Shadowstalkers</PickupPermission>
      <Items>
        <itemID>300</itemID>
        <itemID>1441</itemID>
      </Items>
    </Restriction>
    <Restriction>
      <PickupPermission>ItemRestrictions.InvisibleGuns</PickupPermission>
      <Items>
        <itemID>1300</itemID>
        <itemID>1394</itemID>
        <itemID>1471</itemID>
      </Items>
    </Restriction>
  </ItemRestrictions>
```

 
Plugins I used for refrences because this is my first ever RM plugin (always give credit when its due):
https://github.com/BTPlugins/BTAdvancedRestrictor
https://github.com/RestoreMonarchyPlugins/MoreHomes
